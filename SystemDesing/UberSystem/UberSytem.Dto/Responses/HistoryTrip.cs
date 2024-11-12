namespace UberSytem.Dto.Responses
{
    public class HistoryTrip
    {

        public string? UserName { get; set; }
        public long CustomerId { get; set; }
                public long Id { get; set; }


        // Assuming status holds a single character, else use string.
        public char Status { get; set; }
        public double SourceLatitude { get; set; }  
        public double SourceLongitude { get; set; } 
        public double DestinationLatitude { get; set; } 
        public double DestinationLongitude { get; set; } 
        public long PaymentId { get; set; }

        public char Method { get; set; }
        public double Amount { get; set; } 
    }
}