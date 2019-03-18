using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ThreeLevelHorizontal_freq : MonoBehaviour
{

    public GameObject prefab;
    public GameObject prefab_high;
    public GameObject prefab_low;
    public static int numberOfObjects = 5;
    public static float radius = 0.45f;
    private static GameObject[] audioArray = new GameObject[15];
    private static int[] audioTrack = new int[15];
    private static int randomIndex;
    private static float yDistance = 1.5f;
    private static double arcLine = Math.Round(2 * radius * Mathf.Sin((36 * Mathf.PI) / 180), 2);
    private static double diagonalLine = Math.Round(Math.Sqrt(1.5 * 1.5 + arcLine * arcLine), 2);
    private static GameObject previousPoint;
    public static GameObject currentPoint;
    public static GameObject actualPoint;
    public static int frame;
    public static double unix;
    public static Color color;
    public static int finished;
    public float targetTime;
    public static int error=0;
    public static bool f=true;
    public float min;


    void Start()
    {
	error=0;
        int counter = 0;
        targetTime = 10.0f;
        finished = 0;
        //yDistance = 0.7F;
        GameObject gg;
	float dd=min+(yDistance*3);
        for (float y = min; y < dd; y += yDistance)

            for (int i = 0; i < numberOfObjects; i++)
            {
                float angle = i * Mathf.PI * 2 / numberOfObjects;
                Vector3 pos = new Vector3(Mathf.Cos(angle), y, Mathf.Sin(angle)) * radius;
                if (counter > 9)
                    gg = (GameObject)Instantiate(prefab_high, pos, Quaternion.identity);
                else if (counter > 4)
                    gg = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
                else
                    gg = (GameObject)Instantiate(prefab_low, pos, Quaternion.identity);
                audioArray[counter] = gg;
                counter++;
            }
        color = audioArray[0].GetComponent<Renderer>().material.color;




        // Debug.Log(arcLine);
        //Debug.Log(diagonalLine);


    }

    void LateUpdate()
    {

        //   Debug.Log(currentPoint.transform.position);
       
            
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //   Debug.Log("diagonal" + Math.Round(diagonalLine,2));
            //  Debug.Log("arc" + arcLine);
              startTheGame();



        }
    }
   

    private static float CalculateDistance(GameObject a, GameObject b)
    {
        return
  Mathf.Sqrt((a.transform.position.x - b.transform.position.x) * (a.transform.position.x - b.transform.position.x)
            + (a.transform.position.y - b.transform.position.y) * (a.transform.position.y - b.transform.position.y) +
            (a.transform.position.z - b.transform.position.z) * (a.transform.position.z - b.transform.position.z));

    }

    //- Position X, Y, Z, ID, Frame, Unix Time.

    private static double calculateUnixTime()
    {
        System.DateTime dateTime = new System.DateTime(System.DateTime.UtcNow.Year, System.DateTime.UtcNow.Month, System.DateTime.UtcNow.Day, System.DateTime.UtcNow.Hour, System.DateTime.UtcNow.Minute, System.DateTime.UtcNow.Second);
        System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (dateTime.ToUniversalTime() - epoch).TotalSeconds;
    }

public static void startTheGame()
    {
        Debug.Log("the flag" + f);
        randomIndex = UnityEngine.Random.Range(0, 15);
        currentPoint = audioArray[randomIndex];
        if (previousPoint != null)

        {

            while (f)

            {
                audioTrack[randomIndex]++;
                Debug.Log("random index before if" + randomIndex);
                // Debug.Log("arcline point disct" + Math.Round(CalculateDistance(previousPoint, currentPoint), 2));
                //  Debug.Log("diaglonal point dis" + Math.Round(CalculateDistance(previousPoint, currentPoint), 2));
                if (!((previousPoint.transform.position.x == currentPoint.transform.position.x && previousPoint.transform.position.z == currentPoint.transform.position.z)
                                        && Math.Round(Mathf.Abs(previousPoint.transform.position.y - currentPoint.transform.position.y), 1) == 0.7
                                        || previousPoint.transform.position == currentPoint.transform.position ||
                                           Math.Round(CalculateDistance(previousPoint, currentPoint), 2) == arcLine || Math.Round(CalculateDistance(previousPoint, currentPoint), 2) == diagonalLine || audioTrack[randomIndex] == 3))


                {
                    error = 0;
                    Debug.Log("random index after if" + randomIndex);
                    currentPoint.GetComponent<Renderer>().material.color = new Color32(1, 1, 1, 1);
                    currentPoint.GetComponent<AudioSource>().Play();
                    actualPoint = currentPoint;
                    // Debug.Log("test script" + currentPoint.transform.position);
                    // Debug.Log(audioTrack[randomIndex]);
                    frame = Time.frameCount;
                    unix = calculateUnixTime();
                    if (audioTrack[randomIndex] == 2)
                        finished++;
                    break;

                }





                audioTrack[randomIndex]--;
                randomIndex = UnityEngine.Random.Range(0, 15);
                currentPoint = audioArray[randomIndex];
                error++;
                Debug.Log("error" + error);


                if (error > 10)
                {
                    Debug.Log("errir>15");
                    for (int i = 0; i < audioTrack.Length; i++)
                    {
                        if (audioTrack[i] < 2)
                        {
                            Debug.Log(audioTrack[i]+"the track which is available");
                            currentPoint = audioArray[i];
                            Debug.Log(audioArray[i] + "the thing");
                            randomIndex = i;
                            break;

                        }
                    }
                    audioTrack[randomIndex]++;
                    error = 0;
                    Debug.Log("In the error if");
                    currentPoint.GetComponent<Renderer>().material.color = new Color32(1, 1, 1, 1);
                    currentPoint.GetComponent<AudioSource>().Play();
                    actualPoint = currentPoint;
                    // Debug.Log("test script" + currentPoint.transform.position);
                    // Debug.Log(audioTrack[randomIndex]);
                    frame = Time.frameCount;
                    unix = calculateUnixTime();
                    if (audioTrack[randomIndex] == 2)
                        finished++;
                    break;
                }
                

                if (error > 30)
                {
                    f = false;
                }
            }






        }
        else
        {
            audioTrack[randomIndex]++;
            currentPoint.GetComponent<Renderer>().material.color = new Color32(1, 1, 1, 1);
            currentPoint.GetComponent<AudioSource>().Play();
            actualPoint = currentPoint;
            frame = Time.frameCount;
            unix = calculateUnixTime();
            //  Debug.Log(currentPoint.transform.position);

        }
        if (previousPoint)
        {
            // Debug.Log("old color is " + previousPoint.GetComponent<Renderer>().material.color);
            previousPoint.GetComponent<Renderer>().material.color = color;
            // Debug.Log("now color is " + color);
        }
        for (int i = 0; i < audioTrack.Length; i++)
        {



            Debug.Log(audioTrack[i]);
        }
        Debug.Log(finished + "finished");
        if (finished == 15)
        {

            Debug.Log("the experiment is done");
            audioTrack[1] = 0;

        }
        previousPoint = currentPoint;
        // Debug.Log(currentPoint.transform.position);
        // Debug.Log(audioArray[randomIndex].transform.position);
    }



}
