using Core;
using UnityEngine;

namespace Game
{
    public class CellSpawner : MonoBehaviour
    {
        private static int _id = 1;
        [SerializeField] private GameObject _cellPrefab;
        private GameField _gameField;

        public void Init(GameField gameField)
        {
            _gameField = gameField;
            _gameField.OnCellCreated += CreateCell;
        }

        /// <summary>
        /// Method that creates cell on the scene
        /// </summary>
        /// <param name="cell">Object of type Cell</param>
        private void CreateCell(Cell cell)
        {
            GameObject cellObject = Instantiate(_cellPrefab);
            cellObject.name = $"Cell {_id}";
            ++_id;
            if (cellObject.TryGetComponent(out CellView view))
            {
                view.Init(cell, cellObject);
            }
        }
    }
}