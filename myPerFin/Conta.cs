using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myPerFin
{
    class Conta
    {
        public string my_Name { get; set; }
        public string my_Description { get; set; }
        public Conta()
        {

        }
        public Conta(string name, string description)
        {
            this.my_Name = name;
            this.my_Description = description;
        }

    }
}
