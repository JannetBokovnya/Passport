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
    [TemplatePart(Name = ExtendedGridSplitter.CollapseButtonElement, Type = typeof(Button))]

    public class ExtendedGridSplitter : GridSplitter
    {
        // public event EventHandler<CountRoutedEventArgs> CollapseButtonClicked;
        public event EventHandler CollapseButtonClicked;

        private const string CollapseButtonElement = "CollapseButton";

        public Button CollapseButton;

        public ExtendedGridSplitter()
        {
            this.DefaultStyleKey = typeof(ExtendedGridSplitter);
        }

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

    public class CountdEventArgs : EventArgs
    {
        public double Count { get; set; }

        public CountdEventArgs(double count)
        {
            Count = count;
        }
    }



    //[TemplatePart(Name = ExtendedGridSplitter.CollapseButtonElement, Type = typeof (Button))]
    //public class ExtendedGridSplitter : GridSplitter
    //{
    //    private const string CollapseButtonElement = "CollapseButton";

    //    public Button CollapseButton;

    //    public ExtendedGridSplitter()
    //    {
    //        this.DefaultStyleKey = typeof (ExtendedGridSplitter);
    //    }

    //    public override void OnApplyTemplate()
    //    {
    //        base.OnApplyTemplate();

    //        CollapseButton = this.GetTemplateChild(CollapseButtonElement) as Button;
    //        if (CollapseButton != null) CollapseButton.Click += new RoutedEventHandler(OnCollapseButtonClickEvent);
    //    }

    //    public delegate void CollapseButtonClickEventHandler(object sender);

    //    public event CollapseButtonClickEventHandler CollapseButtonClickEvent;

    //    private void OnCollapseButtonClickEvent(object sender, RoutedEventArgs e)
    //    {
    //        if (CollapseButtonClickEvent != null) CollapseButtonClickEvent(sender);
    //    }
    //}
}
