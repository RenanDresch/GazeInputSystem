using Gaze.InputSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "Sample Input Map", menuName = "Input/Sample Map")]
public class SampleInputLayout : BaseInputLayout
{
    public override System.Type RealType => typeof(SampleInputLayout);

    [SerializeField]
    private ButtonInput sampleButton = default;
}

