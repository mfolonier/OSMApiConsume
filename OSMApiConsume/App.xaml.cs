using Xamarin.Forms;

namespace OSMApiConsume
{
    public partial class App : Application
    {

        public static ViewModel.ViewModel vmBinding;

        public App()
        {
            InitializeComponent();
           
            vmBinding = new ViewModel.ViewModel();
            MainPage = new NavigationPage(new View.Connect(vmBinding));
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
