﻿using Networkshared.Packets.ClientServer;
using NetworkShared.Packets.ServerClient;
using TMPro;
using TTT.PacketHandlers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TTT.Lobby
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] GameObject _playerRowPrefab;

        private TextMeshProUGUI _playersOnlineLabel;
        private Transform _topPlayersContainer;
        private Transform _logoutBtn;

        private void Start()
        {
            _topPlayersContainer = transform.Find("topPlayersContainer");
            _playersOnlineLabel = transform.Find("playersOnlineLbl").GetComponent<TextMeshProUGUI>();
            _logoutBtn = transform.Find("Footer").Find("LogoutBtn");
            _logoutBtn.GetComponent<Button>().onClick.AddListener(Logout);

            OnServerStatusRequestHandler.OnServerStatus += RefreshUI;
            RequestServerStatus();
        }

        private void OnDestroy()
        {
            OnServerStatusRequestHandler.OnServerStatus -= RefreshUI;
        }

        private void RefreshUI(Net_OnServerStatus msg)
        {
            while (_topPlayersContainer.childCount > 0)
            {
                DestroyImmediate(_topPlayersContainer.GetChild(0).gameObject);
            }

            _playersOnlineLabel.text = $"{msg.PlayersCount} players online";

            for (int i = 0; i < msg.TopPlayers.Length; i++)
            {
                var player = msg.TopPlayers[i];
                var instance = Instantiate(_playerRowPrefab, _topPlayersContainer).GetComponent<PlayerRowUI>();
                instance.Init(player);
            }
        }

        private void Logout()
        {
            NetworkClient.Instance.Disconnect();
            SceneManager.LoadScene("00_Login");
        }

        private void RequestServerStatus()
        {
            var msg = new Net_ServerStatusRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        // FindOpponent()

        // CancelFindOpponent()

        // Logout()

        // + RefreshUI()
    }
}
