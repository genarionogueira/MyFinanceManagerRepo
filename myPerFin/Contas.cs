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
        
        public void add_conta_by_name(string name, string description = "")
        {
            Conta conta = new Conta();
            conta.my_Name = name;
            conta.my_Description = description;            
            if (!allContasByName.ContainsValue(conta))
            {
                this.allContasByName.Add(conta.my_Name, conta);
            }                                      
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
