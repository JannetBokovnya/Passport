
using System;
using System.Windows;
using System.Windows.Controls;

namespace Media
{
    /// <summary>
    /// Sample ChildWindow for demonstration purposes.
    /// </summary>
    public partial class OpenBigMedia : ChildWindow
    {
        public event EventHandler<BrowserItemEventArgs> OnNextPressed;

        /// <summary>
        /// Initializes a DemoChildWindow.
        /// </summary>
        public OpenBigMedia()
        {
            InitializeComponent();
           // optionsStack.DataContext = this;
        }

        /// <summary>
        /// Handles the Click event of the OK button.
        /// </summary>
        /// <param name="sender">OK Button.</param>
        /// <param name="e">Event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by event defined in Xaml.")]
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Handles the Click event of the Cancel button.
        /// </summary>
        /// <param name="sender">Cancel button.</param>
        /// <param name="e">Event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by event defined in Xaml.")]
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnNextPressed!=null)
                OnNextPressed(this, new BrowserItemEventArgs() {BrowserItem = DataContext as BrowserItem, IsNext = true} );
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnNextPressed != null)
                OnNextPressed(this, new BrowserItemEventArgs() { BrowserItem = DataContext as BrowserItem, IsNext = false });
        }
    }

    public class BrowserItemEventArgs:EventArgs
    {
        public BrowserItem BrowserItem { get; set; }
        public bool IsNext { get; set; }
    }
}
