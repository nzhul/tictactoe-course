using NetworkShared.Packets.ServerClient;
using System.Collections.Generic;
using TTT.PacketHandlers;
using UnityEngine;

namespace TTT.Game
{
    public class BoardUI : MonoBehaviour
    {
        [SerializeField] GameObject _boardCellPrefab;

        private Dictionary<int, CellUI> _cells;

        private void Start()
        {
            ResetBoard();
            OnMarkCellHandler.OnMarkCell += UpdateBoard;
            OnNewRoundHandler.OnNewRound += ResetBoard;
        }

        private void OnDestroy()
        {
            OnMarkCellHandler.OnMarkCell -= UpdateBoard;
            OnNewRoundHandler.OnNewRound -= ResetBoard;
        }

        private void UpdateBoard(Net_OnMarkCell msg)
        {
            _cells[msg.Index].UpdateUI(msg.Actor);
        }

        private void ResetBoard()
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            _cells = new Dictionary<int, CellUI>();

            for (int i = 0; i < 9; i++)
            {
                var cell = Instantiate(_boardCellPrefab, transform).GetComponent<CellUI>();
                cell.Init((byte)i);
                _cells.Add(i, cell);
            }
        }
    }
}
