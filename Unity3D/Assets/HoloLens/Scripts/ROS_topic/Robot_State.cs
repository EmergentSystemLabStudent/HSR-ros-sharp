/*
 * クラス名 : Robot_State
 * 説明 ; RobotのState(感情のみ)とGameObjectのPathを紐づけるクラス
 */
using System.Collections.Generic;

//以下のURLに記載されたルールをここに書く
//https://gitlab.com/naripa/nrp_rosbridge_events

public class Robot_State{

    //ステート番号とPathを辞書で登録
    public static Dictionary<string,string> state = new Dictionary<string, string>{
        { "11", "Emotion/Surprised"},
        { "20", "Emotion/Happy"},
        { "22", "Emotion/Confuse"},
        { "26", "Emotion/Task_tran"},
        { "27", "Emotion/Buy"},
        { "28", "Emotion/Cool"},
        { "29", "Emotion/Think"}
    };

    //引数で与えられたステート番号に一致する感情を表すGameObjectのPathを習得するメソッド
    public static string get_state_gameobject(string state_number)
    {
        string path;

        if (!state.ContainsKey(state_number))
        {
            return null;
        }

        state.TryGetValue(state_number, out path);
        return path;
    }
}
