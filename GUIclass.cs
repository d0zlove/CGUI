using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CGUI
{
    public class GUIclass : Control
    {
        private Timer _animationTimer;
        private double _hue = 210;
        private double _saturation = 0.7;
        private double _value = 1.0;
        private bool _isHovered = false;
        private bool _isIncreasingHue = true;

        private float _innerCircleSize = 1.0f;  // Исходный размер светло-голубого круга (отношение к основному)
        private bool _isExpandingInner = false;  // Флаг для изменения размера светло-голубого круга

        public GUIclass()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);

            DoubleBuffered = true;
            Size = new Size(500, 500);

            _animationTimer = new Timer { Interval = 50 };
            _animationTimer.Tick += (s, e) =>
            {
                // Анимация изменения оттенка
                if (_hue >= 240) _isIncreasingHue = false;
                else if (_hue <= 180) _isIncreasingHue = true;

                _hue += _isIncreasingHue ? 1 : -1;

                // Анимация изменения размера внутреннего круга (светло-голубого)
                if (_innerCircleSize >= 1.0f) _isExpandingInner = false;
                else if (_innerCircleSize <= 0.8f) _isExpandingInner = true;

                if (_isHovered && _innerCircleSize <= 1.0f)
                {
                    _innerCircleSize -= 0.01f;
                }
                else
                    _innerCircleSize += _isExpandingInner ? 0.01f : -0.01f;

                Invalidate();
            };
            _animationTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.Clear(Parent.BackColor);

            int innerMargin = 20;
            float innerSize = Width - innerMargin * 2;
            RectangleF innerRect = new RectangleF(
                (Width - innerSize * _innerCircleSize) / 2,
                (Height - innerSize * _innerCircleSize) / 2,
                innerSize * _innerCircleSize, innerSize * _innerCircleSize);
            using (Brush innerBrush = new SolidBrush(Color.LightBlue))
            {
                graph.FillEllipse(innerBrush, innerRect);
            }

            // Основной круг с градиентом
            int mainMargin = 30;
            float mainSize = Width - mainMargin * 2;
            RectangleF mainRect = new RectangleF(mainMargin, mainMargin, mainSize, mainSize);

            Color animatedColor1 = ColorFromHSV(_hue, _saturation, _value);
            Color animatedColor2 = ColorFromHSV(_hue + 30, _saturation, _value);

            if (_isHovered)
            {
                animatedColor1 = ColorFromHSV(_hue, 0.6f, _value);
                animatedColor2 = ColorFromHSV(_hue + 30, 0.6f, _value);
            }

            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                mainRect,
                animatedColor1,
                animatedColor2,
                LinearGradientMode.ForwardDiagonal))
            {
                graph.FillEllipse(gradientBrush, mainRect);
            }

            // Рисуем текст "START" в центре круга
            string text = "START";
            Font font = new Font("Arial", 12, FontStyle.Bold);
            SizeF textSize = graph.MeasureString(text, font);
            PointF textPosition = new PointF(
                (Width - textSize.Width) / 2,
                (Height - textSize.Height) / 2
            );

            using (Brush textBrush = new SolidBrush(Color.Black))
            {
                graph.DrawString(text, font, textBrush, textPosition);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Cursor = Cursors.Hand;  // Устанавливаем курсор "рука" при наведении
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int size = Math.Min(Width, Height);
            Size = new Size(size, size);
        }

        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            return hi switch
            {
                0 => Color.FromArgb(255, v, t, p),
                1 => Color.FromArgb(255, q, v, p),
                2 => Color.FromArgb(255, p, v, t),
                3 => Color.FromArgb(255, p, q, v),
                4 => Color.FromArgb(255, t, p, v),
                _ => Color.FromArgb(255, v, p, q),
            };
        }
    }
}
