/*
 * クラス名 : HSR_TF
 * 説明 ; HSRの地図上の位置をHoloLensに見せるクラス。
 */
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosSocket))]
    public class HSR_TF : MonoBehaviour
    {
        TF_Manager tf;
        public GameObject HSR_tf_marker; //HSRの地図上の位置に出現させるGameObject
        public GameObject Root;
        Vector3 position;
        GeometryPoseStamped topic_tf;

        public void Start()
        {
            tf = gameObject.GetComponent<TF_Manager>(); //HSRのTF情報を習得
            HSR_tf_marker = Instantiate(HSR_tf_marker);
            HSR_tf_marker.transform.parent = Root.transform;
        }

        private void Update() //毎サイクルごとにHSRのTFを習得してHSR_tf_markerを描画する
        {
            topic_tf = tf.base_footprint;
            position = new Vector3(-topic_tf.pose.position.y, 0.5f, topic_tf.pose.position.x);
            HSR_tf_marker.transform.localPosition = position;

            GeometryQuaternion tmp = topic_tf.pose.orientation;
            Quaternion quaternion = new Quaternion(0, tmp.z, 0, -tmp.w);

            HSR_tf_marker.transform.localRotation = quaternion;

        }

    }

}