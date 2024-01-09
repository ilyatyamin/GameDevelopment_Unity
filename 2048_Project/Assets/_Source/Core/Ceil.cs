namespace Core
{
    public class Cell
    {
        private int _x;
        private int _y;
        private int _value;

        public event System.Action<int> OnValueChanged;
        public event System.Action<int, int> OnPositionChanged;
        public event System.Action OnCellDestroyed;

        public int X
        {
            get => _x;
            set
            {
                int prevX = _x;
                _x = value;
                OnPositionChanged?.Invoke(prevX, _y);
            }
        }
        
        public int Y
        {
            get => _y;
            set
            {
                int prevY = _y;
                _y = value;
                OnPositionChanged?.Invoke(_x, prevY);
            }
        }
        
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        public Cell(int x, int y, int value)
        {
            _x = x;
            _y = y;
            _value = value;
        }

        public void DestroyObject()
        {
            OnCellDestroyed?.Invoke();
        }
    }
}

