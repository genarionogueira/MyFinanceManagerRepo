
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
        Dictionary<string,Transaction> allTrans = new Dictionary<string, Transaction>();        
        AmexFileReader amex;
        NuBankFileReader nubank;
        ItauFileReader itauReader;
        public void read_itauFile(string path, string user)
        {
            itauReader = new ItauFileReader(path, user);
            add_transactions(itauReader.alltransactions);
        }
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
                if (!allTrans.ContainsKey(onetrans.my_key))
                {
                    allTrans.Add(onetrans.my_key, onetrans);
                }
                else
                {
                    Console.WriteLine("**Transaction already loaded**");
                    onetrans.print_me();
                }
                
            }
        }



    }
}
