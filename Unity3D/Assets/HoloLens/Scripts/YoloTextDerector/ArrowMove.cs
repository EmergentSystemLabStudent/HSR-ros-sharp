using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour {


    public float ArrowPos;
    void Start () {
        ArrowPos = 0.015f;
    }
	void Update () {
        this.transform.Translate(0, ArrowPos, 0);
       if (this.transform.localPosition.y >= 140 || this.transform.localPosition.y <= 50)
        {
            ArrowPos *= -1f;
        }
    }
}
