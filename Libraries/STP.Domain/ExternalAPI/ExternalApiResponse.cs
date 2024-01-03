using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.ExternalAPI
{
    public class ExternalApiResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public dynamic Data { get; set; }
    }
}
