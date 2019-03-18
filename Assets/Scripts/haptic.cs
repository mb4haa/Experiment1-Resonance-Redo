using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;


public class haptic : MonoBehaviour
{
    //private Valve.VR.EVRButtonId triggeredButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_TrackedObject trackedObject;
    private Hand _hand;
    System.Text.StringBuilder csv = new System.Text.StringBuilder();
    private float targetTime;
    private bool flag = false;
    public static double unix;

    private Player player;

    private SteamVR_Controller.Device device;
    private void Awake()
    {
        csv.AppendLine("CHOSEN_POSITION_X ,CHOSEN_POSITION_Y ,CHOSEN_POSITION_Z ,ACTUAL_POSITION_X ,ACTUAL_POSITION_Y ,ACTUAL_POSITION_Z , CHOSENPOINT_DISTANCE_FROM_ACTUAL , X_DISTANCE_FROM_ACTUAL, Y_DISTANCE_FROM_ACTUAL, Z_DISTANCE_FROM_ACTUAL, ID_AUDIOSOURCE , FRAME ,UNIX TIME of actualPoint, Unix time of chosen , time taken to click , height,headPosX,headPosY");

    }
    // Use this for initialization
    void Start()
    {
        targetTime = 5.0f;
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        // Get the hand componenet
        _hand = GetComponent<Hand>();


        // Set the controller reference
        //  device = _hand.controller;
        System.IO.FileInfo fi = new System.IO.FileInfo(@"D:\\3Levelhaptic.csv");
        if (File.Exists(fi.ToString()))
        {
            fi.Delete();
        }

        //  ThreeLevelHorizontal.startTheGame();

    }

    // Update is called once per frame
    void LateUpdate()
    {



        // Debug.Log(targetTime);



        if (_hand != null)
        {
            device = _hand.controller;

            //Debug.Log(_hand.transform.position);
            //Debug.Log("controller is"+_hand.controller);
            if (device != null)
            {

                //Debug.Log(Player.instance.hmdTransform.position.y);
                // Player.instance.leftController.TriggerHapticPulse(800);
                //_hand.controller.TriggerHapticPulse(3999);

                if (ThreeLevelHorizontal.actualPoint != null)
                {
                    if (CalculateDistance(ThreeLevelHorizontal.actualPoint.transform, this.transform) <= 0.25)
                    {
                        Debug.Log("Inside the distance");
                        Debug.Log(CalculateDistance(ThreeLevelHorizontal.actualPoint.transform, this.transform));
                        _hand.controller.TriggerHapticPulse(800);
                        //  Player.instance.rightController.TriggerHapticPulse(800);
                        //   Player.instance.leftController.TriggerHapticPulse(800);
                    }
                    else if (CalculateDistance(ThreeLevelHorizontal.actualPoint.transform, this.transform) <= 0.4)
                    {
                        Debug.Log("Inside the distance");
                        Debug.Log(CalculateDistance(ThreeLevelHorizontal.actualPoint.transform, this.transform));
                        _hand.controller.TriggerHapticPulse(300);
                        //  Player.instance.rightController.TriggerHapticPulse(800);
                        //   Player.instance.leftController.TriggerHapticPulse(800);
                    }
                    else if (CalculateDistance(ThreeLevelHorizontal.actualPoint.transform, this.transform) <= 0.6)
                    {
                        Debug.Log("Inside the distance");
                        Debug.Log(CalculateDistance(ThreeLevelHorizontal.actualPoint.transform, this.transform));
                        _hand.controller.TriggerHapticPulse(100);
                        //  Player.instance.rightController.TriggerHapticPulse(800);
                        //   Player.instance.leftController.TriggerHapticPulse(800);
                    }
                }


                if (_hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    //Debug.Log("im being pressed");
                    //WRITE TO CSV FILE THE FOLLOWING:
                    //- PosX, PosY, PosZ, frame, and Unixtime, and id of the soundPlayed during that time
                    Debug.Log(this.transform.position);

                    if (ThreeLevelHorizontal.actualPoint != null)
                    {


                        //  

                        if (ThreeLevelHorizontal.actualPoint.GetComponent<AudioSource>().isPlaying)
                        {
                            ThreeLevelHorizontal.actualPoint.GetComponent<AudioSource>().Stop();
                        }
                        unix = calculateUnixTime();
                        targetTime = Time.time;
                        flag = true;

                        csv.AppendLine(this.transform.position.x + " ," + this.transform.position.y + " ," + this.transform.position.z + " ," + ThreeLevelHorizontal.actualPoint.transform.position.x + " ," + ThreeLevelHorizontal.actualPoint.transform.position.y + ", " + ThreeLevelHorizontal.actualPoint.transform.position.z + " ," + CalculateDistance(ThreeLevelHorizontal.actualPoint.transform, this.transform) + " ," + CalculateDistanceX(ThreeLevelHorizontal.actualPoint.transform, this.transform) + " ," + CalculateDistanceY(ThreeLevelHorizontal.actualPoint.transform, this.transform) + "," + CalculateDistanceZ(ThreeLevelHorizontal.actualPoint.transform, this.transform) + ", " + ThreeLevelHorizontal.actualPoint.GetInstanceID() + " ," + ThreeLevelHorizontal.frame + " ," + ThreeLevelHorizontal.unix + "," + unix + "," + calculateTime(ThreeLevelHorizontal.unix, unix) + " ," + Player.instance.hmdTransform.position.y + " ," + Player.instance.hmdTransform.position.x + " ," + Player.instance.hmdTransform.position.z);
                        // Debug.Log(targetTime);




                        ////Debug.Log("current position" + testScript.actualPoint.transform.position);
                        //targetTime -= Time.deltaTime;

                        ////csv.AppendLine(this.transform.position+"");
                        //ThreeLevelHorizontal.actualPoint.GetComponent<AudioSource>().Stop();
                        //if (targetTime <= 0)
                        //{
                        //    timeEnded();
                        //}
                    }


                }
                if (targetTime + 3 <= Time.time && flag)
                {
                    flag = false;
                    Debug.Log("after 5gadgashjskaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                    ThreeLevelHorizontal.startTheGame();
                }


            }
        }



        //device = SteamVR_Controller.Input((int)trackedObject.index);
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        //{
        //    Debug.Log("im being pressed");
        //    Debug.Log(this.transform.position);
        //}




        ///if(Input.GetKeyUp(KeyCode.PageUp))
        //{
        //     Debug.Log(this.transform.position);
        //}
    }

    private double calculateTime(double a, double b)
    {
        return b - a;
    }
    private float CalculateDistance(Transform a, Transform b)
    {
        return
  Mathf.Sqrt((a.position.x - b.position.x) * (a.position.x - b.position.x)
            + (a.position.y - b.position.y) * (a.position.y - b.position.y) +
            (a.position.z - b.position.z) * (a.position.z - b.position.z));

    }

    private float CalculateDistanceX(Transform a, Transform b)
    {
        return
  Mathf.Sqrt((a.position.x - b.position.x) * (a.position.x - b.position.x));


    }
    private float CalculateDistanceY(Transform a, Transform b)
    {
        return
  Mathf.Sqrt(
            +(a.position.y - b.position.y) * (a.position.y - b.position.y)
            );

    }
    private float CalculateDistanceZ(Transform a, Transform b)
    {
        return
  Mathf.Sqrt(
            (a.position.z - b.position.z) * (a.position.z - b.position.z));

    }

    private double calculateUnixTime()
    {
        System.DateTime dateTime = new System.DateTime(System.DateTime.UtcNow.Year, System.DateTime.UtcNow.Month, System.DateTime.UtcNow.Day, System.DateTime.UtcNow.Hour, System.DateTime.UtcNow.Minute, System.DateTime.UtcNow.Second);
        System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (dateTime.ToUniversalTime() - epoch).TotalSeconds;
    }
    private void timeEnded()
    {
        // flag = false;
        Debug.Log("time after 5 sec");
        // ThreeLevelHorizontal.startTheGame();
        targetTime = 5.0f;

        // Debug.Log("im here");
        //    ThreeLevelHorizontal.startTheGame();
    }
    private void OnDestroy()
    {
        string csvpath = "D:\\3Levelhaptic.csv";
        File.AppendAllText(csvpath, csv.ToString());
    }
}
