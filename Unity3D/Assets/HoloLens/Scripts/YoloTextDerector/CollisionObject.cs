using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : MonoBehaviour
{

    void OnCollisionStay(Collision other)
    {
        if ((other.gameObject.tag == "YoloName" && this.gameObject.tag == "YoloName"))
        {
            if (this.transform.position.y >= other.transform.position.y)
            {
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.transform.Translate(0, 0.15f, 0);
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }
    private void Update()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
}
