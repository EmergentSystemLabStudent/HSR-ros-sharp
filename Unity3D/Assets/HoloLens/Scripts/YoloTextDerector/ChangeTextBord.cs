using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ChangeTextBord : MonoBehaviour, IInputClickHandler, IFocusable
{
    public bool focus;
    string changedtext;
    public string origintext;
    private bool istarget;
    public GameObject HSRConnector;
    HSR_Change_Yoloname changescript;
    private float TextLen;
    private int i;

    public Material brightColor;
    public Material darkColor;
    private bool ismonitor;

    private void Start()
    {
        focus = false;
        istarget = false;
        origintext = this.gameObject.GetComponent<TextMesh>().text;
        TextLen = origintext.Length;
        i = 1;
        this.transform.GetChild(0).localScale = new Vector3(TextLen * 0.9f, 1.6f * i, 0.01f);
        InputManager.Instance.PushFallbackInputHandler(gameObject);
        ismonitor = false;
    }

    public void OnFocusEnter()
    {
        if (focus == false || istarget)
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = darkColor.color;
        }
        focus = true;
    }

    public void OnFocusExit()
    {
        if(focus == true && istarget == false)
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = brightColor.color;
        }
            focus = false;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {

        HSRConnector = GameObject.Find("HSR-connector");
        changescript = HSRConnector.GetComponent<HSR_Change_Yoloname>();

        if (focus == true)
        {
            istarget = true;
            this.gameObject.GetComponent<TextMesh>().color = Color.red;
            changescript.select_one = !changescript.select_one;
            ismonitor = changescript.select_one;
        }
    }

    public void Update()
    {
        if(istarget==true && ismonitor != changescript.select_one && focus==false)
        {
            istarget = false;
            focus = false;
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = brightColor.color;
            this.gameObject.GetComponent<TextMesh>().color = Color.white;
        }

        if (istarget && changescript.select_topic)
        {
            if (changedtext == null)
            {
                changedtext = "COLA";
            }
            changedtext = changescript.ChangeYoloName;

            if (TextLen < changedtext.Length)
            {
                TextLen = changedtext.Length;
            }

            this.gameObject.GetComponent<TextMesh>().text = origintext + "\n" + changedtext;
            origintext = origintext + "\n" + changedtext;
            this.transform.GetChild(0).localPosition = new Vector3(0, i + 0.6f, 0);

            i++;
            this.transform.GetChild(0).localScale= new Vector3(TextLen*0.9f, 1.6f*i, 0.01f);
            changescript.select_topic = false; 
            istarget = false;
            this.gameObject.GetComponent<TextMesh>().color = Color.white;
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = brightColor.color;
        }
    }
}