using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class ButtonInputMapper
    {
        #region Fields

        [SerializeField]
        private KeyCode key;
        [SerializeField]
        private Axis axis;

        private bool lastState;

        public bool state;

        #endregion

        #region Properties

        public KeyCode Key
        {
            get
            {
                return key;
            }
            set
            {
                if(value != KeyCode.None)
                {
                    axis = null;
                }
                key = value;
            }
        }

        public Axis Axis
        {
            get
            {
                return axis;
            }
            set
            {
                if(value != null)
                {
                    key = KeyCode.None;
                }
                axis = value;
            }
        }

        public bool Modified
        {
            get
            {
                bool currentState;
                if (key != KeyCode.None)
                {
                    currentState = Input.GetKey(key);
                }
                else if(axis != null)
                {
                    currentState = axis.State;
                }
                else
                {
                    currentState = false;
                }

                lastState = state;
                state = currentState;

                return lastState != state;
            }
        }

        #endregion
    }
}