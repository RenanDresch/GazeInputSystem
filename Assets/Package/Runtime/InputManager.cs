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
                EvaluateButtons(inputLayout.Value);
            }
        }

        private void EvaluateButtons(BaseInputLayout layout)
        {
            foreach (var buttonMap in layout.MapButtons)
            {
                //Debug.Log($"Button {buttonMap.InputName}");
                buttonMap.EvaluateInput();
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

            foreach(var buttonInput in inputLayout.MapButtons)
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
                    foreach (var buttonInput in inputLayouts[player].MapButtons)
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

            foreach (var buttonInput in inputLayout.MapButtons)
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
                    foreach (var buttonInput in inputLayouts[player].MapButtons)
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

            foreach (var tackable in untrack)
            {
                trackedBoolActions.Remove(tackable);
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
                foreach(var buttonInput in inputLayout.MapButtons)
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
                foreach(var buttonInput in inputLayout.MapButtons)
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
            foreach (var button in inputLayouts[0].MapButtons)
            {
                if (button.InputName == "Jump")
                {
                    button.Mappers[0].Remap(TesteDahora);
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

        #endregion
    }
}