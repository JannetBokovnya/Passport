﻿#pragma checksum "C:\My_Work\DotNet\Passport_All_99\src_files\Passpot\Controls\TextLabelControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3EEA1AEFEBF7F2008D900D4C35185349"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Passpot.components;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Passpot.Controls {
    
    
    public partial class TextLabelControl : System.Windows.Controls.UserControl {
        
        internal Passpot.components.CustomTextBlock titleBox;
        
        internal Passpot.components.CustomTextBlock valueBox;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Passpot;component/Controls/TextLabelControl.xaml", System.UriKind.Relative));
            this.titleBox = ((Passpot.components.CustomTextBlock)(this.FindName("titleBox")));
            this.valueBox = ((Passpot.components.CustomTextBlock)(this.FindName("valueBox")));
        }
    }
}

