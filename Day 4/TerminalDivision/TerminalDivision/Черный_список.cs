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
    
    public partial class Черный_список
    {
        public int Код_черного_списка { get; set; }
        public int Код_посетителя { get; set; }
        public string Причина_добавления { get; set; }
    
        public virtual Посетитель Посетитель { get; set; }
    }
}
