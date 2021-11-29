using System.Windows;
using System.Windows.Controls;

namespace Passpot.Controls
{
	public partial class ChildWindowDelete : ChildWindow
	{
		public ChildWindowDelete()
		{
			InitializeComponent();
		}

		private void OKButtonDelete_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
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