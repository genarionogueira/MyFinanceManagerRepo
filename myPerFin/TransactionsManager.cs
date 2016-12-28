
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace myPerFin
{
    class TransactionsManager
    {
        List<Transaction> allTrans = new List<Transaction>();
        AmexFileReader amex;
        NuBankFileReader nubank;      

        public void read_NuBank(string path, string user)
        {
            nubank = new NuBankFileReader(path, user);
            add_transactions(nubank.allTransactions);
        }
        public void read_americanExpress(string path)
        {
            amex = new AmexFileReader(path);
            add_transactions(amex.allTransactions);
        }

        private void add_transactions(List<Transaction> transList)
        {
            foreach(Transaction onetrans in transList)
            {
                allTrans.Add(onetrans);
            }
        }



    }
}
