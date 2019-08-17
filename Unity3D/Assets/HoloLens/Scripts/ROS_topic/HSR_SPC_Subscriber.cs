using RosSharp.RosBridgeClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

//場所概念を描画するスクリプト

public class HSR_SPC_Subscriber : MonoBehaviour
{
    private RosSocket rosSocket;
    public int UpdateTime = 0;
    public GameObject spatial_concept;
    public GameObject space_name;
    public GameObject Root;
    private float probility;
    public bool check_topic = false;
    MarkerArray marker;

    Regex re;
    string[] divide;

    // Use this for initialization
    void Start()
    {
        re = new Regex(@"[^0-9.-]");
        Invoke("Init", 1.0f);
    }


    void Init()
    {
#if UNITY_IOS //iOS doesn't connect to ros-sharp

#else
        rosSocket = GetComponent<HSR_Connector>().RosSocket;
        rosSocket.Subscribe("/em/draw_position/array", "visualization_msgs/MarkerArray", GetSpaceName, UpdateTime);
#endif
    }

    private void Update()
    {

        if (check_topic)
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("SPC"))
            {
                Destroy(item);
            }

            foreach (var target_marker in marker.markers)
            {

                if (target_marker.type == 9)
                {
                    divide = target_marker.text.Split('\n');
                    target_marker.text = "";

                    for (int i = 0; i < divide.Length; i++)
                    {
                        space_name = Instantiate(space_name) as GameObject; //インスタンス生成
                        space_name.transform.parent = Root.transform;
                        GeometryPoint geometry = target_marker.pose.position;
                        space_name.transform.localPosition = new Vector3(-geometry.y, 1.5f - 0.2f*i, geometry.x); //位置



                        divide[i] += "% \n";

                        probility = float.Parse(re.Replace(divide[i], ""));

                        space_name.GetComponent<TextMesh>().text = divide[i]; //テキストの内容
                        space_name.GetComponent<TextMesh>().characterSize = 0.45f + (0.005f * probility / 2);
                        space_name.GetComponent<TextMesh>().color = Color.white; //色
                    }

                }
                else
                {
                    spatial_concept = Instantiate(spatial_concept) as GameObject; //インスタンス生成
                    spatial_concept.transform.parent = Root.transform;
                    spatial_concept.transform.localPosition = new Vector3(-target_marker.pose.position.y, target_marker.pose.position.z, target_marker.pose.position.x); //位置
                    spatial_concept.transform.localScale = new Vector3(target_marker.scale.y, 1.2f, target_marker.scale.x); //分散
                    spatial_concept.GetComponent<Renderer>().material.color = new Color(target_marker.color.r, target_marker.color.g, target_marker.color.b, 0.2f); //色
                }
                check_topic = false;
            }

            Array.Clear(marker.markers, 0, marker.markers.Length);

        }
    }

    private void GetSpaceName(Message message)
    {
        marker = (MarkerArray)message;
        check_topic = true;
    }
}
