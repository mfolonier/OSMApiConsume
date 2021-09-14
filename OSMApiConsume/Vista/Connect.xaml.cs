
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OSMApiConsume.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Connect : ContentPage
    {
        public Connect()
        {
            InitializeComponent();
        }
        public Connect(VistaModelo.VistaModelo vmBLE)
        {
            InitializeComponent();
            BindingContext = vmBLE;

        }

      
    }
}