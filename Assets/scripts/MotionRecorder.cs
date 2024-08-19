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

    private float recordDuration = 3.0f; // Record for 3 seconds

    void Start()
    {
        Debug.Log("Script started.");
        device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.isValid)
        {
            Debug.Log("Device detected: " + device.name);
        }
        else
        {
            Debug.LogError("No valid device found. Ensure your controller is connected and recognized.");
        }
    }

    void Update()
    {
        // Check if the right controller is detected
        if (!device.isValid)
        {
            device = InputDevices.GetDeviceAtXRNode(controllerNode);
        }

        // Check if 'A' button is pressed to start recording
        if (device.isValid)
        {
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isAPressed))
            {
                if (isAPressed && !isRecording)
                {
                    Debug.Log("'A' button pressed. Starting recording...");
                    StartCoroutine(RecordMotion());
                }
            }
            else
            {
                Debug.LogWarning("Failed to read the 'A' button state.");
            }
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
                recordedPositions.Add(position);
                recordedRotations.Add(rotation);
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
        try
        {
            string path = Application.dataPath + "/motion_data.txt";

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
