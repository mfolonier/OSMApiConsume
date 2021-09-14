using Xamarin.Forms;

namespace OSMApiConsume
{
    public partial class App : Application
    {

        public static VistaModelo.VistaModelo vmBinding;

        public App()
        {
            InitializeComponent();
           
            vmBinding = new VistaModelo.VistaModelo();
            MainPage = new NavigationPage(new Vista.Connect(vmBinding));
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
