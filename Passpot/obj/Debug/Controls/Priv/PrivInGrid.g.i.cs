﻿#pragma checksum "C:\My_Work\DotNet\Passport_All_99\src_files\Passpot\Controls\Priv\PrivInGrid.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2677D6B90CA02E90AC378B736C73AD97"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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


namespace Passpot.Controls.Priv {
    
    
    public partial class PrivInGrid : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadButton button_add;
        
        internal Telerik.Windows.Controls.RadGridView Link;
        
        internal Telerik.Windows.Controls.RadContextMenu GridContextMenu;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Passpot;component/Controls/Priv/PrivInGrid.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.button_add = ((Telerik.Windows.Controls.RadButton)(this.FindName("button_add")));
            this.Link = ((Telerik.Windows.Controls.RadGridView)(this.FindName("Link")));
            this.GridContextMenu = ((Telerik.Windows.Controls.RadContextMenu)(this.FindName("GridContextMenu")));
        }
    }
}
