//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Iskovoe.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tip_documents
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tip_documents()
        {
            this.Docement_by_pravonor = new HashSet<Docement_by_pravonor>();
        }
    
        public int id_tip_dop { get; set; }
        public string name_tip_dop { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Docement_by_pravonor> Docement_by_pravonor { get; set; }
    }
}