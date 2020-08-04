using Gaze.InputSystem;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField]
    private BaseInputLayout inputMap = default;

    [SerializeField]
    private Vector3 direction = default;

    [SerializeField]
    private float value = default;

    [SerializeField]
    private bool state = default;

    private void Awake()
    {
        InputManager.Instance.InitializeMap(inputMap);
    }

    private void Update()
    {
        state = InputManager.Instance.GetButton("Sample Map", "Sample Buttom");
    }
}
