//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace biApi
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserMenu
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> MenuId { get; set; }
        public Nullable<int> CanAdd { get; set; }
        public Nullable<int> CanEdit { get; set; }
        public Nullable<int> CanDelete { get; set; }
        public Nullable<int> CanView { get; set; }
        public string Status { get; set; }
    }
}
