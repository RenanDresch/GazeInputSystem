using System.Collections.Generic;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    public abstract class BaseInputLayout : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private string mapName = default;

        internal Dictionary<string, ButtonInput> buttons;

        #endregion

        #region Properties

        public abstract System.Type RealType { get; }

        public string MapName => mapName;

        #endregion
    }
}