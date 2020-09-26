using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class ButtonInputMapper : IEquatable<ButtonInputMapper>
    {
        #region Fields

        [SerializeField]
        private KeyCode key;
        [SerializeField]
        private Axis axis;

        private Func<bool> function;

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
                else if(function != null)
                {
                    if (function.Target == null)
                    {
                        function = null;
                        currentState = false;
                    }
                    else
                    {
                        currentState = function();
                    }
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

        #region Public Methods

        public void Remap(KeyCode newKey)
        {
            key = newKey;
            axis = null;
            function = null;
        }

        public void Remap(Axis newAxis)
        {
            key = KeyCode.None;
            axis = newAxis;
            function = null;
        }

        public void Remap(Func<bool> newMapFunction)
        {
            key = KeyCode.None;
            axis = null;
            function = newMapFunction;
        }

        public bool Equals(ButtonInputMapper other)
        {
            if (key != KeyCode.None && other.key != KeyCode.None)
            {
                return key == other.key;
            }
            else if(axis != null && other.axis != null)
            {
                return axis == other.axis;
            }
            return false;
        }

        #endregion
    }
}