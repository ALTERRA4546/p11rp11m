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
    
    public partial class Авторизация
    {
        public int Код_авторизации { get; set; }
        public int Код_посетителя { get; set; }
        public string Логин { get; set; }
        public string Пароль { get; set; }
    
        public virtual Посетитель Посетитель { get; set; }
    }
}
