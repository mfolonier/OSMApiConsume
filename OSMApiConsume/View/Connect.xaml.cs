
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OSMApiConsume.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Connect : ContentPage
    {
        public Connect()
        {
            InitializeComponent();
        }
        public Connect(ViewModel.ViewModel VmBinding)
        {
            InitializeComponent();
            BindingContext = VmBinding;

        }

      
    }
}