using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MouseKeyboardActivityMonitor;
using WindowsInput;
using System.Runtime.InteropServices;

namespace KeyboardOverride_Forms
{
    public partial class Form1 : Form
    {
        KeyboardHookListener keyboardListener; //Used to listen to keyboard events.

        //Imported function used to simulate mouse click events.
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        bool rCtrlIsDown = false; //Tracks whether the right ctrl key is down.
        bool appsKeyIsDown = false; //Tracks whether the apps key is down.
        bool rAltIsDown = false; //Tracks whether the right alt key is down.

        public Form1()
        {
            InitializeComponent();

            //Initialize keyboardListener to handle global events, be enabled, and handle key down and key up events.
            keyboardListener = new KeyboardHookListener(new MouseKeyboardActivityMonitor.WinApi.GlobalHooker());
            keyboardListener.Enabled = true;
            keyboardListener.KeyDown += keyboardListener_KeyDown;
            keyboardListener.KeyUp += keyboardListener_KeyUp;
        }

        void keyboardListener_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.Text = e.KeyCode.ToString();

            //Simulate left click down with apps key.
            if(e.KeyCode == Keys.Apps)
            {
                if (appsKeyIsDown == false)
                {
                    uint X = (uint)Cursor.Position.X;
                    uint Y = (uint)Cursor.Position.Y;
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);

                    appsKeyIsDown = true;
                }

                e.SuppressKeyPress = true;
            }

            /*
            //Simulate left click down with right alt key.
            if (e.KeyCode == Keys.RMenu)
            {
                e.SuppressKeyPress = true;

                if (rAltIsDown == false)
                {
                    uint X = (uint)Cursor.Position.X;
                    uint Y = (uint)Cursor.Position.Y;
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);

                    rAltIsDown = true;
                }
            }
             */

            //Set flag if right ctrl is pressed down.
            if (e.KeyCode == Keys.RControlKey)
            {
                rCtrlIsDown = true;
            }

            //Simulate (ctrl + c) if (right ctrl + left arrow) is pressed.
            if( e.KeyCode == Keys.Left)
            {
                if(rCtrlIsDown)
                {
                    InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                    e.SuppressKeyPress = true;
                }
            }

            //Simulate (ctrl + v) if (right ctrl + right arrow) is pressed.
            if((e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Down))
            {
                if(rCtrlIsDown)
                {
                    InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void keyboardListener_KeyUp(object sender, KeyEventArgs e)
        {
            //Simulate left mouse click up with apps key.
            if (e.KeyCode == Keys.Apps)
            {
                uint X = (uint)Cursor.Position.X;
                uint Y = (uint)Cursor.Position.Y;
                mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);

                appsKeyIsDown = false;
                
                e.SuppressKeyPress = true;
            }

            //Simulate left mouse click up with right alt key.
            if (e.KeyCode == Keys.RMenu)
            {
                //e.SuppressKeyPress = true;

                InputSimulator.SimulateKeyUp(VirtualKeyCode.RMENU);

                uint X = (uint)Cursor.Position.X;
                uint Y = (uint)Cursor.Position.Y;
                mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);

                rAltIsDown = false;
            }

            //Set flag if right ctrl is released.
            if(e.KeyCode == Keys.RControlKey)
            {
                rCtrlIsDown = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void enabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(keyboardListener.Enabled == true)
            {
                keyboardListener.Enabled = false;
                contextMenuStrip1.Items[0].Text = "Disabled";
            }
            else if(keyboardListener.Enabled == false)
            {
                keyboardListener.Enabled = true;
                contextMenuStrip1.Items[0].Text = "Enabled";
            }
        }
    }
}
