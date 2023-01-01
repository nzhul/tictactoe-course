using Assets.Scripts.Games;
using Assets.Scripts.PacketHandlers;
using NetworkShared.Models;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace TTT.Game
{
    public class GameUI : MonoBehaviour
    {
        private Transform _turn;
        private TextMeshProUGUI _xUsername;
        private TextMeshProUGUI _xScoreText;
        private TextMeshProUGUI _oUsername;
        private TextMeshProUGUI _oScoreText;

        private void Start()
        {
            var header = transform.Find("Header");
            _xUsername = header.Find("xUsername").GetComponent<TextMeshProUGUI>();
            _xScoreText = header.Find("xScore").GetComponent<TextMeshProUGUI>();
            _oUsername = header.Find("oUsername").GetComponent<TextMeshProUGUI>();
            _oScoreText = header.Find("oScore").GetComponent<TextMeshProUGUI>();
            _turn = transform.Find("Turn");

            OnMarkCellHandler.OnMarkCell += HandleMarkCell;

            InitHeader();
        }

        private void OnDestroy()
        {
            OnMarkCellHandler.OnMarkCell -= HandleMarkCell;
        }

        private void HandleMarkCell(NetworkShared.Packets.ServerClient.Net_OnMarkCell msg)
        {
            if (msg.Outcome != MarkOutcome.None)
            {
                var isDraw = msg.Outcome == MarkOutcome.Draw;
                Debug.Log("Showing End Round Screen!");
                return;
            }

            StopCoroutine(ShowTurn());
            StartCoroutine(ShowTurn());
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
