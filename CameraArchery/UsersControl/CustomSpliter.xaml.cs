
namespace CameraArchery.UsersControl
{
    /// <summary>
    /// Interaction logic for customSpliter.xaml
    /// </summary>
    public partial class CustomSpliter : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// value in the spliter
        /// </summary>
        public double Value
        {
            get
            {
                return slValue.Value;
            }
            set
            {
                slValue.Value = value;
            }
        }

        /// <summary>
        /// maximum of the spliter
        /// </summary>
        public double Maximum 
        {
            get 
            { 
                return slValue.Maximum; 
            } 
            set 
            {
                slValue.Maximum = value;
            } 
        }

        /// <summary>
        /// frequency of the spliter
        /// </summary>
        public double TickFrequency 
        { 
            get
            {
                return slValue.TickFrequency;
            }
            set
            {
                slValue.TickFrequency = value;
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public CustomSpliter()
        {
            InitializeComponent();
        }
    }
}
