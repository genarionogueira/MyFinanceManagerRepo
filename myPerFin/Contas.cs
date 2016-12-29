using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myPerFin
{
   
    class Contas
    {
        Dictionary<string, Conta> allContasByName = new Dictionary<string, Conta>();
        
        public void add_conta(Conta conta)
        {
            if (!allContasByName.ContainsKey(conta.my_Name))
            {
                this.allContasByName.Add(conta.my_Name, conta);
            }                                      
        }
        public Conta get_conta_by_name(string name)
        {
            return allContasByName[name];
        }


        public void remove_conta_by_name(string name)
        {
            if (allContasByName.ContainsKey(name))
            {
                allContasByName.Remove(name);
            }
        }


    }
}
