using NetworkShared.Packets.ServerClient;
using TMPro;
using UnityEngine;

namespace TTT.Lobby
{
    public class PlayerRowUI : MonoBehaviour
    {
        [SerializeField] GameObject _onlineImage, _offlineImage;
        [SerializeField] TextMeshProUGUI _username, _score;

        public void Init(PlayerNetDto player)
        {
            if (player.IsOnline)
            {
                _onlineImage.SetActive(true);
            }
            else
            {
                _offlineImage.SetActive(true);
            }

            _username.text = player.Username;
            _score.text = player.Score.ToString();
        }
    }
}
