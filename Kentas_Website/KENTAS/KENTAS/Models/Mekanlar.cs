//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KENTAS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mekanlar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mekanlar()
        {
            this.AnketCevaplamaSonus = new HashSet<AnketCevaplamaSonu>();
            this.Anketlers = new HashSet<Anketler>();
        }
    
        public int Id { get; set; }
        public string Adi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnketCevaplamaSonu> AnketCevaplamaSonus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Anketler> Anketlers { get; set; }
    }
}
