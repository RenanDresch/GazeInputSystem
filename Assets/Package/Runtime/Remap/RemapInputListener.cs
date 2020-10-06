using Gaze.InputSystem;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RemapInputListener : MonoBehaviour
{
    #region Fields

    private UnityEvent<Axis> mewInputAxis = new UnityEvent<Axis>();

    private List<KeyCode> availableKeys;
    private Dictionary<string, float> availableUnityAxes;
    private List<Func<bool>> availableBooleanMethods;
    private List<Func<float>> availableFloatMethods;

    private float axisDeadZone;

    #endregion

    #region Private Methods

    private void Start()
    {
        GenerateInputListeners(null);
    }

    private void Update()
    {
        ListForInputs();
    }

    public void GenerateInputListeners(UnityAction callback,
        int player = 0,
        float defaultDeadzone = 0.5f,
        bool allowKeyboard = true,
        bool allowMouseButtons = false,
        bool allowMouseMovement = false,
        IEnumerable<KeyCode> unavailableKeys = null,
        IEnumerable<string> unavailableUnityAxes = null,
        IEnumerable<Func<bool>> boolMethods = null,
        IEnumerable<Func<float>> floatMethods = null)
    {
        availableKeys = new List<KeyCode>();
        availableUnityAxes = new Dictionary<string, float>();

        if (player < 0)
        {
            player = 0;
        }

        string joystickButtonPrefix = player < 1 ? $"JoystickButton" : $"Joystick{player}Button";

        availableKeys = Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Where(key => (unavailableKeys != null) ? unavailableKeys.Contains(key) : true)
            .Where(key => (key.ToString().Contains("Mouse")) ? allowMouseButtons : true)
            .Where(key => (key.ToString().Contains("Joystick")) ? key.ToString().Contains(joystickButtonPrefix) : true)
            .Where(key => (!key.ToString().Contains("Joystick")) ? allowKeyboard : true)
            .ToList();

        string joystickAxisPrefix = $"Joystick_{player}_";

        for (int i = 1; i < 29; i++)
        {
            var uAxis = $"{joystickAxisPrefix}{i}";
            if (unavailableUnityAxes != null)
            {
                if (!unavailableUnityAxes.Contains(uAxis))
                {
                    availableUnityAxes.Add(uAxis, Input.GetAxis(uAxis));
                }
            }
            else
            {
                availableUnityAxes.Add(uAxis, Input.GetAxis(uAxis));
            }
        }

        if(allowMouseMovement)
        {
            availableUnityAxes.Add("Mouse_X", 0);
            availableUnityAxes.Add("Mouse_Y", 0);
            availableUnityAxes.Add("Mouse_Z", 0);
        }

        axisDeadZone = defaultDeadzone;

        availableBooleanMethods = boolMethods?.ToList();
        availableFloatMethods = floatMethods?.ToList();
    }

    public void ListForInputs()
    {
        foreach(var key in availableKeys)
        {
            if(Input.GetKeyDown(key))
            {
                Debug.Log(key);
            }
        }

        foreach (var axis in availableUnityAxes)
        {
            if (Input.GetAxis(axis.Key) > axis.Value + axisDeadZone)
            {
                Debug.Log($"{axis} positive!");
            }
            else if (Input.GetAxis(axis.Key) < axis.Value - axisDeadZone)
            {
                Debug.Log($"{axis} negative!");
            }
        }

        foreach(var boolMethod in availableBooleanMethods)
        {
            if(boolMethod())
            {

            }
        }

        foreach (var floaMethod in availableFloatMethods)
        {
            if (floaMethod() > axisDeadZone)
            {

            }
        }
    }

    #endregion
}
