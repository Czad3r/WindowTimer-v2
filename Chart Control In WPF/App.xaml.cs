using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices; //Dyrektywa potrzebna do użycia niezbędnych nam metod
using System.Text;
using System.Windows;

namespace WindowsTimer
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        public static Stack applnames = new Stack();

        public static Hashtable applhash = new Hashtable();

        public static DateTime applfocustime; //data wstawienia ostatniego okna

        public static string appltitle;

        public static TimeSpan timeInterval; //ostatni przedział czasu

        public static string appName, prevvalue; //prevvalue- pełna nazwa ostatniego okna


        //Potrzebne są nam funkcje z Windows API - tutaj następuje ich import
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        //Ta funkcja jest używana do uzyskania danych o aktywnym oknie
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 GetWindowThreadProcessId(IntPtr handle, out Int32 processId);

        /// <summary>
        /// Funkcja zwracająca nazwę aktualnego okna
        /// </summary>
        /// <returns>Zwraca "" gdy nie uda się utworzyć uchwytu, "unknown" gdy nazwa jest zbyt krótka lub długa</returns>
        public static string GetActiveWindow() // Metoda statyczna, w skrócie wytwarza nazwę aktywnego okna
        {
            const int nChars = 256;
            IntPtr handle;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (handle.Equals(IntPtr.Zero)) return "";

            int intLength = GetWindowText(handle, Buff, nChars); //Do Buff wrzucamy nazwę aktualnego okna

            if ((intLength <= 0) || (intLength > Buff.Length)) return "unknown";

            return Buff.ToString();
        }

        public static Int32 GetWindowProcessID(IntPtr handle)

        {
            //This Function is used to get Active process ID... Ta funkcja jest używana go uzyskania uzyskania identyfikatora aktywnego okna... Niezbędne do poprawnego działania, nie ma co wnikać w szczegóły

            Int32 id;

            GetWindowThreadProcessId(handle, out id);

            return id;

        }

        public static void Monitoring() //Statyczna metoda która po wciśnięciu startu ciągle działa w tle za pomocą BackgroundWorkera
        {
            
            try

            {
                
                
                bool isNewAppl = false; // Bool do sprawdzania czy liczymy jeszcze czas poprzedniego okna czy już nowego

                IntPtr handle = GetForegroundWindow(); //Uchwyt do okna

                Int32 id = GetWindowProcessID(handle);

                Process p = Process.GetProcessById(id); //Klasa Process z System.Diagnostics umożliwia nam m.in. odczytanie nazwy procesu

                appName = p.ProcessName;

                appltitle = GetActiveWindow();

                string fullName = appltitle;

                if (!applnames.Contains(fullName) && fullName != "MainWindow") //If dzięki któremu unikniemy ciągłego uwzględniania naszego okna do obliczeń (w związku że to okno pracuje w tle, jest rozpoznawane przez różne systemy inaczej, np. jest ciągle traktowane jako aktywne=powoduje błędy w odczycie)

                {

                    applnames.Push(fullName);

                    applhash.Add(fullName, 0);

                    isNewAppl = true;

                }

                if (prevvalue != (fullName)) //Jeśli nazwa aktualnego okna jest inna od tego które funkcja sprawdzała przed chwilą to rozpoczynamy obliczenia

                {

                    IDictionaryEnumerator en = applhash.GetEnumerator(); //Korzystamy z enumeratora by przejść przez nasze hashtable "applhash" w poszukiwaniu czy już przypadkiem nie zapisaliśmy nazwy obecnego okna wcześniej w bazie-jeśli tak to doliczamy mu czas

                    timeInterval = DateTime.Now.Subtract(applfocustime); //Sztandarowe wykorzystanie metody statycznej na rzecz typu DateTime

                    while (en.MoveNext()) //Przesuwanie się po "applhash"

                    {

                        if (en.Key.ToString() == prevvalue)

                        {

                            double prevseconds = Convert.ToDouble(en.Value);

                            applhash.Remove(prevvalue);

                            applhash.Add(prevvalue, ((int)timeInterval.TotalSeconds + (int)prevseconds)); //Aktualizujemy czas

                            break;

                        }

                    }

                    prevvalue = fullName; //Przypisujemy nową wartość

                    applfocustime = DateTime.Now;

                }

                if (isNewAppl)

                    applfocustime = DateTime.Now;

            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.Message + ":" + ex.StackTrace);

            }
        }
    }
}
