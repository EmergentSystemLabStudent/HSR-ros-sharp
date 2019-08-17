using UnityEngine;
using RosSharp.RosBridgeClient;
using System;


public class YoloSubscriber : MonoBehaviour
{
    private RosSocket rosSocket;
    public int UpdateTime = 0;
    private string names;
    private float pro;
    private int count;
    //public BoundingBoxes BB;
    private int xmin;
    private int xmax;
    private int ymin;
    private int ymax;

    private int x_middle;
    private int y_middle;
    private int width;
    private int height;

    private int z_kari;
    //private int[,] combi;

    public GameObject Yolo_name_point;
    public GameObject Root;
    public bool check_topic = false; // Check the topic data

    private int i;

    void Start()
    {
      //  BB = new BoundingBoxes(); //add 

        Debug.Log("Start...");
        Invoke("Init", 1.0f);
        i = 0;
    }

    /*public void Init()
    {
#if UNITY_IOS //iOS doesn't connect to ros-sharp

#else
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/darknet_ros/bounding_boxes", "darknet_ros_msgs/BoundingBoxes", YoloRes, UpdateTime);
#endif
    }

    void Update()
    {
        if (check_topic)
        {
            
            foreach (BoundingBox bbox in BB.bounding_boxes)
            {
       
                names = bbox.Class;
                pro = bbox.probability;
                xmin= bbox.xmin;
                xmax = bbox.xmax;
                ymin = bbox.ymin;
                ymax = bbox.ymax;

                x_middle = (xmax + xmin)/2;
                y_middle = (ymax + ymin)/2;
                width = xmax - xmin;
                height = ymax - ymin;
                //Debug.Log(i+" : "+ names);
                

                Yolo_name_point.transform.parent = Root.transform;
                
                //Yolo_name_point.transform.localPosition = new Vector3(-y_middle, z, x_middle);
                i = i+ 1;
                Debug.Log(i);
            }
            
            check_topic = false;
            i = 0;

        }

    }

    void YoloRes(Message message)
    {
 
        BB = (BoundingBoxes)message;
        check_topic = true;
    }*/

}
