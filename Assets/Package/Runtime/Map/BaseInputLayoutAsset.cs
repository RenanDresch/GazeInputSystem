using UnityEngine;

namespace Gaze.InputSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Input Map", menuName = "Input/Input Map")]
    public class BaseInputLayoutAsset : ScriptableObject
    {
        public int player;
        public BaseInputLayout inputLayout;
    }
}