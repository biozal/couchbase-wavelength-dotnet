﻿using System;

namespace Wavelength.Models
{
    public class Item
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset StopTime { get; set; }
    }
}
