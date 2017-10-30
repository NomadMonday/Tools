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
using System.Windows.Interop;
using System.Text.RegularExpressions;

namespace GBHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Next clipboard viewer window 
        /// </summary>
        private IntPtr hWndNextViewer;

        /// <summary>
        /// The <see cref="HwndSource"/> for this window.
        /// </summary>
        private HwndSource hWndSource;

        public MainWindow()
        {
            InitializeComponent();
            //this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        /* Moved to textBoxGB_Loaded handler.
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.VirtualScreenLeft;
            this.Top = SystemParameters.VirtualScreenHeight - this.Height - 240; //240 is Time Tracker height (210) and approximate taskbar height (30).
            
        }
         */

        private void InitCBViewer()
        {
            WindowInteropHelper wih = new WindowInteropHelper(this);
            hWndSource = HwndSource.FromHwnd(wih.Handle);

            hWndSource.AddHook(this.WinProc);   // start processing window messages
            hWndNextViewer = Win32.SetClipboardViewer(hWndSource.Handle);   // set this window as a viewer
            //isViewing = true;
        }

        private void CloseCBViewer()
        {
            // remove this window from the clipboard viewer chain
            Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);

            hWndNextViewer = IntPtr.Zero;
            hWndSource.RemoveHook(this.WinProc);
            //pnlContent.Children.Clear();
            //isViewing = false;
        }

        /*private void DrawContent()
        {
            pnlContent.Children.Clear();

            if (Clipboard.ContainsText())
            {
                // we have some text in the clipboard.
                TextBox tb = new TextBox();
                tb.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                tb.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                tb.Text = Clipboard.GetText();
                tb.IsReadOnly = true;
                tb.TextWrapping = TextWrapping.NoWrap;
                pnlContent.Children.Add(tb);
            }
            else if (Clipboard.ContainsFileDropList())
            {
                // we have a file drop list in the clipboard
                ListBox lb = new ListBox();
                lb.ItemsSource = Clipboard.GetFileDropList();
                pnlContent.Children.Add(lb);
            }
            else if (Clipboard.ContainsImage())
            {
                // Because of a known issue in WPF,
                // we have to use a workaround to get correct
                // image that can be displayed.
                // The image have to be saved to a stream and then 
                // read out to workaround the issue.
                MemoryStream ms = new MemoryStream();
                BmpBitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(Clipboard.GetImage()));
                enc.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);

                BmpBitmapDecoder dec = new BmpBitmapDecoder(ms,
                    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                Image img = new Image();
                img.Stretch = Stretch.Uniform;
                img.Source = dec.Frames[0];
                pnlContent.Children.Add(img);
            }
            else
            {
                Label lb = new Label();
                lb.Content = "The type of the data in the clipboard is not supported by this sample.";
                pnlContent.Children.Add(lb);
            }
        }
         */

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WM_CHANGECBCHAIN:
                    if (wParam == hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it.
                        hWndNextViewer = lParam;
                    }
                    else if (hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer.
                        Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
                    }
                    break;

                case Win32.WM_DRAWCLIPBOARD:
                    // clipboard content changed
                    //this.DrawContent();
                    this.ConvertToGB();
                    /*if (Clipboard.ContainsText())
                    {
                        textBoxGB.Text = Clipboard.GetText();
                    }
                     */
                    // pass the message to the next viewer.
                    Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        private void ConvertToGB()
        {
            if(Clipboard.ContainsText()) //Only convert text.
            {
                string content = Clipboard.GetText();
                if(this.IsActive == false) //Only convert if current window is not active to allow copying of resultant GB value without reconverting.
                {
                    if(Regex.IsMatch(content, @"^\(?((\d){1,3}(,\d{3})*|\d+)(\.?\d*)[\) ]?$")) //Test if value is in a numerical format
                    {
                        content = Regex.Replace(content, @"[\(\),]", ""); //Remove parenthesis and comma.
                        double gb = Math.Round(Convert.ToDouble(content) / 1024 / 1024 / 1024, 3, MidpointRounding.AwayFromZero);
                        if(gb == 0)
                        {
                            gb = .001;
                        }
                        textBoxGB.Text = gb.ToString();
                    }

                    comboBoxClipBoard.Items.Insert(0, content);
                    comboBoxClipBoard.Items.MoveCurrentToFirst();
                }
            }
        }

        private void textBoxGB_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.VirtualScreenLeft;
            this.Top = 0;//SystemParameters.VirtualScreenHeight - this.Height - 240; //240 is Time Tracker height (210) and approximate taskbar height (30).
            
            this.InitCBViewer();
        }

        private void textBoxGB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Copy textbox value to clipboard.
            try
            {
                textBoxGB.SelectAll();
                Clipboard.SetText(textBoxGB.Text);
                comboBoxClipBoard.Items.Insert(0, textBoxGB.Text);
                comboBoxClipBoard.Items.MoveCurrentToFirst();
            }
            catch (Exception ex)
            {
//                if (ex.HResult != -2147221040) //Ignore OpenClipboard error.
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBoxClipBoard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.IsActive)
            {
                //Copy selected value to clipboard.
                try
                {
                    Clipboard.SetText(comboBoxClipBoard.SelectedValue.ToString());
                }
                catch (Exception ex)
                {
//                    if (ex.HResult != -2147221040) //Ignore OpenClipboard error.
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
