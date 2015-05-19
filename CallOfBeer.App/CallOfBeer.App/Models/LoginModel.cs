using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.Models
{
    public class LoginModel
    {
        public TypeEnum Type { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Mail { get; set; }
    }

    public enum TypeEnum
    {
        Login,
        Register
    }
}
