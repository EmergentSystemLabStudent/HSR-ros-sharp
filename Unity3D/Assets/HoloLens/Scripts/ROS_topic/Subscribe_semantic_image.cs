using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RosSharp.RosBridgeClient;
using System;

//ROSをに送信した画像がSemantic Segmentationされて帰ってくるので
//それをSubscribeするスクリプト

public class Subscribe_semantic_image : MonoBehaviour {

    private int UpdateTime = 0;
    private RosSocket rosSocket;
    public bool arrived = false;
    public SensorCompressedImage segmented_image;
    public EventHandler semantic_result;

    void Start () {
        segmented_image = new SensorCompressedImage();
        Invoke("Init", 1.0f);
    }

    private void Init()
    {
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/segmented_image_for_hololens", "sensor_msgs/CompressedImage", Semantic_result_arrived, UpdateTime);
        Debug.Log("segmented_image_for_hololens initation finished");
    }

    void Semantic_result_arrived(Message message)
    {
        arrived = true;

        segmented_image = (SensorCompressedImage)message;

        semantic_result(this, EventArgs.Empty);

        Debug.Log("segmented_image arrived");

    }

}
