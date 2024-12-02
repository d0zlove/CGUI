using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CGUI
{
    public class GUIclass : Control
    {
        private Timer _colorTimer;             // Таймер для анимации
        private double _hue = 210;            // Начальный тон в синем диапазоне
        private double _saturation = 0.7;     // Насыщенность
        private double _value = 1.0;          // Яркость
        private bool _isHovered = false;      // Флаг состояния наведения
        private bool _isIncreasing = true;    // Направление изменения (увеличение или уменьшение)

        public GUIclass()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);

            DoubleBuffered = true;
            Size = new Size(500, 500); 

            _colorTimer = new Timer();
            _colorTimer.Interval = 50; 
            _colorTimer.Tick += (s, e) =>
            {

                if (_hue >= 240)
                    _isIncreasing = false;

                else if (_hue <= 180)
                    _isIncreasing = true;

                if (_isIncreasing)
                    _hue += 1; 
                else
                    _hue -= 1; 
                Invalidate(); 
            };
            _colorTimer.Start();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;

            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.Clear(Parent.BackColor);

            // Получаем текущие цвета из ограниченного синего диапазона
            Color animatedColor1 = ColorFromHSV(_hue, _saturation, _value);
            Color animatedColor2 = ColorFromHSV(_hue + 30, _saturation, _value); // Отступ для градиента

            if (_isHovered)
            {
                // При наведении делаем цвета ярче
                animatedColor1 = Color.White;
                animatedColor2 = Color.LightGray;
            }

            // Прямоугольник для градиента
            Rectangle gradientRect = new Rectangle(0, 0, Width, Height);

            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                gradientRect,
                animatedColor1,
                animatedColor2,
                LinearGradientMode.ForwardDiagonal))
            {
                graph.FillEllipse(gradientBrush, 0, 0, Width - 1, Height - 1); // Рисуем круг с градиентом
            }

        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true; // Устанавливаем флаг наведения
            Invalidate(); // Перерисовываем
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false; // Сбрасываем флаг наведения
            Invalidate(); // Перерисовываем
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Обеспечиваем, что ширина и высота всегда равны
            int size = Math.Min(Width, Height); // Находим меньшую сторону
            Size = new Size(size, size); // Устанавливаем квадратный размер
        }

        // Метод для получения цвета из HSV
        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));
           
            switch (hi)
            {
                case 0: return Color.FromArgb(255, v, t, p);
                case 1: return Color.FromArgb(255, q, v, p);
                case 2: return Color.FromArgb(255, p, v, t);
                case 3: return Color.FromArgb(255, p, q, v);
                case 4: return Color.FromArgb(255, t, p, v);
                default: return Color.FromArgb(255, v, p, q);
            }
        }
    }
}
