using Xamarin.Forms;

namespace OSMApiConsume
{
    public partial class App : Application
    {

        public static VistaModelo.VistaModelo vmBLE;

        public App()
        {
            InitializeComponent();
           
            vmBLE = new VistaModelo.VistaModelo();
            MainPage = new NavigationPage(new Vista.Connect(vmBLE));
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
