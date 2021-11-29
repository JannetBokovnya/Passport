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
    public class CustomTextBlock:TextBox
    {
        private Border MouseOverBorder;
        private Border ReadOnlyBorder;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var zeroThickness = new Thickness(0);
            var transparent = new SolidColorBrush(Colors.Transparent);
            var contentElement = GetTemplateChild("ContentElement") as ScrollViewer;

            MouseOverBorder = contentElement.Parent as Border;
            ReadOnlyBorder = ((System.Windows.Controls.Grid)MouseOverBorder.Parent).Children[0] as Border;

            ReadOnlyBorder.Background = transparent;
            ReadOnlyBorder.BorderThickness = zeroThickness;

            Background = transparent;
            BorderThickness = zeroThickness;

            Padding = zeroThickness;
            TextWrapping = TextWrapping.Wrap;
            IsReadOnly = true;
        }
    }
}

 //<cs:CustomTextBlock Grid.Column="2"  Margin="4" >
 //                   <cs:CustomTextBlock.Text>
 //                       Lorem ipsum dolor sit amet, consectetur adipiscing elit...
 //                   </cs:CustomTextBlock.Text>
 //               </cs:CustomTextBlock>
 //xmlns:cs="clr-namespace:SilverlightApplication6"
