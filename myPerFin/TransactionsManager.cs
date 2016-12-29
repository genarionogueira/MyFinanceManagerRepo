
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
        public Dictionary<string,Transaction> transRepository = new Dictionary<string, Transaction>();        
        private AmexFileReader amex;
        private NuBankFileReader nubank;
        private ItauFileReader itauReader;
        private XMLTransactionsFile xmlFile;
        public TransactionsManager(string xmlPath )
        {
            xmlFile = new XMLTransactionsFile(xmlPath);
            add_transactions(xmlFile.get_transaction_list());
        }

        public void read_itauFile(string path, string user)
        {
            itauReader = new ItauFileReader(path, user);
            add_transactions(itauReader.alltransactions);
            xmlFile.save_xml_file();
        }
        public void read_NuBank(string path, string user)
        {
            nubank = new NuBankFileReader(path, user);
            add_transactions(nubank.allTransactions);
            xmlFile.save_xml_file();
        }
        public void read_americanExpress(string path)
        {
            amex = new AmexFileReader(path);
            add_transactions(amex.allTransactions);
            xmlFile.save_xml_file();
        }

        private void add_transactions(List<Transaction> transList)
        {
            foreach(Transaction onetrans in transList)
            {

                if (!transRepository.ContainsKey(onetrans.my_key))
                {
                    transRepository.Add(onetrans.my_key,onetrans);
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
