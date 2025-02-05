using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Speech_2._0
{
    public partial class App : Application
    {
        public App()
        {
            // Application.Current.UserAppTheme = OSAppTheme.Light;
            InitializeComponent();
            
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
