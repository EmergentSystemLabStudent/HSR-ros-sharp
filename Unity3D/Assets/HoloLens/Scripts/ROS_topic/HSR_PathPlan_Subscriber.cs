/*
 * クラス名 : HSR_PathPlan_Subscriber
 * 説明 ; HSRの移動経路(PathPlan)をHoloLensで可視化するクラス
 */

using RosSharp.RosBridgeClient;
using UnityEngine;

//HSRのパスプランニングを描画するスクリプト

public class HSR_PathPlan_Subscriber : MonoBehaviour
{
    private RosSocket rosSocket;
    public int UpdateTime = 0;
    PathPlan path;
    public GameObject path_plan;
    public GameObject Root;
    LineRenderer path_line;
    public bool check_topic = false;
    Vector3 each_point;
    int counter = 0;

    void Start()
    {
        Invoke("Init", 1.0f);
    }

    public void Init()
    {
#if UNITY_IOS //iOS doesn't connect to ros-sharp

#else
        //PathPlanをSubscribeする
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/base_local_path", "nav_msgs/Path", GetPath, UpdateTime);
#endif
    }

    void Update()
    {
        if (check_topic)
        {
            // PathPlanを再描画する為に古いPathを消す
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Path"))
            {
                Destroy(item);
                counter = 0;
            }

            //Pathを描画する設定
            path_plan = Instantiate(path_plan) as GameObject;
            path_plan.transform.parent = Root.transform;
            path_line = path_plan.GetComponent<LineRenderer>();
            path_line.positionCount = path.poses.Length;
            path_line.startWidth = 0.02f;
            path_line.endWidth = 0.02f;
            path_line.material.color = Color.green; //ここでPathの色を変更できる

            //Topicで得られたPathの配列から１つずつ座標を習得する
            foreach (GeometryPoseStamped point in path.poses)
            {
                each_point.Set(-point.pose.position.y, -1.1f, point.pose.position.x);
                path_line.SetPosition(counter, each_point);
                counter++;
            }
            check_topic = false;
        }

    }

    void GetPath(Message message)
    {
        path = (PathPlan)message;
        check_topic = true;
    }

}
