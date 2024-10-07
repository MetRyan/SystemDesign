using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberSytem.Dto.Requests
{
    public class CreateFeedback
    {
        /*CustomerId = rating.CustomerId,
                DriverId = rating.DriverId,
                TripId = rating.TripId,
                Feedback = rating.Feedback,
                Rating1 = rating.Rating1,*/
        public long CustomerId { get; set; }

        public long DriverId { get; set; }

        public long TripId {  get; set; }   

        public string Feedback { get; set; }
        public int Rating { get; set; } 
    }
}
