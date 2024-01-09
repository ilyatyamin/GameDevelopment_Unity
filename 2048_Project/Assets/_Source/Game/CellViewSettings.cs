using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Cell View Setting", menuName = "SO/new Cell View Setting")]
    public class CellViewSettings : ScriptableObject
    {
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;

        public Color StartColor => _startColor;
        public Color EndColor => _endColor;
    }
}