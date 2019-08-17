/*
 * クラス名 : HSR_Emotion
 * 説明 ; HSRの感情を可視化するクラス
 */
using UnityEngine;

//HSRの頭上にそのときの感情を描画するスクリプト

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosSocket))]
    public class HSR_Emotion : MonoBehaviour
    {
        TF_Manager tf;
        GeometryTransformStamped torso_tf;
        GeometryPoseStamped base_foot_tf;

        private GameObject target;
        public GameObject Root;
        private string current_state;
        private bool State_change = false;
        private bool subscribed = false;
        private Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);

        public void Start()
        {
            //HSRの頭部の座標とステート情報をSubscribeする
            tf = gameObject.GetComponent<TF_Manager>();
            gameObject.GetComponent<TF_Manager>().Topic_EventHandler += (sender, e) => Subscribed_tf();
            gameObject.GetComponent<State_Manager>().State_EventHandler += (sender, e) => Subscribed_State(e);
        }

        public void Subscribed_tf()
        {
            subscribed = true;
        }

        public void Subscribed_State(string state_num)
        {
            current_state = state_num;
            State_change = true;
        }

        private void Update()
        {
            //Topicで指定された感情のGameObjectを表示
            if (State_change)
            {
                Destroy(target);
                target = generate_gameobject(current_state, target);
                State_change = false;
            }

            //上で表示したGameObjectをHSRの頭部部分に移動させる
            if (subscribed && target != null)
            {

                torso_tf = tf.torso_lift_link;
                base_foot_tf = tf.base_footprint;

                position.y = torso_tf.transform.translation.z + 0.21f;
                position.x = -(base_foot_tf.pose.position.y + 0.02f);
                position.z = base_foot_tf.pose.position.x - 0.06f;

                target.transform.localPosition = position;
            }

        }

        //感情を示すGameObjectを描画する
        private GameObject generate_gameobject(string state_key, GameObject state_object)
        {
            string path = "";

            if (state_key == "11" || state_key == "20" || state_key == "22" || state_key == "28" || state_key == "29")
            {
                path = Robot_State.get_state_gameobject(state_key);
                state_object = (GameObject)Resources.Load(path);
                state_object = Instantiate(state_object);
                state_object.transform.parent = Root.transform;
                return state_object;

            }
            else
            {
                return null;
            }
        }
    }

}

