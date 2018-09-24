using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Threading;

namespace WindowsTimer
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NotifyIcon nIcon;

        BackgroundWorker backgroundWorker1 = null;

        private readonly DispatcherTimer activityTimer;  //Zmienne potrzebne do analizy aktywności użytkownika, wykorzystuje eventy związane z ruchem myszy i klawiatury
        private System.Drawing.Point MousePosition = new System.Drawing.Point(0, 0);


        public MainWindow()
        {
            InitializeComponent();
            cancelBtn.Visibility = Visibility.Hidden; //Początkowo przycisk stopu jest niewidoczny, będziemy go pokazywać dopiero po starcie
            activityTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(15), IsEnabled = true }; //Można tu ustawić długość braku aktywności przed ostrzeżeniem
            activityTimer.Tick += _activityTimer_Tick;
            
            
        }

        private void _activityTimer_Tick(object sender, EventArgs e) //Funkcja wywoływana przy każdym ticku activityTimera
        {
            
            System.Drawing.Point actualPosition = System.Windows.Forms.Control.MousePosition;
            if (MousePosition == actualPosition)
            {
                
                System.Windows.Forms.MessageBox.Show(new Form() { TopMost = true }, "Byłeś nieaktywny! Zatrzymano liczenie!");
                
                CancelButton_Click(null, new RoutedEventArgs());
            }
            MousePosition = actualPosition;
        }


        private void Button_Click(object sender, RoutedEventArgs e) //Przycisk start
        {
            nIcon = new NotifyIcon(); //Obiekt odpowiedzialny za wyświetlanie ikonki obok zegara systemowego
            nIcon.Visible = true;
            this.WindowState = WindowState.Minimized; //Minimalizujemy główne okno po kliknięciu startu


            this.nIcon.Icon = Properties.Resources.Icon1; // Dodajemy graficzną ikonkę z zasobów podpiętych pod projekt(Resources) do naszego obiektu NotifyIcon
            this.nIcon.Text = "Kliknij podwójnie by wywołać okno!";

            this.nIcon.ShowBalloonTip(5000, "Start", "Mierzenie czasu rozpoczęte!", ToolTipIcon.Info);

            nIcon.DoubleClick += new EventHandler(notifyIcon1_DoubleClick); //Dodajemy do eventu DoubleClick delegata (ustanowienie działania dwukliku na ikonce obok zegara systemowego)
            cancelBtn.Visibility = Visibility.Visible;
            startBtn.Visibility = Visibility.Hidden;

            backgroundWorker1 = new BackgroundWorker(); // Po starcie uruchamiamy BackgroundWorkera - będzie wykonywał funkcje w tle, które do niego wrzuciłem (szczegóły w BackgroundWorker.cs)
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork); //wykorzystanie delegata
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.RunWorkerAsync();

            activityTimer.Start();


        }
        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            // Pokazuje okienko naszej aplikacji po podwójnym wciśnięciu ikony obok zegara

            //  Ustawia okno na normalne jeśli było zminimalizowane

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
                this.Activate();
            }
            else WindowState = WindowState.Minimized;

        } //Ikona przybornik systemowy


        private void CancelButton_Click(object sender, RoutedEventArgs e)//Przycisk stop
        {
            cancelBtn.Visibility = Visibility.Hidden;
            startBtn.Visibility = Visibility.Visible;
            nIcon.Visible = false;
            backgroundWorker1.CancelAsync();

            activityTimer.Stop();
        }


        private void graph1Btn_Click(object sender, RoutedEventArgs e) //Tworzenie tabeli po zaznaczeniu przycisku Tabela
        {
            List<myWindow> win = new List<myWindow>(); //Tworzymy listę obiektów myWindow posiadające pole nazwa i sekundy(czas)
            foreach (DictionaryEntry pair in App.applhash)
            {
                win.Add(new myWindow() { nazwa = (string)pair.Key, sekundy = (int)pair.Value });
            }


            DataGrid1.ItemsSource = win; //Dodajemy wszystkie dane do tabli zawartej w xaml

            Chart1.Visibility = Visibility.Hidden;
            DataGrid1.Visibility = Visibility.Visible;

        }

        private void graph2Btn_Click(object sender, RoutedEventArgs e) //Słupkowy
        {
            List<myWindow> win = new List<myWindow>();
            List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();

            foreach (DictionaryEntry pair in App.applhash)
            {
                win.Add(new myWindow() { nazwa = (string)pair.Key, sekundy = (int)pair.Value });
            }

            int sumaSekund = 0; //SUma jest nam potrzebna do przeliczenia procentowego udziału każdego okna
            foreach (myWindow item in win)
            {
                sumaSekund += item.sekundy;
            }
            foreach (myWindow item in win)
            {
                double procent = ((double)item.sekundy / sumaSekund) * 100;
                if(Math.Round(procent)<=100 && Math.Round(procent)>=0)
                item.procent = (int)Math.Round(procent);
            }

            foreach (myWindow item in win)
            {
                MyValue.Add(new KeyValuePair<string, int>(item.nazwa, item.procent)); 
            }

            try
            {
                InitializeComponent();
                MyChart1.ItemsSource = OdrzucamyOknaPrzeglądarki(MyValue);
            }
            catch (Exception e2)
            { }
            DataGrid1.Visibility = Visibility.Hidden;
            Chart1.Visibility = Visibility.Visible;

        }
        

        private void MyChart1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColumnSeries cs = (ColumnSeries)sender;
            KeyValuePair<string, int> kv = (KeyValuePair<string, int>)cs.SelectedItem;
            cs.Title = kv.Key;
        }

        private List<KeyValuePair<string, int>> OdrzucamyOknaPrzeglądarki(List<KeyValuePair<string, int>> listaPrzesłana) //Łączy wpisy o oknach które mają poniżej 15% udziału w całości w jeden wpis "Inne strony internetowe" Działa dla Google Chrome
        {
            List<KeyValuePair<string, int>> listaZmodyfikowana = new List<KeyValuePair<string, int>>();
            KeyValuePair<string,int> RekordInnychStron=new KeyValuePair<string, int>("Inne strony internetowe", 0);
            foreach (KeyValuePair<string,int> item in listaPrzesłana)
            {
                if (item.Key.Contains("Google Chrome") && item.Value <= 15)
                {
                    RekordInnychStron = new KeyValuePair<string, int>("Inne strony internetowe", RekordInnychStron.Value + item.Value); //Nadpisujemy nową wartością, ponieważ nie można edytować pól KeyValuePair<>
                }
                else listaZmodyfikowana.Add(new KeyValuePair<string, int>(item.Key, item.Value));
            }
            listaZmodyfikowana.Add(RekordInnychStron);
            
            
            return listaZmodyfikowana;
        }



       
        

        
    }
}

