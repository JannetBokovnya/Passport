﻿#pragma checksum "C:\My_Work\DotNet\Passport_All_99\src_files\Passpot\Controls\NumberControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9CFFB90F335A20E4C17C195B03384222"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    
    
    public partial class NumberControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.TextBlock titleNumberBox;
        
        internal System.Windows.Controls.TextBox valueNumberBox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Passpot;component/Controls/NumberControl.xaml", System.UriKind.Relative));
            this.titleNumberBox = ((System.Windows.Controls.TextBlock)(this.FindName("titleNumberBox")));
            this.valueNumberBox = ((System.Windows.Controls.TextBox)(this.FindName("valueNumberBox")));
        }
    }
}

