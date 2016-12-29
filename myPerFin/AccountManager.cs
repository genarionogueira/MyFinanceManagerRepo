using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myPerFin
{
    class AccountManager
    {
        public Dictionary<string, Conta> allAccounts { get; set; }
        
        //constructor
        public AccountManager()
        {
            this.allAccounts = new Dictionary<string, Conta>();
        }

        public void create_accounts_by_transactions(Dictionary<String,Transaction> transactions)
        {
            foreach (Transaction trans in transactions.Values )
            {
                add_one_account(trans.my_contaCredito);
                add_one_account(trans.my_contaCredito);
            }
        }

        private void add_one_account(Conta account)
        {
            if (!allAccounts.ContainsKey(account.my_Name))
            {
                allAccounts.Add(account.my_Name, account);
            }
        }



    }
}
