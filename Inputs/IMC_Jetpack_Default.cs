//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/INPUT/IMC_Jetpack_Default.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @IMC_Jetpack_Default: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @IMC_Jetpack_Default()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""IMC_Jetpack_Default"",
    ""maps"": [
        {
            ""name"": ""DefaultInput"",
            ""id"": ""5c26ad9b-8283-464f-bf6a-4901bec61c36"",
            ""actions"": [
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""035ee3f0-35ba-44a6-994b-6d7e9a1501f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fly"",
                    ""type"": ""Button"",
                    ""id"": ""e3ff9668-27e6-48f2-b07c-7fbfa684f4ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7ecff893-10f3-49b7-83dd-2564c265fdb1"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f88436c5-606d-44b2-8e16-71f63cd5778d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fly"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04b7a766-97d0-44ee-bb76-416cb88509f6"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fly"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // DefaultInput
        m_DefaultInput = asset.FindActionMap("DefaultInput", throwIfNotFound: true);
        m_DefaultInput_Run = m_DefaultInput.FindAction("Run", throwIfNotFound: true);
        m_DefaultInput_Fly = m_DefaultInput.FindAction("Fly", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // DefaultInput
    private readonly InputActionMap m_DefaultInput;
    private List<IDefaultInputActions> m_DefaultInputActionsCallbackInterfaces = new List<IDefaultInputActions>();
    private readonly InputAction m_DefaultInput_Run;
    private readonly InputAction m_DefaultInput_Fly;
    public struct DefaultInputActions
    {
        private @IMC_Jetpack_Default m_Wrapper;
        public DefaultInputActions(@IMC_Jetpack_Default wrapper) { m_Wrapper = wrapper; }
        public InputAction @Run => m_Wrapper.m_DefaultInput_Run;
        public InputAction @Fly => m_Wrapper.m_DefaultInput_Fly;
        public InputActionMap Get() { return m_Wrapper.m_DefaultInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultInputActions set) { return set.Get(); }
        public void AddCallbacks(IDefaultInputActions instance)
        {
            if (instance == null || m_Wrapper.m_DefaultInputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DefaultInputActionsCallbackInterfaces.Add(instance);
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Fly.started += instance.OnFly;
            @Fly.performed += instance.OnFly;
            @Fly.canceled += instance.OnFly;
        }

        private void UnregisterCallbacks(IDefaultInputActions instance)
        {
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Fly.started -= instance.OnFly;
            @Fly.performed -= instance.OnFly;
            @Fly.canceled -= instance.OnFly;
        }

        public void RemoveCallbacks(IDefaultInputActions instance)
        {
            if (m_Wrapper.m_DefaultInputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDefaultInputActions instance)
        {
            foreach (var item in m_Wrapper.m_DefaultInputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DefaultInputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DefaultInputActions @DefaultInput => new DefaultInputActions(this);
    public interface IDefaultInputActions
    {
        void OnRun(InputAction.CallbackContext context);
        void OnFly(InputAction.CallbackContext context);
    }
}