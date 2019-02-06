using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models.Api
{
    public class CheckinResponse
    {
        [JsonProperty("data")]
        public CheckinBody Data { get; set; }

        [JsonProperty("message")]
        public CheckinMessage Message { get; set; }
    }
}