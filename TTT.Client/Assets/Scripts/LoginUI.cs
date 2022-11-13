using Networkshared.Packets.ClientServer;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] int _maxUsernameLength = 10;
    [SerializeField] int _maxPasswordLength = 10;

    private Transform _loginButton;
    private TextMeshProUGUI _loginText;
    private TMP_InputField _usernameInput;
    private Transform _usernameError;
    private TMP_InputField _passwordInput;
    private Transform _passwordError;
    private Transform _loadingUI;

    private bool _isConnected;

    private string _username = string.Empty;
    private string _password = string.Empty;

    private void Start()
    {
        _loginButton = transform.Find("LoginBtn");
        _loginButton.GetComponent<Button>().onClick.AddListener(Login);
        _loginText = _loginButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        _usernameInput = transform.Find("UsernameInput").GetComponent<TMP_InputField>();
        _usernameInput.onValueChanged.AddListener(UpdateUsername);
        _usernameError = _usernameInput.transform.Find("Error");

        _passwordInput = transform.Find("PasswordInput").GetComponent<TMP_InputField>();
        _passwordInput.onValueChanged.AddListener(UpdatePassword);
        _passwordError = _passwordInput.transform.Find("Error");

        _loadingUI = transform.Find("Loading");

        NetworkClient.Instance.OnServerConnected += SetIsConnected;
    }

    private void OnDestroy()
    {
        NetworkClient.Instance.OnServerConnected -= SetIsConnected;
    }

    private void SetIsConnected()
    {
        _isConnected = true;
    }

    private void UpdatePassword(string value)
    {
        _password = value;
        ValidateAndUpdateUI();
    }

    private void UpdateUsername(string value)
    {
        _username = value;
        ValidateAndUpdateUI();
    }

    private void ValidateAndUpdateUI()
    {
        var usernameRegex = Regex.Match(_username, "^[a-zA-Z0-9]+$");

        var interactable =
            (!string.IsNullOrWhiteSpace(_username) &&
            !string.IsNullOrWhiteSpace(_password)) &&
            (_username.Length <= _maxUsernameLength && _password.Length <= _maxPasswordLength) &&
            usernameRegex.Success;

        EnableLoginButton(interactable);

        if (_password != null)
        {
            var passwordTooLong = _password.Length > _maxPasswordLength;
            _passwordError.gameObject.SetActive(passwordTooLong);
        }

        if (_username != null)
        {
            var usernameTooLong = _username.Length > _maxUsernameLength || !usernameRegex.Success;
            _usernameError.gameObject.SetActive(usernameTooLong);
        }
    }

    private void EnableLoginButton(bool interactable)
    {
        _loginButton.GetComponent<Button>().interactable = interactable;
        var color = _loginButton.GetComponent<Button>().interactable ? Color.white : Color.gray;
        _loginText.color = color;
    }

    private void Login()
    {
        StopCoroutine(LoginRoutine());
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine()
    {
        EnableLoginButton(false);
        _loadingUI.gameObject.SetActive(true);

        NetworkClient.Instance.Connect();

        while (!_isConnected)
        {
            Debug.Log("WAITING!");
            yield return null;
        }

        Debug.Log("Connected to the server!");

        var authRequest = new Net_AuthRequest
        {
            Username = _username,
            Password = _password,
        };

        NetworkClient.Instance.SendServer(authRequest);
    }
}
