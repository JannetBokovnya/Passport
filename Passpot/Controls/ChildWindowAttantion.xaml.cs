
using System.Windows;
using System.Windows.Controls;

namespace Passpot.Controls
{
    public partial class ChildWindowAttantion : ChildWindow
    {
        public ChildWindowAttantion()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public TextBlock TitleBox
        {
            get { return titleBox; }
        }
    }
}

