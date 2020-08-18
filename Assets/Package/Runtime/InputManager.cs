using System.Collections.Generic;
using UnityEngine;

namespace Gaze.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        #region Fields

        private static InputManager instance;
        

        private Dictionary<string, BaseInputLayout> loadedMaps = new Dictionary<string, BaseInputLayout>();

        #endregion

        #region Properties

        public static InputManager Instance
        {
            get
            {
                if (!instance)
                {
                    instance = new GameObject("InputManager", new System.Type[] { typeof(InputManager) }).GetComponent<InputManager>();
                }
                return instance;
            }
        }

        #endregion

        #region Private Methods

        private void Update()
        {
            foreach(var map in loadedMaps)
            {
                foreach(var buttonMap in map.Value.buttons)
                {
                    buttonMap.Value.EvaluateInput();
                }
            }
        }

        #endregion

        #region Public Methods

        public void InitializeMap(BaseInputLayout inputLayout)
        {
            inputLayout.buttons = new Dictionary<string, ButtonInput>();

            foreach (var buttonMap in inputLayout.MapButtons)
            {
                inputLayout.buttons.Add(buttonMap.InputName, buttonMap);
            }

            loadedMaps.Add(inputLayout.MapName, inputLayout);
        }

        public bool GetButton(string mapName, string buttonName)
        {
            BaseInputLayout map;
            if(loadedMaps.TryGetValue(mapName, out map))
            {
                ButtonInput button;
                if(map.buttons.TryGetValue(buttonName, out button))
                {
                    return button.Value;
                }
                else
                {
                    Debug.LogError("Input Not Found!");
                }
            }
            else
            {
                Debug.LogError("Map Not Found!");
            }

            return false;
        }

        #endregion
    }
}