//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnboardingProjectFinal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.ProductSolds = new HashSet<ProductSold>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> Price { get; set; }
    
        public virtual ICollection<ProductSold> ProductSolds { get; set; }
    }
}
