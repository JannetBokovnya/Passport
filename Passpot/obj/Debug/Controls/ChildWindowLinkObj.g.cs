﻿#pragma checksum "C:\My_Work\DotNet\Passport_All_99\src_files\Passpot\Controls\ChildWindowLinkObj.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6ECA95D095C7E6EDFF21D07C02F0E9FF"
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
using Telerik.Windows.Controls;


namespace Passpot.Controls {
    
    
    public partial class ChildWindowLinkObj : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadComboBox comboBoxLink;
        
        internal System.Windows.Controls.TextBlock tbNameObj;
        
        internal Telerik.Windows.Controls.RadGridView grid;
        
        internal Telerik.Windows.Controls.RadButton btnOk;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Passpot;component/Controls/ChildWindowLinkObj.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.comboBoxLink = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("comboBoxLink")));
            this.tbNameObj = ((System.Windows.Controls.TextBlock)(this.FindName("tbNameObj")));
            this.grid = ((Telerik.Windows.Controls.RadGridView)(this.FindName("grid")));
            this.btnOk = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnOk")));
        }
    }
}

