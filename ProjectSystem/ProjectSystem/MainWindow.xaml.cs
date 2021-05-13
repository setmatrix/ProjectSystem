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
            setTrainMove();
            setCarMove();
        }
        private Random rnd;
        private void setTrainMove()
        {
            rnd = new Random();
            Storyboard storyboard = new Storyboard();
            Rectangle rec = new Rectangle()
            {
                Width = 600,
                Height = 420
            };
            rec.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/train.png", UriKind.Relative))
            };
            //- MOVE - 

            DoubleAnimation animMove = new DoubleAnimation();
            animMove.Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(1, 10)));
            animMove.From = 1280 + rec.Width;
            animMove.To = 0 - rec.Width;
            //animMove.To = 0 - rec.Width;
            Storyboard.SetTarget(animMove, rec);
            Storyboard.SetTargetProperty(animMove, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animMove);
            canvas.Children.Add(rec);
            Canvas.SetTop(rec, 0);
            //animMove.Completed += new EventHandler(animate_Completed);
            storyboard.Begin();
            /*
            void animate_Completed(object sender, EventArgs e)
            {
                //MessageBox.Show("Hello, world!");


                DoubleAnimation animMove_1 = new DoubleAnimation();
                animMove_1.Duration = new Duration(TimeSpan.FromSeconds(10));
                animMove_1.From = 100;
                animMove_1.To = -100;
                //canvas.Children.Remove(rec);
                storyboard.Children.Remove(animMove);
                Storyboard.SetTarget(animMove_1, rec);
                Storyboard.SetTargetProperty(animMove_1, new PropertyPath(Canvas.TopProperty));
                storyboard.Children.Add(animMove_1);
                //canvas_1.Children.Add(rec);
                //Canvas.SetTop(rec, 0);
                animMove_1.Completed += new EventHandler(animate_Completed_1);
                storyboard.Begin();
            }
            
            void animate_Completed_1(object sender, EventArgs e)
            {
               // MessageBox.Show("Hello, world!");


                DoubleAnimation animMove_1 = new DoubleAnimation();
                animMove_1.Duration = new Duration(TimeSpan.FromSeconds(10));
                animMove_1.From = 100;
                animMove_1.To = -200;
                //canvas.Children.Remove(rec);
                storyboard.Children.Remove(animMove);
                Storyboard.SetTarget(animMove_1, rec);
                Storyboard.SetTargetProperty(animMove_1, new PropertyPath(Canvas.TopProperty));
                storyboard.Children.Add(animMove_1);
                //canvas_1.Children.Add(rec);
                //Canvas.SetTop(rec, 0);
                storyboard.Begin();
            }
            */
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
