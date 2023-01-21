using Assets.Scripts.Games;
using Networkshared.Packets.ClientServer;
using NetworkShared.Models;
using NetworkShared.Packets.ServerClient;
using System.Collections;
using TMPro;
using TTT.PacketHandlers;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.Game
{
    public class GameUI : MonoBehaviour
    {
        private Transform _surrenderBtn;
        private Transform _endRoundPanel;
        private Transform _turn;
        private TextMeshProUGUI _xUsername;
        private TextMeshProUGUI _xScoreText;
        private TextMeshProUGUI _oUsername;
        private TextMeshProUGUI _oScoreText;

        private int _xScore = 0;
        private int _oScore = 0;

        private void Start()
        {
            var header = transform.Find("Header");
            _xUsername = header.Find("xUsername").GetComponent<TextMeshProUGUI>();
            _xScoreText = header.Find("xScore").GetComponent<TextMeshProUGUI>();
            _oUsername = header.Find("oUsername").GetComponent<TextMeshProUGUI>();
            _oScoreText = header.Find("oScore").GetComponent<TextMeshProUGUI>();
            _turn = transform.Find("Turn");

            _surrenderBtn = transform.Find("Footer").Find("SurrenderBtn");
            _surrenderBtn.GetComponent<Button>().onClick.AddListener(Surrender);
            _endRoundPanel = transform.Find("EndRound");

            OnSurrenderHandler.OnSurrender += HandleSurrender;
            OnMarkCellHandler.OnMarkCell += HandleMarkCell;
            OnNewRoundHandler.OnNewRound += HandleNewRound;
            OnQuitGameHandler.OnQuitGame += HandleOpponentLeft;

            InitHeader();
        }

        private void OnDestroy()
        {
            OnMarkCellHandler.OnMarkCell -= HandleMarkCell;
            OnNewRoundHandler.OnNewRound -= HandleNewRound;
            OnSurrenderHandler.OnSurrender -= HandleSurrender;
            OnQuitGameHandler.OnQuitGame -= HandleOpponentLeft;
            _surrenderBtn.GetComponent<Button>().onClick.RemoveListener(Surrender);
        }

        private void HandleOpponentLeft(Net_OnQuitGame msg)
        {
            if (!_endRoundPanel.gameObject.activeSelf)
            {
                _endRoundPanel.gameObject.SetActive(true);
                _endRoundPanel.GetComponent<EndRoundUI>().HandleOpponentLeft(msg);
            }
        }

        private void HandleSurrender(Net_OnSurrender msg)
        {
            DisplayEndRoundUI(msg.Winner, false);
        }

        private void Surrender()
        {
            var msg = new Net_SurrenderRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void HandleNewRound()
        {
            StopCoroutine(ShowTurn());
            StartCoroutine(ShowTurn());
        }

        private void HandleMarkCell(NetworkShared.Packets.ServerClient.Net_OnMarkCell msg)
        {
            if (msg.Outcome != MarkOutcome.None)
            {
                var isDraw = msg.Outcome == MarkOutcome.Draw;
                StartCoroutine(EndRoundRoutine(msg.Actor, isDraw));
                return;
            }

            StopCoroutine(ShowTurn());
            StartCoroutine(ShowTurn());
        }

        private IEnumerator EndRoundRoutine(string actor, bool isDraw)
        {
            var waitTime = isDraw ? 1.5f : 2;
            yield return new WaitForSeconds(waitTime);
            DisplayEndRoundUI(actor, isDraw);
        }

        private void DisplayEndRoundUI(string winnerId, bool isDraw)
        {
            _endRoundPanel.gameObject.SetActive(true);
            _endRoundPanel.GetComponent<EndRoundUI>().Init(winnerId, isDraw);

            if (isDraw)
            {
                return;
            }

            var playerType = GameManager.Instance.ActiveGame.GetPlayerType(winnerId);
            if (playerType == MarkType.X)
            {
                _xScore++;
                _xScoreText.text = _xScore.ToString();
            }
            else
            {
                _oScore++;
                _oScoreText.text = _oScore.ToString();
            }
        }

        private IEnumerator ShowTurn()
        {
            _turn.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _turn.gameObject.SetActive(true);
        }

        private void InitHeader()
        {
            var game = GameManager.Instance.ActiveGame;
            _xUsername.text = "[X] " + game.XUser;
            _oUsername.text = "[O] " + game.OUser;
        }
    }
}
