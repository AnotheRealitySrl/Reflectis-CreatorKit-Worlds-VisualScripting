using Reflectis.CreatorKit.Worlds.Core.Interaction;
using Reflectis.SDK.Core.ApplicationManagement;
using Reflectis.SDK.Core.SystemFramework;

using System;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private ScriptMachine interactionScriptMachine = null;

        [SerializeField] private ScriptMachine unselectOnDestroyScriptMachine = null;

        [Header("Allowed states")]
        [SerializeField] private EAllowedVisualScriptingInteractableState desktopAllowedStates = EAllowedVisualScriptingInteractableState.Selected | EAllowedVisualScriptingInteractableState.Interacting;
        [SerializeField] private EAllowedVisualScriptingInteractableState vrAllowedStates = EAllowedVisualScriptingInteractableState.Selected | EAllowedVisualScriptingInteractableState.Interacting;
        [SerializeField] private EVRVisualScriptingInteraction vrVisualScriptingInteraction = (EVRVisualScriptingInteraction)~0;

        public Action<GameObject> OnSelectedActionVisualScripting;

        public ScriptMachine InteractionScriptMachine { get => interactionScriptMachine; set => interactionScriptMachine = value; }
        public ScriptMachine UnselectOnDestroyScriptMachine { get => unselectOnDestroyScriptMachine; set => unselectOnDestroyScriptMachine = value; }

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

        private List<UnselectOnDestroyUnit> unselectOnDestroyEventUnits = new List<UnselectOnDestroyUnit>();

        private GameObject unselectOnDestroyGameobject;
        private async void OnDestroy()
        {
            if (!IsIdleState && CurrentInteractionState != EVisualScriptingInteractableState.SelectExiting)
            {
                foreach (var unit in unselectOnDestroyEventUnits)
                {
                    await unit.AwaitableTrigger(unselectOnDestroyScriptMachine.GetReference().AsReference(), this);
                }
            }
            if (unselectOnDestroyGameobject != null)
            {
                Destroy(unselectOnDestroyGameobject);
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
                    }
                    if (unit is InteractEventUnit interactEventUnit)
                    {
                        interactEventUnits.Add(interactEventUnit);
                    }
                }
            }

            if (unselectOnDestroyScriptMachine != null)
            {
                if (unselectOnDestroyScriptMachine.gameObject == gameObject)
                {
                    Debug.LogError("Unselect on destroy script machine inserted on interactable gameObject." +
                        " This is not allowed please insert the script machine on a different empty gameobject");
                }
                else
                {
                    bool foundUnit = false;
                    foreach (var unit in unselectOnDestroyScriptMachine.graph.units)
                    {
                        if (unit is UnselectOnDestroyUnit unselectOnDestroyEventUnit)
                        {
                            unselectOnDestroyEventUnits.Add(unselectOnDestroyEventUnit);
                            foundUnit = true;
                        }
                    }
                    if (foundUnit)
                    {
                        unselectOnDestroyGameobject = unselectOnDestroyScriptMachine.gameObject;
                        unselectOnDestroyGameobject.transform.parent = null;
                        DontDestroyOnLoad(unselectOnDestroyGameobject);
                    }
                }
            }
            return Task.CompletedTask;
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
                    //EditorGUILayout.LabelField($"<b>Can interact:</b> {interactable.CanInteract}", style);
                }
            }
        }

#endif
    }
}
