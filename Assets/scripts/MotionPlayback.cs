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
}
