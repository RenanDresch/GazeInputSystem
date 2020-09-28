using System;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class AxisInputMapper : IEquatable<AxisInputMapper>
    {
        #region Fields

        [SerializeField]
        private Axis positiveReader = new Axis();

        [SerializeField]
        private Axis negativeReader = new Axis();

        #endregion

        #region Properties

        public float LastValue { get; internal set; }
        public float Value { get; internal set; }

        public Axis PositiveReader => positiveReader;
        public Axis NegativeReader => negativeReader;

        public bool Modified
        {
            get
            {
                float currentValue =  Mathf.Abs(positiveReader.Value) - Mathf.Abs(negativeReader.Value);

                LastValue = Value;
                Value = currentValue;

                return LastValue != Value;
            }
        }

        #endregion

        #region Public Methods

        public void Remap(Axis newPositiveReader, Axis newNegativeReader)
        {
            positiveReader = newPositiveReader;
            negativeReader = newNegativeReader;
        }

        public bool Equals(AxisInputMapper other)
        {
            return (positiveReader == other.positiveReader) && (negativeReader == other.negativeReader);
        }

        #endregion
    }
}