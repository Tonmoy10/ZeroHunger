using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroHunger.db;

namespace ZeroHunger.Models
{
    public class ReqEmp
    {
        public Request Request { get; set; }
        public List<Employee> Employee { get; set; }
    }
}