using System.Collections.Generic;
using Xamarin.Forms;

using OSMApiConsume.Modelo;

using Xamarin.Forms.GoogleMaps;

namespace OSMApiConsume.Class
{
    public  class MapRender 
    {   
        static Map _Map;
        public static void NewMap(Map map)
        {

            _Map = new Map();
            _Map = map;
            
        }



        static OSMmodel OSMdata;
        public static void Goomap(OSMmodel osmModelData)
        {

            OSMdata = new OSMmodel();
            OSMdata = osmModelData;
            Polydraw();
        }

      
        public static void CleanDraw()
        {
            _Map.Polylines.Clear();
            _Map.Pins.Clear();
            var pinposition = new Pin
            {
                Type = PinType.Place,
                Position = new Position(GeolocationGPS.Lat, GeolocationGPS.Lng),
                Label = "",
                Address = ""

            };
            _Map.Pins.Add(pinposition);
        }


        public static  void Polydraw()
        {
            double endlat;
            double endlng;
            _Map.Polylines.Clear();
            _Map.Pins.Clear();

            endlat = OSMdata.Waypoints[1].Location[1];
            endlng = OSMdata.Waypoints[1].Location[0];


                var pinendposition = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(endlat,endlng),
                    Label = "",
                    Address = ""

                };
                _Map.Pins.Add(pinendposition);

                var _polyline = new Polyline();
                List<Position> Poly = new List<Position>();


           


            Poly = DecodePolyline.DecodePolylinePoints(OSMdata.Routes[0].Geometry);
            foreach (var posi in Poly)
            {

                    _polyline.Positions.Add(posi);

            }

                _polyline.StrokeColor = Color.Blue;
                _polyline.StrokeWidth = 5f;
                _Map.Polylines.Add(_polyline);

                Position position = new Position(GeolocationGPS.Lat, GeolocationGPS.Lng);
                _Map.MoveToRegion(MapSpan.FromCenterAndRadius (position, Distance.FromKilometers(0.3)), true);




        }

    }
}
