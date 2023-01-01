using System.Collections.Generic;
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
