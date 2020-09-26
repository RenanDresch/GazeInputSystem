using Gaze.InputSystem;
using UnityEngine;

public class InputMapInitializer : MonoBehaviour
{
    [SerializeField]
    private BaseInputLayoutAsset[] inputMapAssets = default;

    [ContextMenu("Testar")]
    private void Awake()
    {
        foreach (var map in inputMapAssets)
        {
            InputManager.Instance.InitializeMap(map, map.player);
        }
    }
}
