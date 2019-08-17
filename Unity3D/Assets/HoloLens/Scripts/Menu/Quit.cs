/*
 * クラス名 : Quit
 * 説明 ; Menu画面の"Quit"で使われるクラス。 アプリが終了する。
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アプリケーションを終了するスクリプト

public class Quit : MonoBehaviour {
    private void Awake() { 
        Application.Quit();
    }
}
