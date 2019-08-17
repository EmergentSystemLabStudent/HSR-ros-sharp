/*
 * クラス名 : TimeLimitObj
 * 説明 ; GameObjectを指定した時間が経つと自動で消滅させるクラス。life_timeで生存時間を指定する。
 */

using UnityEngine;
using System.Collections;

//このスクリプトとアタッチされているオブジェクトは
//life_timeに指定した時間後に消える

public class TimeLimitObj : MonoBehaviour
{

    public float life_time = 1.5f; //GameObjectの生存時間
    float time = 0f;

    // Use this for initialization
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        print(time);
        if (time > life_time)
        {
            Destroy(gameObject);
        }
    }
}