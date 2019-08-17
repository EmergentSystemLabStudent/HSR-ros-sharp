/*
 * クラス名 : Put_virtual_ruck
 * 説明 ; WRSのプログラム。HSRの横に仮想の商品棚を出すクラス。
 */
using RosSharp.RosBridgeClient;
using UnityEngine;
using UnityEngine.Networking;

//Put_virtual_ruckのスペクテイタービューバージョン
//うまく動作していないので使用非推奨

public class Spec_put_virtual_ruck : NetworkBehaviour
{
    public const string Destroy_Name = "PointCard";
    public GameObject DisplayCase;
    public GameObject HSR_connector;
    public GameObject buy_item;
    public GameObject Root;

    [SyncVar] private Vector3 position_share;


    [SyncVar] private Quaternion rotation_share;
    [SyncVar] private Vector3 des_position_share;
    [SyncVar] private Quaternion des_rotation_share;
    [SyncVar] private bool flag = false;
    [SyncVar] private bool flag2 = false;
    private bool visualize = false;
    private bool destroy = false;
    private Vector3 position;
    private State_Manager State_Manager;
    private GeometryPose hand_tf;
    private TF_Manager tf;

    void Start()
    {
        position = new Vector3();
        position_share = new Vector3();
        rotation_share = new Quaternion();

#if UNITY_IOS //iOS doesn't connect to ros-sharp

#else
        //HSRのTF情報とHSRの現在のステートを習得する
        tf = HSR_connector.GetComponent<TF_Manager>();
        State_Manager = HSR_connector.GetComponent<State_Manager>();
        State_Manager.State_EventHandler += (sender, e) => generate(e);
#endif
    }

    private void Update()
    {

#if UNITY_IOS
        if (flag)
        {
            DisplayCase = Instantiate(DisplayCase) as GameObject;
            DisplayCase.transform.SetParent(Root.transform, false);
            DisplayCase.transform.localPosition = position_share;
            DisplayCase.transform.localRotation = rotation_share;
            flag = false;
        }

        if (flag2)
        {
            Destroy(DisplayCase.transform.Find(Destroy_Name).gameObject);
            buy_item = Instantiate(buy_item) as GameObject;
            buy_item.transform.SetParent(Root.transform, false);
            buy_item.transform.localPosition = des_position_share;
            buy_item.transform.localRotation = des_rotation_share;
            flag2 = false;

        }
#endif

        if (visualize)
        {
#if UNITY_IOS

#else
            //見栄えを大事にするために、余計なObjectを削除する
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("SPC"))
            {
                Destroy(item);
            }

            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Goal"))
            {
                Destroy(item);
            }

            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Path"))
            {
                Destroy(item);
            }

            //棚をHSRの横に表示する
            position = new Vector3(0, -0.5f, -0.55f);
            DisplayCase = Instantiate(DisplayCase) as GameObject;
            DisplayCase.transform.SetParent(GameObject.Find("Transform(Clone)").transform,false);
            DisplayCase.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            DisplayCase.transform.localPosition = position;
            DisplayCase.transform.localRotation = Quaternion.Euler(0,-90,0);
            position_share = DisplayCase.transform.position;
            rotation_share = DisplayCase.transform.rotation;
            flag = true;
            visualize = false;
#endif

        }

        if (destroy)
        {

#if UNITY_IOS
#else
            //HSRの手の部分に商品を出現させる
            Destroy(DisplayCase.transform.Find(Destroy_Name).gameObject);
            buy_item = Instantiate(buy_item) as GameObject;
            buy_item.transform.SetParent(Root.transform, false);
            hand_tf = tf.hand_camera_frame;
            position = new Vector3(-hand_tf.position.y, hand_tf.position.z, hand_tf.position.x);
            buy_item.transform.localPosition = position;
            buy_item.transform.localRotation = Quaternion.Euler(0, -90, 0);
            des_position_share = buy_item.transform.position;
            des_rotation_share = buy_item.transform.rotation;
            destroy = false;
            flag2 = true;
#endif

        }

    }

    void generate(string topic_num)
    {
        if (topic_num == "26") //HSRの横に棚を出すステート
        {
            visualize = true;
        }

        if (topic_num == "27") //HSRの手に商品を出すステート
        {
            destroy = true;
        }

    }

}
