using ControllerEmulatorAPI.Global.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ControllerEmulatorAPI.UserInterface.UserControls.Selectors
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl, ISetting
    {
        private readonly List<KeyValuePair<Line, GradientStopCollection>> lines = new List<KeyValuePair<Line, GradientStopCollection>>();

        public HSVColor CurrentColor { get => (HSVColor)Value; private set { Value = value; OnValueChanged?.Invoke(this, null); } }

        public event EventHandler OnValueChanged;

        public bool IgnoreEvents { get; set; } = true;
        public object Value { get; set; }

        public ColorPicker()
        {
            InitializeComponent();
            IgnoreEvents = false;
            AdvancedSlidersGrid.Visibility = Visibility.Collapsed;
            BuildColorWheel();
        }

        private void BuildColorWheel()
        {
            lines.Clear();
            ColorWheelBox.Children.Clear();
            for (int i = 0; i < 360;)
            {
                var direction = new Point(Math.Cos((i - 90) * Math.PI / 180) * 80, Math.Sin((i - 90) * Math.PI / 180) * 80);
                var stop0 = new GradientStop(HSVColor.FromHSV((int)AlphaSlider.Value, i, 0, LightSlider.Value), 0);
                var stop1 = new GradientStop(HSVColor.FromHSV((int)AlphaSlider.Value, i, 1, LightSlider.Value), 1);
                var stops = new GradientStopCollection { stop0, stop1 };
                var brush = new LinearGradientBrush(stops, default, direction)
                {
                    MappingMode = BrushMappingMode.Absolute
                };

                Line line = new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = direction.X,
                    Y2 = direction.Y,
                    Stroke = brush,
                    StrokeThickness = 3,
                    RenderTransform = new TranslateTransform(80, 80)
                };
                lines.Add(new KeyValuePair<Line, GradientStopCollection>(line, stops));
                ColorWheelBox.Children.Add(line);
                i++;
            }
        }

        private void UpdateColorWheel()
        {
            int i = 0;
            foreach (var line in lines)
            {
                lock (line.Key.Stroke)
                {
                    line.Value[0].Color = HSVColor.FromHSV((int)AlphaSlider.Value, i, 0, LightSlider.Value);
                    line.Value[1].Color = HSVColor.FromHSV((int)AlphaSlider.Value, i, 1, LightSlider.Value);
                }
                i++;
            }
        }

        private void UpdateUI()
        {
            LightSlider.Value = CurrentColor.Value;
            AlphaSlider.Value = CurrentColor.Color.A;
            RedSlider.Value = CurrentColor.Color.R;
            GreenSlider.Value = CurrentColor.Color.G;
            BlueSlider.Value = CurrentColor.Color.B;
            AlphaTextBox.Text = CurrentColor.Color.A.ToString(CultureInfo.CurrentCulture);
            RedTextBox.Text = CurrentColor.Color.R.ToString(CultureInfo.CurrentCulture);
            GreenTextBox.Text = CurrentColor.Color.G.ToString(CultureInfo.CurrentCulture);
            BlueTextBox.Text = CurrentColor.Color.B.ToString(CultureInfo.CurrentCulture);
            LightSlider.Value = CurrentColor.Value;
            Point point = new Point(CurrentColor.GetPoint().X * 80, CurrentColor.GetPoint().Y * 80);
            ColorSelector.RenderTransform = new TranslateTransform(point.X, point.Y);
            ColorPreview.Fill = new SolidColorBrush(CurrentColor);
        }

        private void UpdateUI(MouseEventArgs args)
        {
            var point = args.GetPosition(ColorWheelBox);
            point.Offset(-80, -80);
            point.X *= -1;
            var angle = point.GetAngleDegree(default) % 360;
            var distance = ((Point)default).GetDistance(point) / 80;
            if (distance > 1)
            {
                distance = 1;
            }

            CurrentColor = HSVColor.FromHSV((int)AlphaSlider.Value, angle, distance, LightSlider.Value);
            UpdateUI();
        }

        private int ParseInt(string str)
        {
            if (int.TryParse(str, NumberStyles.Integer, CultureInfo.CurrentCulture, out int f))
            {
                if (f <= 0)
                {
                    return 0;
                }

                if (f >= 255)
                {
                    return 255;
                }

                return f;
            }
            else
            {
                return 0;
            }
        }

        private void ColorWheelBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;
            UpdateUI(e);
            IgnoreEvents = false;
        }

        private void ColorWheelBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IgnoreEvents = true;
                UpdateUI(e);
                IgnoreEvents = false;
            }
        }

        private void LightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;
            CurrentColor = HSVColor.FromHSV((int)CurrentColor.Alpha, CurrentColor.Hue, CurrentColor.Saturation, (int)LightSlider.Value);
            UpdateColorWheel();
            UpdateUI();
            IgnoreEvents = false;
        }

        private void AlphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;
            CurrentColor = HSVColor.FromHSV((int)AlphaSlider.Value, CurrentColor.Hue, CurrentColor.Saturation, CurrentColor.Value);
            UpdateColorWheel();
            UpdateUI();
            IgnoreEvents = false;
        }

        private void AlphaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;

            int value = ParseInt(AlphaTextBox.Text);
            CurrentColor = HSVColor.FromHSV(value, CurrentColor.Hue, CurrentColor.Saturation, CurrentColor.Value);
            AlphaTextBox.Text = value.ToString(CultureInfo.CurrentCulture);
            UpdateColorWheel();
            IgnoreEvents = false;
        }

        private void RedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;
            CurrentColor = HSVColor.FromRGB((int)CurrentColor.Alpha, (int)RedSlider.Value, CurrentColor.Color.G, CurrentColor.Color.B);
            UpdateColorWheel();
            UpdateUI();
            IgnoreEvents = false;
        }

        private void RedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;

            int value = ParseInt(RedTextBox.Text);
            CurrentColor = HSVColor.FromRGB((int)CurrentColor.Alpha, value, CurrentColor.Color.G, CurrentColor.Color.B);
            RedTextBox.Text = value.ToString(CultureInfo.CurrentCulture);
            UpdateColorWheel();
            IgnoreEvents = false;
        }

        private void GreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;
            CurrentColor = HSVColor.FromRGB((int)CurrentColor.Alpha, CurrentColor.Color.R, (int)GreenSlider.Value, CurrentColor.Color.B);
            UpdateColorWheel();
            UpdateUI();
            IgnoreEvents = false;
        }

        private void GreenTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;

            int value = ParseInt(GreenTextBox.Text);
            CurrentColor = HSVColor.FromRGB((int)CurrentColor.Alpha, CurrentColor.Color.R, value, CurrentColor.Color.B);
            GreenTextBox.Text = value.ToString(CultureInfo.CurrentCulture);
            UpdateColorWheel();
            IgnoreEvents = false;
        }

        private void BlueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;
            CurrentColor = HSVColor.FromRGB((int)CurrentColor.Alpha, CurrentColor.Color.R, CurrentColor.Color.G, (int)BlueSlider.Value);
            UpdateColorWheel();
            UpdateUI();
            IgnoreEvents = false;
        }

        private void BlueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IgnoreEvents)
            {
                return;
            }

            IgnoreEvents = true;

            int value = ParseInt(BlueTextBox.Text);
            CurrentColor = HSVColor.FromRGB((int)CurrentColor.Alpha, CurrentColor.Color.R, CurrentColor.Color.G, value);
            BlueTextBox.Text = value.ToString(CultureInfo.CurrentCulture);
            UpdateColorWheel();
            IgnoreEvents = false;
        }

        private void Advanced_Click(object sender, RoutedEventArgs e)
        {
            if (AdvancedSlidersGrid.Visibility == Visibility.Visible)
            {
                AdvancedSlidersGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                AdvancedSlidersGrid.Visibility = Visibility.Visible;
            }
        }

        public void SetValue(object obj)
        {
            if (obj is Color color)
            {
                IgnoreEvents = true;
                CurrentColor = HSVColor.FromRGB(color.A, color.R, color.G, color.B);
                UpdateUI();
                UpdateColorWheel();
                IgnoreEvents = false;
            }
        }

        public void ClearEvents()
        {
            if (OnValueChanged != null)
            {
                foreach (Delegate @delegate in OnValueChanged.GetInvocationList())
                {
                    OnValueChanged -= (EventHandler)@delegate;
                }
            }
        }

        public struct HSVColor
        {
            public Color Color { get; set; }

            public double Alpha { get; set; }

            public double Hue { get; set; }

            public double Saturation { get; set; }

            public double Value { get; set; }

            public static HSVColor FromHSV(int a, double h, double s, double v)
            {
                return new HSVColor { Alpha = a, Hue = h, Saturation = s, Value = v, Color = ColorFromHSV(a, h, s, v) };
            }

            public static HSVColor FromRGB(int a, int r, int g, int b)
            {
                var hsv = RGBToHSV(Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b));
                return new HSVColor { Alpha = a, Hue = hsv.Item1, Saturation = hsv.Item2, Value = hsv.Item3, Color = Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b) };
            }

            public static HSVColor FromColor(Color color)
            {
                var hsv = RGBToHSV(color);
                return new HSVColor { Alpha = color.A, Hue = hsv.Item1, Saturation = hsv.Item2, Value = hsv.Item3, Color = color };
            }

            public static Color ColorFromHSV(double alpha, double hue, double saturation, double value)
            {
                int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
                double f = (hue / 60) - Math.Floor(hue / 60);
                int v = Convert.ToInt32(value);
                int p = Convert.ToInt32(value * (1 - saturation));
                int q = Convert.ToInt32(value * (1 - (f * saturation)));
                int t = Convert.ToInt32(value * (1 - ((1 - f) * saturation)));

                return hi switch
                {
                    0 => Color.FromArgb(Convert.ToByte(alpha), Convert.ToByte(v), Convert.ToByte(t), Convert.ToByte(p)),
                    1 => Color.FromArgb(Convert.ToByte(alpha), Convert.ToByte(q), Convert.ToByte(v), Convert.ToByte(p)),
                    2 => Color.FromArgb(Convert.ToByte(alpha), Convert.ToByte(p), Convert.ToByte(v), Convert.ToByte(t)),
                    3 => Color.FromArgb(Convert.ToByte(alpha), Convert.ToByte(p), Convert.ToByte(q), Convert.ToByte(v)),
                    4 => Color.FromArgb(Convert.ToByte(alpha), Convert.ToByte(t), Convert.ToByte(p), Convert.ToByte(v)),
                    _ => Color.FromArgb(Convert.ToByte(alpha), Convert.ToByte(v), Convert.ToByte(p), Convert.ToByte(q))
                };
            }

            public static (float, float, float) RGBToHSV(Color color)
            {
                System.Drawing.Color colorGDI = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                return (colorGDI.GetHue(), colorGDI.GetSaturation(), colorGDI.GetBrightness() * 255);
            }

            public Color CreateColorWithAlpha(int alhpa)
            {
                return Color.FromArgb((byte)alhpa, Color.R, Color.G, Color.B);
            }

            public Point GetPoint()
            {
                var direction = new Point(Math.Cos((Hue - 90) * Math.PI / 180) * Saturation, Math.Sin((Hue - 90) * Math.PI / 180) * Saturation);
                return direction;
            }

            public static implicit operator Color(HSVColor c) => c.Color;
        }
    }

    public static class MathExtensions
    {
        public static double GetAngleDegree(this Point origin, Point target)
        {
            var n = 270 - (Math.Atan2(origin.Y - target.Y, origin.X - target.X) * 180 / Math.PI);
            return n % 360;
        }

        public static double GetDistance(this Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
    }
}