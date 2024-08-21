using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class DisplayInputData : MonoBehaviour
{
    public XRController leftController;
    public XRController rightController;

    // Right controller displays
    public TextMeshProUGUI rightPositionXText;
    public TextMeshProUGUI rightPositionYText;
    public TextMeshProUGUI rightPositionZText;
    public TextMeshProUGUI rightVelocityXText;
    public TextMeshProUGUI rightVelocityYText;
    public TextMeshProUGUI rightVelocityZText;
    public TextMeshProUGUI rightRotationXText;
    public TextMeshProUGUI rightRotationYText;
    public TextMeshProUGUI rightRotationZText;

    // Left controller displays
    public TextMeshProUGUI leftPositionText;
    public TextMeshProUGUI leftVelocityText;
    public TextMeshProUGUI leftRotationText;

/*    private InputData inputData;
*/
   /* void Start()
    {
        inputData = GetComponent<InputData>();
    }*/

    void Update()
    {
        // Left controller data
        if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftPosition))
        {
            leftPositionText.text = $"Left Position: {leftPosition}";
        }

        if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftVelocity))
        {
            leftVelocityText.text = $"Left Velocity: {leftVelocity}";
        }

        if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion leftRotation))
        {
            leftRotationText.text = $"Left Rotation: {leftRotation.eulerAngles}";
        }

        // Right controller data
        if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rightPosition))
        {
            rightPositionXText.text = $"Bat Position X: {rightPosition.x} m";
            rightPositionYText.text = $"Bat Position Y: {rightPosition.y} m";
            rightPositionZText.text = $"Bat Position Z: {rightPosition.z} m";
        }

        if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVelocity))
        {
            rightVelocityXText.text = $"Bat Velocity X: {rightVelocity.x} m/s";
            rightVelocityYText.text = $"Bat Velocity Y: {rightVelocity.y} m/s";
            rightVelocityZText.text = $"Bat Velocity Z: {rightVelocity.z} m/s";
        }

        if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rightRotation))
        {
            Vector3 rightRotationEulerAngles = rightRotation.eulerAngles;
            rightRotationXText.text = $"Bat Rotation X: {rightRotationEulerAngles.x} degrees";
            rightRotationYText.text = $"Bat Rotation Y: {rightRotationEulerAngles.y} degrees";
            rightRotationZText.text = $"Bat Rotation Z: {rightRotationEulerAngles.z} degrees";
        }
    }
}
