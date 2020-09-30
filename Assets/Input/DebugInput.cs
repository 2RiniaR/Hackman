// GENERATED AUTOMATICALLY FROM 'Assets/Input/DebugInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Hackman
{
    public class @DebugInput : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @DebugInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""DebugInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""dc2324ad-31ac-42a9-a700-98fc97459e3e"",
            ""actions"": [
                {
                    ""name"": ""MoveHorizontal"",
                    ""type"": ""Button"",
                    ""id"": ""b0f80261-2166-4fc2-9758-aa6dac1dab50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveVertical"",
                    ""type"": ""Button"",
                    ""id"": ""ae23a960-f04d-48eb-a2a8-e56a8db07046"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Axis"",
                    ""id"": ""bda56177-a89e-4856-872d-23123507e77d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveHorizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0fa02763-88bf-450d-90d5-da59416ce810"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""67f22011-33b0-4eb4-947f-9405a6e43c0f"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Axis"",
                    ""id"": ""60a78592-618c-4a30-84b4-410a33da7a6f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveVertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9403cb29-18dc-464d-ac02-d4c75deb50c9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""64b13f9f-8d73-44f0-84c7-ed18224f8e37"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_MoveHorizontal = m_Player.FindAction("MoveHorizontal", throwIfNotFound: true);
            m_Player_MoveVertical = m_Player.FindAction("MoveVertical", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_MoveHorizontal;
        private readonly InputAction m_Player_MoveVertical;
        public struct PlayerActions
        {
            private @DebugInput m_Wrapper;
            public PlayerActions(@DebugInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveHorizontal => m_Wrapper.m_Player_MoveHorizontal;
            public InputAction @MoveVertical => m_Wrapper.m_Player_MoveVertical;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @MoveHorizontal.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveHorizontal;
                    @MoveHorizontal.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveHorizontal;
                    @MoveHorizontal.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveHorizontal;
                    @MoveVertical.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveVertical;
                    @MoveVertical.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveVertical;
                    @MoveVertical.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveVertical;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MoveHorizontal.started += instance.OnMoveHorizontal;
                    @MoveHorizontal.performed += instance.OnMoveHorizontal;
                    @MoveHorizontal.canceled += instance.OnMoveHorizontal;
                    @MoveVertical.started += instance.OnMoveVertical;
                    @MoveVertical.performed += instance.OnMoveVertical;
                    @MoveVertical.canceled += instance.OnMoveVertical;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        public interface IPlayerActions
        {
            void OnMoveHorizontal(InputAction.CallbackContext context);
            void OnMoveVertical(InputAction.CallbackContext context);
        }
    }
}
