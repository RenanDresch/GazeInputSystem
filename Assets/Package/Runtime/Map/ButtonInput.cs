namespace Gaze.InputSystem
{
    [System.Serializable]
    public class ButtonInput : BaseInput<bool>
    {
        #region Fields

        [UnityEngine.SerializeField]
        private ButtonInputMapper[] mappers;

        #endregion

        #region Constructor

        public ButtonInput(string name) : base(name) { }

        #endregion

        #region Internal Methods

        internal override void EvaluateInput()
        {
            foreach(var mapper in mappers)
            {
                if(mapper.Modified)
                {
                    Value = mapper.state;
                }
            }
        }

        #endregion
    }
}