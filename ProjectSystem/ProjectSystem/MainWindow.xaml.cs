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
using System.Windows.Threading;

namespace ProjectSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rnd;
        private Rectangle szlaban1;
        private Rectangle szlaban2;
        private ImageBrush szlaban_open;
        private ImageBrush szlaban_closed;
        private Storyboard train_storyboard = new Storyboard();
        private Rectangle train;
        private Rectangle BlueCar;
        private Rectangle YellowCar;
        private Rectangle RedCar;
        private DoubleAnimation trainMove = new DoubleAnimation();

        public MainWindow()
        {
            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri("Images/mapa_v5.png", UriKind.Relative)));
            Background = brush;
            InitializeComponent();
            szlaban_open = new ImageBrush(new BitmapImage(new Uri("Images/szlaban_open.png", UriKind.Relative)));
            szlaban_closed = new ImageBrush(new BitmapImage(new Uri("Images/szlaban_closed.png", UriKind.Relative)));
            szlaban1 = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            szlaban1.Fill = szlaban_open;
            szlaban2 = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            szlaban2.Fill = szlaban_open;
            train_canvas.Children.Add(szlaban1);
            Canvas.SetLeft(szlaban1, 250);
            Canvas.SetZIndex(szlaban1, 100);
            Canvas.SetBottom(szlaban1, -120);
            train_canvas.Children.Add(szlaban2);
            Canvas.SetLeft(szlaban2, 220);
            Canvas.SetBottom(szlaban2, -50);
            train = new Rectangle()
            {
                Width = 600,
                Height = 420
            };
            train.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/train.png", UriKind.Relative))
            };
            BlueCar = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            BlueCar.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/BlueCar.png", UriKind.Relative))
            };
            RedCar = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            RedCar.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/RedCar.png", UriKind.Relative))
            };
            YellowCar = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            YellowCar.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/YellowCar.png", UriKind.Relative))
            };
            Storyboard.SetTarget(trainMove, train);
            Storyboard.SetTargetProperty(trainMove, new PropertyPath(Canvas.LeftProperty));
            trainMove.From = 1280 + train.Width;
            trainMove.To = 0 - train.Width;
            train_storyboard.Children.Add(trainMove);
            train_canvas.Children.Add(train);
            Canvas.SetRight(train, 0);

            Thread thr = new Thread(Threads);
            thr.Start();
        }

        private void Threads()
        {
            Thread trainthr = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    setTrainMove();
                }, null);
            });
            //Thread trainwait = new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;
            //    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
            //    {
            //        TrainRestart();
            //    }, null);
            //});
            Thread carthr = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    setCarMove();
                }, null);
            });
            //setTrainMove();
            //setCarMove();
            rnd = new Random();
            trainthr.Start();
            //Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 9)));
            carthr.Start();
            //trainwait.Start();
        }

        private async void TrainRestart(object sender, EventArgs e)
        {
            szlaban1.Fill = szlaban_open;
            szlaban2.Fill = szlaban_open;
            rnd = new Random();
            await Task.Delay(rnd.Next(2000, 10000));
            train_storyboard.Stop();
            rnd = new Random();
            trainMove.Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(4, 13)));
            
            train_storyboard.Begin();
            szlaban1.Fill = szlaban_closed;
            szlaban2.Fill = szlaban_closed;
        }

        private Object TakeTheCar(int ch)
        {
            switch(ch)
            {
                case 1:
                    return BlueCar;
                case 2:
                    return YellowCar;
                case 3:
                    return RedCar;
            }
            return BlueCar;
        }

        private void setTrainMove()
        {
            rnd = new Random();
            trainMove.Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(4, 13)));
            
            train_storyboard.Completed += new EventHandler(TrainRestart);

            train_storyboard.Begin();
            szlaban1.Fill = szlaban_closed;
            szlaban2.Fill =szlaban_closed;
        }

        //void onButtonClick(object sender, EventArgs e)
        //{
        //    Delay(1000, (o, a) => MessageBox.Show("Test"));
        //}

        //static void Delay(int ms, EventHandler action)
        //{
        //    var tmp = new Timer { Interval = ms };
        //    tmp.Tick += new EventHandler((o, e) => tmp.Enabled = false);                     <====== na póżniej
        //    tmp.Tick += action;
        //    tmp.Enabled = true;
        //}
        private void setCarMove()
        {
            rnd = new Random();
            int rnd_1 = rnd.Next(1, 10);
            int carrnd = rnd.Next(1, 4);
            Storyboard storyboard = new Storyboard();
            //MOVE
            DoubleAnimation animMove = new DoubleAnimation();
            Rectangle rec = (Rectangle)TakeTheCar(carrnd);
            animMove.Duration = new Duration(TimeSpan.FromSeconds(rnd_1));
                animMove.From = 0 - rec.Width;
                animMove.To = 800;
            double distance_x = 800 + rec.Width;
            double reverse_velocity = rnd_1/ distance_x;
            double distance = 0;
            double distance_y = 0;

            Storyboard.SetTarget(animMove, rec);
            Storyboard.SetTargetProperty(animMove, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animMove);
            canvas_1.Children.Add(rec);
            Canvas.SetTop(rec, 125);
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
