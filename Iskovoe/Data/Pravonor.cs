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
    
    public partial class Pravonor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pravonor()
        {
            this.Docement_by_pravonor = new HashSet<Docement_by_pravonor>();
        }
    
        public int id_pravonor { get; set; }
        public Nullable<int> id_iskovoe { get; set; }
        public Nullable<int> id_tip_form { get; set; }
        public Nullable<int> id_sostav { get; set; }
        public string opis { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Docement_by_pravonor> Docement_by_pravonor { get; set; }
        public virtual Iskovoe Iskovoe { get; set; }
        public virtual Sostav Sostav { get; set; }
        public virtual Tip_forms Tip_forms { get; set; }
    }
}
