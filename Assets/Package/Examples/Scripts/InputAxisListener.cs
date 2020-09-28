using Gaze.InputSystem;
using UnityEngine;

public class InputAxisListener : MonoBehaviour
{
    [SerializeField]
    private string axisName = default;
    [SerializeField]
    private RectTransform arrow = default;

    private void Start()
    {
        InputManager.Instance.AddAxisListener(axisName, OnButtonStateChange);
    }

    private void OnButtonStateChange(float value)
    {
        arrow.sizeDelta = new Vector2(100, 375 * Mathf.Abs(value));
        arrow.localScale = new Vector3(1, Mathf.Sign(value), 1);
    }
}
