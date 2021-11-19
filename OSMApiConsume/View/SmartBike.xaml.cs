using OSMApiConsume.Class;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;


namespace OSMApiConsume.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SmartBike : ContentPage
    {
        
        private GeolocationGPS _geolocation = new GeolocationGPS();
        
        Position _MapPosition;

        public SmartBike()
        {
            InitializeComponent();


        }



        protected async override void OnAppearing()
        {

            base.OnAppearing();
            
            await _geolocation.GetLocation();

            if (GeolocationGPS._locator.IsGeolocationAvailable)
            {

                _MapPosition = new Position(GeolocationGPS.Lat, GeolocationGPS.Lng);
                Map.MoveToRegion(
                    MapSpan.FromCenterAndRadius
                        (_MapPosition,
                            Distance.FromKilometers(0.5)
                        ), true
                );
                System.Diagnostics.Debug.WriteLine("position is : {0}  and {1} ", _MapPosition.Latitude , _MapPosition.Longitude);

                Map.MyLocationEnabled = true;

              
                var pinposition = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(GeolocationGPS.Lat, GeolocationGPS.Lng),
                    Label = "",
                    Address = ""

                };
                Map.Pins.Add(pinposition);
                MapRender.NewMap(Map);

                
            }
            else
            {

                await DisplayAlert("Message", "GPS Not Available", "OK");

            }
            

        }


        public SmartBike(ViewModel.ViewModel vmBinding)
        {
            InitializeComponent();
            BindingContext = vmBinding;
           

        
        }


    }
}