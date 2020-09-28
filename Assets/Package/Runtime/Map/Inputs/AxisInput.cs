using System.Collections.Generic;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class AxisInput : BaseInput<float>
    {
        #region Fields

        [SerializeField]
        private List<AxisInputMapper> mappers = new List<AxisInputMapper>();

        #endregion

        #region Properties

        public bool Clamped { get; set; }

        public AxisInputMapper[] Mappers => mappers.ToArray();

        #endregion

        #region Constructor

        public AxisInput(string name) : base(name) { }

        #endregion

        #region Internal Methods

        internal override void EvaluateInput()
        {
            foreach (var mapper in mappers)
            {
                if (mapper.Modified)
                {
                    if(Clamped)
                    {
                        Value = Mathf.Clamp(mapper.Value, -1, 1);
                    }
                    else
                    {
                        Value = mapper.Value;
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public void AddAxisMapper(AxisInputMapper newMapper)
        {
            foreach (var mapper in mappers)
            {
                if (newMapper == mapper)
                {
                    return;
                }
            }

            mappers.Add(newMapper);
        }

        #endregion
    }
}