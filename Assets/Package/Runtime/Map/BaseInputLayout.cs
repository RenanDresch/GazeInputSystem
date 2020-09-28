using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class BaseInputLayout
    {
        #region Fields

        [SerializeField]
        private List<ButtonInput> buttonMaps = new List<ButtonInput>();

        [SerializeField]
        private List<AxisInput> axisMaps = new List<AxisInput>();

        [SerializeField]
        private List<DirectionalInput> directionalMaps = new List<DirectionalInput>();

        #endregion

        #region Properties

        internal ButtonInput[] ButtonMaps => buttonMaps.ToArray();
        internal AxisInput[] AxisMaps => axisMaps.ToArray();
        internal DirectionalInput[] DirectionalMaps => directionalMaps.ToArray();


        #endregion

        #region Public Methods

        public void AddButtonInput(ButtonInput newButton)
        {
            foreach(var buttonInput in buttonMaps)
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

            buttonMaps.Add(newButton);
        }

        public void AddAxisInput(AxisInput newAxis)
        {
            foreach (var axisInput in axisMaps)
            {
                if (axisInput.InputName == newAxis.InputName)
                {
                    foreach (var newMapper in newAxis.Mappers)
                    {
                        axisInput.AddAxisMapper(newMapper);
                    }
                    return;
                }
            }

            axisMaps.Add(newAxis);
        }

        public void AddDirectionalInput(DirectionalInput newDirectional)
        {
            foreach (var directionalInput in directionalMaps)
            {
                if (directionalInput.InputName == newDirectional.InputName)
                {
                    foreach (var newMapper in newDirectional.Mappers)
                    {
                        directionalInput.AddDirectionalMapper(newMapper);
                    }
                    return;
                }
            }

            directionalMaps.Add(newDirectional);
        }

        #endregion
    }
}