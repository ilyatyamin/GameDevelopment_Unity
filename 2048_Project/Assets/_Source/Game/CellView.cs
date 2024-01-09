using System;
using System.Collections;
using Core;
using TMPro;
using UnityEngine;

namespace Game
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Cell _cell;
        private GameObject _gameObject;

        public void Init(Cell cell, GameObject unityGameObj)
        {
            _cell = cell;
            _cell.OnPositionChanged += UpdatePosition;
            _cell.OnValueChanged += UpdateValue;
            _cell.OnCellDestroyed += DestroyCell;
            _gameObject = unityGameObj;

            UpdatePosition(cell.X, cell.Y);
            UpdateValue(_cell.Value);
        }

        /// <summary>
        /// Destroying cell by method Destroy
        /// </summary>
        private void DestroyCell()
        {
            _cell.OnPositionChanged -= UpdatePosition;
            _cell.OnValueChanged -= UpdateValue;
            Destroy(_gameObject);
        }

        /// <summary>
        /// Update cell's value on the new
        /// </summary>
        /// <param name="value">new value of cell</param>
        private void UpdateValue(int value)
        {
            var val = Math.Pow(2, _cell.Value);
            _valueText.text = $"{val}";
            
            _spriteRenderer.color = Color.Lerp(Color.white, Color.yellow,  (_cell.Value / 17.0f));
            
        }

        /// <summary>
        /// Updates position of cell
        /// </summary>
        /// <param name="x">PREVIOUS X coordinate of cell</param>
        /// <param name="y">PREVIOUS Y coordinate of cell</param>
        private void UpdatePosition(int x, int y)
        {
            StartCoroutine(AnimationPositionChange(x, y));
        }

        private IEnumerator AnimationPositionChange(int startX, int startY)
        {
            // Animation using coroutine
            if (startX != _cell.X)
            {
                // Move by x
                for (int i = 1; i <= 20; ++i)
                {
                    // Moving coordinate by delta 
                    transform.position = new Vector2(startX + (_cell.X - startX) * (i / 20.0f), _cell.Y);
                    yield return new WaitForEndOfFrame();
                }
            }
            if (startY != _cell.Y)
            {
                // Move by y
                for (int i = 1; i <= 20; ++i)
                {
                    transform.position = new Vector2(_cell.X, startY + (_cell.Y - startY) * (i / 20.0f));
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                // Case when cell is initialized and we not need to move it
                transform.position = new Vector2(-10000, -10000);
                for (int i = 1; i <= 21; ++i)
                {
                    yield return new WaitForEndOfFrame();
                }
                transform.position = new Vector2(_cell.X, _cell.Y);
            }
        }
    }
}
