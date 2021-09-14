using Newtonsoft.Json;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using OSMApiConsume.Class;
using OSMApiConsume.Modelo;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace OSMApiConsume.VistaModelo
{


    public class VistaModelo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

       
        public GeolocationGPS _geolocation = new GeolocationGPS();


        //definimos el objeto modelo del tipo BLEmodel
       
        //definimo el objeto ruteo del tipo OSMmodel
        private OSMmodel _ruteo;
        public OSMmodel Ruteo

        {
            get
            {
                return _ruteo;
            }
            set
            {
                _ruteo = value;
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
        private string _txtDireccion;
        public string txtDireccion
        {
            get => _txtDireccion;
            set
            {
                if (_txtDireccion != value)
                {
                    _txtDireccion = value;
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

                    await ((NavigationPage)App.Current.MainPage).PushAsync(new Vista.SmartBike(App.vmBLE));

                });




                return _CmdStart;
            }
        }









        private Command _cmdIr;
        public Command  CmdIr
        {
            get
            {
                if (_cmdIr == null)
                {
                   
                     _cmdIr = new Command(async () =>
                     {
                        
                         IsWorking = true;
                         await GetDirection();
                     }

                   );
                   

                   
                }
                return _cmdIr;
            }
        }

        public async Task<OSMmodel> GetDirection()  
        {

            IsWorking = true;
            
            string Destino = "";
           
            if (txtDireccion != null  )
            {
                                
              
                    try
                    {
                        var locations_destino = await Geocoding.GetLocationsAsync(txtDireccion);
                        var location_destino = locations_destino?.FirstOrDefault();
                        if(location_destino != null) { 
                        Destino = location_destino.Longitude.ToString() + "," + location_destino.Latitude.ToString();
                        }
                        else
                        {
                            txtDireccion = null;
                            await App.Current.MainPage.DisplayAlert("Error!", "Invalid address", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                    
                    }
    
                    string Origen = "";
                    await _geolocation.GetLocation();
                    Origen = GeolocationGPS.Lng.ToString() + "," + GeolocationGPS.Lat.ToString();
                   
                    var conexion = $"http://router.project-osrm.org/route/v1/driving/{Origen};{Destino}?overview=simplified&geometries=polyline&steps=true";

         
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
                                    Ruteo = JsonConvert.DeserializeObject<OSMmodel>(json); 

                                    if (Ruteo.Code.Equals("Ok"))
                                    {

                                        MapRender.Goomap(Ruteo); 
                                       
                                             
                                    }
                                    else
                                    {

                                        await App.Current.MainPage.DisplayAlert("Error!", "Address not found", "OK");
                                 
                                    
                                    }

                                
                                    if (Ruteo.Routes[0].Legs[0].Steps[1].Maneuver.Type == "arrive" && Ruteo.Routes[0].Legs[0].Steps[0].Distance < 50)
                                    {
                                        IsArrive = true;
                                    
                                        await stopTracking();


                                        return null;
                                    }
                                    else { 
                                    await startTracking(); 
                                
                                    return Ruteo;
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
                Destino = "";
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
