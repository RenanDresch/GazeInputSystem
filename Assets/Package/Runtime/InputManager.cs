using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gaze.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        #region Fields

        private static InputManager instance;

        private Dictionary<int, BaseInputLayout> inputLayouts = new Dictionary<int, BaseInputLayout>();

        private List<TrackedBoolActions> trackedBoolActions = new List<TrackedBoolActions>();
        private List<TrackedFloatActions> trackedFloatActions = new List<TrackedFloatActions>();

        #endregion

        #region Properties

        public static InputManager Instance
        {
            get
            {
                if (!instance)
                {
                    if (Application.isPlaying)
                    {
                        instance = new GameObject("InputManager", new System.Type[] { typeof(InputManager) }).GetComponent<InputManager>();
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Private Methods

        private void Update()
        {
            foreach (var inputLayout in inputLayouts)
            {
                //Debug.Log($"Player {inputLayout.Key} map");
                EvaluateInputs(inputLayout.Value);
            }
        }

        private void EvaluateInputs(BaseInputLayout layout)
        {
            foreach (var buttonMap in layout.ButtonMaps)
            {
                //Debug.Log($"Button {buttonMap.InputName}");
                buttonMap.EvaluateInput();
            }
            foreach (var axisMap in layout.AxisMaps)
            {
                //Debug.Log($"Button {buttonMap.InputName}");
                axisMap.EvaluateInput();
            }
        }


        #endregion

        #region Public Methods

        public void InitializeMap(BaseInputLayout inputLayout, int player = 0)
        {
            if(!inputLayouts.ContainsKey(player))
            {
                inputLayouts.Add(0, new BaseInputLayout());
            }

            foreach(var buttonInput in inputLayout.ButtonMaps)
            {
                inputLayouts[player].AddButtonInput(buttonInput);
            }

            var untrack = new List<TrackedBoolActions>();

            for (int i = 0; i < trackedBoolActions.Count; i++)
            {
                var md = (MulticastDelegate)trackedBoolActions[i].Action;
                if (md.Target == null)
                {
                    untrack.Add(trackedBoolActions[i]);
                }
                else if (trackedBoolActions[i].Player == player)
                {
                    foreach (var buttonInput in inputLayouts[player].ButtonMaps)
                    {
                        if (buttonInput.InputName == trackedBoolActions[i].ButtonName)
                        {
                            buttonInput.OnInputValueChange.AddListener(trackedBoolActions[i].Action);
                            untrack.Add(trackedBoolActions[i]);
                            break;
                        }
                    }
                }
            }

            foreach(var tackable in untrack)
            {
                trackedBoolActions.Remove(tackable);
            }
        }

        public void InitializeMap(BaseInputLayoutAsset inputLayoutAsset, int player = 0)
        {
            var inputLayout = Instantiate(inputLayoutAsset).inputLayout;

            if (!inputLayouts.ContainsKey(player))
            {
                inputLayouts.Add(0, new BaseInputLayout());
            }

            foreach (var buttonInput in inputLayout.ButtonMaps)
            {
                inputLayouts[player].AddButtonInput(buttonInput);
            }

            foreach (var axisInput in inputLayout.AxisMaps)
            {
                inputLayouts[player].AddAxisInput(axisInput);
            }

            BoolTrackChecks(player);
            FloatTrackChecks(player);
        }

        public void BoolTrackChecks(int player)
        {
            var boolUntracks = new List<TrackedBoolActions>();

            for (int i = 0; i < trackedBoolActions.Count; i++)
            {
                var md = (MulticastDelegate)trackedBoolActions[i].Action;
                if (md.Target == null)
                {
                    boolUntracks.Add(trackedBoolActions[i]);
                }
                else if (trackedBoolActions[i].Player == player)
                {
                    foreach (var buttonInput in inputLayouts[player].ButtonMaps)
                    {
                        if (buttonInput.InputName == trackedBoolActions[i].ButtonName)
                        {
                            buttonInput.OnInputValueChange.AddListener(trackedBoolActions[i].Action);
                            boolUntracks.Add(trackedBoolActions[i]);
                            break;
                        }
                    }
                }
            }

            foreach (var tackable in boolUntracks)
            {
                trackedBoolActions.Remove(tackable);
            }
        }

        public void FloatTrackChecks(int player)
        {
            var floatUntracks = new List<TrackedFloatActions>();

            for (int i = 0; i < trackedFloatActions.Count; i++)
            {
                var md = (MulticastDelegate)trackedFloatActions[i].Action;
                if (md.Target == null)
                {
                    floatUntracks.Add(trackedFloatActions[i]);
                }
                else if (trackedFloatActions[i].Player == player)
                {
                    foreach (var axisInput in inputLayouts[player].AxisMaps)
                    {
                        if (axisInput.InputName == trackedFloatActions[i].AxisName)
                        {
                            axisInput.OnInputValueChange.AddListener(trackedFloatActions[i].Action);
                            floatUntracks.Add(trackedFloatActions[i]);
                            break;
                        }
                    }
                }
            }

            foreach (var tackable in floatUntracks)
            {
                trackedFloatActions.Remove(tackable);
            }
        }

        public void AddButtonListener(string buttonName, UnityAction<bool> action, int player = 0)
        {
            if (action == null)
            {
                Debug.LogError("Trying to add null action as button listener!");
                return;
            }

            var newTrackedAction = new TrackedBoolActions()
            {
                Player = player,
                ButtonName = buttonName,
                Action = action
            };

            BaseInputLayout inputLayout;
            if(inputLayouts.TryGetValue(player, out inputLayout))
            {
                foreach(var buttonInput in inputLayout.ButtonMaps)
                {
                    if(buttonInput.InputName == buttonName)
                    {
                        buttonInput.OnInputValueChange.AddListener(action);
                        return;
                    }
                }
            }

            trackedBoolActions.Add(newTrackedAction);
        }

        public bool GetButton(string buttonName, int player = 0)
        {
            BaseInputLayout inputLayout;
            if (inputLayouts.TryGetValue(player, out inputLayout))
            {
                foreach(var buttonInput in inputLayout.ButtonMaps)
                {
                    if(buttonInput.InputName == buttonName)
                    {
                        return buttonInput.Value;
                    }
                }
                Debug.LogError($"Button {buttonName} is not mapped for player {player}!");
                return false;
            }
            else
            {
                Debug.LogError("Player does not have mapped inputs!");
                return false;
            }
        }

        public bool GetButtonDown(string buttonName, int player = 0)
        {
            BaseInputLayout inputLayout;
            if (inputLayouts.TryGetValue(player, out inputLayout))
            {
                foreach (var buttonInput in inputLayout.ButtonMaps)
                {
                    if (buttonInput.InputName == buttonName)
                    {
                        return buttonInput.Value && !buttonInput.LastValue;
                    }
                }
                Debug.LogError($"Button {buttonName} is not mapped for player {player}!");
                return false;
            }
            else
            {
                Debug.LogError("Player does not have mapped inputs!");
                return false;
            }
        }

        public bool GetButtonUp(string buttonName, int player = 0)
        {
            BaseInputLayout inputLayout;
            if (inputLayouts.TryGetValue(player, out inputLayout))
            {
                foreach (var buttonInput in inputLayout.ButtonMaps)
                {
                    if (buttonInput.InputName == buttonName)
                    {
                        return !buttonInput.Value && buttonInput.LastValue;
                    }
                }
                Debug.LogError($"Button {buttonName} is not mapped for player {player}!");
                return false;
            }
            else
            {
                Debug.LogError("Player does not have mapped inputs!");
                return false;
            }
        }

        public void AddAxisListener(string axisName, UnityAction<float> action, int player = 0)
        {
            if (action == null)
            {
                Debug.LogError("Trying to add null action as button listener!");
                return;
            }

            BaseInputLayout inputLayout;
            if (inputLayouts.TryGetValue(player, out inputLayout))
            {
                foreach (var axisInput in inputLayout.AxisMaps)
                {
                    if (axisInput.InputName == axisName)
                    {
                        axisInput.OnInputValueChange.AddListener(action);
                        return;
                    }
                }
            }

            var newTrackedAction = new TrackedFloatActions()
            {
                Player = player,
                AxisName = axisName,
                Action = action
            };

            trackedFloatActions.Add(newTrackedAction);
        }

        public float GetAxis(string axisNAme, int player = 0)
        {
            BaseInputLayout inputLayout;
            if (inputLayouts.TryGetValue(player, out inputLayout))
            {
                foreach (var axisInput in inputLayout.AxisMaps)
                {
                    if (axisInput.InputName == axisNAme)
                    {
                        return axisInput.Value;
                    }
                }
                Debug.LogError($"Axis {axisNAme} is not mapped for player {player}!");
                return 0;
            }
            else
            {
                Debug.LogError("Player does not have mapped inputs!");
                return 0;
            }
        }
        
        [ContextMenu("TESTE")]
        public void GetHangingActions()
        {
            foreach(var tracked in trackedBoolActions)
            {
                Debug.Log(tracked.ButtonName);
            }
        }

        [ContextMenu("TESTE REMAP")]
        public void RemapTest()
        {
            foreach (var button in inputLayouts[0].ButtonMaps)
            {
                if (button.InputName == "Jump")
                {
                    var newAxis = new Axis()
                    {
                        boolFunction = TesteDahora
                    };
                    button.Mappers[0].Remap(newAxis);
                }
            }
        }

        private bool testeDahora;


        [ContextMenu("Teste dahora")]
        private void MudarTeste()
        {
            testeDahora = !testeDahora;
        }

        public bool TesteDahora()
        {
            return testeDahora;
        }

        #endregion

        #region Private Classes

        private class TrackedBoolActions
        {
            public int Player { get; set; }
            public string ButtonName { get; set; }
            public UnityAction<bool> Action { get; set; }
        }

        private class TrackedFloatActions
        {
            public int Player { get; set; }
            public string AxisName { get; set; }
            public UnityAction<float> Action { get; set; }
        }

        #endregion
    }
}