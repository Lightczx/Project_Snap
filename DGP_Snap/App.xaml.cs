
using DGP_Snap.Helpers;
using DGP_Snap.Models;
using DGP_Snap.Pages;
using DGP_Snap.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DGP_Snap
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
            
        //}

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            NavigationService.Navigate<HomePage>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            NativeMethods.ShowSystemTaskBar();
            
        }

    }
}
