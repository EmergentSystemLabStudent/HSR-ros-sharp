/*
 * クラス名 : IP
 * 説明 ; Menu画面に現在設定されているIPアドレスを確認するクラス
 */
using UnityEngine;
using UnityEngine.UI;

//IPアドレスを管理するスクリプト

public class IP : MonoBehaviour {

    public string HSR_IP = "192.168.0.1"; //IPの初期値
    Text IP_string; //Menu表示用の文字列

    // Use this for initialization
    void Start () {
        IP_string = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        IP_string.text = "IP:" + HSR_IP;
    }
}
