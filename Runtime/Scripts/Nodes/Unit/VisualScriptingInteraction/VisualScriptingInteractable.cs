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
        public static List<GameObject> OnDestroyObjects = new List<GameObject>();

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
                    InteractableRef.InteractionState = IInteractable.EInteractionState.Idle;
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
            if (unselectOnDestroyScriptMachine != null && unselectOnDestroyScriptMachine.gameObject != null)
            {
                if (!IsIdleState && CurrentInteractionState != EVisualScriptingInteractableState.SelectExiting)
                {
                    try
                    {
                        OnDestroyObjects.Add(unselectOnDestroyScriptMachine.gameObject);
                        foreach (var unit in unselectOnDestroyEventUnits)
                        {
                            await unit.AwaitableTrigger(unselectOnDestroyScriptMachine.GetReference().AsReference(), this);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error while executing unselect on {unselectOnDestroyScriptMachine.gameObject}: {e} ");
                    }
                    finally
                    {
                        OnDestroyObjects.Remove(unselectOnDestroyScriptMachine.gameObject);
                    }
                }
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

        private void SetupOnDestroy(SelectExitEventUnit unselectUnit)
        {
            if (unselectOnDestroyScriptMachine == null)
            {
                GameObject go = new GameObject("UnselectOnDestroy" + gameObject.name);
                CopyVariables(go, InteractionScriptMachine.GetComponent<Variables>());
                DontDestroyOnLoad(go);
                unselectOnDestroyScriptMachine = go.AddComponent<ScriptMachine>();
                unselectOnDestroyScriptMachine.nest.SwitchToEmbed(new FlowGraph());

                //adds the CheckChangeSyncedVariable to the visual scripting change variable callback that triggers every time any variable changes.
                PropertyInfo prop = unselectOnDestroyScriptMachine.graph.GetType().GetProperty("variables");

                var current = (VariableDeclarations)prop.GetValue(unselectOnDestroyScriptMachine.graph);

                MethodInfo setter = prop.GetSetMethod(true); // `true` per ottenere anche metodi non pubblici

                var newList = new VariableDeclarations();
                foreach (var variable in InteractionScriptMachine.graph.variables)
                {
                    newList.Set(variable.name, variable.value);
                }

                setter.Invoke(unselectOnDestroyScriptMachine.graph, new object[] { newList });

            }
            SelectExitEventUnit newSelectExitUnit = CopyScriptMachineUnit(unselectOnDestroyScriptMachine, unselectUnit) as SelectExitEventUnit;
            unselectOnDestroyEventUnits.Add(newSelectExitUnit);
        }

        public IUnit CopyScriptMachineUnit(ScriptMachine scriptMachine, IUnit unit)
        {
            if (!scriptMachine.graph.units.Any(x => x.guid.Equals(unit.guid)))
            {
                IUnit newUnit = CloneUnit(unit);
                newUnit.guid = unit.guid;
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

            // Find method CopyFrom() and use it to create a copy of the original unit

            MethodInfo copyFromMethod = unitType.GetMethod("CopyFrom", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { unitType }, null);

            if (copyFromMethod != null)
            {
                try
                {
                    copyFromMethod.Invoke(clonedUnit, new object[] { originalUnit });
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to invoke Unit.CopyFrom on {unitType.FullName}: {ex}");
                }
            }
            else
            {
                Debug.LogError($"Unit.CopyFrom(Unit) method not found via reflection on type {unitType.FullName}.");
            }


            foreach (var field in unitType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (field.Name.Equals("graph"))
                {
                    continue;
                }
                field.SetValue(clonedUnit, field.GetValue(originalUnit));
            }

            foreach (var property in unitType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (property.Name.Equals("graph"))
                {
                    continue;
                }

                var originalDefaultValues = property.GetValue(originalUnit);
                MethodInfo methodInfo = property.GetSetMethod(true);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(clonedUnit, new object[] { originalDefaultValues });
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

        public override async void OnHoverStateEntered()
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

        public override async void OnHoverStateExited()
        {
            //if (!CanInteract || !hasHoveredState)
            if (CurrentBlockedState != 0 || !hasHoveredState ||
                (LockHoverDuringInteraction && currentInteractionState != EVisualScriptingInteractableState.Idle))
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

        public override async Task EnterInteractionState()
        {
            //if (!CanInteract)
            if (CurrentBlockedState != 0)
                return;
            if (!SkipSelectState)
            {
                await base.EnterInteractionState();

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

        public override async Task ExitInteractionState()
        {
            //if (!CanInteract)
            if (CurrentBlockedState != 0)
                return;

            await base.ExitInteractionState();

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
                }
            }
        }

#endif
    }
}
