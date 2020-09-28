using System;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class DirectionalInputMapper : IEquatable<DirectionalInputMapper>
    {
        #region Fields

        [SerializeField]
        private Axis positiveHorizontalReader = new Axis();

        [SerializeField]
        private Axis negativeHorizontalReader = new Axis();

        [SerializeField]
        private Axis positiveVerticalReader = new Axis();

        [SerializeField]
        private Axis negativeVerticalReader = new Axis();

        #endregion

        #region Properties

        public Vector2 LastValue { get; internal set; }
        public Vector2 Value { get; internal set; }

        public Axis PositiveHorizontalReader => positiveHorizontalReader;
        public Axis NegativeHorizontalReader => negativeHorizontalReader;
        public Axis PositiveVerticalReader => positiveVerticalReader;
        public Axis NegativeVerticalReader => negativeVerticalReader;

        public bool Modified
        {
            get
            {
                var currentValue = new Vector2(
                    Mathf.Abs(positiveHorizontalReader.Value) - Mathf.Abs(negativeHorizontalReader.Value),
                    Mathf.Abs(positiveVerticalReader.Value) - Mathf.Abs(negativeVerticalReader.Value));

                LastValue = Value;
                Value = currentValue;

                return LastValue != Value;
            }
        }

        #endregion

        #region Public Methods

        public void Remap(Axis newPositiveHorizontalReader, Axis newNegativeHorizontalReader,
            Axis newPositiveVerticalReader, Axis newNegativeVerticalReader)
        {
            positiveHorizontalReader = newPositiveHorizontalReader;
            negativeHorizontalReader = newNegativeHorizontalReader;
            positiveVerticalReader = newPositiveVerticalReader;
            negativeVerticalReader = newNegativeVerticalReader;
        }

        public bool Equals(DirectionalInputMapper other)
        {
            return (positiveHorizontalReader == other.positiveHorizontalReader) && (negativeHorizontalReader == other.negativeHorizontalReader) &&
                (positiveVerticalReader == other.positiveVerticalReader) && (negativeVerticalReader == other.negativeVerticalReader);
        }

        #endregion
    }
}