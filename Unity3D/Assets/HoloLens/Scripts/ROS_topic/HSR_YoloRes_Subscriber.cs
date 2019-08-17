using RosSharp.RosBridgeClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSR_YoloRes_Subscriber : MonoBehaviour {

    TF_Manager tf;
    private RosSocket rosSocket;
    public int UpdateTime = 0;
    MarkerArray yolores;
    public bool check_topic = false;

    public GameObject Root;
    public GameObject ObjectLabels;
    

    void Start () {
        
        Invoke("Init", 1.0f);

    }
    public void Init()
    {
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/yolo_object_coordinate", "visualization_msgs/MarkerArray", Yolo_Res, UpdateTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (check_topic)
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("YoloName"))
            {
                Destroy(item);
            }

            foreach (var yolo_target in yolores.markers)
            {
                GeometryPoint obj_position = yolo_target.pose.position;
                GameObject obj_label = Instantiate(ObjectLabels) as GameObject;
                obj_label.transform.parent = Root.transform;
                
                string obj_name = yolo_target.text;

                obj_label.transform.localPosition =
                    new Vector3(-obj_position.y, obj_position.z, obj_position.x);


                obj_label.GetComponent<TextMesh>().text = obj_name;
            }

            Array.Clear(yolores.markers, 0, yolores.markers.Length);
            check_topic = false;
        }
        
    }

    void Yolo_Res(Message message)
    {
        yolores= (MarkerArray)message;
        check_topic = true;
    }
}
