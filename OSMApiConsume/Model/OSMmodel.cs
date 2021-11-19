using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OSMApiConsume.Model
{
    public class OSMmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

           
        public string Code { get; set; }
             
        public List<Waypoint> Waypoints { get; set; }
              
        public List<Route> Routes { get; set; }

      

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
             {
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
             }

    }


    public partial class Route
    {
        
        public List<Leg> Legs { get; set; }

        
        public string WeightName { get; set; }

       
        public string Geometry { get; set; }

        
        public double Weight { get; set; }

      
        public long Distance { get; set; }

        
        public double Duration { get; set; }
    }

    public partial class Leg
    {
       
        public List<Step> Steps { get; set; }

        public double Weight { get; set; }

        public long Distance { get; set; }

        
        public string Summary { get; set; }

     
        public double Duration { get; set; }
    }

    public partial class Step
    {
        
        public List<Intersection> Intersections { get; set; }

       
        public string DrivingSide { get; set; }

       
        public string Geometry { get; set; }

       
        public double Duration { get; set; }

       
        public double Distance { get; set; }

        
        public string Name { get; set; }

        public double Weight { get; set; }

       
        public string Mode { get; set; }

     
        public Maneuver Maneuver { get; set; }

        
        public string Ref { get; set; }
    }

    public partial class Intersection
    {
        
        public long? Out { get; set; }

       
        public List<bool> Entry { get; set; }

    
        public List<double> Location { get; set; }


        public List<long> Bearings { get; set; }

        
        public long? In { get; set; }
    }

    public partial class Maneuver
    {
        
        public long BearingAfter { get; set; }

        
        public long BearingBefore { get; set; }

      
        public string Type { get; set; }

        
        public List<double> Location { get; set; }

        
        public string Modifier { get; set; }
    }

    public partial class Waypoint
    {
       
        public string Hint { get; set; }

        
        public List<double> Location { get; set; }

        
        public string Name { get; set; }
    }





}
