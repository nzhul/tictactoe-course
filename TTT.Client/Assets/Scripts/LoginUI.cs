using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    private Button _loginButton;
    private Button _sendButton;

    private void Start()
    {
        _loginButton = transform.Find("Connect").GetComponent<Button>();
        _loginButton.onClick.AddListener(Connect);

        _sendButton = transform.Find("Send").GetComponent<Button>();
        _sendButton.onClick.AddListener(Send);
    }

    private void Connect()
    {
        NetworkClient.Instance.Connect();
    }

    private void Send()
    {
        NetworkClient.Instance.SendServer("Hello there!");
    }
}
