using UnityEngine;
using UnityEngine.XR;

public class BatMovement : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand; // Using the right controller
    private Vector3 controllerPosition;
    private Quaternion controllerRotation;

    // Height of the cylinder object
    private float cylinderHeight;

    // Rotation offset to apply at startup
    private Quaternion rotationOffset;

    void Start()
    {
        // Assuming the cylinder's height is derived from its scale
        // Modify this if your cylinder's height is different
        cylinderHeight = GetComponent<Collider>().bounds.size.y;

        // Define the rotation offset (60 degrees around the X axis)
        rotationOffset = Quaternion.Euler(60, -10, 0);
        
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out controllerPosition) &&
            device.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerRotation))
        {
            UpdateCylinderTransform();
        }
    }

    void UpdateCylinderTransform()
    {
        // Offset the position to place the controller at the base of the cylinder
        Vector3 offsetPosition = controllerPosition - (-transform.up * (cylinderHeight / 2));

        // Apply the position to the cylinder
        transform.position = offsetPosition;

        // Apply the rotation offset and the controller's rotation to the cylinder
        transform.rotation = controllerRotation * rotationOffset;
    }
}
