using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberSytem.Dto.Requests
{
    public class TripRequest
    {


        public double SourceLatitude { get; set; }  // Vĩ độ điểm đón
        public double SourceLongitude { get; set; } // Kinh độ điểm đón
        public double DestinationLatitude { get; set; } // Vĩ độ điểm đến
        public double DestinationLongitude { get; set; } // Kinh độ điểm đến

        public long CustomerId { get; set; } // ID của yêu cầu
        public char Method { get; set; } // Phương thức thanh toán (1 ký tự)
        public double Amount { get; set; } // Số tiền
    }
}
