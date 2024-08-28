/*// Approach 1
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MotionPlayback : MonoBehaviour
{
    private List<Vector3> recordedPositions = new List<Vector3>();
    private List<Quaternion> recordedRotations = new List<Quaternion>();

    public float playbackSpeed = 1.0f; // Adjust speed of playback

    void Start()
    {
        LoadMotionData();
        StartCoroutine(PlayMotion());
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
        for (int i = 0; i < recordedPositions.Count; i++)
        {
            transform.position = recordedPositions[i];
            transform.rotation = recordedRotations[i];

            yield return new WaitForSeconds(Time.deltaTime / playbackSpeed); // Adjust playback speed
        }

        Debug.Log("Playback completed.");
    }
}*/



// Approach 2

/*using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;

public class MotionPlayback : MonoBehaviour
{
    private List<Vector3> recordedPositions = new List<Vector3>();
    private List<Quaternion> recordedRotations = new List<Quaternion>();

    public float playbackSpeed = 1.0f; // Adjust speed of playback
    private bool isPlaying = false;
    private bool isPaused = false;
    private bool isDestroyed = false;
    private int currentFrame = 0;

    public XRNode controllerNode = XRNode.RightHand;
    private float buttonPressDuration = 0f;
    private float holdTimeThreshold = 3.0f;

    void Start()
    {
        // Set object inactive by default
        gameObject.SetActive(false);
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
        {
            if (isPressed)
            {
                buttonPressDuration += Time.deltaTime;

                if (buttonPressDuration >= holdTimeThreshold)
                {
                    if (!isDestroyed)
                    {
                        Destroy(gameObject);
                        isDestroyed = true;
                        buttonPressDuration = 0f;
                        Debug.Log("Object destroyed.");
                    }
                }
            }
            else
            {
                if (buttonPressDuration > 0 && buttonPressDuration < holdTimeThreshold)
                {
                    buttonPressDuration = 0f;

                    if (isDestroyed)
                    {
                        RecreateObject();
                    }
                    else if (!isPlaying)
                    {
                        ActivateAndPlay();
                        Debug.Log("'A' pressed, making object active!!");
                    }
                    else
                    {
                        TogglePause();
                    }
                }
            }
        }
    }

    void RecreateObject()
    {
        GameObject newObject = Instantiate(gameObject);
        newObject.SetActive(true);
        newObject.GetComponent<MotionPlayback>().ActivateAndPlay();
        Debug.Log("Object recreated and playback started.");
        isDestroyed = false;
    }

    void ActivateAndPlay()
    {
        gameObject.SetActive(true);
        LoadMotionData();
        StartCoroutine(PlayMotion());
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log(isPaused ? "Playback paused." : "Playback resumed.");
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
        while (currentFrame < recordedPositions.Count)
        {
            if (!isPaused)
            {
                transform.position = recordedPositions[currentFrame];
                transform.rotation = recordedRotations[currentFrame];
                currentFrame++;
            }
            yield return new WaitForSeconds(Time.deltaTime / playbackSpeed); // Adjust playback speed
        }
        Debug.Log("Playback completed.");
        isPlaying = false;
    }
}*/



using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;

public class MotionPlayback : MonoBehaviour
{
    private List<Vector3> recordedPositions = new List<Vector3>();
    private List<Quaternion> recordedRotations = new List<Quaternion>();

    public float playbackSpeed = 1.0f; // Adjust speed of playback
    private bool isPlaying = false;
    private bool isPaused = false;
    private bool isDestroyed = false;
    private int currentFrame = 0;

    public XRNode controllerNode = XRNode.LeftHand; // Changed to left hand controller
    private float buttonPressDuration = 0f;
    private float holdTimeThreshold = 3.0f;

    void Start()
    {
        // Set object inactive by default
        gameObject.SetActive(false);
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed)) // X button on left controller
        {
            if (isPressed)
            {
                buttonPressDuration += Time.deltaTime;

                if (buttonPressDuration >= holdTimeThreshold)
                {
                    if (!isDestroyed)
                    {
                        Destroy(gameObject);
                        isDestroyed = true;
                        buttonPressDuration = 0f;
                        Debug.Log("Object destroyed.");
                    }
                }
            }
            else
            {
                if (buttonPressDuration > 0 && buttonPressDuration < holdTimeThreshold)
                {
                    buttonPressDuration = 0f;

                    if (isDestroyed)
                    {
                        RecreateObject();
                    }
                    else if (!isPlaying)
                    {
                        ActivateAndPlay();
                        Debug.Log("'X' pressed, making object active!!");
                    }
                    else
                    {
                        TogglePause();
                    }
                }
            }
        }
    }

    void RecreateObject()
    {
        GameObject newObject = Instantiate(gameObject);
        newObject.SetActive(true);
        newObject.GetComponent<MotionPlayback>().ActivateAndPlay();
        Debug.Log("Object recreated and playback started.");
        isDestroyed = false;
    }

    void ActivateAndPlay()
    {
        gameObject.SetActive(true);
        LoadMotionData();
        StartCoroutine(PlayMotion());
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log(isPaused ? "Playback paused." : "Playback resumed.");
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
        while (currentFrame < recordedPositions.Count)
        {
            if (!isPaused)
            {
                transform.position = recordedPositions[currentFrame];
                transform.rotation = recordedRotations[currentFrame];
                currentFrame++;
            }
            yield return new WaitForSeconds(Time.deltaTime / playbackSpeed); // Adjust playback speed
        }
        Debug.Log("Playback completed.");
        isPlaying = false;
    }
}
