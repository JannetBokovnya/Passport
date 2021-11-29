using System.Windows;
using System.Windows.Controls;

namespace Media
{
	public partial class MediaChildWindowDelete : ChildWindow
	{
		public MediaChildWindowDelete()
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