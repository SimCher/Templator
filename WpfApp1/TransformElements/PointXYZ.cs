namespace Templator.TransformElements
{
    /// <summary>
    /// Структура, представляющая точку на трёхмерной оси координат, но z здесь является индексом (int).
    /// Хранит координаты в double, в отличии от стандартного Point (там int)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="_zIndex"></param>
    internal struct PointXYZ
    {
        private double _x;
        private double _y;
        private int _zIndex;

        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }
        /// <summary>
        /// Z-индекс, не является координатой
        /// </summary>
        public int ZIndex
        {
            get => _zIndex;
            set => _zIndex = value;
        }

        public PointXYZ(double x, double y, int zIndex)
        {
            _x = x;
            _y = y;
            _zIndex = zIndex;
        }

        public override string ToString()
        {
            return $"[X = {_x}, Y = {_y}, ZIndex = {_zIndex}]";
        }
    }
}
