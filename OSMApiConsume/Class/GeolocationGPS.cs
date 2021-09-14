using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace OSMApiConsume.Class
{
    public class GeolocationGPS
    {

        
        public static double Lat { get; set; }
        public static double Lng { get; set; }
  
        public static IGeolocator _locator { get; set; }

        public async Task GetLocation()
        {
            try
            {
                var location = CrossGeolocator.Current;
                location.DesiredAccuracy = 10;
                var position = await location.GetPositionAsync(TimeSpan.FromMilliseconds(500));
                
                
                if (position != null)
                {

                    Lat = position.Latitude;
                    Lng = position.Longitude;
                   
                    _locator = location;

                }

                else
                {

                    var knowposicion = await location.GetLastKnownLocationAsync();
                    Lat = knowposicion.Latitude;
                    Lng = knowposicion.Longitude;
                
                    _locator = location;
                }

                

               
            }

            catch(Exception ex)
            {

            }



        }


        


    }
}
