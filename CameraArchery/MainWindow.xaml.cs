using CameraArcheryLib.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using CameraArchery.View;
using CameraArcheryLib;
using System.Windows.Media.Imaging;
using Accord.Video.DirectShow;

namespace CameraArchery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VideoWindow VideoWindow;

        /// <summary>
        /// time of the PopUp
        /// </summary>
        private const int TimePopUp = 5000;

        /// <summary>
        ///  inform if a device in selected
        /// </summary>
        private bool deviceSelected { get; set; }

        /// <summary>
        /// collection of the devices
        /// </summary>
        private FilterInfoCollection videoDevices { get; set; }

        /// <summary>
        /// inform if the window will be restart or not
        /// </summary>
        private bool IsWindowsRestart { get; set; }

        /// <summary>
        /// init the view
        /// <para>init the language</para>
        /// <para>check if the application in not already existing</para>
        /// <para>if existing, open message box end close application</para>
        /// <para>init the components</para>
        /// <para>refresh the list in the combo box of device</para>
        /// <para>init the Controller</para>
        /// </summary>
        public MainWindow()
        {
            LogHelper.Write(""); LogHelper.Write(""); // saut de ligne
            LogHelper.Write("-----------------START OF THE APPLICATION--------------------");

            // init the dictionnary
            LanguageController.InitLanguage(this.Resources.MergedDictionaries);

            // quit if application already existing
            var exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1;
            if (exists)
            {
                MessageBox.Show(LanguageController.Get("DuplicationApplication"), LanguageController.Get("AlreadyStart"), MessageBoxButton.OK, MessageBoxImage.Error);
                LogHelper.Write("--------------------STOP DUPLICATE  APPLICATION---------------------");
                LogHelper.Write(""); LogHelper.Write(""); // saut de ligne
                Environment.Exit(1);
            }

            // start
            InitializeComponent();

            IsWindowsRestart = false;

            // init icon
            Uri iconUri = new Uri(@"pack://application:,,,/Ressources/Logos/logoViseur.ico");
            this.Icon = BitmapFrame.Create(iconUri);

            rfsh_Click();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == IsActiveProperty && (bool)e.NewValue)
            {
                VideoWindow?.Activate();
            }
        }

        /// <summary>
        /// event of the refresh button
        /// <para>refresh the list of the combo box of devices</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rfsh_Click(object sender, EventArgs e)
        {
            LogHelper.Write("refresh the list of devices");
            rfsh_Click();
        }

        /// <summary>
        /// open the window of settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setting_Click(object sender, EventArgs e)
        {
            var window = new SettingView();
            window.ShowDialog();
            if (window.IsChange)
            {
                new MainWindow().Show();
                IsWindowsRestart = true;
                this.Close();
            }
        }

        /// <summary>
        /// Quit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quit_Click(object sender, EventArgs e)
        {
            LogHelper.Write("-------------------END OF THE APPLICATION-------------------");
            LogHelper.Write(""); LogHelper.Write("");// saut de ligne
            Environment.Exit(0);
        }

        /// <summary>
        ///  get the list of devices name
        ///  <para>get all the devices</para>
        ///  <para>clear the list</para>
        ///  <para>add the name of the devices</para>
        ///  <para>select the first if there is a device</para>
        private void rfsh_Click()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                new CustomMessageBox("CollectDeviceError", "CollectDeviceErrorMessage", e.Message).ShowDialog();
                Environment.Exit(4);
            }
            comboBox1.Items.Clear();

            if (videoDevices.Count == 0)
            {
                LogHelper.Write("empty values in the devices list");

                comboBox1.SelectedIndex = -1;
                return;
            }

            foreach (FilterInfo device in videoDevices)
            {
                comboBox1.Items.Add(device.Name);
            }

            //log the list of device
            var listDevices = "";
            foreach (var item in comboBox1.Items)
                listDevices += item.ToString() + ", ";
            LogHelper.Write("new value in the devices list : " + listDevices);

            // change the seleted index
            comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// start the filming
        /// <para>check if a device is selected</para>
        /// <para>refresh the list of devices</para>
        /// <para>check if the device selected is already connected</para>
        /// <para> start the video window</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_Click(object sender, EventArgs e)
        {
            LogHelper.Write("click to start video windows");
            // check if a combobox is selected
            if (!deviceSelected)
            {
                LogHelper.Write("no device selected, show pop up");
                return;
            }

            var selectedDevice = videoDevices[comboBox1.SelectedIndex];
            rfsh_Click();

            // check if the device selected is already existing
            if (!comboBox1.Items.Contains(selectedDevice.Name))
            {
                LogHelper.Write("device not still connected");
                return;
            }

            LogHelper.Write("video window will be started");

            // start window
            VideoWindow = new VideoWindow(selectedDevice);
            VideoWindow.ShowDialog();
            VideoWindow = null;
        }

        /// <summary>
        /// event when the selection is change in the list of devices
        /// <para>if selection is not null, set the color of the list to blue and set <code>deviceSelected</code> to true</para>
        /// <para>else, set the color of the list to red and set <code>deviceSelected</code> to false</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var combobox = (sender as ComboBox);
            LogHelper.Write("combo box of device selection changed : " + combobox.SelectedValue);

            if (combobox.SelectedIndex != -1)
            {
                deviceSelected = true;
                comboBox1.Background = System.Windows.Media.Brushes.LightBlue;
            }
            else
            {
                deviceSelected = false;
                comboBox1.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsWindowsRestart)
            {
                LogHelper.Write("--------------------STOP OF THE APPLICATION---------------------");
                Environment.Exit(0);
            }
            else
                IsWindowsRestart = false;
        }
    }
}
