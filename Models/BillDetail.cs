using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EBilling.Models;

namespace EBilling.Models
{
    public class BillDetail
    {
        public int Id { get; set; } 
        public string CoustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public int TotalAmount {  get; set; }
        public List<Items> Items { get; set; }
        public BillDetail() 
        {
         Items= new List<Items>();
        }
    }
}