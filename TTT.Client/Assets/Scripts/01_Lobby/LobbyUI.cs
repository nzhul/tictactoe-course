using Networkshared.Packets.ClientServer;
using NetworkShared.Packets.ServerClient;
using TMPro;
using TTT.PacketHandlers;
using UnityEngine;

namespace TTT.Lobby
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] GameObject _playerRowPrefab;

        private TextMeshProUGUI _playersOnlineLabel;
        private Transform _topPlayersContainer;

        private void Start()
        {
            _topPlayersContainer = transform.Find("topPlayersContainer");
            _playersOnlineLabel = transform.Find("playersOnlineLbl").GetComponent<TextMeshProUGUI>();

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
