using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models.Api
{
    public class CheckinBody
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("number_of_visitors")]
        public int NumberOfVisitors { get; set; }

        [JsonProperty("timeslot")]
        public string TimeSlot { get; set; }
    }
}