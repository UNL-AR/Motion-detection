// Approach 1 (Without rotation offset)
/*using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;

public class MotionRecorder : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand;
    private InputDevice device;

    private List<Vector3> recordedPositions = new List<Vector3>();
    private List<Quaternion> recordedRotations = new List<Quaternion>();

    private bool isRecording = false;
    private float recordDuration = 4.0f; // Record for X seconds

    void Start()
    {
        Debug.Log("Script started.");
    }

    void Update()
    {
        // Check if device is valid
        if (!device.isValid)
        {
            device = InputDevices.GetDeviceAtXRNode(controllerNode);

            if (device.isValid)
            {
                //Print out device name
                Debug.Log("Device detected: " + device.name);
            }
            else
            {
                Debug.LogWarning("Device not yet detected, continuing to check...");
                return; // Skip the rest of Update until the device is valid
            }
        }

        // Check if 'B' button is pressed to start recording
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isBPressed) && isBPressed && !isRecording)
        {
            Debug.Log("'B' button pressed. Starting recording...");
            StartCoroutine(RecordMotion());
        }
    }

    IEnumerator RecordMotion()
    {
        // change initial false to true
        isRecording = true;

        recordedPositions.Clear();
        recordedRotations.Clear();

        float timer = 0f;

        while (timer < recordDuration)
        {
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position) &&
                device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
            {
                recordedPositions.Add(position);
                recordedRotations.Add(rotation);
            }
            else
            {
                Debug.LogWarning("Failed to retrieve position or rotation.");
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // flip recrding boolean to false after recording is done.
        isRecording = false;
        Debug.Log("Recording completed. Saving motion data...");
        SaveMotionData();
    }

    void SaveMotionData()
    {
        string path = Application.streamingAssetsPath + "/motion_data.txt";

        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("PositionX,PositionY,PositionZ,RotationX,RotationY,RotationZ,RotationW");

                for (int i = 0; i < recordedPositions.Count; i++)
                {
                    Vector3 position = recordedPositions[i];
                    Quaternion rotation = recordedRotations[i];

                    string line = $"{position.x},{position.y},{position.z},{rotation.x},{rotation.y},{rotation.z},{rotation.w}";
                    writer.WriteLine(line);
                }
            }

            Debug.Log($"Motion data saved to {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save motion data: " + e.Message);
        }
    }
}
*/


//Approach 2 (Data from PC) 
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;

public class MotionRecorder : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand;
    private InputDevice device;

    private List<Vector3> recordedPositions = new List<Vector3>();
    private List<Quaternion> recordedRotations = new List<Quaternion>();

    private bool isRecording = false;
    private float recordDuration = 4.0f; // Record for X seconds

    // Height of the cylinder object
    private float cylinderHeight;

    // Rotation offset to apply at startup
    private Quaternion rotationOffset;

    void Start()
    {
        Debug.Log("Script started.");

        // Assuming the cylinder's height is derived from its scale
        // Modify this if your cylinder's height is different
        cylinderHeight = GetComponent<Collider>().bounds.size.y;

        // Define the rotation offset (same as BatMovement script)
        rotationOffset = Quaternion.Euler(60, -10, 0);
    }

    void Update()
    {
        // Check if device is valid
        if (!device.isValid)
        {
            device = InputDevices.GetDeviceAtXRNode(controllerNode);

            if (device.isValid)
            {
                // Print out device name
                Debug.Log("Device detected: " + device.name);
            }
            else
            {
                Debug.LogWarning("Device not yet detected, continuing to check...");
                return; // Skip the rest of Update until the device is valid
            }
        }

        // Check if 'B' button is pressed to start recording
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isBPressed) && isBPressed && !isRecording)
        {
            Debug.Log("'B' button pressed. Recording...");
            StartCoroutine(RecordMotion());
        }
    }

    IEnumerator RecordMotion()
    {
        isRecording = true;

        recordedPositions.Clear();
        recordedRotations.Clear();

        float timer = 0f;

        while (timer < recordDuration)
        {
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position) &&
                device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
            {
                // Adjust the position using the cylinder height offset
                Vector3 offsetPosition = position - (-transform.up * (cylinderHeight / 2));

                // Apply the rotation offset
                Quaternion adjustedRotation = rotation * rotationOffset;

                // Record the adjusted position and rotation
                recordedPositions.Add(offsetPosition);
                recordedRotations.Add(adjustedRotation);
            }
            else
            {
                Debug.LogWarning("Failed to retrieve position or rotation.");
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isRecording = false;
        Debug.Log("Recording completed. Saving motion data...");
        SaveMotionData();
    }

    void SaveMotionData()
    {
        // Use streamingAssetsPath to save the file in the correct location
        string path = Path.Combine(Application.streamingAssetsPath, "motion_data.txt");

        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("PositionX,PositionY,PositionZ,RotationX,RotationY,RotationZ,RotationW");

                for (int i = 0; i < recordedPositions.Count; i++)
                {
                    Vector3 position = recordedPositions[i];
                    Quaternion rotation = recordedRotations[i];

                    string line = $"{position.x},{position.y},{position.z},{rotation.x},{rotation.y},{rotation.z},{rotation.w}";
                    writer.WriteLine(line);
                }
            }

            Debug.Log($"Motion data saved to {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save motion data: " + e.Message);
        }
    }

}



// Method 3 Deployed App
