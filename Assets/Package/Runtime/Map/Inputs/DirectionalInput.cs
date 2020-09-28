using System.Collections.Generic;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public class DirectionalInput : BaseInput<Vector2>
    {
        #region Fields

        [SerializeField]
        private List<DirectionalInputMapper> mappers = new List<DirectionalInputMapper>();

        #endregion

        #region Properties

        public bool Clamped { get; set; }

        public DirectionalInputMapper[] Mappers => mappers.ToArray();

        #endregion

        #region Constructor

        public DirectionalInput(string name) : base(name) { }

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
                        Value = Vector2.ClampMagnitude(mapper.Value, 1);
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

        public void AddDirectionalMapper(DirectionalInputMapper newMapper)
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