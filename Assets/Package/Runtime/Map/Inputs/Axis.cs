using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class Axis
    {
        #region Fields

        public string axisName;
        public float sensitivity;
        public float deadzone;
        public float offset;

        #endregion

        #region Properties

        public float RawValue
        {
            get
            {
                return (Input.GetAxisRaw(axisName) + offset) * sensitivity;
            }
        }

        public bool State
        {
            get
            {
                return Mathf.Abs(RawValue) > deadzone;
            }
        }

        public float Value
        {
            get
            {
                return State ? RawValue : 0;
            }
        }

        #endregion
    }
}