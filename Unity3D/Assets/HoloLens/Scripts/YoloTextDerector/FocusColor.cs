using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class FocusColor : MonoBehaviour, IFocusable
{

    public bool focus;
    public Material GreenColor;
    public Material RedColor;
    private void Start()
    {
        focus = false;
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public void OnFocusEnter()
    {
        focus = true;
    }

    public void OnFocusExit()
    {
        focus = false;
    }

    public void Update()
    {
        if (focus == true)
        {
            this.GetComponent<Renderer>().material.color = RedColor.color;

        }
        else if (focus == false)
        {
            this.GetComponent<Renderer>().material.color = GreenColor.color;

        }

    }
}