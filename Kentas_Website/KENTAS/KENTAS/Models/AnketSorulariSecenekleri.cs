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
    
    public partial class AnketSorulariSecenekleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AnketSorulariSecenekleri()
        {
            this.AnketCevaplaris = new HashSet<AnketCevaplari>();
        }
    
        public int Id { get; set; }
        public int SoruId { get; set; }
        public string Icerik { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnketCevaplari> AnketCevaplaris { get; set; }
        public virtual AnketSorulari AnketSorulari { get; set; }
    }
}