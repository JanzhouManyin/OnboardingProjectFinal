using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnboardingProjectFinal.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> Price { get; set; }
    }
}