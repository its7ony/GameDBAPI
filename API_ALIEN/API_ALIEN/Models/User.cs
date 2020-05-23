using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_ALIEN.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Pwd { get; set; }
        public string Created { get; set; }
        public int Score { get; set; }
    }
}