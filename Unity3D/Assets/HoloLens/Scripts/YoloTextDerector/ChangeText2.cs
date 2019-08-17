using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ChangeText2 : MonoBehaviour, IInputClickHandler, IFocusable
{

    private bool focus;
    string changedtext;
    public string origintext;
    private bool istarget;
    public GameObject HSRConnector;
    HSR_Change_Yoloname changescript;
    private Vector3 vecbig;
    private Vector3 vecsmall;

    private void Start()
    {
        focus = false;
        istarget = false;
        origintext = this.gameObject.GetComponent<TextMesh>().text;
        InputManager.Instance.PushFallbackInputHandler(gameObject);
        vecbig= new Vector3(0.08f, 0.08f, 0.08f);
        vecsmall = new Vector3(0.05f, 0.05f, 0.05f);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {

        HSRConnector = GameObject.Find("HSR-connector");
        changescript = HSRConnector.GetComponent<HSR_Change_Yoloname>();

        istarget = true;
    }

    public void OnFocusEnter()
    {
        if (focus == false ||istarget)
        {
            this.transform.localScale = vecbig;
        }
            focus = true;
        
    }

    public void OnFocusExit()
    {
        if(focus == true &&istarget==false)
        {
            this.transform.localScale = vecsmall;
        }
        focus = false;
    }
    public void Update()
    {
        if (istarget && changescript.select_topic)
        {
            if (changedtext == null)
            {
                changedtext = "COLA";
            }
            changedtext = changescript.ChangeYoloName;
            this.gameObject.GetComponent<TextMesh>().text = origintext + "\n" + changedtext;
            origintext = origintext + "\n" + changedtext;
            changescript.select_topic = false;
            istarget = false;
        }
    }
}