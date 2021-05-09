using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri("Images/mapa_v5.png", UriKind.Relative)));
            Background = brush;
            InitializeComponent();
            setTrainMove();
        }
        private void setTrainMove()
        {
            Storyboard BubbleStoryboard = new Storyboard();
            Rectangle rec = new Rectangle()
            {
                Width = 400,
                Height = 280
            };
            rec.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/train.png", UriKind.Relative))
            };
            //- MOVE - 
            DoubleAnimation animMove = new DoubleAnimation();
            animMove.Duration = new Duration(TimeSpan.FromSeconds(10));
            animMove.From = 1280 + rec.Width;
            animMove.To = 0 - rec.Width;
            Storyboard.SetTarget(animMove, rec);
            Storyboard.SetTargetProperty(animMove, new PropertyPath(Canvas.LeftProperty));
            BubbleStoryboard.Children.Add(animMove);
            canvas.Children.Add(rec);
            Canvas.SetTop(rec, 0);
            BubbleStoryboard.Begin();
        }
    }
}
