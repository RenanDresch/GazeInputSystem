using Gaze.InputSystem;
using System.Collections;
using UnityEngine;

public class InputButtonListener : MonoBehaviour
{
    [SerializeField]
    private string buttonName = default;
    [SerializeField]
    private GameObject circleFill = default;

    private void Start()
    {
        InputManager.Instance.AddButtonListener(buttonName, OnButtonStateChange);
    }

    private void OnButtonStateChange(bool state)
    {
        circleFill.SetActive(state);
    }
}
