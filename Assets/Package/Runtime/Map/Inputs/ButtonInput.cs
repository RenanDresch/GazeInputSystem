using System.Collections.Generic;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class ButtonInput : BaseInput<bool>
    {
        #region Fields

        [SerializeField]
        private List<ButtonInputMapper> mappers = new List<ButtonInputMapper>();

        #endregion

        #region Properties

        public ButtonInputMapper[] Mappers => mappers.ToArray();

        #endregion

        #region Constructor

        public ButtonInput(string name) : base(name) { }

        #endregion

        #region Internal Methods

        internal override void EvaluateInput()
        {
            foreach (var mapper in mappers)
            {
                if (mapper.Modified)
                {
                    Value = mapper.state;
                }
            }
        }

        #endregion

        #region Public Methods

        public void AddButtonMapper(ButtonInputMapper newMapper)
        {
            foreach(var mapper in mappers)
            {
                if(newMapper == mapper)
                {
                    return;
                }
            }

            mappers.Add(newMapper);
        }

        #endregion
    }
}