using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberSytem.Dto.Responses
{
    public class DriverReponse
    {
        public long id { get; set; }
        public long cabId {  get; set; }

        public float locationLatitude {  get; set; }
        public float locationLongitude { get; set; } 
        // table user 

        public string userName {  get; set; }   



    }
}
