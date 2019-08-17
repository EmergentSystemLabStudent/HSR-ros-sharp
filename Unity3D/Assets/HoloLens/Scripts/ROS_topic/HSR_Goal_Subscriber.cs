/*
 * クラス名 : HSR_Goal_Subscriber
 * 説明 ; HSRの移動目的地(NavigationGoal)をHoloLensで可視化するクラス
 */

using RosSharp.RosBridgeClient;
using UnityEngine;

//HSRのナビゲーションゴールを表示するスクリプト

public class HSR_Goal_Subscriber : MonoBehaviour
{
    private RosSocket rosSocket;
    public int UpdateTime = 0;
    public GameObject goal_point;
    public GameObject Root;
    GeometryPoseStamped goal;
    public bool check_topic = false; // Check the topic data

    void Start()
    {
        Invoke("Init", 1.0f);
    }

    public void Init()
    {
#if UNITY_IOS //iOS doesn't connect to ros-sharp

#else
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/move_base/move/goal", "move_base_msgs/MoveBaseActionGoal", GetGoal, UpdateTime);
#endif
    }

    void Update()
    {
        if (check_topic)
        {
            // Destory the game objects to re-visualize
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Goal"))
            {
                Destroy(item);
            }

            //Topicから得られたGoal地点に指定したGameObjectを配置する
            goal_point = Instantiate(goal_point) as GameObject;
            goal_point.transform.parent = Root.transform;
            goal_point.transform.localPosition = new Vector3(-goal.pose.position.y, 0.0f, goal.pose.position.x);

            check_topic = false;
        }

    }

    void GetGoal(Message message)
    {
        MoveBaseActionGoal moveBaseActionGoal = (MoveBaseActionGoal)message;
        goal = moveBaseActionGoal.goal.target_pose;
        check_topic = true;
    }

}
