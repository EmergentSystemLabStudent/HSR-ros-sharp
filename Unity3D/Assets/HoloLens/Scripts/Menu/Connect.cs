/*
 * クラス名 : Connect
 * 説明 ; Menu画面の"Connect to HSR"で使われるクラス。ROSとの通信を開始する機能を持ったGameObjectをconnectにアタッチすること．
 */

using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//HoloLensがROSに接続した際にステートを更新するスクリプト

public class Connect : MonoBehaviour
{
    public GameObject connect;
    private bool connected = false;
    Text connection_state;
  
    private void Start()
    {
        connection_state = GetComponentInChildren<Text>();

#if UNITY_EDITOR 
        OnClick();
#endif
    }

    public void OnClick() //ボタンが押された時に動くメソッド
    {
#if UNITY_IOS //iOS doesn't connect to ros-sharp

#else
        if (!connected) //HSRとの接続を開始する
        {
            connect.SetActive(true);
            connect.GetComponent<HSR_Connector>().Connect();
            connection_state.text = "Connected";
            connected = true;
        }
        else //HSRとの接続を停止する
        {
            connection_state.text = "Connect\n to HSR";
            connect.GetComponent<HSR_Connector>().Disconnect();
            connected = false;
        }
#endif
    }
}