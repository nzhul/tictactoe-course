using Assets.Scripts.Games;
using NetworkShared.Models;
using NetworkShared.Packets.ClientServer;
using System;
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

        public void UpdateUI(string actor)
        {
            var actorType = GameManager.Instance.ActiveGame.GetPlayerType(actor);

            if (actorType == MarkType.X)
            {
                _x.gameObject.SetActive(true);
                LeanTween.scale(_x.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.5f).setEase(LeanTweenType.easeOutBounce);
            }
            else
            {
                _o.gameObject.SetActive(true);
                LeanTween.scale(_o.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.5f).setEase(LeanTweenType.easeOutBounce);
            }

            _button.interactable = false;
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
