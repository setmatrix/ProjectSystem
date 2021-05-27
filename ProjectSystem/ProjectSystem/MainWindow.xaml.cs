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
        private Rectangle BlueCar2;
        private Rectangle YellowCar;
        private Rectangle YellowCar2;
        private Rectangle RedCar;
        private Rectangle RedCar2;
        private DoubleAnimation trainMove = new DoubleAnimation();
        public List<double> list_speed = new List<double>();
        public List<int> list_speed2 = new List<int>();
        private int dex;
        private int carrnd;
        private int number = -1;
        private ScaleTransform trainrot = new ScaleTransform();
        private int kier = 0;

        Rect auto1HitBox = new Rect();
        Rect auto2HitBox = new Rect();
        public MainWindow()
        {
            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri("Images/mapa_v5.png", UriKind.Relative)));
            Background = brush;
            InitializeComponent();
            szlaban_open = new ImageBrush(new BitmapImage(new Uri("Images/szlaban_open.png", UriKind.Relative)));
            szlaban_closed = new ImageBrush(new BitmapImage(new Uri("Images/szlaban_closed.png", UriKind.Relative)));
            szlaban1 = new Rectangle()
            {
                Width = 150,
                Height = 50
            };
            szlaban1.Fill = szlaban_open;
            szlaban2 = new Rectangle()
            {
                Width = 125,
                Height = 50
            };
            szlaban2.Fill = szlaban_open;
            ScaleTransform rt = new ScaleTransform();
            rt.ScaleX = -1;
            szlaban2.RenderTransform = rt;
            train_canvas.Children.Add(szlaban1);
            Canvas.SetLeft(szlaban1, 220);
            Canvas.SetZIndex(szlaban1, 100);
            Canvas.SetBottom(szlaban1, -120);
            train_canvas.Children.Add(szlaban2);
            Canvas.SetLeft(szlaban2, 170);
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
            BlueCar2 = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            BlueCar2.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/BlueCar2.png", UriKind.Relative))
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
            RedCar2 = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            RedCar2.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/RedCar2.png", UriKind.Relative))
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
            YellowCar2 = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            YellowCar2.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/YellowCar2.png", UriKind.Relative))
            };
            setTabReverse();
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
                    //setCarMove();
                    setCarMove(animXBef, animXTo, animYBef, animYTo, canvas_2);
                }, null);
            });

            Thread carthr2 = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    //setCarMove();
                    setCarMove(animXBef, animXTo, animYBef, animYTo, canvas_2);
                }, null);
            });

            Thread carthr3 = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    //setCarMove();
                    setCarMove(animXBef, animXTo, animYBef, animYTo, canvas_2);
                }, null);
            });
            Thread carthr4 = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    //setCarMove();
                    setCarMove(animXBef, animXTo, animYBef, animYTo, canvas_2);
                }, null);
            });
            Thread carthr5 = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    //setCarMove();
                    setCarMove(animXBef, animXTo, animYBef, animYTo, canvas_2);
                }, null);
            });
            Thread carthr6 = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    //setCarMove();
                    setCarMove(animXBef, animXTo, animYBef, animYTo, canvas_2);
                }, null);
            });

            //setTrainMove();
            //setCarMove();
            rnd = new Random();
            trainthr.Start();
            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 9)));
            carthr.Start();

            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(3, 4)));
            carthr2.Start();

            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(3, 4)));
            carthr3.Start();
            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(3, 4)));
            carthr4.Start();
            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(3, 4)));
            carthr5.Start();
            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(3, 4)));
            carthr6.Start();
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
            kier = rnd.Next(1, 3);
            if(kier == 1)
            {
                trainrot.ScaleX = -1;
                train.RenderTransform = trainrot;
                trainMove.From = 0 - train.Width;
                trainMove.To = 1280 + train.Width;
            }
            else if(kier == 2)
            {
                trainrot.ScaleX = 1;
                train.RenderTransform = trainrot;
                trainMove.From = 1280 + train.Width;
                trainMove.To = 0 - train.Width;
            }
            
            train_storyboard.Begin();
            szlaban1.Fill = szlaban_closed;
            szlaban2.Fill = szlaban_closed;
        }

        private Rectangle TakeTheCar(int ch)
        {
            switch(ch)
            {
                case 1:
                    return BlueCar;
                case 2:
                    return YellowCar;
                case 3:
                    return RedCar;
                case 4:
                    return BlueCar2;
                case 5:
                    return YellowCar2;
                case 6:
                    return RedCar2;
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
            szlaban2.Fill = szlaban_closed;
        }

        private int[] animXBef = new int[7];
        private int[] animXTo = new int[7];
        private int[] animYBef = new int[7];
        private int[] animYTo = new int[7];

        private void setTabReverse()
        {
            animXBef[0] = 1200;
            animXTo[0] = 240;
            animYBef[0] = 110;
            animYTo[0] = 110;

            animXBef[1] = animXTo[0];
            animXTo[1] = 150;
            animYBef[1] = animYTo[0];
            animYTo[1] = -25;

            animXBef[2] = animXTo[1];
            animXTo[2] = 230;
            animYBef[2] = animYTo[1];
            animYTo[2] = -100;

            animXBef[3] = animXTo[2];
            animXTo[3] = 850;
            animYBef[3] = animYTo[2];
            animYTo[3] = -90;
            
            animXBef[4] = animXTo[3];
            animXTo[4] = 1000;
            animYBef[4] = animYTo[3];
            animYTo[4] = -260;

            animXBef[5] = animXTo[4];
            animXTo[5] = 900;
            animYBef[5] = animYTo[4];
            animYTo[5] = -420;

            animXBef[6] = animXTo[5];
            animXTo[6] = -78;
            animYBef[6] = animYTo[5];
            animYTo[6] = -400;
        }

        private void setCarMove(int[] animxbef, int[] animxto, int[] animybef, int[] animyto, Canvas can)
        {
            Random rnd = new Random();

            number++;


            carrnd = number+1;

            int dex = rnd.Next(3, 7);

            int index = 0;
            Control.Content = index;
            Rectangle autko = TakeTheCar(carrnd);
            autko.Name = "auto" + number;
            list_speed2.Add(dex);
            Storyboard story = new Storyboard();
            DoubleAnimation animMove_x = new DoubleAnimation();
            DoubleAnimation animMove_y = new DoubleAnimation();
                //animMove_1_x
                animMove_x.From = animXBef[index];
                animMove_x.To = animXTo[index];
                //distance_x = 900 - 800;
                //animMove_1_y
                animMove_y.From = animYBef[index];
                animMove_y.To = animYTo[index];
                animMove_x.Duration = new Duration(TimeSpan.FromSeconds(list_speed2[number]));
                animMove_y.Duration = new Duration(TimeSpan.FromSeconds(list_speed2[number]));
                //animMove_1_x
                Storyboard.SetTarget(animMove_x, autko);
                Storyboard.SetTargetProperty(animMove_x, new PropertyPath(Canvas.LeftProperty));
                story.Children.Add(animMove_x);
                //animMove_1_y
                Storyboard.SetTarget(animMove_y, autko);
                Storyboard.SetTargetProperty(animMove_y, new PropertyPath(Canvas.TopProperty));
                story.Children.Add(animMove_y);
            ////
            double reverse_velocity = (double)dex / Math.Abs((animXTo[index]- animXBef[index])) ;
            can.Children.Add(autko);
            //list_speed.Add(dex/(animXBef[index]-animXTo[index]));
            list_speed.Add(reverse_velocity);
            Canvas.SetTop(autko, 125);
            Canvas.SetLeft(autko, 78);
            //index++;
            
            story.Completed += new EventHandler(MoveReverse);
            story.CurrentTimeInvalidated += new EventHandler(MainTimerEvent);
            story.Begin();

            void MainTimerEvent(object sender, EventArgs e)
            {
                foreach (var x in can.Children.OfType<Rectangle>())
                {
    
                    auto1HitBox = new Rect(Canvas.GetLeft(autko)-35/dex, Canvas.GetTop(autko), autko.Width + 30, autko.Height+12);
                    auto2HitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width + 30, x.Height +12);

                    if (auto1HitBox.IntersectsWith(auto2HitBox) && (string)x.Name != (string)autko.Name && (autko.Name[(autko.Name).Length - 1]) > (x.Name[(x.Name).Length - 1]))
                    {

                        double pomocnicza = list_speed[(autko.Name[(autko.Name).Length - 1]) - 48];
                        double pomocnicza2 = list_speed[(x.Name[(x.Name).Length - 1]) - 48];

                        animMove_x.Duration = new Duration(TimeSpan.FromSeconds(list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48]));
                        animMove_y.Duration = new Duration(TimeSpan.FromSeconds(list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48]));

                        story.SetSpeedRatio((double)(pomocnicza / pomocnicza2));

                        autko.Name = x.Name;                   

                    }
                }
            }
            

            void MoveReverse(object sender, EventArgs e)
            {
                if (index < 6)
                {
                    index++;
                    story.CurrentTimeInvalidated += new EventHandler(MainTimerEvent);
                    Control.Content = index;
                    //animMove_1_x
                    animMove_x.From = animxbef[index];
                    animMove_x.To = animxto[index];
                    //distance_x = 900 - 800;
                    //animMove_1_y
                    animMove_y.From = animybef[index];
                    animMove_y.To = animyto[index];
                    //animMove_1_x
                    Storyboard.SetTarget(animMove_x, autko);
                    Storyboard.SetTargetProperty(animMove_x, new PropertyPath(Canvas.LeftProperty));
                    story.Children.Add(animMove_x);
                    //animMove_1_y
                    Storyboard.SetTarget(animMove_y, autko);
                    Storyboard.SetTargetProperty(animMove_y, new PropertyPath(Canvas.TopProperty));
                    story.Children.Add(animMove_y);
                    ////


                    //Zmiana prędkości na zakrętach
                    //Narazie na oko, poźniej można dokładnie wyznaczyć
                    switch (index)
                    {
                        case 1:
                        
                            animMove_x.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 4.5));
                            animMove_y.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 4.5));
                            break;
                        case 2:
                            animMove_x.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 6));
                            animMove_y.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 6));
                            break;
                        case 3:
                            animMove_x.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 1.5));
                            animMove_y.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 1.5));
                            break;
                        case 4:
                            animMove_x.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 3.5));
                            animMove_y.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 3.5));
                            break;
                        case 5:
                            animMove_x.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 4));
                            animMove_y.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 4));
                            break;
                        case 6:
                            animMove_x.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 1.2));
                            animMove_y.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] / 1.2));
                            break;
                    }
                    
                    
                    story.Begin();
                }
                else
                {
                    canvas_2.Children.Remove(autko);
                    story.Remove();
                    return;
                }
            }
        }

        //private void setCarMove()
        //{
        //    rnd = new Random();
        //    number++;

            
        //    // ABy sprawdzić jak auto się zbliża do innego auto, bez zbędnego ponownego urochamiania aby dobre prędkości wylosowało
        //    if (number == 0)
        //    {
        //        rnd_1 = 8;
        //        carrnd = 1;
        //    }
        //    else if (number == 1)
        //    {
        //        rnd_1 = 4;
        //        carrnd = 2;
        //    }
        //    else if (number == 2)
        //    {
        //        rnd_1 = 1;
        //        carrnd = 3;
        //    }
        //    //rnd_1 = rnd.Next(1, 10);
        //    //int carrnd = rnd.Next(1, 4);
        //    Storyboard storyboard = new Storyboard();
        //    //MOVE
        //    DoubleAnimation animMove = new DoubleAnimation();
        //    Rectangle rec = TakeTheCar(carrnd);
        //    rec.Name = "myRectangle" + number;
        //    animMove.Duration = new Duration(TimeSpan.FromSeconds(rnd_1));
        //        animMove.From = 0 - rec.Width;
        //        animMove.To = 800;
        //    double distance_x = (double)(animMove.To+ rec.Width);
        //    double reverse_velocity = rnd_1/ distance_x;
        //    double distance = 0;
        //    double distance_y = 0;
        //    list_speed.Add(reverse_velocity);
        //    Storyboard.SetTarget(animMove, rec);
        //    Storyboard.SetTargetProperty(animMove, new PropertyPath(Canvas.LeftProperty));
        //    storyboard.Children.Add(animMove);
        //    canvas_1.Children.Add(rec);
        //    Canvas.SetTop(rec, 125);
        //    animMove.Completed += new EventHandler(curve_1);
        //    animMove.CurrentTimeInvalidated += new EventHandler(MainTimerEvent);
        //    storyboard.Begin();
        //    //NEW ANIM

        //    void MainTimerEvent(object sender, EventArgs e)
        //    {
        //        foreach (var x in canvas_1.Children.OfType<Rectangle>())
        //        {
        //            //W załeżności jak szybko auto jedzie inne potrzebujemy Canvas.GetLeft(rec)+4+35/rnd_1
        //            Rect auto1HitBox = new Rect(Canvas.GetLeft(rec)+3+35/rnd_1, Canvas.GetTop(rec), rec.Width, rec.Height);
        //            Rect auto2HitBox = new Rect(Canvas.GetLeft(x) - 20, Canvas.GetTop(x), x.Width, x.Height);

        //            if (auto1HitBox.IntersectsWith(auto2HitBox) && (string)x.Name != (string)rec.Name && (rec.Name[(rec.Name).Length - 1]) > (x.Name[(x.Name).Length - 1]))
        //            {
        //                storyboard.SetSpeedRatio(list_speed[(rec.Name[(rec.Name).Length - 1]) - 48] / list_speed[(x.Name[(x.Name).Length - 1]) - 48]);
        //                rec.Name = x.Name;
        //            }
        //        }
        //    }

        //    void curve_1(object sender, EventArgs e)
        //    {
        //        Storyboard storyboard_1 = new Storyboard();
        //        //MessageBox.Show("Hello, world!");
        //        DoubleAnimation animMove_1_x = new DoubleAnimation() ;
        //        DoubleAnimation animMove_1_y = new DoubleAnimation() ;
        //        //animMove_1_x
        //        animMove_1_x.From = 800;
        //        animMove_1_x.To = 900;
        //        distance_x = 900 - 800;
        //        //animMove_1_y
        //        animMove_1_y.From = 125;
        //        animMove_1_y.To = 200;
        //        distance_y = (double)(animMove_1_y.To - animMove_1_y.From);
        //        distance = Math.Sqrt(distance_x* distance_x + distance_y* distance_y);
        //        animMove_1_x.Duration = new Duration(TimeSpan.FromSeconds(reverse_velocity * distance));
        //        animMove_1_y.Duration = new Duration(TimeSpan.FromSeconds(reverse_velocity * distance));
        //        //animMove_1_x
        //        Storyboard.SetTarget(animMove_1_x, rec);
        //        Storyboard.SetTargetProperty(animMove_1_x, new PropertyPath(Canvas.LeftProperty));
        //        storyboard_1.Children.Add(animMove_1_x);
        //        //animMove_1_y
        //        Storyboard.SetTarget(animMove_1_y, rec);
        //        Storyboard.SetTargetProperty(animMove_1_y, new PropertyPath(Canvas.TopProperty));
        //        storyboard_1.Children.Add(animMove_1_y);
        //        ////
        //        animMove_1_x.CurrentTimeInvalidated += new EventHandler(MainTimerEvent);
        //        animMove_1_y.CurrentTimeInvalidated += new EventHandler(MainTimerEvent);
        //        storyboard_1.Begin();
        //    }
        //}
        
    }
}
