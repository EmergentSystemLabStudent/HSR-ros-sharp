using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ChangePlace : MonoBehaviour, IInputClickHandler, IFocusable
{

    public bool focus;
    string changedtext;
    public string origintext;
    private bool istarget;
    public GameObject HSRConnector;
    HSR_Change_Placename changescript;
    BoxCollider NameCollider;
    private void Start()
    {
        focus = false;
        istarget = false;
        NameCollider = this.GetComponent<BoxCollider>();
        origintext = this.gameObject.GetComponent<TextMesh>().text;
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {

        HSRConnector = GameObject.Find("HSR-connector");
        changescript = HSRConnector.GetComponent<HSR_Change_Placename>();

        istarget = true;
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
        if (focus == true || istarget)
        {
            this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        else if (focus == false)
        {
            this.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }

        // NameCollider.size = new Vector3(origintext.Length * 0.5f, 1, 1);

        if (istarget && changescript.select_topic)
        {
            if (changedtext == null)
            {
                changedtext = "Yoshiki's desk";
            }

            changedtext = changescript.ChangePlaceName;
            this.gameObject.GetComponent<TextMesh>().text = origintext + changedtext;
            origintext = origintext + changedtext+ "\n";
            //NameCollider.size = new Vector3(changedtext.Length * 0.5f, 1, 1);
            changescript.select_topic = false;
            istarget = false;
        }
    }
}