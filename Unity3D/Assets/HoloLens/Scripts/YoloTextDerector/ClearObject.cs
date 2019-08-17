using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObject : MonoBehaviour {
    public float Clear;
    // Use this for initialization
    void Start () {
        Clear = 0.0f;
        this.GetComponent<TextMesh>().color = new Color(0, 255, 12, Clear);
    }
	
	// Update is called once per frame
	void Update () {
        if (Clear < 1.0)
        {
            Clear = Clear + 0.005f;
            this.GetComponent<TextMesh>().color = new Color(0, 255, 0, Clear);
        }
    }
}
