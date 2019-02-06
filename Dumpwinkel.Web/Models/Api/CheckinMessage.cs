using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models.Api
{
    public class CheckinMessage
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}