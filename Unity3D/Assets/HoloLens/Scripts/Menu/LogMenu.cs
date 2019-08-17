using UnityEngine;
using UnityEngine.UI;

//HoloLensにプリントデバッグを可視化する

public class LogMenu : MonoBehaviour
{
    [SerializeField]
    private Text m_textUI = null;

    private void Awake()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    private void OnLogMessage(string i_logText, string i_stackTrace, LogType i_type)
    {
        if (string.IsNullOrEmpty(i_logText))
        {
            return;
        }

        if (i_logText.Contains("ara:"))
        {
            m_textUI.text += i_logText + System.Environment.NewLine;
        }

    }

}