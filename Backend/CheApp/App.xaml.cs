﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CheApp.View;

namespace CheApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    Resources.MergedWith = typeof(Xamarin.Forms.Themes.LightThemeResources);
                    break;
                case Device.UWP:
                    // must be set in the uwp app constructor because f*** you that's why
                    break;
            }
            Resources.TryGetValue("Xamarin.Forms.StyleClass.Success", out object temp);
            
            MainPage = new MainMenu();
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}