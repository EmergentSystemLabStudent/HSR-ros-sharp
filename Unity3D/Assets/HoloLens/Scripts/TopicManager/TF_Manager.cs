/*
 * クラス名 : TF_Manager
 * 説明 ; HSRのTF Topicなどを常にSubscribeしているクラス。
 * TFの情報を習得したい場合、このクラスにアクセスすればよい。
 * 更にアクセスしたいTFがあれば、このクラスを変更。
 */
using UnityEngine;
using RosSharp.RosBridgeClient;
using System;

//TF関連のメインの関数
//毎サイクルROSからSubscribeして変数に保持している
//ここにTF関連で受け取りたいTopicを加えればよい
//外部からこのクラスのインスタンスにアクセスするとその情報を利用可能

public class TF_Manager : MonoBehaviour {

    private RosSocket rosSocket;
    public int UpdateTime = 10;
    public GeometryPoseStamped base_footprint;
    public GeometryTransformStamped torso_lift_link;
    public GeometryPose hand_camera_frame;
    public EventHandler Topic_EventHandler;

    public void Start()
    {
        base_footprint = new GeometryPoseStamped();
        torso_lift_link= new GeometryTransformStamped();
        hand_camera_frame = new GeometryPose();
        Invoke("Init", 1.0f);
    }

    public void Init()
    {
#if UNITY_IOS //iOS doesn't connect to ros-sharp

#else
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/tf", "tf2_msgs/TFMessage", UpdateTF, UpdateTime);
        rosSocket.Subscribe("/global_pose", "geometry_msgs/PoseStamped", UpdatePosition, UpdateTime);
        rosSocket.Subscribe("/current/hand_pose", "geometry_msgs/Pose", Buy_item, UpdateTime);
        Topic_EventHandler?.Invoke(this, EventArgs.Empty);
#endif
    }

    private void UpdatePosition(Message message)
    {

        base_footprint = (GeometryPoseStamped)message;
    }

    void UpdateTF(Message message)
    {
        TF2TFMessage tfMessage = (TF2TFMessage)message;

        foreach (GeometryTransformStamped tf in tfMessage.transforms)
        {

            if(tf.child_frame_id == "torso_lift_link")
            {
                torso_lift_link = tf;
            }

        }
    }

    void Buy_item(Message message)
    {
        hand_camera_frame = (GeometryPose)message;
    }

}
