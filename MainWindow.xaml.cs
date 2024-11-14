using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Fractal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            this.MinWidth = screenWidth / 2;
            this.MinHeight = screenHeight / 2;

            this.MaxWidth = screenWidth;
            this.MaxHeight = screenHeight;
        }

        private Color mainColor = Colors.Blue;
        private Color gradientColor = Colors.Aqua;

        private string selectedFractal = string.Empty;

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(selectedFractal))
            {
                fractalForCanvas.Children.Clear(); // Очищаем канвас перед отрисовкой

                switch (selectedFractal)
                {
                    case "Обдуваемое ветром фрактальное дерево":
                        DrawTree(sender, e);
                        break;
                    case "Кривая Коха":
                        DrawKochCurve(sender, e);
                        break;
                    case "Ковер Серпинского":
                        DrawSponge(sender, e);
                        break;
                    case "Треугольник Серпинского":
                        DrawTriangle(sender, e);
                        break;
                    case "Множество Кантора":
                        DrawCantorSet(sender, e);
                        break;
                }
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            fractalForCanvas.Children.Clear(); // Очищаем канвас
        }

        private void fractal_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, был ли выбран элемент
            if (cb_fractal.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedFractal = (string)selectedItem.Content; // Сохраняем выбранный фрактал

                // Устанавливаем разные значения для слайдера в зависимости от выбранного фрактала
                switch (selectedFractal)
                {
                    case "Обдуваемое ветром фрактальное дерево":
                        Step.Minimum = 1;
                        Step.Maximum = 20;
                        Step.Value = 5;
                        break;
                    case "Кривая Коха":
                        Step.Minimum = 1;
                        Step.Maximum = 6;
                        Step.Value = 3;
                        break;
                    case "Ковер Серпинского":
                        Step.Minimum = 1;
                        Step.Maximum = 6;
                        Step.Value = 4;
                        break;
                    case "Треугольник Серпинского":
                        Step.Minimum = 1;
                        Step.Maximum = 6;
                        Step.Value = 4;
                        break;
                    case "Множество Кантора":
                        Step.Minimum = 0;
                        Step.Maximum = 6;
                        Step.Value = 4;
                        break;
                }
            }
        }

        private void DrawTree(object sender, RoutedEventArgs e)
        {
            fractalForCanvas.Children.Clear(); // Очищаем канвас перед отрисовкой
            int depth = (int)Step.Value; // Выбор глубины из слайдера
            DrawBranch(fractalForCanvas, new Point(200, 400), -90, 80, depth, mainColor, gradientColor);
        }

        private void DrawBranch(Canvas canvas, Point start, double angle, double length, int depth, Color mainColor, Color gradientColor)
        {
            if (depth == 0) return;

            Point end = new Point(
                start.X + length * Math.Cos(angle * Math.PI / 180),
                start.Y + length * Math.Sin(angle * Math.PI / 180));

            Line line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = new SolidColorBrush(mainColor),
                StrokeThickness = depth
            };
            canvas.Children.Add(line);

            DrawBranch(canvas, end, angle - 20, length * 0.7, depth - 1, gradient(mainColor, gradientColor, depth), gradientColor);
            DrawBranch(canvas, end, angle + 20, length * 0.7, depth - 1, gradient(mainColor, gradientColor, depth), gradientColor);
        }

        private void DrawKochCurve(object sender, RoutedEventArgs e)
        {
            fractalForCanvas.Children.Clear(); // Очищаем канвас перед отрисовкой
            int depth = (int)Step.Value;
            DrawKoch(new Point(50, 300), new Point(350, 300), depth, mainColor, gradientColor);
        }

        private void DrawKoch(Point p1, Point p2, int depth, Color mainColor, Color gradientColor)
        {
            if (depth == 0)
            {
                Line line = new Line
                {
                    X1 = p1.X,
                    Y1 = p1.Y,
                    X2 = p2.X,
                    Y2 = p2.Y,
                    Stroke = new SolidColorBrush(mainColor),
                    StrokeThickness = 2
                };
                fractalForCanvas.Children.Add(line);
            }
            else
            {
                Point pA = new Point((2 * p1.X + p2.X) / 3, (2 * p1.Y + p2.Y) / 3);
                Point pB = new Point((p1.X + 2 * p2.X) / 3, (p1.Y + 2 * p2.Y) / 3);
                Point pPeak = new Point(
                    0.5 * (p1.X + p2.X) + Math.Sqrt(3) / 6 * (p1.Y - p2.Y),
                    0.5 * (p1.Y + p2.Y) + Math.Sqrt(3) / 6 * (p2.X - p1.X));

                DrawKoch(p1, pA, depth - 1, mainColor, gradientColor);
                DrawKoch(pA, pPeak, depth - 1, mainColor, gradientColor);
                DrawKoch(pPeak, pB, depth - 1, mainColor, gradientColor);
                DrawKoch(pB, p2, depth - 1, mainColor, gradientColor);
            }
        }

        private void DrawSponge(object sender, RoutedEventArgs e)
        {
            fractalForCanvas.Children.Clear(); // Очищаем канвас перед отрисовкой
            int depth = (int)Step.Value;
            DrawSponge(depth, 0, 0, 400, mainColor, gradientColor);
        }

        private void DrawSponge(int depth, double x, double y, double size, Color mainColor, Color gradientColor)
        {
            if (depth == 0)
            {
                Rectangle rect = new Rectangle
                {
                    Width = size,
                    Height = size,
                    Fill = new SolidColorBrush(mainColor)
                };
                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
                fractalForCanvas.Children.Add(rect);
            }
            else
            {
                double newSize = size / 3;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1) continue; // Пропускаем центр
                        DrawSponge(depth - 1, x + i * newSize, y + j * newSize, newSize, mainColor, gradientColor);
                    }
                }
            }
        }
        private void DrawTriangle(object sender, RoutedEventArgs e)
        {
            fractalForCanvas.Children.Clear(); // Очищаем канвас перед отрисовкой
            int depth = (int)Step.Value;
            Point p1 = new Point(200, 30);
            Point p2 = new Point(30, 370);
            Point p3 = new Point(370, 370);
            DrawTriangle(depth, p1, p2, p3, mainColor, gradientColor);
        }

        private void DrawTriangle(int depth, Point p1, Point p2, Point p3, Color mainColor, Color gradientColor)
    {
            if (depth == 0)
            {
                Polygon triangle = new Polygon
                {
                    Points = new PointCollection { p1, p2, p3 },
                    Fill = new SolidColorBrush(mainColor)
                };

                fractalForCanvas.Children.Add(triangle);
            }
            else
            {
                Point mid1 = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                Point mid2 = new Point((p2.X + p3.X) / 2, (p2.Y + p3.Y) / 2);
                Point mid3 = new Point((p1.X + p3.X) / 2, (p1.Y + p3.Y) / 2);

                DrawTriangle(depth - 1, p1, mid1, mid3, mainColor, gradientColor);
                DrawTriangle(depth - 1, mid1, p2, mid2, mainColor, gradientColor);
                DrawTriangle(depth - 1, mid3, mid2, p3, mainColor, gradientColor);
            }
        }
        private void DrawCantorSet(object sender, RoutedEventArgs e)
        {
            fractalForCanvas.Children.Clear(); // Очищаем канвас перед отрисовкой
            int depth = (int)Step.Value; // Глубина, выбранная пользователем
            for (int i = 0; i <= depth; i++)
            {
                DrawCantor(0, new Point(50, 20 + 40 * i), 300, i, mainColor, gradientColor);
            }
        }

        private void DrawCantor(int currentDepth, Point start, double length, int maxDepth, Color mainColor, Color gradientColor)
        {
            if (currentDepth >= maxDepth)
            {
                // Отрисовка линии
                Line line = new Line
                {
                    X1 = start.X,
                    Y1 = start.Y,
                    X2 = start.X + length,
                    Y2 = start.Y,
                    Stroke = new SolidColorBrush(mainColor),
                    StrokeThickness = 2
                };
                fractalForCanvas.Children.Add(line);
            }
            else
            {
                double thirdLength = length / 3;

                DrawCantor(currentDepth + 1, start, thirdLength, maxDepth, mainColor, gradientColor);
                DrawCantor(currentDepth + 1, new Point(start.X + 2 * thirdLength, start.Y), thirdLength, maxDepth, mainColor, gradientColor);
            }
        }

        private Color gradient(Color mainColor, Color gradientColor, int depth)
        {
            byte r = (byte)(mainColor.R + (gradientColor.R - mainColor.R) * (1.0 - (double)depth / 10));
            byte g = (byte)(mainColor.G + (gradientColor.G - mainColor.G) * (1.0 - (double)depth / 10));
            byte b = (byte)(mainColor.B + (gradientColor.B - mainColor.B) * (1.0 - (double)depth / 10));
            return Color.FromRgb(r, g, b);
        }

        private void btn_Gradient_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                gradientColor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                GradientCantorColorEllipse.Fill = new SolidColorBrush(gradientColor);
            }
        }

        private void btn_MainTree_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mainColor = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                MainTreeColorEllipse.Fill = new SolidColorBrush(mainColor);
            }
        }
    }
}
