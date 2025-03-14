using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.SDK.Core.ApplicationManagement;
using Reflectis.SDK.Core.SystemFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEditor;

using UnityEngine;
using UnityEngine.Events;

using static Reflectis.CreatorKit.Worlds.Core.Interaction.IVisualScriptingInteractable;

namespace Reflectis.CreatorKit.Worlds.VisualScripting
{
    //[RequireComponent(typeof(BaseInteractable))]
    public abstract class VisualScriptingInteractable : InteractableBehaviourBase, IInteractableBehaviour, IVisualScriptingInteractable
    {
        private static VisualScriptingInteractable nextSelectedInteractable;

        private static VisualScriptingInteractable selectedInteractable;

        [SerializeField] private ScriptMachine interactionScriptMachine = null;

        private ScriptMachine unselectOnDestroyScriptMachine = null;

        [Header("Allowed states")]
        [SerializeField] private EAllowedVisualScriptingInteractableState desktopAllowedStates = EAllowedVisualScriptingInteractableState.Selected | EAllowedVisualScriptingInteractableState.Interacting;
        [SerializeField] private EAllowedVisualScriptingInteractableState vrAllowedStates = EAllowedVisualScriptingInteractableState.Selected | EAllowedVisualScriptingInteractableState.Interacting;
        [SerializeField] private EVRVisualScriptingInteraction vrVisualScriptingInteraction = (EVRVisualScriptingInteraction)~0;

        public Action<GameObject> OnSelectedActionVisualScripting;

        public ScriptMachine InteractionScriptMachine { get => interactionScriptMachine; set => interactionScriptMachine = value; }

        public EAllowedVisualScriptingInteractableState DesktopAllowedStates { get => desktopAllowedStates; set => desktopAllowedStates = value; }
        public EAllowedVisualScriptingInteractableState VRAllowedStates { get => vrAllowedStates; set => vrAllowedStates = value; }
        public EVRVisualScriptingInteraction VrVisualScriptingInteraction { get => vrVisualScriptingInteraction; set => vrVisualScriptingInteraction = value; }

        public bool SkipSelectState => skipSelectState;

        public override bool IsIdleState => CurrentInteractionState == EVisualScriptingInteractableState.Idle;

        private EVisualScriptingInteractableState currentInteractionState;
        public EVisualScriptingInteractableState CurrentInteractionState
        {
            get => currentInteractionState;
            set
            {
                currentInteractionState = value;
                if (currentInteractionState == EVisualScriptingInteractableState.Idle)
                {
                    InteractableRef.IsHovered = false;
                }
            }
        }



        protected bool hasHoveredState = false;
        private bool skipSelectState = false;
        private bool hasInteractState = false;

        public List<HoverEnterEventUnit> hoverEnterEventUnits = new List<HoverEnterEventUnit>();

        public List<HoverExitEventUnit> hoverExitEventUnits = new List<HoverExitEventUnit>();

        public List<SelectEnterEventUnit> selectEnterEventUnits = new List<SelectEnterEventUnit>();

        public List<SelectExitEventUnit> selectExitEventUnits = new List<SelectExitEventUnit>();

        public List<InteractEventUnit> interactEventUnits = new List<InteractEventUnit>();

        private List<SelectExitEventUnit> unselectOnDestroyEventUnits = new List<SelectExitEventUnit>();

        private async void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            if (!IsIdleState && CurrentInteractionState != EVisualScriptingInteractableState.SelectExiting)
            {
                foreach (var unit in unselectOnDestroyEventUnits)
                {
                    try
                    {
                        await unit.AwaitableTrigger(unselectOnDestroyScriptMachine.GetReference().AsReference(), this);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error on unit {unit} on {gameObject.name} on destroy: {e.Message}", unselectOnDestroyScriptMachine.gameObject);
                    }
                }
            }
            if (unselectOnDestroyScriptMachine != null && unselectOnDestroyScriptMachine.gameObject != null)
            {
                Destroy(unselectOnDestroyScriptMachine.gameObject);
            }

            if (this == SM.GetSystem<IVisualScriptingInteractionSystem>().SelectedInteractable as VisualScriptingInteractable)
            {
                SM.GetSystem<IVisualScriptingInteractionSystem>().UnselectCurrentInteractable();
            }
        }


        #region UnityEvents Callbacks
        public UnityEvent OnHoverGrabEnter { get; set; } = new UnityEvent();
        public UnityEvent OnHoverGrabExit { get; set; } = new UnityEvent();
        public UnityEvent OnHoverRayEnter { get; set; } = new UnityEvent();
        public UnityEvent OnHoverRayExit { get; set; } = new UnityEvent();
        public UnityEvent OnHoverMouseEnter { get; set; } = new UnityEvent();
        public UnityEvent OnHoverMouseExit { get; set; } = new UnityEvent();
        #endregion


        public override Task Setup()
        {
            switch (SM.GetSystem<IPlatformSystem>().RuntimePlatform)
            {
                case ESupportedPlatform.WebGL:
                    skipSelectState = !DesktopAllowedStates.HasFlag(EAllowedVisualScriptingInteractableState.Selected);
                    hasInteractState = DesktopAllowedStates.HasFlag(EAllowedVisualScriptingInteractableState.Interacting);
                    hasHoveredState = DesktopAllowedStates.HasFlag(EAllowedVisualScriptingInteractableState.Hovered);
                    break;
                case ESupportedPlatform.VR:
                    skipSelectState = !VRAllowedStates.HasFlag(EAllowedVisualScriptingInteractableState.Selected);
                    hasInteractState = VRAllowedStates.HasFlag(EAllowedVisualScriptingInteractableState.Interacting);
                    hasHoveredState = VRAllowedStates.HasFlag(EAllowedVisualScriptingInteractableState.Hovered);
                    break;
            }

            if (interactionScriptMachine != null)
            {
                if (interactionScriptMachine.graph != null)
                {
                    foreach (var unit in interactionScriptMachine.graph.units)
                    {
                        if (unit is HoverEnterEventUnit hoverEnterEventUnit)
                        {
                            hoverEnterEventUnits.Add(hoverEnterEventUnit);
                        }
                        if (unit is HoverExitEventUnit hoverExitEventUnit)
                        {
                            hoverExitEventUnits.Add(hoverExitEventUnit);
                        }
                        if (unit is SelectEnterEventUnit selectEnterEventUnit)
                        {
                            selectEnterEventUnits.Add(selectEnterEventUnit);
                        }
                        if (unit is SelectExitEventUnit selectExitEventUnit)
                        {
                            selectExitEventUnits.Add(selectExitEventUnit);
                            SetupOnDestroy(selectExitEventUnit);
                        }
                        if (unit is InteractEventUnit interactEventUnit)
                        {
                            interactEventUnits.Add(interactEventUnit);
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Null graph on script machine {interactionScriptMachine.gameObject}!", interactionScriptMachine.gameObject);
                }
            }

            return Task.CompletedTask;
        }
        #region OnUnselectOnDestroy
        private void SetupOnDestroy(SelectExitEventUnit unselectUnit)
        {
            if (unselectOnDestroyScriptMachine == null)
            {
                GameObject go = new GameObject("UnselectOnDestroy" + gameObject.name);
                CopyVariables(go, InteractionScriptMachine.GetComponent<Variables>());
                DontDestroyOnLoad(go);
                unselectOnDestroyScriptMachine = go.AddComponent<ScriptMachine>();
                unselectOnDestroyScriptMachine.nest.SwitchToEmbed(new FlowGraph());
            }
            SelectExitEventUnit newSelectExitUnit = CopyScriptMachineUnit(unselectOnDestroyScriptMachine, unselectUnit) as SelectExitEventUnit;
            unselectOnDestroyEventUnits.Add(newSelectExitUnit);
        }

        public IUnit CopyScriptMachineUnit(ScriptMachine scriptMachine, IUnit unit)
        {
            if (!scriptMachine.graph.units.Any(x => x.guid.Equals(unit.guid)))
            {
                IUnit newUnit = CloneUnit(unit);
                scriptMachine.graph.units.Add(newUnit);

                foreach (var port in unit.ports)
                {
                    if (port.hasValidConnection)
                    {
                        foreach (var connection in port.connections)
                        {
                            IUnitOutputPort source = null;
                            IUnitInputPort destination = null;
                            if (connection.source != port)
                            {
                                IUnit connectedUnit = CopyScriptMachineUnit(scriptMachine, connection.source.unit);
                                foreach (var connectedUnitPort in connectedUnit.ports)
                                {
                                    if (connectedUnitPort.key == connection.source.key && connectedUnitPort is IUnitOutputPort unitOutputPort)
                                    {
                                        source = unitOutputPort;
                                    }
                                }
                                foreach (var newPort in newUnit.ports)
                                {
                                    if (newPort.key == port.key && newPort is IUnitInputPort newInputPort)
                                    {
                                        destination = newInputPort;
                                    }
                                }
                            }
                            if (connection.destination != port)
                            {
                                IUnit connectedUnit = CopyScriptMachineUnit(scriptMachine, connection.destination.unit);
                                foreach (var connectedUnitPort in connectedUnit.ports)
                                {
                                    if (connectedUnitPort.key == connection.destination.key && connectedUnitPort is IUnitInputPort unitOutputPort)
                                    {
                                        destination = unitOutputPort;
                                    }
                                }
                                foreach (var newPort in newUnit.ports)
                                {
                                    if (newPort.key == port.key && newPort is IUnitOutputPort newInputPort)
                                    {
                                        source = newInputPort;
                                    }
                                }
                            }
                            source.ValidlyConnectTo(destination);
                        }
                    }
                }
                return newUnit;
            }
            else
            {
                return scriptMachine.graph.units.FirstOrDefault(x => x.guid.Equals(unit.guid));
            }
        }

        private IUnit CloneUnit(IUnit originalUnit)
        {
            var unitType = originalUnit.GetType();
            var clonedUnit = (IUnit)Activator.CreateInstance(unitType);

            if (originalUnit is SubgraphUnit subgraphUnit)
            {
                clonedUnit = new SubgraphUnit(subgraphUnit.nest.macro);
            }

            // Creiamo una nuova istanza dell'unità

            // Copiamo tutte le proprietà pubbliche da originalConnection a clonedConnection
            foreach (var field in unitType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (field.Name.Equals("graph"))
                {
                    continue;
                }
                field.SetValue(clonedUnit, field.GetValue(originalUnit));
            }

            // Copiamo anche le proprietà di tipo complesso tramite Reflection, se necessario
            foreach (var property in unitType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (property.Name.Equals("graph"))
                {
                    continue;
                }
                if (property.CanWrite)
                {
                    property.SetValue(clonedUnit, property.GetValue(originalUnit));
                }
            }

            return clonedUnit;
        }



        public void CopyVariables(GameObject destination, Variables original)
        {
            if (original == null) return;

            // Aggiungi un nuovo componente Variables al GameObject di destinazione
            Variables newVariables = destination.AddComponent<Variables>();

            // Copia tutte le variabili
            foreach (var variable in original.declarations)
            {
                newVariables.declarations.Set(variable.name, variable.value);
            }

            return;
        }
        #endregion
        public async void OnHoverStateEntered()
        {
            //if (!CanInteract || !hasHoveredState)
            if (CurrentBlockedState != 0 || !hasHoveredState)
                return;

            IEnumerable<Task> hoverEnterUnitsTask = hoverEnterEventUnits.Select(async unit =>
            {
                await unit.AwaitableTrigger(interactionScriptMachine.GetReference().AsReference(), this);
            });

            await Task.WhenAll(hoverEnterUnitsTask);

        }

        public async void OnHoverStateExited()
        {
            //if (!CanInteract || !hasHoveredState)
            if (CurrentBlockedState != 0 || !hasHoveredState
                || (InteractableRef.LockHoverDuringInteraction && currentInteractionState != EVisualScriptingInteractableState.Idle)
                )
                return;

            IEnumerable<Task> hoverExitUnitsTask = hoverExitEventUnits.Select(async unit =>
            {
                if (unit == null || interactionScriptMachine == null)
                {
                    return;
                }
                await unit.AwaitableTrigger(interactionScriptMachine.GetReference().AsReference(), this);
            });

            await Task.WhenAll(hoverExitUnitsTask);
        }

        public async void OnInteract()
        {
            if (skipSelectState)
            {
                _ = Interact();
                return;
            }
            if (selectedInteractable != null) //if entering another object interaction await for its completion
            {
                if (selectedInteractable != this)
                {
                    nextSelectedInteractable = this;
                }
                while (selectedInteractable.CurrentInteractionState == EVisualScriptingInteractableState.SelectEntering)
                {
                    await Task.Yield();
                }
            }
            //the user clicked on another interactable object while exiting the current one
            //this object is not interesting anymore
            //the other object will call the select state
            if (nextSelectedInteractable != this)
            {
                return;
            }
            if (selectedInteractable != this)
            {
                //Exit current selected interactable if not in transition
                if (selectedInteractable != null)
                {
                    //if we already exiting the interaction wait until the exit is completed
                    if (selectedInteractable.CurrentInteractionState == EVisualScriptingInteractableState.SelectExiting)
                    {
                        while (selectedInteractable != null)
                        {
                            await Task.Yield();
                        }
                    }
                    //otherwise exit the current selection
                    else
                    {
                        await selectedInteractable.ExitInteractionState();
                        //SelectedInteractable?.OnHoverStateExited();
                        selectedInteractable = null;
                    }
                }
                if (nextSelectedInteractable == this)
                {
                    return;
                }

                selectedInteractable = this;
                await EnterInteractionState();

            }
            else
            {
                _ = Interact();
            }
        }

        public async Task EnterInteractionState()
        {
            //if (!CanInteract)
            if (CurrentBlockedState != 0)
                return;
            if (!SkipSelectState)
            {
                base.EnterInteractionState();

                CurrentInteractionState = EVisualScriptingInteractableState.SelectEntering;


                IEnumerable<Task> selectEnterUnitsTasks = selectEnterEventUnits.Select(async unit =>
                {
                    await unit.AwaitableTrigger(interactionScriptMachine.GetReference().AsReference(), this);
                });

                await Task.WhenAll(selectEnterUnitsTasks);

                CurrentInteractionState = EVisualScriptingInteractableState.Selected;
            }
            else
            {
                await Interact();
            }
        }

        public async Task ExitInteractionState()
        {
            //if (!CanInteract)
            if (CurrentBlockedState != 0)
                return;

            base.ExitInteractionState();

            if (!SkipSelectState)
            {
                CurrentInteractionState = EVisualScriptingInteractableState.SelectExiting;

                IEnumerable<Task> selectExitUnitsTasks = selectExitEventUnits.Select(async unit =>
                {
                    await unit.AwaitableTrigger(interactionScriptMachine.GetReference().AsReference(), this);
                });

                await Task.WhenAll(selectExitUnitsTasks);
            }

            CurrentInteractionState = EVisualScriptingInteractableState.Idle;

        }

        public async Task Interact()
        {
            //if (!CanInteract)
            if (CurrentBlockedState != 0)
                return;

            if (CurrentInteractionState != EVisualScriptingInteractableState.Selected && hasInteractState && !SkipSelectState)
                return;

            CurrentInteractionState = EVisualScriptingInteractableState.Interacting;

            IEnumerable<Task> interactUnitsTasks = interactEventUnits.Select(async unit =>
            {
                await unit.AwaitableTrigger(interactionScriptMachine.GetReference().AsReference(), this);
            });

            await Task.WhenAll(interactUnitsTasks);

            CurrentInteractionState = SkipSelectState ? EVisualScriptingInteractableState.Idle : EVisualScriptingInteractableState.Selected;

        }


#if UNITY_EDITOR

        [CustomEditor(typeof(VisualScriptingInteractable))]
        public class VisualScriptingInteractableEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                VisualScriptingInteractable interactable = (VisualScriptingInteractable)target;

                GUIStyle style = new(EditorStyles.label)
                {
                    richText = true
                };

                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

                if (Application.isPlaying)
                {
                    EditorGUILayout.LabelField($"<b>Current state:</b> {interactable.CurrentInteractionState}", style);
                    //EditorGUILayout.LabelField($"<b>Can interact:</b> {interactable.CanInteract}", style);
                }
            }
        }

#endif
    }
}
