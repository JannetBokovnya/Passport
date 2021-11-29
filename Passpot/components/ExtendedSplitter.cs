using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Passpot.components
{
    [TemplatePart(Name = ExtendedSplitter.CollapseButtonElement, Type = typeof(Button))]
    public class ExtendedSplitter: GridSplitter
    {
        public event EventHandler CollapseButtonClicked;

        private const string CollapseButtonElement = "CollapseButton";
         public Button CollapseButton;

         //public ExtendedSplitter()
         //{
         //    this.DefaultStyleKey = typeof(ExtendedSplitter);
         //}

         public override void OnApplyTemplate()
         {
             base.OnApplyTemplate();

             CollapseButton = this.GetTemplateChild(CollapseButtonElement) as Button;

             if (CollapseButton != null)
             {
                 CollapseButton.Click += RaiseCollapseButtonClicked;
             }
         }

         private void RaiseCollapseButtonClicked(object sender, RoutedEventArgs e)
         {
             var temp = CollapseButtonClicked;

             if (temp != null)
             {
                 //temp(this, new CountRoutedEventArgs(ActualWidth));
                 temp(this, new EventArgs());
             }
         }
    }
}
