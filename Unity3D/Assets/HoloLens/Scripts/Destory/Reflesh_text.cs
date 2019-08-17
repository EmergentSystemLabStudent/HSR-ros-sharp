using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//テキスト文字を10秒後にクリアするスクリプト

public class Reflesh_text : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("Reflesh");
    }

    private IEnumerator Reflesh()
    {
        while (true)
        {
            gameObject.GetComponent<Text>().text = "";
            yield return new WaitForSeconds(10.0f);
        }
    }
}