using UnityEngine;
using UnityEngine.Events;

namespace Gaze.InputSystem
{
    public abstract class BaseInput<T>
    {
        #region Fields

        [SerializeField]
        private string inputName;
        private UnityEvent<T> onInputValueChange;
        private T lastValue;
        private T value;

        #endregion

        #region Properties

        public string InputName => inputName;

        public UnityEvent<T> OnInputValueChange
        {
            get
            {
                if (onInputValueChange == null)
                {
                    onInputValueChange = new UnityEvent<T>();
                }
                return onInputValueChange;
            }
        }

        public T LastValue => lastValue;

        public T Value
        {
            get
            {
                return value;
            }
            internal set
            {
                if (onInputValueChange != null)
                {
                    lastValue = this.value;
                    if (!value.Equals(this.value))
                    {
                        onInputValueChange.Invoke(value);
                    }
                }
                this.value = value;
            }
        }

        #endregion

        #region Constructor

        public BaseInput(string name)
        {
            inputName = name;
        }

        #endregion

        #region Internal Methods

        internal abstract void EvaluateInput();

        #endregion
    }
}