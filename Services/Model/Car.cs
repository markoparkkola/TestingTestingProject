using System;
using System.Collections.Generic;
using System.Text;

namespace Marko.Services.Model
{
    public class Car
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string PriceUnit { get; set; }
        public int Horsepower { get; set; }
        public int TopSpeed { get; set; }
    }
}
