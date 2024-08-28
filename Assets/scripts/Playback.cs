using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;


// Approach 1(Playback on A press.)
public class Playback : MonoBehaviour
{
    private List<Vector3> recordedPositions = new List<Vector3>();
    private List<Quaternion> recordedRotations = new List<Quaternion>();

    public float playbackSpeed = 1.0f; // Adjust speed of playback
    private bool isPlaying = false;
    private int currentFrame = 0;

    public XRNode controllerNode = XRNode.RightHand; // Using the right hand controller

    // Define the rotation offset to match the recording
    private Quaternion rotationOffset = Quaternion.Euler(60, -10, 0);

    void Start()
    {
        LoadMotionData();
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isAPressed) && isAPressed)
        {
            if (!isPlaying)
            {
                Debug.Log("'A' button pressed, playback started...");
                StartCoroutine(PlayMotion());
            }
        }
    }

    void LoadMotionData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "motion_data.txt");

        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                reader.ReadLine(); // Skip the header line

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    Vector3 position = new Vector3(
                        float.Parse(values[0]),
                        float.Parse(values[1]),
                        float.Parse(values[2])
                    );

                    Quaternion rotation = new Quaternion(
                        float.Parse(values[3]),
                        float.Parse(values[4]),
                        float.Parse(values[5]),
                        float.Parse(values[6])
                    );

                    recordedPositions.Add(position);
                    recordedRotations.Add(rotation);
                }
            }

            Debug.Log("Motion data loaded successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load motion data: " + e.Message);
        }
    }

    IEnumerator PlayMotion()
    {
        isPlaying = true;
        currentFrame = 0; // Start from the beginning


        while (currentFrame < recordedPositions.Count)
        {
            transform.position = recordedPositions[currentFrame];
            transform.rotation = recordedRotations[currentFrame];
            currentFrame++;
            yield return new WaitForSeconds(Time.deltaTime / playbackSpeed); // Adjust playback speed
        }

        Debug.Log("Playback completed.");
        isPlaying = false;
    }
}


// Approach 2 (for storing and accessing motion_data.txt using persistent path)

/*public class Playback : MonoBehaviour
{
    private List<Vector3> recordedPositions = new List<Vector3>();
    private List<Quaternion> recordedRotations = new List<Quaternion>();

    public float playbackSpeed = 1.0f; // Adjust speed of playback
    private bool isPlaying = false;
    private int currentFrame = 0;

    public XRNode controllerNode = XRNode.RightHand; // Using the right hand controller

    private string motionDataFilePath;

    void Start()
    {
        motionDataFilePath = Path.Combine(Application.persistentDataPath, "motion_data.txt");
        LoadMotionData();
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isAPressed) && isAPressed)
        {
            if (!isPlaying)
            {
                Debug.Log("'A' button pressed, playback started...");
                StartCoroutine(PlayMotion());
            }
        }
    }

    void LoadMotionData()
    {
        if (!File.Exists(motionDataFilePath))
        {
            Debug.LogError("Motion data file not found: " + motionDataFilePath);
            return;
        }

        try
        {
            using (StreamReader reader = new StreamReader(motionDataFilePath))
            {
                recordedPositions.Clear();
                recordedRotations.Clear();

                reader.ReadLine(); // Skip the header line

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    Vector3 position = new Vector3(
                        float.Parse(values[0]),
                        float.Parse(values[1]),
                        float.Parse(values[2])
                    );

                    Quaternion rotation = new Quaternion(
                        float.Parse(values[3]),
                        float.Parse(values[4]),
                        float.Parse(values[5]),
                        float.Parse(values[6])
                    );

                    recordedPositions.Add(position);
                    recordedRotations.Add(rotation);
                }
            }

            Debug.Log("Motion data loaded successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load motion data: " + e.Message);
        }
    }

    public void SaveMotionData(List<Vector3> positions, List<Quaternion> rotations)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(motionDataFilePath))
            {
                writer.WriteLine("posX,posY,posZ,rotX,rotY,rotZ,rotW"); // Header

                for (int i = 0; i < positions.Count; i++)
                {
                    Vector3 position = positions[i];
                    Quaternion rotation = rotations[i];

                    writer.WriteLine($"{position.x},{position.y},{position.z},{rotation.x},{rotation.y},{rotation.z},{rotation.w}");
                }
            }

            Debug.Log("Motion data saved successfully.");
            LoadMotionData(); // Load the new motion data immediately after saving
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save motion data: " + e.Message);
        }
    }

    IEnumerator PlayMotion()
    {
        isPlaying = true;
        currentFrame = 0; // Start from the beginning

        while (currentFrame < recordedPositions.Count)
        {
            transform.position = recordedPositions[currentFrame];
            transform.rotation = recordedRotations[currentFrame];
            currentFrame++;
            yield return new WaitForSeconds(Time.deltaTime / playbackSpeed); // Adjust playback speed
        }

        Debug.Log("Playback completed.");
        isPlaying = false;
    }
}*/

