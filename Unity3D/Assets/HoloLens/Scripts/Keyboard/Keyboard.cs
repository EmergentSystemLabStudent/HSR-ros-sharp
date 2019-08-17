/*
 * クラス名 : Keyboard
 * 説明 ; IPアドレス変更の際に使用するキーボードのプログラム
 */
using System;
using UnityEngine;
using UnityEngine.UI;

//IPアドレスを変更する際に使用するキーボードのスクリプト

public class Keyboard : MonoBehaviour
{
    public GameObject Current_IP;
    public IP ip;
    public Button[] bNumber;
    public Button bDell;
    public Button bEnter;
    
  
    public void InputNumber(String number)
    {
        Current_IP = GameObject.FindGameObjectWithTag("IP");
        ip = Current_IP.GetComponent<IP>();
        ip.HSR_IP += number;
    }
    
    public void InputDell()
    {
        Current_IP = GameObject.FindGameObjectWithTag("IP");
        ip = Current_IP.GetComponent<IP>();
        ip.HSR_IP = ip.HSR_IP.Remove(ip.HSR_IP.Length - 1);
    }
}