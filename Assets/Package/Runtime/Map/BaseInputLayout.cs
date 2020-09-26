using System.Collections.Generic;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class BaseInputLayout
    {
        #region Fields

        [SerializeField]
        private List<ButtonInput> mapButtons = new List<ButtonInput>();

        [SerializeField]
        private ButtonInput[] mapAxes = default;

        [SerializeField]
        private ButtonInput[] mapDirectionals = default;

        #endregion

        #region Properties

        internal ButtonInput[] MapButtons => mapButtons.ToArray();
        internal ButtonInput[] MapAxes => mapAxes;
        internal ButtonInput[] MapDirectionals => mapDirectionals;


        #endregion

        #region Public Methods

        public void AddButtonInput(ButtonInput newButton)
        {
            foreach(var buttonInput in mapButtons)
            {
                if(buttonInput.InputName == newButton.InputName)
                {
                    foreach (var newMapper in newButton.Mappers)
                    {
                        buttonInput.AddButtonMapper(newMapper);
                    }
                    return;
                }
            }

            mapButtons.Add(newButton);
        }

        public void DisposeDirtyActions()
        {
            foreach (var buttonInput in mapButtons)
            {
                buttonInput.OnInputValueChange.GetPersistentEventCount();
            }
        }

        #endregion
    }
}