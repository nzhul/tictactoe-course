using Assets.Scripts.Games;
using NetworkShared.Packets.ClientServer;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.Game
{
    public class CellUI : MonoBehaviour
    {
        private Button _button;
        private byte _index;
        private byte _row;
        private byte _col;

        private Transform _o;
        private Transform _x;

        private void Start()
        {
            _o = transform.Find("O");
            _x = transform.Find("X");
        }

        public void Init(byte index)
        {
            _index = index;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(CellClicked);
            _row = (byte)(index / 3);
            _col = (byte)(index % 3);
        }

        private void CellClicked()
        {
            if (!GameManager.Instance.IsMyTurn || !GameManager.Instance.InputsEnabled)
            {
                Debug.Log("Not my turn!");
                return;
            }

            _button.interactable = false;
            GameManager.Instance.InputsEnabled = false;

            Debug.Log("Sending MarkCellRequest to server!");

            var msg = new Net_MarkCellRequest
            {
                Index = _index,
            };

            NetworkClient.Instance.SendServer(msg);
        }
    }
}
