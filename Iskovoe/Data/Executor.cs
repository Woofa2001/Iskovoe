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
    
    public partial class Executor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Executor()
        {
            this.Iskovoe = new HashSet<Iskovoe>();
        }
    
        public int id_executor { get; set; }
        public Nullable<int> id_post { get; set; }
        public string name_executor { get; set; }
        public Nullable<int> passport { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public byte[] image { get; set; }
    
        public virtual Post Post { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Iskovoe> Iskovoe { get; set; }
    }
}
