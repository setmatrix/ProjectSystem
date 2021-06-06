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
        private Rectangle swiatla1;
        private Rectangle swiatla2;

        private ImageBrush szlaban_open;
        private ImageBrush szlaban_closed;
        private ImageBrush szlaban_off;
        private ImageBrush szlaban_on1;
        private ImageBrush szlaban_on2;

        private Storyboard train_storyboard = new Storyboard();

        private Rectangle train;

        private DoubleAnimation trainMove = new DoubleAnimation();

        public List<double> list_speed = new List<double>();
        public List<int> list_speed2 = new List<int>();

        private int leftrightlight = 0;

        private int number = -1;

        private ScaleTransform trainrot = new ScaleTransform();

        private bool train_isMoving = false;

        private DispatcherTimer timer;

        Rect auto1HitBox = new Rect();
        Rect auto2HitBox = new Rect();

        public MainWindow()
        {
            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri("Images/mapa_v5.png", UriKind.Relative)));
            Background = brush;
            InitializeComponent();
            szlaban_init();
            train_init();
            setTabs();
            Thread thr = new Thread(Threads);
            thr.Start();
        }

        private void train_init()
        {
            train = new Rectangle()
            {
                Width = 600,
                Height = 420
            };
            train.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Images/train.png", UriKind.Relative))
            };
            Storyboard.SetTarget(trainMove, train);
            Storyboard.SetTargetProperty(trainMove, new PropertyPath(Canvas.LeftProperty));
            trainMove.From = 1280 + train.Width;
            trainMove.To = 0 - train.Width;
            train_storyboard.Children.Add(trainMove);
            train_canvas.Children.Add(train);
            Canvas.SetRight(train, 0);
            Canvas.SetZIndex(train, 100);
        }

        private void szlaban_init()
        {
            szlaban_open = new ImageBrush(new BitmapImage(new Uri("Images/szlaban_open.png", UriKind.Relative)));
            szlaban_closed = new ImageBrush(new BitmapImage(new Uri("Images/szlaban_closed.png", UriKind.Relative)));
            szlaban_off = new ImageBrush(new BitmapImage(new Uri("Images/swiatla_off.png", UriKind.Relative)));
            szlaban_on1 = new ImageBrush(new BitmapImage(new Uri("Images/swiatla_on1.png", UriKind.Relative)));
            szlaban_on2 = new ImageBrush(new BitmapImage(new Uri("Images/swiatla_on2.png", UriKind.Relative)));
            szlaban1 = new Rectangle()
            {
                Width = 180,
                Height = 180
            };
            szlaban1.Fill = szlaban_open;
            szlaban2 = new Rectangle()
            {
                Width = 180,
                Height = 180
            };
            szlaban2.Fill = szlaban_open;
            swiatla1 = new Rectangle()
            {
                Width = 80,
                Height = 101
            };
            swiatla1.Fill = szlaban_off;
            swiatla2 = new Rectangle()
            {
                Width = 80,
                Height = 101
            };
            swiatla2.Fill = szlaban_off;
            ScaleTransform rt = new ScaleTransform();
            rt.ScaleX = -1;
            szlaban1.RenderTransform = rt;
            train_canvas.Children.Add(swiatla1);
            Canvas.SetLeft(swiatla1, 330);
            Canvas.SetBottom(swiatla1, -120);
            Canvas.SetZIndex(swiatla1, 120);
            train_canvas.Children.Add(swiatla2);
            Canvas.SetLeft(swiatla2, -10);
            Canvas.SetBottom(swiatla2, -50);
            Canvas.SetZIndex(swiatla2, 100);
            train_canvas.Children.Add(szlaban1);
            Canvas.SetLeft(szlaban1, 350);
            Canvas.SetZIndex(szlaban1, 120);
            Canvas.SetBottom(szlaban1, -120);
            train_canvas.Children.Add(szlaban2);
            Canvas.SetLeft(szlaban2, 25);
            Canvas.SetBottom(szlaban2, -65);
            Canvas.SetZIndex(szlaban2, 100);
            Thread light = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    light_on();
                }, null);
            });
            light.Start();
        }

        private void light_on()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (train_isMoving)
            {
                if(leftrightlight == 0)
                {
                    swiatla1.Fill = szlaban_on1;
                    swiatla2.Fill = szlaban_on1;
                    leftrightlight = 1;
                }
                else if (leftrightlight == 1)
                {
                    swiatla1.Fill = szlaban_on2;
                    swiatla2.Fill = szlaban_on2;
                    leftrightlight = 0;
                }
            }
            else
            {
                swiatla1.Fill = szlaban_off;
                swiatla2.Fill = szlaban_off;
            }
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
            Thread res= new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    RespawnCars();
                }, null);
            });
            Thread resrev = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    RespawnCarsRev();
                }, null);
            });
            rnd = new Random();
            trainthr.Start();
            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 2)));
            res.Start();
            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 2)));
            resrev.Start();
        }

        private async void RespawnCars()
        {
            while (true)
            {
                Thread car = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                    {
                        setCarMove(animXBef, animXTo, animYBef, animYTo, canvas_1, 5);
                    }, null);
                });
                car.Start();
                await Task.Delay(rnd.Next(3000, 4000));
            }
        }

        private async void RespawnCarsRev()
        {
            while (true)
            {
                Thread carrev = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                    {
                        setCarMove(animXBefRev, animXToRev, animYBefRev, animYToRev, canvas_2, 1);
                    }, null);
                });
                carrev.Start();
                await Task.Delay(rnd.Next(3000, 4000));
            }
        }

        private void setTrainMove()
        {
            rnd = new Random();
            int kier = 0;
            trainMove.Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(4, 7)));

            train_storyboard.Completed += new EventHandler(TrainRestart);

            train_storyboard.Begin();
            train_isMoving = true;
            szlaban1.Fill = szlaban_closed;
            szlaban2.Fill = szlaban_closed;

            async void TrainRestart(object sender, EventArgs e)
            {
                szlaban1.Fill = szlaban_open;
                szlaban2.Fill = szlaban_open;
                train_isMoving = false;
                rnd = new Random();
                await Task.Delay(rnd.Next(2000, 10000));
                train_storyboard.Stop();
                rnd = new Random();
                trainMove.Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(4, 13)));
                kier = rnd.Next(1, 3);
                if (kier == 1)
                {
                    trainrot.ScaleX = -1;
                    train.RenderTransform = trainrot;
                    trainMove.From = 0 - train.Width;
                    trainMove.To = 1280 + train.Width;
                }
                else if (kier == 2)
                {
                    trainrot.ScaleX = 1;
                    train.RenderTransform = trainrot;
                    trainMove.From = 1280 + train.Width;
                    trainMove.To = 0 - train.Width;
                }

                train_storyboard.Begin();
                train_isMoving = true;
                szlaban1.Fill = szlaban_closed;
                szlaban2.Fill = szlaban_closed;
            }
        }

        private int[] animXBef = new int[7];
        private int[] animXTo = new int[7];
        private int[] animYBef = new int[7];
        private int[] animYTo = new int[7];

        private int[] animXBefRev = new int[7];
        private int[] animXToRev = new int[7];
        private int[] animYBefRev = new int[7];
        private int[] animYToRev = new int[7];

        private void setTabs()
        {
            animXBef[0] = -78;
            animXTo[0] = 850;
            animYBef[0] = 120;
            animYTo[0] = 120;

            animXBef[1] = animXTo[0];
            animXTo[1] = 935;
            animYBef[1] = animYTo[0];
            animYTo[1] = 230;

            animXBef[2] = animXTo[1];
            animXTo[2] = 840;
            animYBef[2] = animYTo[1];
            animYTo[2] = 340;

            animXBef[3] = animXTo[2];
            animXTo[3] = 170;
            animYBef[3] = animYTo[2];
            animYTo[3] = 350;

            animXBef[4] = animXTo[3];
            animXTo[4] = 80;
            animYBef[4] = animYTo[3];
            animYTo[4] = 470;

            animXBef[5] = animXTo[4];
            animXTo[5] = 160;
            animYBef[5] = animYTo[4];
            animYTo[5] = 660;

            animXBef[6] = animXTo[5];
            animXTo[6] = 1358;
            animYBef[6] = animYTo[5];
            animYTo[6] = 660;

            animXBefRev[0] = 1200;
            animXToRev[0] = 240;
            animYBefRev[0] = 110;
            animYToRev[0] = 110;

            animXBefRev[1] = animXToRev[0];
            animXToRev[1] = 150;
            animYBefRev[1] = animYToRev[0];
            animYToRev[1] = -25;

            animXBefRev[2] = animXToRev[1];
            animXToRev[2] = 230;
            animYBefRev[2] = animYToRev[1];
            animYToRev[2] = -100;

            animXBefRev[3] = animXToRev[2];
            animXToRev[3] = 850;
            animYBefRev[3] = animYToRev[2];
            animYToRev[3] = -90;

            animXBefRev[4] = animXToRev[3];
            animXToRev[4] = 1000;
            animYBefRev[4] = animYToRev[3];
            animYToRev[4] = -260;

            animXBefRev[5] = animXToRev[4];
            animXToRev[5] = 900;
            animYBefRev[5] = animYToRev[4];
            animYToRev[5] = -420;

            animXBefRev[6] = animXToRev[5];
            animXToRev[6] = -78;
            animYBefRev[6] = animYToRev[5];
            animYToRev[6] = -400;
        }

        private void setCarMove(int[] animxbef, int[] animxto, int[] animybef, int[] animyto, Canvas can, int direction)
        {
            Random rnd = new Random();
            number+=1;

            int dex = rnd.Next(6, 9);
            int index = 0;

            Rectangle autko = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            rnd = new Random();
            switch(rnd.Next(0,3))
            {
                case 0:
                    autko.Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri("Images/BlueCar.png", UriKind.Relative))
                    };
                    break;
                case 1:
                    autko.Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri("Images/RedCar.png", UriKind.Relative))
                    };
                    break;
                case 2:
                    autko.Fill = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri("Images/YellowCar.png", UriKind.Relative))
                    };
                    break;
            }
            autko.Name = "auto" + number;
            list_speed2.Add(dex);
            Storyboard story = new Storyboard();
            DoubleAnimation animMove_x = new DoubleAnimation();
            DoubleAnimation animMove_y = new DoubleAnimation();
                //animMove_1_x
                animMove_x.From = animxbef[index];
                animMove_x.To = animxto[index];
                //distance_x = 900 - 800;
                //animMove_1_y
                animMove_y.From = animybef[index];
                animMove_y.To = animyto[index];
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
            double reverse_velocity = (double)dex / Math.Abs((animxto[index]- animxbef[index])) ;
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
            async void MoveReverse(object sender, EventArgs e)
            {
                if (index < 6)
                {
                    index +=1;
                    story.CurrentTimeInvalidated += new EventHandler(MainTimerEvent);
                   // Control.Content = index;
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
                    double distance_y = (double)Math.Abs((int)(animMove_y.To - animMove_y.From));
                    double distance_x = (double)Math.Abs((int)(animMove_x.To - animMove_x.From));
                    double distance = Math.Sqrt(distance_x * distance_x + distance_y * distance_y);

                    //Zmiana prędkości na zakrętach
                    //Narazie na oko, poźniej można dokładnie wyznaczyć
                    animMove_x.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed[(autko.Name[(autko.Name).Length - 1]) - 48] * distance));
                    animMove_y.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed[(autko.Name[(autko.Name).Length - 1]) - 48] * distance));
                    if (index == direction)
                    {
                        while (train_isMoving)
                        {
                            story.Pause();
                            await Task.Delay(25);
                        }
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
