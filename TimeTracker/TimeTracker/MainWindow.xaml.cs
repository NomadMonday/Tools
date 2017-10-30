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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Threading;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TimeSpan objects for tracking the individual timers.
        TimeSpan timer1 = new TimeSpan(0);
        TimeSpan timer2 = new TimeSpan(0);
        TimeSpan timer3 = new TimeSpan(0);
        TimeSpan timer4 = new TimeSpan(0);
        TimeSpan timer5 = new TimeSpan(0);

        static System.Timers.Timer _timer = new System.Timers.Timer(1000); //Timer will tick every second.

        public MainWindow()
        {
            InitializeComponent();
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Left = SystemParameters.VirtualScreenLeft;
            //this.Top = SystemParameters.VirtualScreenHeight - this.Height -30;
            System.Windows.Forms.Screen[] s = System.Windows.Forms.Screen.AllScreens;
            System.Drawing.Rectangle r = s[s.Length-1].WorkingArea;
            this.Left = r.Left;
            this.Top = r.Bottom - this.Height;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateTick(); //Update UI each tick.
        }

        private void UpdateTick()
        {
            Dispatcher.Invoke((Action)delegate()
            {
                //Increment each timer that is on.
                if (toggleButtonTimer1.IsChecked == true)
                {
                    timer1 = timer1.Add(TimeSpan.FromSeconds(1));
                }

                if (toggleButtonTimer2.IsChecked == true)
                {
                    timer2 = timer2.Add(TimeSpan.FromSeconds(1));
                }

                if (toggleButtonTimer3.IsChecked == true)
                {
                    timer3 = timer3.Add(TimeSpan.FromSeconds(1));
                }

                if (toggleButtonTimer4.IsChecked == true)
                {
                    timer4 = timer4.Add(TimeSpan.FromSeconds(1));
                }

                if (toggleButtonTimer5.IsChecked == true)
                {
                    timer5 = timer5.Add(TimeSpan.FromSeconds(1));
                }

                //Update UI display.
                textBoxTime1.Text = timer1.ToString(@"h\:mm\:ss");
                textBoxTime2.Text = timer2.ToString(@"h\:mm\:ss");
                textBoxTime3.Text = timer3.ToString(@"h\:mm\:ss");
                textBoxTime4.Text = timer4.ToString(@"h\:mm\:ss");
                textBoxTime5.Text = timer5.ToString(@"h\:mm\:ss");

                textBoxDecimalTime1.Text = ConvertTimeToDecimal(timer1);
                textBoxDecimalTime2.Text = ConvertTimeToDecimal(timer2);
                textBoxDecimalTime3.Text = ConvertTimeToDecimal(timer3);
                textBoxDecimalTime4.Text = ConvertTimeToDecimal(timer4);
                textBoxDecimalTime5.Text = ConvertTimeToDecimal(timer5);
            });
        }

        private string ConvertTimeToDecimal(TimeSpan ts)
        {
            double time = ts.TotalHours; //Get time in double format.
            time *= 10; //Move one decimal place.
            time = Math.Ceiling(time); //Take ceiling value.
            time /= 10; //Return decimal place.
            return time.ToString("0.0"); //Format to single decimal place.
        }

        private void buttonReset1_Click(object sender, RoutedEventArgs e)
        {
            //Reset all values.
            toggleButtonTimer1.IsChecked = false;
            textBoxTaskDescription1.Text = "";
            textBoxTime1.Text = "0:00:00";
            textBoxDecimalTime1.Text = "0.0";
            timer1 = new TimeSpan(0);
        }

        private void buttonReset2_Click(object sender, RoutedEventArgs e)
        {
            //Reset all values.
            toggleButtonTimer2.IsChecked = false;
            textBoxTaskDescription2.Text = "";
            textBoxTime2.Text = "0:00:00";
            textBoxDecimalTime2.Text = "0.0";
            timer2 = new TimeSpan(0);
        }

        private void buttonReset3_Click(object sender, RoutedEventArgs e)
        {
            //Reset all values.
            toggleButtonTimer3.IsChecked = false;
            textBoxTaskDescription3.Text = "";
            textBoxTime3.Text = "0:00:00";
            textBoxDecimalTime3.Text = "0.0";
            timer3 = new TimeSpan(0);
        }

        private void buttonReset4_Click(object sender, RoutedEventArgs e)
        {
            //Reset all values.
            toggleButtonTimer4.IsChecked = false;
            textBoxTaskDescription4.Text = "";
            textBoxTime4.Text = "0:00:00";
            textBoxDecimalTime4.Text = "0.0";
            timer4 = new TimeSpan(0);
        }

        private void buttonReset5_Click(object sender, RoutedEventArgs e)
        {
            //Reset all values.
            toggleButtonTimer5.IsChecked = false;
            textBoxTaskDescription5.Text = "";
            textBoxTime5.Text = "0:00:00";
            textBoxDecimalTime5.Text = "0.0";
            timer5 = new TimeSpan(0);
        }

        private void textBoxDecimalTime1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Will automatically select the time value and attempt to copy it to clipboard.
            try
            {
                textBoxDecimalTime1.SelectAll();
                Clipboard.SetText(textBoxDecimalTime1.Text);
            }
            catch (Exception ex)
            {
                if (ex.HResult != -2147221040) //Ignore OpenClipboard error.
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBoxDecimalTime2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Will automatically select the time value and attempt to copy it to clipboard.
            try
            {
                textBoxDecimalTime2.SelectAll();
                Clipboard.SetText(textBoxDecimalTime2.Text);
            }
            catch (Exception ex)
            {
                if (ex.HResult != -2147221040) //Ignore OpenClipboard error.
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBoxDecimalTime3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Will automatically select the time value and attempt to copy it to clipboard.
            try
            {
                textBoxDecimalTime3.SelectAll();
                Clipboard.SetText(textBoxDecimalTime3.Text);
            }
            catch (Exception ex)
            {
                if (ex.HResult != -2147221040) //Ignore OpenClipboard error.
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBoxDecimalTime4_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Will automatically select the time value and attempt to copy it to clipboard.
            try
            {
                textBoxDecimalTime4.SelectAll();
                Clipboard.SetText(textBoxDecimalTime4.Text);
            }
            catch (Exception ex)
            {
                if (ex.HResult != -2147221040) //Ignore OpenClipboard error.
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBoxDecimalTime5_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Will automatically select the time value and attempt to copy it to clipboard.
            try
            {
                textBoxDecimalTime5.SelectAll();
                Clipboard.SetText(textBoxDecimalTime5.Text);
            }
            catch (Exception ex)
            {
                if (ex.HResult != -2147221040) //Ignore OpenClipboard error.
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void toggleButtonTimer1_Checked(object sender, RoutedEventArgs e)
        {
            //Start the static timer, if not already running.
            if(_timer.Enabled != true)
            {
                _timer.Start();
            }
        }

        private void toggleButtonTimer2_Checked(object sender, RoutedEventArgs e)
        {
            //Start the static timer, if not already running.
            if (_timer.Enabled != true)
            {
                _timer.Start();
            }
        }

        private void toggleButtonTimer3_Checked(object sender, RoutedEventArgs e)
        {
            //Start the static timer, if not already running.
            if (_timer.Enabled != true)
            {
                _timer.Start();
            }
        }

        private void toggleButtonTimer4_Checked(object sender, RoutedEventArgs e)
        {
            //Start the static timer, if not already running.
            if (_timer.Enabled != true)
            {
                _timer.Start();
            }
        }

        private void toggleButtonTimer5_Checked(object sender, RoutedEventArgs e)
        {
            //Start the static timer, if not already running.
            if (_timer.Enabled != true)
            {
                _timer.Start();
            }
        }

        private void toggleButtonTimer1_Unchecked(object sender, RoutedEventArgs e)
        {
            //If all buttons are unchecked, stop background worker.
            if(toggleButtonTimer1.IsChecked == false
                && toggleButtonTimer2.IsChecked == false
                && toggleButtonTimer3.IsChecked == false
                && toggleButtonTimer4.IsChecked == false
                && toggleButtonTimer5.IsChecked == false)
            {
                _timer.Stop();
            }
        }

        private void toggleButtonTimer2_Unchecked(object sender, RoutedEventArgs e)
        {
            //If all buttons are unchecked, stop background worker.
            if (toggleButtonTimer1.IsChecked == false
                && toggleButtonTimer2.IsChecked == false
                && toggleButtonTimer3.IsChecked == false
                && toggleButtonTimer4.IsChecked == false
                && toggleButtonTimer5.IsChecked == false)
            {
                _timer.Stop();
            }
        }

        private void toggleButtonTimer3_Unchecked(object sender, RoutedEventArgs e)
        {
            //If all buttons are unchecked, stop background worker.
            if (toggleButtonTimer1.IsChecked == false
                && toggleButtonTimer2.IsChecked == false
                && toggleButtonTimer3.IsChecked == false
                && toggleButtonTimer4.IsChecked == false
                && toggleButtonTimer5.IsChecked == false)
            {
                _timer.Stop();
            }
        }

        private void toggleButtonTimer4_Unchecked(object sender, RoutedEventArgs e)
        {
            //If all buttons are unchecked, stop background worker.
            if (toggleButtonTimer1.IsChecked == false
                && toggleButtonTimer2.IsChecked == false
                && toggleButtonTimer3.IsChecked == false
                && toggleButtonTimer4.IsChecked == false
                && toggleButtonTimer5.IsChecked == false)
            {
                _timer.Stop();
            }
        }

        private void toggleButtonTimer5_Unchecked(object sender, RoutedEventArgs e)
        {
            //If all buttons are unchecked, stop background worker.
            if (toggleButtonTimer1.IsChecked == false
                && toggleButtonTimer2.IsChecked == false
                && toggleButtonTimer3.IsChecked == false
                && toggleButtonTimer4.IsChecked == false
                && toggleButtonTimer5.IsChecked == false)
            {
                _timer.Stop();
            }
        }

        private void checkBoxTopmost_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void checkBoxTopmost_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }

        private void textBoxTime1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string s = Microsoft.VisualBasic.Interaction.InputBox("Enter time in minutes:"); //Input new time value.
                if (s.Length != 0)
                {
                    timer1 = TimeSpan.FromMinutes(Convert.ToDouble(s)); //Update timer.
                    textBoxTime1.Text = timer1.ToString(@"h\:mm\:ss"); //Update display.
                    textBoxDecimalTime1.Text = ConvertTimeToDecimal(timer1); //Update decimal display.
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxTime2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string s = Microsoft.VisualBasic.Interaction.InputBox("Enter time in minutes:"); //Input new time value.
                if (s.Length != 0)
                {
                    timer2 = TimeSpan.FromMinutes(Convert.ToDouble(s)); //Update timer.
                    textBoxTime2.Text = timer2.ToString(@"h\:mm\:ss"); //Update display.
                    textBoxDecimalTime2.Text = ConvertTimeToDecimal(timer2); //Update decimal display.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxTime3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string s = Microsoft.VisualBasic.Interaction.InputBox("Enter time in minutes:"); //Input new time value.
                if (s.Length != 0)
                {
                    timer3 = TimeSpan.FromMinutes(Convert.ToDouble(s)); //Update timer.
                    textBoxTime3.Text = timer3.ToString(@"h\:mm\:ss"); //Update display.
                    textBoxDecimalTime3.Text = ConvertTimeToDecimal(timer3); //Update decimal display.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxTime4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string s = Microsoft.VisualBasic.Interaction.InputBox("Enter time in minutes:"); //Input new time value.
                if (s.Length != 0)
                {
                    timer4 = TimeSpan.FromMinutes(Convert.ToDouble(s)); //Update timer.
                    textBoxTime4.Text = timer4.ToString(@"h\:mm\:ss"); //Update display.
                    textBoxDecimalTime4.Text = ConvertTimeToDecimal(timer4); //Update decimal display.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxTime5_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string s = Microsoft.VisualBasic.Interaction.InputBox("Enter time in minutes:"); //Input new time value.
                if (s.Length != 0)
                {
                    timer5 = TimeSpan.FromMinutes(Convert.ToDouble(s)); //Update timer.
                    textBoxTime5.Text = timer5.ToString(@"h\:mm\:ss"); //Update display.
                    textBoxDecimalTime5.Text = ConvertTimeToDecimal(timer5); //Update decimal display.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
