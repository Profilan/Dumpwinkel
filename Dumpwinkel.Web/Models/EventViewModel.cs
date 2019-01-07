﻿using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dumpwinkel.Web.Models
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public int MaxPersons { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int Amount { get; set; }
        public Unit Unit { get; set; }
    }
}