using Core;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Game : MonoBehaviour
    {
        private GameField _gameField;
        [SerializeField] private CellSpawner _cellSpawner;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private TMP_Text _score;

        private void Awake()
        {
            StartGame();
        }

        /// <summary>
        /// Initialize game and creating first two cells
        /// </summary>
        private void StartGame()
        {
            _gameField = new GameField(4);
            _cellSpawner.Init(_gameField);
            _gameField.InitializeField();
            _playerInput.Init(_gameField);
            
            _gameField.CreateCell();
            _gameField.CreateCell();
            
        }

        private void Update()
        {
            _score.text = $"Score: {_gameField.Score}";
        }
    }
}