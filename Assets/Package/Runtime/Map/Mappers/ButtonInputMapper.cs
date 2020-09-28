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
        private Axis axis = new Axis();

        private bool lastState;

        public bool state;

        #endregion

        #region Properties


        public Axis Axis => axis;

        public bool Modified
        {
            get
            {
                lastState = state;
                state = axis.State;

                return lastState != state;
            }
        }


        #endregion

        #region Public Methods

        public void Remap(Axis newAxis)
        {
            axis = newAxis;
        }

        public bool Equals(ButtonInputMapper other)
        {
            return axis == other.axis;
        }

        #endregion
    }
}