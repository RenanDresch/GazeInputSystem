using System;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class Axis : IEquatable<Axis>
    {
        #region Fields

        public bool readPositive;

        public KeyCode key;
        public string axisName;
        public Func<float> floatFunction;
        public Func<bool> boolFunction;

        public float sensitivity;
        public float deadzone;
        public float offset;

        #endregion

        #region Properties

        public float RawValue
        {
            get
            {
                if (key != KeyCode.None)
                {
                    return Input.GetKey(key) ? 1 : 0;
                }
                else if(!string.IsNullOrEmpty(axisName))
                {
                    return (Input.GetAxisRaw(axisName) + offset) * sensitivity;
                }
                else if(floatFunction != null)
                {
                    if(floatFunction.Target == null)
                    {
                        floatFunction = null;
                    }
                    else
                    {
                        return floatFunction();
                    }
                }
                else if (boolFunction != null)
                {
                    if (boolFunction.Target == null)
                    {
                        boolFunction = null;
                    }
                    else
                    {
                        return boolFunction() ? 1 : 0;
                    }
                }
                return 0;
            }
        }

        public bool State
        {
            get
            {
                if(readPositive)
                {
                    return RawValue > deadzone;
                }
                else
                {
                    return RawValue < -deadzone;
                }
            }
        }

        public float Value
        {
            get
            {
                return State ? RawValue : 0;
            }
        }

        public bool Equals(Axis other)
        {
            if (key == KeyCode.None && other.key == KeyCode.None)
            {
                return axisName == other.axisName;
            }
            else if (key != KeyCode.None && other.key != KeyCode.None)
            {
                return key == other.key;
            }
            else if(floatFunction != null && other.floatFunction != null)
            {
                return floatFunction == other.floatFunction;
            }
            else if (boolFunction != null && other.boolFunction != null)
            {
                return boolFunction == other.boolFunction;
            }
            return false;
        }

        #endregion

        #region Public Methods



        #endregion
    }
}