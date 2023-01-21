using Assets.Scripts.Games;
using Networkshared.Packets.ClientServer;
using NetworkShared.Packets.ClientServer;
using NetworkShared.Packets.ServerClient;
using TMPro;
using TTT.PacketHandlers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TTT.Game
{
    public class EndRoundUI : MonoBehaviour
    {
        [SerializeField] Color _winColor;
        [SerializeField] Color _looseColor;
        [SerializeField] Color _drawColor;
        [SerializeField] string _winText = "YOU WIN!";
        [SerializeField] string _looseText = "YOU LOOSE!";
        [SerializeField] string _drawText = "DRAW";

        private Transform _root;
        private Transform _playaAgainBtn;
        private Transform _acceptBtn;
        private Transform _opponentLeftText;
        private Transform _waitingForOpponentText;
        private Transform _playAgainText;
        private Transform _quitBtn;
        private TextMeshProUGUI _winLooseText;
        private Image _winLoosePanel;
        private bool _opponentLeft;

        private float _originalPanelHeight;

        private void OnEnable()
        {
            _root = transform.Find("PopUpPanel");
            _playaAgainBtn = _root.Find("PlayAgainBtn");
            _acceptBtn = _root.Find("AcceptBtn");
            _quitBtn = _root.Find("QuitBtn");

            _winLooseText = _root.Find("WinLooseText").GetComponent<TextMeshProUGUI>();
            _winLoosePanel = _root.Find("WinLoosePanel").GetComponent<Image>();
            _opponentLeftText = _root.Find("OpponentLeftText");
            _playAgainText = _root.Find("PlayAgainText");
            _waitingForOpponentText = _root.Find("WaitingForOpponentText");

            _playaAgainBtn.GetComponent<Button>().onClick.AddListener(RequestPlayAgain);
            _acceptBtn.GetComponent<Button>().onClick.AddListener(Accept);
            _quitBtn.GetComponent<Button>().onClick.AddListener(Quit);

            OnQuitGameHandler.OnQuitGame += HandleOpponentLeft;
            OnPlayAgainHandler.OnPlayAgain += HandlePlayAgainRequest;
            OnNewRoundHandler.OnNewRound += ResetUI;

            var rt = _root.GetComponent<RectTransform>();
            _originalPanelHeight = rt.sizeDelta.y;

            LeanTween.scale(_root.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 1f).setEase(LeanTweenType.easeOutBounce);
        }

        private void OnDisable()
        {
            _playaAgainBtn.GetComponent<Button>().onClick.RemoveListener(RequestPlayAgain);
            _acceptBtn.GetComponent<Button>().onClick.RemoveListener(Accept);
            _quitBtn.GetComponent<Button>().onClick.RemoveListener(Quit);

            OnPlayAgainHandler.OnPlayAgain -= HandlePlayAgainRequest;
            OnNewRoundHandler.OnNewRound -= ResetUI;
            OnQuitGameHandler.OnQuitGame -= HandleOpponentLeft;

            var rt = _root.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, _originalPanelHeight);
        }

        public void Init(string winnerId, bool isDraw)
        {
            if (isDraw)
            {
                _winLoosePanel.color = _drawColor;
                _winLooseText.text = _drawText;
                return;
            }

            var isWin = GameManager.Instance.MyUsername == winnerId;

            if (isWin)
            {
                _winLoosePanel.color = _winColor;
                _winLooseText.text = _winText;
            }
            else
            {
                _winLoosePanel.color = _looseColor;
                _winLooseText.text = _looseText;
            }
        }

        public void HandleOpponentLeft(Net_OnQuitGame msg)
        {
            _playaAgainBtn.gameObject.SetActive(false);
            _opponentLeftText.gameObject.SetActive(true);
            _waitingForOpponentText.gameObject.SetActive(false);
            _playAgainText.gameObject.SetActive(false);
            var rt = _root.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, _originalPanelHeight);
            _acceptBtn.gameObject.SetActive(false);
            _opponentLeft = true;
        }

        private void Quit()
        {
            if (_opponentLeft)
            {
                SceneManager.LoadScene("01_Lobby");
                return;
            }

            _quitBtn.GetComponent<Button>().interactable = false;
            var msg = new Net_QuitGameRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void ResetUI()
        {
            _playaAgainBtn.gameObject.SetActive(true);
            _waitingForOpponentText.gameObject.SetActive(false);
            _acceptBtn.GetComponent<Button>().interactable = true;
            _acceptBtn.gameObject.SetActive(false);
            _playAgainText.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void Accept()
        {
            _acceptBtn.GetComponent<Button>().interactable = false;
            var msg = new Net_AcceptPlayAgainRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void HandlePlayAgainRequest()
        {
            _playaAgainBtn.gameObject.SetActive(false);
            _acceptBtn.gameObject.SetActive(true);
            _playAgainText.gameObject.SetActive(true);

            var rt = _root.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 75f);
        }

        private void RequestPlayAgain()
        {
            _playaAgainBtn.gameObject.SetActive(false);
            _waitingForOpponentText.gameObject.SetActive(true);

            var msg = new Net_PlayAgainRequest();
            NetworkClient.Instance.SendServer(msg);
        }
    }
}
