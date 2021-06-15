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

        //private List<double> list_speed = new List<double>();
        //private List<int> list_speed2 = new List<int>();
        //private List<int> czy_czeka = new List<int>();
        //private List<int> direct = new List<int>();

        int[] direct = new int[10];
        double[] list_speed = new double[10];
        int[] list_speed2 = new int[10];
        int[] czy_czeka = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };



        private int leftrightlight = 0;

        private int number = -1;
        private int kazde_inne = -1;

        private ScaleTransform trainrot = new ScaleTransform();

        private bool train_isMoving = false;

        private DispatcherTimer timer;

        Rect auto1HitBox = new Rect();
        Rect auto2HitBox = new Rect();

        public MainWindow()
        {
            //wczytanie obrazka jako tło
            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri("Images/mapa_v5.png", UriKind.Relative)));
            Background = brush;

            InitializeComponent();
                            //inicjalizacja:
            szlaban_init(); //szlabanu
            train_init();   //pociągu
            setTabs();      //parametrów (współrzędnych) dla animacji

            Thread thr = new Thread(Threads); 
            thr.Start();
        }
        //inicjalizacja pociągu
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
            Storyboard.SetTarget(trainMove, train); //powiązanie animacji z obrazkiem pociągu 
            Storyboard.SetTargetProperty(trainMove, new PropertyPath(Canvas.LeftProperty)); //ustawienie animacji jako animacje poziomą
            //współrzędne pierwszej animacji (pierwszy ruch pociągu jest z prawej strony do lewej)
            trainMove.From = 1280 + train.Width; //1280 to długość okna
            trainMove.To = 0 - train.Width; //aby pociąg się schował za lewą stroną okna
            train_storyboard.Children.Add(trainMove); //przypisanie animacji do scenorysu
            train_canvas.Children.Add(train); //przypisanie canvasu jako punktu odniesienia pociągu
            Canvas.SetRight(train, 0); //ustawienie picągu w odległości 0 od prawej strony canvasu
            Canvas.SetZIndex(train, 100); //ustawienie pociągu na 100 warstwie (w ten sposób wszystko będzie pod obrazkiem pociągu)
        }

        private void szlaban_init()
        {
            //zainicjowanie potrzebnych obrazków 
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
            rt.ScaleX = -1; //odbicie lustrzne. Użyliśmy tego samego obrazka szlabanu.
            szlaban1.RenderTransform = rt;
            //ustawianie szalabnów oraz świateł do odpowiednich lokalizacji 
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
            //uruchamianie świateł jako jednego wątku
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
            //zegar, który będzie wyznaczał w jakich odstępach czasu mają migać światła
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            
            if (train_isMoving)
            {
                //jeśli pociąg jedzie to światła zaczną świecić (czli będą się zmieniać ich obrazki)
                if (leftrightlight == 0)
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
                //jeśli pociąg nie jedzie lub przestał jechać to zamkną się szlabany
                swiatla1.Fill = szlaban_off;
                swiatla2.Fill = szlaban_off;
            }
        }

        private void Threads()
        {
            //ruch pociągu
            Thread trainthr = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    setTrainMove(); 
                }, null);
            });
            //auta jadące z lewej strony
            Thread res = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                {
                    RespawnCars();
                }, null);
            });
            //auta jadące z prawej strony
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
            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 2))); //czekamy 1-2 s na stworzenie się pociągu
            res.Start();
            Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 2))); //czekamy 1-2 s na stworzenie się aut jadących z lewej strony
            resrev.Start();
        }
        //tworzenie wątków dla każdego auta jadącego z lewej strony
        private async void RespawnCars()
        {
            while (true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread car = new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                        {
                            setCarMove(animXBef, animXTo, animYBef, animYTo, canvas_1, 5); //funkcja tworząca auto 
                        }, null);
                    });
                    car.Start();
                    await Task.Delay(rnd.Next(2000, 3000)); //odsęp czasowy aby auta się nie nałożyły na siebie
                }
                while (canvas_1.Children.Count > 0 && canvas_2.Children.Count > 0) 
                {
                    await Task.Delay(3000); //co 3 s sprawdzamy czy ostatnie auto po jednej ze stron zaczyna swoją ostatnią animacje lub ją skończyło
                }
            }
        }
        //tworzenie wątków dla każdego auta jadącego z prawej strony
        private async void RespawnCarsRev()
        {
            while (true)
            {
                for (int i = 0; i < 5; i++)
                {


                    Thread carrev = new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                        {
                            setCarMove(animXBefRev, animXToRev, animYBefRev, animYToRev, canvas_2, 1); //funkcja tworząca auto 
                        }, null);
                    });
                    carrev.Start();
                    await Task.Delay(rnd.Next(2000, 3000)); //odsęp czasowy aby auta się nie nałożyły na siebie
                }
                while (canvas_2.Children.Count > 0 && canvas_1.Children.Count > 0)
                {
                    await Task.Delay(3000); //co 3 s sprawdzamy czy ostatnie auto po jednej ze stron zaczyna swoją ostatnią animacje lub ją skończyło
                }
            }
        }
        //funkcja setTrainMove działa następująco:
        /*
         * 1.Skonfigurowanie pierwszej animacji (przypisanie jej czasu trwania) 
         * 2.Zamknięcie szlabanu
         * 3.Rozpoczęcie animacji (uruchomienie scenorysu)
         * 4. Po zakończeniu animacji szlabany się otwierają
         * 5.Scenorys jest zatrzymywany 
         * 6.Animacja jest rekonfigurowana (przypisywany jest jej inny czas trwania)
         * 7.Rozpoczęcie animacji (uruchomienie scenorysu)
          */
        private void setTrainMove()
        {
            rnd = new Random();
            int kier = 0; 
            trainMove.Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(4, 7))); //ustawiamy czas pierwszej aniacji losowo od 4s do 7s

            train_storyboard.Completed += new EventHandler(TrainRestart); //funkcja TrainRestart uruchomi się po zakończeniu pierwszej animacji

            train_storyboard.Begin(); //rozpoczęcie ruchu pociągu
            train_isMoving = true; //oznajmienie że pociąg jeździ
            //zamknięcie szlabanów
            szlaban1.Fill = szlaban_closed; 
            szlaban2.Fill = szlaban_closed;

            async void TrainRestart(object sender, EventArgs e)
            {
                //otwarcie szlabanów
                szlaban1.Fill = szlaban_open;
                szlaban2.Fill = szlaban_open;
                train_isMoving = false; //oznajmienie, że pociąg nie jedzie
                rnd = new Random();
                await Task.Delay(rnd.Next(4000, 8000)); //odczekanie kilku sekund aby samochody mogły przejechać przez tory
                train_storyboard.Stop(); //zatrzymanie scenorysu, aby dokonać rekonfiguracji animacji
                rnd = new Random();
                trainMove.Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(5, 13))); //zmiana czasu trwania animacji ruchu pociągu
                kier = rnd.Next(1, 3); //wyznaczenie kierunku jazdy pociągu 
                if (kier == 1) //z lewej do prawej
                {
                    trainrot.ScaleX = -1; //odbicie lustrzane obrazka 
                    train.RenderTransform = trainrot; //zastosowanie odbicia
                    trainMove.From = 0 - train.Width; //0 - train.Width , aby pociąg wyjechał z poza okna
                    trainMove.To = 1280 + train.Width;//1280 + train.Width , aby pociąg wyjechał poza okno
                }
                else if (kier == 2) //z prawej do lewej
                {
                    trainrot.ScaleX = 1; //zachowanie/przywrócenie domyślnego obrazka
                    train.RenderTransform = trainrot;
                    trainMove.From = 1280 + train.Width;
                    trainMove.To = 0 - train.Width;
                }

                train_storyboard.Begin(); //rozpoczęcie animacji
                train_isMoving = true; //oznajmienie że pociąg się rusza
                //zamknięcie szlabanów
                szlaban1.Fill = szlaban_closed;
                szlaban2.Fill = szlaban_closed;
            }
        }
        //tablice przechowywujące współrzędne dla animacji
        private int[] animXBef = new int[7];
        private int[] animXTo = new int[7];
        private int[] animYBef = new int[7];
        private int[] animYTo = new int[7];

        private int[] animXBefRev = new int[7];
        private int[] animXToRev = new int[7];
        private int[] animYBefRev = new int[7];
        private int[] animYToRev = new int[7];

        //ustawianie współrzędnych dla animacji
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
        //tworzenie i poruszanie się auta
        private void setCarMove(int[] animxbef, int[] animxto, int[] animybef, int[] animyto, Canvas can, int direction)
        {
            //ten blok warunkowy jest potrzebny aby nie doszło do sytuacji gdzie autko ma nr nie jednocyfrowy ponieważ 
            //algorytm jest przystosowany do indetyfikacji aut poprzez jednocyfrowe numery
            if (number % 9 == 0 && number > 1) 
            {
                number = -1;
            }
            Random rnd = new Random();
            number++;
            kazde_inne++;
            int dex = rnd.Next(4, 9);
            int index = 0;

            Rectangle autko = new Rectangle()
            {
                Width = 78,
                Height = 50
            };
            rnd = new Random();
            //wybieranie 3 różnych dostępnych kolorów autka
            switch (rnd.Next(0, 3))
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

            autko.Name = "auto" + kazde_inne + number; //nadanie imienia autkowi
            list_speed2[number] = dex; //wrzucanie czasu pierwszej animacji animacji do tablicy przechowywującej czasy trwania pierwszych animacji
            direct[number] = direction; //wrzucanie kierunku jazdy do tablicy przechowywującej kierunki jazdy aut
            //inicjacja scenorysu oraz animacji po osiach x i y
            Storyboard story = new Storyboard();
            DoubleAnimation animMove_x = new DoubleAnimation();
            DoubleAnimation animMove_y = new DoubleAnimation();
            //przypisanie animacji wpółrzędnych początkowych i końcowych
            animMove_x.From = animxbef[index];
            animMove_x.To = animxto[index];
            animMove_y.From = animybef[index];
            animMove_y.To = animyto[index];
            //przypisanie animacji czasu trwania
            animMove_x.Duration = new Duration(TimeSpan.FromSeconds(list_speed2[number]));
            animMove_y.Duration = new Duration(TimeSpan.FromSeconds(list_speed2[number]));
            //powiązanie za pomocą scenorysu autka i animacji. Autko ma dwie animacje które wykonują się rownocześnie
            //Jedna wykonuje się na osi X a druga na osi Y. 
            Storyboard.SetTarget(animMove_x, autko);
            Storyboard.SetTargetProperty(animMove_x, new PropertyPath(Canvas.LeftProperty));
            story.Children.Add(animMove_x);
            Storyboard.SetTarget(animMove_y, autko);
            Storyboard.SetTargetProperty(animMove_y, new PropertyPath(Canvas.TopProperty));
            story.Children.Add(animMove_y);
            //obliczanie odwrotności prędkości, która będzie stosowana przy wyznaczaniu czasu kolejnych animacji. 
            //Obliczana jest odwrotność prędkości, a nie prędkość, ponieważ wystarczy pomnożyć odwrotność prędkości przez dystans
            //(który można obliczyć za każdym razem dla różnych animacji) aby otrzymać czas. Nie trzeba wtedy odwracać otrzymanej liczby
            //tak jakby to miało miejsce w przypadku prędkości.
            double reverse_velocity = (double)dex / Math.Abs((animxto[index] - animxbef[index]));
            can.Children.Add(autko); //przypisanie lokalizacji auta względem canvasu 
             
            list_speed[number] = reverse_velocity; //wrzucanie odwrotności prędkości do tablicy przechowywującej odwrotności prędkości kolejnych aut
           //ustawienie lokalizacji autka
            Canvas.SetTop(autko, 125);
            Canvas.SetLeft(autko, 78);
            
            story.Completed += new EventHandler(MoveReverse); //gdy animacja się zakończy odpali funkcje MoveReverse
            story.CurrentTimeInvalidated += new EventHandler(MainTimerEvent); //za każdym "tickiem" animacji odpala funkcje MainTimerEvent
            story.Begin(); // rozpoczęcie animacji
            //ta funkcja służy do wykrywania bliskości między autkami. Stosuje się tutaj hitboxy.
            async void MainTimerEvent(object sender, EventArgs e)
            {
                //nadajemy hitboxy dla każdego istniejącego autka 
                foreach (var x in can.Children.OfType<Rectangle>())
                {
                    //tworzenie hitboxów i nadanie im trochę większych wymiarów jak autka
                    auto1HitBox = new Rect(Canvas.GetLeft(autko) - 35 / dex, Canvas.GetTop(autko), autko.Width + 30, autko.Height + 12);
                    auto2HitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width + 30, x.Height + 12);
                    //sprawdzanie czy hitboxy mają część wspólną tj. czy nachodzą na siebie
                    if (auto1HitBox.IntersectsWith(auto2HitBox) && (string)x.Name != (string)autko.Name && (autko.Name[(autko.Name).Length - 1]) > (x.Name[(x.Name).Length - 1]))
                    {
                        //pobranie odwrotności prędkości
                        double pomocnicza = list_speed[(autko.Name[(autko.Name).Length - 1]) - 48];
                        double pomocnicza2 = list_speed[(x.Name[(x.Name).Length - 1]) - 48];

                        //sprawdzanie czy prędkość następnego auta jest mniejsza niż auta poprzedzającego
                        if (list_speed2[(autko.Name[(autko.Name).Length - 1]) - 48] < list_speed2[(x.Name[(x.Name).Length - 1]) - 48])
                        {
                            //zmiana czasu trwania scenorysu (animacji) auta poprzedzającego na auta następnego. 
                            story.SetSpeedRatio((double)(pomocnicza / (pomocnicza2))); 
                            autko.Name = x.Name; //przypisanie tej samej identyfikacji obu autom
                        }

                        //spradza czy następne auto czeka w kolejce przed szlabanem
                        if (czy_czeka[(x.Name[(x.Name).Length - 1]) - 48] == 1)
                        {
                            czy_czeka[(autko.Name[(autko.Name).Length - 1]) - 48] = 1; //jeśli następne auto czeka to niech te też czeka
                        }

                        break; //skoro już znaleźliśmy odpowiednie auto to nie ma sensu dalej szukać

                    }
                }
                //jeśli auto czeka to 
                if (czy_czeka[(autko.Name[(autko.Name).Length - 1]) - 48] == 1)
                {
                    //sprawdź czy znajduje się w miare blisko szlabanu
                    while (train_isMoving&& (index==0 || index==4 || index==3 ))
                    {
                        story.Pause();
                        await Task.Delay(50);
                    }
                    story.Resume();
                    czy_czeka[(autko.Name[(autko.Name).Length - 1]) - 48] = 0; //po przejechaniu pociągu przestań czekać
                }

            }
            async void MoveReverse(object sender, EventArgs e)
            {
                if (index < 6)
                {
                    index += 1;
                    //przypisanie nowych współrzędnych animacji
                    animMove_x.From = animxbef[index];
                    animMove_x.To = animxto[index];
                    animMove_y.From = animybef[index];
                    animMove_y.To = animyto[index];
                    //powiązanie zrekonfigurowanych animacji z autkiem
                    Storyboard.SetTarget(animMove_x, autko);
                    Storyboard.SetTargetProperty(animMove_x, new PropertyPath(Canvas.LeftProperty));
                    story.Children.Add(animMove_x);
                    Storyboard.SetTarget(animMove_y, autko);
                    Storyboard.SetTargetProperty(animMove_y, new PropertyPath(Canvas.TopProperty));
                    story.Children.Add(animMove_y);
                    //wyznaczenie dystansu jaki autko przejedzie w tej animacji
                    double distance_y = (double)Math.Abs((int)(animMove_y.To - animMove_y.From));
                    double distance_x = (double)Math.Abs((int)(animMove_x.To - animMove_x.From));
                    double distance = Math.Sqrt(distance_x * distance_x + distance_y * distance_y);
                    //wyznaczenie czasu trwania animacji: odwrotność prędkości * dystans = czas    
                    animMove_x.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed[(autko.Name[(autko.Name).Length - 1]) - 48] * distance));
                    animMove_y.Duration = new Duration(TimeSpan.FromSeconds((double)list_speed[(autko.Name[(autko.Name).Length - 1]) - 48] * distance));
                    //sprawdzanie czy auto znajduje się przed szlabanem. Zmienna direction może mieć 2 wartości 5 lub 1 
                    //te 2 wartości odpowiadają numerom animacji, które rozpoczynają się przed szlabanem
                    if (index == direction)
                    {
                        //sprawdzenie czy jedzie pociąg 
                        while (train_isMoving)
                        {
                            czy_czeka[(autko.Name[(autko.Name).Length - 1]) - 48] = 1;
                            story.Pause();
                            await Task.Delay(50);
                        }
                        czy_czeka[(autko.Name[(autko.Name).Length - 1]) - 48] = 0; //po skończeniu jazdy pociągu nie czekaj
                    }
                    //MainTimerEvent to funkcja wykrywająca bliskie zderzenia (generuje hitboxy itd)
                    story.CurrentTimeInvalidated += new EventHandler(MainTimerEvent);

                    story.Begin(); //rozpocznij zrekonfigurowaną animacje
                }
                else
                {
                    //auto jest usuwane z canvasa, aby algorytm tworzący hitboxy nie sprawdzał zużytych aut przy wyznaczaniu bliskości
                    can.Children.Remove(autko);

                    story.Remove(); //usunięcie autka ze scenorysu
                    return; //zakończ działanie funkcji
                }
            }
        }
    }
}
