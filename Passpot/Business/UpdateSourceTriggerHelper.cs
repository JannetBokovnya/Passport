using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Passpot.Business
{
    public static class UpdateSourceTriggerHelper
    {
        public static readonly DependencyProperty UpdateSourceTriggerProperty =
            DependencyProperty.RegisterAttached("UpdateSourceTrigger", typeof(bool), typeof(UpdateSourceTriggerHelper),
                                                new PropertyMetadata(false, OnUpdateSourceTriggerChanged));

        public static bool GetUpdateSourceTrigger(DependencyObject d)
        {
            return (bool)d.GetValue(UpdateSourceTriggerProperty);
        }

        public static void SetUpdateSourceTrigger(DependencyObject d, bool value)
        {
            d.SetValue(UpdateSourceTriggerProperty, value);
        }

        private static void OnUpdateSourceTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool && d is PasswordBox)
            {
                PasswordBox textBox = d as PasswordBox;
                textBox.PasswordChanged -= PassportBoxPasswordChanged;

                if ((bool)e.NewValue)
                    textBox.PasswordChanged += PassportBoxPasswordChanged;
            }
        }

        private static void PassportBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as PasswordBox;
            if (frameworkElement != null)
            {
                BindingExpression bindingExpression = frameworkElement.GetBindingExpression(PasswordBox.PasswordProperty);
                if (bindingExpression != null)
                    bindingExpression.UpdateSource();
            }
        }
    }
}
