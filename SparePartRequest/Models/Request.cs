using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SparePartRequest.Models
{
    public class Request
    {
        [Key]
        public long RequestId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsCanceled { get; set; }
        public long RequestTypeId { get; set; }
        public virtual RequestType RequestType { get; set; }
        
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } // so each user views their own requests (identity model) sends the ID of the user with their requests

    }
    public class RequestType
    {
        [Key]
        public long RequestTypeId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
    public class Manager
    {
        [Key]
        public long NationalId { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
            
    }
}