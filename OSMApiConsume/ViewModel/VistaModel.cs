using Newtonsoft.Json;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using OSMApiConsume.Class;
using OSMApiConsume.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace OSMApiConsume.ViewModel
{


    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

       
        public GeolocationGPS _geolocation = new GeolocationGPS();


        //definimos el objeto modelo del tipo BLEmodel
       
        //definimo el objeto ruteo del tipo OSMmodel
        private OSMmodel _route;
        public OSMmodel Route

        {
            get
            {
                return _route;
            }
            set
            {
                _route = value;
                OnPropertyChanged("googlemap");

            }
        }

        private DecodePolyline _Positions;
        public DecodePolyline Positions
        {
            get
            {
                return _Positions;
            }
            set
            {
                _Positions = value;
                OnPropertyChanged("Positions");

            }
        }
        private string _txtAddress;
        public string txtAddress
        {
            get => _txtAddress;
            set
            {
                if (_txtAddress != value)
                {
                    _txtAddress = value;
                    OnPropertyChanged();
                }

            }
        }

        private bool _IsArrive;
        public bool IsArrive
        {
            get => _IsArrive;
            set
            {

                _IsArrive = value;
                OnPropertyChanged("IsArrive");
            }

        }


       
       
        private bool _IsWorking;
        public bool IsWorking
        {
            get => _IsWorking;
            set
            {

                _IsWorking = value;
                OnPropertyChanged();
            }

        }

      
       


        private Command _CmdStart;
        public Command cmdStart
        {

            get
            {

                _CmdStart = new Command(async () =>
                {
                    IsWorking = false;

                    await ((NavigationPage)App.Current.MainPage).PushAsync(new View.SmartBike(App.vmBinding));

                });




                return _CmdStart;
            }
        }









        private Command _cmdGo;
        public Command cmdGo
        {
            get
            {
                if (_cmdGo == null)
                {
                   
                     _cmdGo = new Command(async () =>
                     {
                        
                         IsWorking = true;
                         await GetDirection();
                     }

                   );
                   

                   
                }
                return _cmdGo;
            }
        }

        public async Task<OSMmodel> GetDirection()  
        {

            IsWorking = true;
            
            string finalAddress = "";
           
            if (txtAddress != null  )
            {
                                
              
                    try
                    {
                        var locations_destino = await Geocoding.GetLocationsAsync(txtAddress);
                        var location_destino = locations_destino?.FirstOrDefault();
                        if(location_destino != null) { 
                        finalAddress = location_destino.Longitude.ToString() + "," + location_destino.Latitude.ToString();
                        }
                        else
                        {
                            txtAddress = null;
                            await App.Current.MainPage.DisplayAlert("Error!", "Invalid address", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                    
                    }
    
                    string Origin = "";
                    await _geolocation.GetLocation();
                    Origin = GeolocationGPS.Lng.ToString() + "," + GeolocationGPS.Lat.ToString();
                   
                    var conexion = $"http://router.project-osrm.org/route/v1/driving/{Origin};{finalAddress}?overview=simplified&geometries=polyline&steps=true";

         
                if (IsArrive == false)
                {
                    try
                    {
                       
                      
                           HttpClient cliente = new HttpClient();
                            var peticion = await cliente.GetAsync(conexion);
                            

                            if (peticion.StatusCode == System.Net.HttpStatusCode.OK)
                            {

                                var json = await peticion.Content.ReadAsStringAsync(); 

                                if (peticion.IsSuccessStatusCode)
                                {
                                    Route = JsonConvert.DeserializeObject<OSMmodel>(json); 

                                    if (Route.Code.Equals("Ok"))
                                    {

                                        MapRender.Goomap(Route); 
                                       
                                             
                                    }
                                    else
                                    {

                                        await App.Current.MainPage.DisplayAlert("Error!", "Address not found", "OK");
                                 
                                    
                                    }

                                
                                    if (Route.Routes[0].Legs[0].Steps[1].Maneuver.Type == "arrive" && Route.Routes[0].Legs[0].Steps[0].Distance < 50)
                                    {
                                        IsArrive = true;
                                    
                                        await stopTracking();


                                        return null;
                                    }
                                    else { 
                                    await startTracking(); 
                                
                                    return Route;
                                    }
                                    
                                }
                                return null;

                            }

                        

                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Not connect with OSM", "Poor Conection", "OK");
                    }
                }

            }
            else
            {

                await App.Current.MainPage.DisplayAlert("Empty address", "Enter an Address", "OK");
                MapRender.CleanDraw();
                finalAddress = "";
                await stopTracking();
                
            }
            return default;
        }


        public  async Task startTracking()
         {

             if (CrossGeolocator.Current.IsListening)
                 return;
              await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(7), 10,true);  
                         CrossGeolocator.Current.PositionChanged += PositionChanged;
         }
                     
        public async Task stopTracking()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();
           

        }
        
        private async void PositionChanged(object sender, PositionEventArgs e)
        {
           

                await GetDirection();
                  
               
            
        }

        

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
