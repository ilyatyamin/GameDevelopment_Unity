using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class GameField
    {
        private List<Cell> _ceils = new();
        private List<(int, int)> _emptyPositions = new();

        private int _fieldSize;
        public event System.Action<Cell> OnCellCreated;

        public GameField(int fieldSize)
        {
            _fieldSize = fieldSize;
        }

        /// <summary>
        /// Method than initialize field by empty positions
        /// </summary>
        public void InitializeField()
        {
            for (int i = 0; i < _fieldSize; i++)
            {
                for (int j = 0; j < _fieldSize; j++)
                {
                    _emptyPositions.Add((i, j));
                }
            }
        }

        /// <summary>
        /// Moving all cells by vector. Actually, user makes one move.
        /// </summary>
        /// <param name="moveVector">direction of moving: up, left, right, down</param>
        public void MoveCells(Vector2Int moveVector)
        {
            var copy = moveVector;
            bool isAnybodyMoved = false;
            
            var sortedCells = _ceils;
            // Depending on directon of moving sorting cells by it order on moving
            switch ((moveVector.x, moveVector.y))
            {
                case (0, 1): // up
                    // Firstly moving cells with larger Y
                    sortedCells = _ceils.OrderByDescending(y => y.Y).ToList();
                    break;
                case (1, 0): // right
                    // Firstly moving cells with larger X
                    sortedCells = _ceils.OrderByDescending(x => x.X).ToList();
                    break;
                case (-1, 0): // left
                    // Firstly moving cells with smaller X
                    sortedCells = _ceils.OrderBy(x => x.X).ToList();
                    break;
                case (0, -1): // down
                    // Firstly moving cells with smaller Y
                    sortedCells = _ceils.OrderBy(y => y.Y).ToList();
                    break;
            }
            
            // For all cells, that was placed on game field...
            foreach (var sortedCell in sortedCells)
            {
                var initialPosition = (sortedCell.X, sortedCell.Y);
                bool isCellMoved = false;
                bool isDeleted = false;
                
                // Strategy: while we can increase moveVector, we increase it)
                while (CanMoveCell(sortedCell, moveVector))
                {
                    isAnybodyMoved = true;
                    isCellMoved = true;
                    
                    // if cell this same coordinate and same value exists, we can union them with one cell with value 2*previousValue
                    if (_ceils.Exists(x => (x.X, x.Y, x.Value) == (sortedCell.X + moveVector.x, sortedCell.Y + moveVector.y, sortedCell.Value)))
                    {
                        // find this cell by linq
                        Cell foundedCell = _ceils.Find(x => (x.X, x.Y, x.Value) == (sortedCell.X + moveVector.x,
                            sortedCell.Y + moveVector.y, sortedCell.Value));
                        foundedCell.Value += 1;

                        // destroy game object in field 
                        sortedCell.DestroyObject();
                        
                        // now position of destroyed cell is opened for other cells
                        _emptyPositions.Add(initialPosition);
                        _ceils.Remove(sortedCell);
                        isDeleted = true;
                        break;
                    }
                    // increase vector
                    moveVector += copy;
                }

                // Case when cell not deleted but it moved to another coordinate
                if (isCellMoved && !isDeleted)
                {
                    // warning!! moveVector now in one value larger than it needs
                    // (we increase vector and it not approached to our field, so we need to decrease it)
                    moveVector -= copy;
                    sortedCell.X += moveVector.x;
                    sortedCell.Y += moveVector.y;
                    
                    // previous position is available, but current position is occupied
                    _emptyPositions.Add(initialPosition);
                    _emptyPositions.Remove((sortedCell.X, sortedCell.Y));
                }
            }
            
            if (isAnybodyMoved)
            {
                CreateCell();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell">Current cell</param>
        /// <param name="vect">Direction of moving</param>
        /// <returns>Return true if cell + vect exists, false in another case </returns>
        private bool CanMoveCell(Cell cell, Vector2Int vect)
        {
            bool isFind = false;
            if (_emptyPositions.Contains((cell.X + vect.x, cell.Y + vect.y)))
            {
                isFind = true;
            } else if (_ceils.Exists(x => (x.X, x.Y, x.Value) == (cell.X + vect.x, cell.Y + vect.y, cell.Value)))
            {
                isFind = true;
            }
            return isFind;
        }

        /// <summary>
        /// Return randomized position in list of all empty positions.
        /// </summary>
        /// <returns>Tuple of coordinates (X, Y)</returns>
        private (int, int) GetEmptyPosition()
        {
            int position = Random.Range(0, _emptyPositions.Count);
            (int, int) emptyCell = _emptyPositions[position];
            _emptyPositions.Remove(emptyCell);
            return emptyCell;
        }

        /// <summary>
        /// Calculates random start value of cell
        /// </summary>
        /// <returns>int with value 2 (probability is 90%) or 1 (probability is 10%)</returns>
        private int CalculateCellValue()
        {
            int value = Random.Range(0, 101);
            return value <= 10 ? 2 : 1;
        }

        /// <summary>
        /// Creates new cell with randomized position and randomized value
        /// </summary>
        public void CreateCell()
        {
            var cellPosition = GetEmptyPosition();
            Cell cell = new Cell(cellPosition.Item1, cellPosition.Item2, CalculateCellValue());
            _ceils.Add(cell);
            OnCellCreated?.Invoke(cell);
        }

        /// <summary>
        /// Calculates score of game as sum of all cell values
        /// </summary>
        public int Score
        {
            get
            {
                double score = 0;
                foreach (var cell in _ceils)
                {
                    score += Math.Pow(2, cell.Value);
                }
                return Convert.ToInt32(score);
            }
        }
    }
}