using CameraArcheryLib;
using CameraArcheryLib.Utils;
using System.Windows;

namespace CameraArchery.View
{
    /// <summary>
    /// custom messagebox to inform of an error
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        /// <summary>
        /// window to inform an error
        /// </summary>
        /// <param name="caption">title of the window</param>
        /// <param name="text"> text in the window</param>
        /// <param name="detail">detail of the exception</param>
        public CustomMessageBox(string captionKey, string textKey, string detail)
        {
            InitializeComponent();

            string caption;
            string text;

            caption = LanguageController.Get(captionKey);
            text = LanguageController.Get(textKey);
            DetailButton.Content = LanguageController.Get("ShowDetail");

            // adapt the args
            this.Title = caption;
            MessageBlock.Text = text;
            DetailBlock.Text = detail;

            // get the last log
            LogTextBox.Text = "";
            foreach (var item in LogHelper.ReadLast())
                LogTextBox.Text += item + "\n";
        }

        /// <summary>
        /// event on the detail button
        /// <para>show and hide the log</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            // show detail
            if (DetailButton.Content.ToString() == LanguageController.Get("ShowDetail"))
            {
                PanelDetail.Visibility = Visibility.Visible;
                this.Height = 300;
                DetailButton.Content = LanguageController.Get("HideDetail");
            }
            //hide detail
            else
            {
                PanelDetail.Visibility = Visibility.Collapsed;
                this.Height = 200;
                DetailButton.Content = LanguageController.Get("ShowDetail");
            }
        }

        /// <summary>
        /// event of the click on the return button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
