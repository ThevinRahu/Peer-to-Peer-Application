//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Job
    {
        public int Id { get; set; }
        public int client_job_id { get; set; }
        public int client_id { get; set; }
        public string description { get; set; }
    }
}
