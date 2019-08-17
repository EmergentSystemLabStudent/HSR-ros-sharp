using RosSharp.RosBridgeClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSR_Change_Yoloname : MonoBehaviour
{
    private RosSocket rosSocket;
    public int UpdateTime = 0;
    public bool check_topic = false;
    public bool select_topic = false;
    StandardString changedname;
    public string ChangeYoloName;


    public bool select_one = false;

    void Start()
    {
        ChangeYoloName = "Cola";
        Invoke("Init", 1.0f);
        
    }
    public void Init()
    {
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/changed_yolo_object_name", "std_msgs/String", Change_ObjName, UpdateTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (check_topic)
        {   
            ChangeYoloName = changedname.data;
            select_topic = true;
            check_topic = false;
        }

    }

    void Change_ObjName(Message message)
    {
        changedname = (StandardString)message;
        check_topic = true;
    }
}
