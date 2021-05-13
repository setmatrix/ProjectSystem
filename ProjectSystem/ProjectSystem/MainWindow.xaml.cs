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
using System.Threading;


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
            train = new Rectangle()
            {
                Width = 600,
                Height = 420
            };
            train.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/train.png", UriKind.Relative))
            };
            setTrainMove();
            setCarMove();
        }
        private Random rnd;
        private Storyboard train_storyboard;
        private Rectangle train;
        private DoubleAnimation trainMove;

        private void setTrainMove()
        {
            rnd = new Random();
            train_storyboard = new Storyboard();
            trainMove = new DoubleAnimation();
            trainMove.Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(4, 15)));
            trainMove.From = 1280 + train.Width;
            trainMove.To = 0 - train.Width;
            Storyboard.SetTarget(trainMove, train);
            Storyboard.SetTargetProperty(trainMove, new PropertyPath(Canvas.LeftProperty));
            train_storyboard.Children.Add(trainMove);
            train_canvas.Children.Add(train);
            Canvas.SetRight(train, 0);
            
            train_storyboard.Begin();       
        }
        private void setCarMove()
        {
            rnd = new Random();
            int rnd_1 = rnd.Next(1, 10);
            Storyboard storyboard = new Storyboard();
            Rectangle rec = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            rec.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/BlueCar.png", UriKind.Relative))
            };
            //MOVE
            DoubleAnimation animMove = new DoubleAnimation();
            animMove.Duration = new Duration(TimeSpan.FromSeconds(rnd_1));
            animMove.From = 0 - rec.Width;
            animMove.To = 800;
            double distance_x = 800 + rec.Width;
            double  reverse_velocity = rnd_1/ distance_x;
            double distance = 0;
            double distance_y = 0;

            Storyboard.SetTarget(animMove, rec);
            Storyboard.SetTargetProperty(animMove, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animMove);
            canvas_1.Children.Add(rec);
            Canvas.SetTop(rec, 100);
            animMove.Completed += new EventHandler(curve_1);
            storyboard.Begin();
            //NEW ANIM
            void curve_1(object sender, EventArgs e)
            {
                Storyboard storyboard_1 = new Storyboard();
                //MessageBox.Show("Hello, world!");
                DoubleAnimation animMove_1_x = new DoubleAnimation();
                DoubleAnimation animMove_1_y = new DoubleAnimation();
                //animMove_1_x
                animMove_1_x.From = 800;
                animMove_1_x.To = 900;
                distance_x = 900 - 800;
                //animMove_1_y
                animMove_1_y.From = 100;
                animMove_1_y.To = 200;
                distance_y = 200 - 100;
                distance = Math.Sqrt(distance_x* distance_x + distance_y* distance_y);
                animMove_1_x.Duration = new Duration(TimeSpan.FromSeconds(reverse_velocity * distance));
                animMove_1_y.Duration = new Duration(TimeSpan.FromSeconds(reverse_velocity * distance));
                //animMove_1_x
                Storyboard.SetTarget(animMove_1_x, rec);
                Storyboard.SetTargetProperty(animMove_1_x, new PropertyPath(Canvas.LeftProperty));
                storyboard_1.Children.Add(animMove_1_x);
                //animMove_1_y
                Storyboard.SetTarget(animMove_1_y, rec);
                Storyboard.SetTargetProperty(animMove_1_y, new PropertyPath(Canvas.TopProperty));
                storyboard_1.Children.Add(animMove_1_y);
                ////
                storyboard_1.Begin();
            }
        }
        
    }
}
