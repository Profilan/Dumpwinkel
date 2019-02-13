using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class ScanViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}