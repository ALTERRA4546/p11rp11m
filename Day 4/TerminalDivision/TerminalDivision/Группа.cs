//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TerminalDivision
{
    using System;
    using System.Collections.Generic;
    
    public partial class Группа
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Группа()
        {
            this.Посещающие = new HashSet<Посещающие>();
        }
    
        public int Код_группы { get; set; }
        public System.DateTime Дата_создания { get; set; }
        public string Название { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Посещающие> Посещающие { get; set; }
    }
}
