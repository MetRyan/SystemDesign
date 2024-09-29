using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberSystem.Domain.Entities;

namespace UberSytem.Dto.Responses
{
    public class TripReponse
    {
        public long id { get; set; }
        public long CustomerId { get; set; }
        public long DriverId { get; set; }
        public long PaymentId { get; set; }

        // Assuming status holds a single character, else use string.
        public char Status { get; set; }
        public double SourceLatitude { get; set; }  
        public double SourceLongitude { get; set; } 
        public double DestinationLatitude { get; set; } 
        public double DestinationLongitude { get; set; } 

        public char Method { get; set; }
        public double Amount { get; set; } 
    }
}
