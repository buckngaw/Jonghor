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
    
    public partial class Dorm_Picture
    {
        public int Dorm_ID { get; set; }
        public string URL_Picture { get; set; }
    
        public virtual Dorm Dorm { get; set; }
    }
}
