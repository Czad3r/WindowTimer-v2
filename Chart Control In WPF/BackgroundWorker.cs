using System;
using System.Windows;
using System.ComponentModel;

namespace WindowsTimer
{
    public partial class MainWindow : Window
    {
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) //Skorzystano z przykładu zawartego w Microsoftowej dokumentacj, odpowiednio uproszczonego, dlatego odsyłam po szczegóły tam
        {
    
            BackgroundWorker bw = sender as BackgroundWorker;

            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            while (!bw.CancellationPending)//Dopóki niewołamy BackgroundWorkera by się zatrzymał to wykonuje tą pętle
            {
                App.Monitoring(); //Metoda statyczna
            }
           
        }

        private void backgroundWorker1_RunWorkerCompleted(
            object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                
                // The user canceled the operation.
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                System.Windows.MessageBox.Show(msg);
            }
            else
            {
                // The operation completed normally.
                
            }
        }

        // This method models an operation that may take a long time 
        // to run. It can be cancelled, it can raise an exception,
        // or it can exit normally and return a result. 
        

        

        

    }
}
