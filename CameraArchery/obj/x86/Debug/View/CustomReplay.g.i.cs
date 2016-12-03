﻿#pragma checksum "..\..\..\..\View\CustomReplay.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FF27BD507E85BEF74F636F4EABAB4BE7"
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
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CameraArchery.View {
    
    
    /// <summary>
    /// CustomReplay
    /// </summary>
    public partial class CustomReplay : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 9 "..\..\..\..\View\CustomReplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox VideoList;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\View\CustomReplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button StartButton;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\View\CustomReplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button StopButton;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\View\CustomReplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CheckButton;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\View\CustomReplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label SpeedLabel;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\View\CustomReplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblStatus;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\View\CustomReplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider TimeSlider;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\View\CustomReplay.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MediaElement MediaElementVideo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CameraArchery;component/view/customreplay.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\CustomReplay.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\..\View\CustomReplay.xaml"
            ((CameraArchery.View.CustomReplay)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.UserControl_Unloaded);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\..\View\CustomReplay.xaml"
            ((CameraArchery.View.CustomReplay)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.VideoList = ((System.Windows.Controls.ListBox)(target));
            
            #line 10 "..\..\..\..\View\CustomReplay.xaml"
            this.VideoList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.VideoList_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\View\CustomReplay.xaml"
            this.VideoList.Loaded += new System.Windows.RoutedEventHandler(this.VideoList_Loaded);
            
            #line default
            #line hidden
            return;
            case 4:
            this.StartButton = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\View\CustomReplay.xaml"
            this.StartButton.Click += new System.Windows.RoutedEventHandler(this.Start_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.StopButton = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\..\View\CustomReplay.xaml"
            this.StopButton.Click += new System.Windows.RoutedEventHandler(this.Stop_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 49 "..\..\..\..\View\CustomReplay.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SpeedDown_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CheckButton = ((System.Windows.Controls.CheckBox)(target));
            
            #line 58 "..\..\..\..\View\CustomReplay.xaml"
            this.CheckButton.Checked += new System.Windows.RoutedEventHandler(this.Frame_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.SpeedLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            
            #line 63 "..\..\..\..\View\CustomReplay.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SpeedUp_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.lblStatus = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.TimeSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 75 "..\..\..\..\View\CustomReplay.xaml"
            this.TimeSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.TimeSlider_ValueChanged);
            
            #line default
            #line hidden
            
            #line 76 "..\..\..\..\View\CustomReplay.xaml"
            this.TimeSlider.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.TimeSlider_GotMouseCapture);
            
            #line default
            #line hidden
            
            #line 76 "..\..\..\..\View\CustomReplay.xaml"
            this.TimeSlider.LostMouseCapture += new System.Windows.Input.MouseEventHandler(this.TimeSlider_LostMouseCapture);
            
            #line default
            #line hidden
            return;
            case 12:
            this.MediaElementVideo = ((System.Windows.Controls.MediaElement)(target));
            
            #line 81 "..\..\..\..\View\CustomReplay.xaml"
            this.MediaElementVideo.MediaEnded += new System.Windows.RoutedEventHandler(this.MediaElementVideo_MediaEnded);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 3:
            
            #line 20 "..\..\..\..\View\CustomReplay.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteItem_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

