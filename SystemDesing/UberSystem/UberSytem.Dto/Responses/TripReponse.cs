using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberSytem.Dto.Responses
{
    public class TripReponse
    {
        public double SourceLatitude { get; set; }  
        public double SourceLongitude { get; set; } 
        public double DestinationLatitude { get; set; } 
        public double DestinationLongitude { get; set; } 

        public long CustomerId { get; set; } 
        public char Method { get; set; }
        public double Amount { get; set; } 
    }
}
