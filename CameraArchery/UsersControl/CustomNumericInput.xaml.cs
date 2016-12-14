using CameraArcheryLib.Utils;
using System;
using System.Windows.Controls;

namespace CameraArchery.UsersControl
{
    /// <summary>
    /// Interaction logic for CustomNumericInput.xaml
    /// </summary>
    public partial class CustomNumericInput : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// inform if the value must be double or just int
        /// </summary>
        public bool IsDouble { get; set; }

        /// <summary>
        /// text on the component
        /// </summary>
        public String Text
        {
            get
            {
                return TextBox.Text; 
            }
            set
            {
                TextBox.Text = value;
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public CustomNumericInput()
        {
            InitializeComponent();
            this.IsDouble = false;
        }

        #region event
        /// <summary>
        /// event to limit the numeric input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if(IsDouble)
                e.Handled = !FormatHelper.IsNumericText(TextBox.Text + e.Text);
            else
                e.Handled = !FormatHelper.IsTextNumericInteger(TextBox.Text + e.Text);
        }
        #endregion
    }
}
