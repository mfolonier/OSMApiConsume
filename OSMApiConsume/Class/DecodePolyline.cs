
using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;


namespace OSMApiConsume.Class
{
    public class DecodePolyline
    {

        

        
        public static List<Position> DecodePolylinePoints(string encodedPoints )
        {

            if (encodedPoints == null || encodedPoints == "") return null;
           
            List<Position> Poly = new List<Position>();
 
            Char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;
            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while(index < polylinechars.Length)
                {
                   
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                        

                    } while (next5bits >= 32 && index < polylinechars.Length);
                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                  
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;

                    } while (next5bits >= 32 && index < polylinechars.Length);
                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);


                    Position p = new Position(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                    Poly.Add(p);

                }


            }
            catch(Exception Ex)
            {


            }


            return Poly;

        }



    }


   
}
