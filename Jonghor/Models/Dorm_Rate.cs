//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jonghor.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dorm_Rate
    {
        public string Username { get; set; }
        public int Dorm_ID { get; set; }
        public double Score { get; set; }
        public string Text { get; set; }
    
        public virtual Dorm Dorm { get; set; }
        public virtual Person Person { get; set; }
    }
}
