using System.Collections.Generic;
using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Input Map", menuName = "Input/Input Map")]
    public class BaseInputLayout : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private string mapName = default;

        [SerializeField]
        private ButtonInput[] mapButtons = default;

        [SerializeField]
        private ButtonInput[] mapAxes = default;

        [SerializeField]
        private ButtonInput[] mapDirectionals = default;

        internal Dictionary<string, ButtonInput> buttons;
        internal Dictionary<string, ButtonInput> axis;
        internal Dictionary<string, ButtonInput> directionals;

        #endregion

        #region Properties

        public string MapName => mapName;

        internal ButtonInput[] MapButtons => mapButtons;
        internal ButtonInput[] MapAxes => mapAxes;
        internal ButtonInput[] MapDirectionals => mapDirectionals;


        #endregion
    }
}