/*
 * クラス名 : State_Manager
 * 説明 ; HSRのステートを習得できるTopicを常にSubscribeしているクラス。
 * ステート状態を習得したい場合、このクラスにアクセスすればよい。
 */
using UnityEngine;
using RosSharp.RosBridgeClient;
using System;

//以下のサイトのイベント情報を取得するスクリプト(詳細は以下のURLを参照)
//https://gitlab.com/naripa/nrp_rosbridge_events

public class State_Manager : MonoBehaviour {

    private RosSocket rosSocket;
    public EventHandler<string> State_EventHandler; //ステートTopicが飛んできた時に発生するイベント
    public int UpdateTime = 10;
    public StandardString robot_state; //ロボットのステート状態を保存

    public void Start()
    {
        robot_state = new StandardString();
        Invoke("Init", 1.0f);
    }

    public void Init()
    {
#if UNITY_IOS //iOS doesn't connect to ros-sharp

#else
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/nrp_rosbridge_hsr_out", "std_msgs/String", UpdateState, UpdateTime);
#endif
    }

    void UpdateState(Message message)
    {
        robot_state = (StandardString)message;
        State_EventHandler(this, robot_state.data); //イベントを発生させる
    }

}
