using myPerFin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myPerFin
{
    
    class NuBankFileReader
    {        
        public List<Transaction> allTransactions { get; set; }
        public NuBankFileReader (string path, string user)
        {
            var reader = new StreamReader(File.OpenRead(path));
            this.allTransactions = new List<Transaction>();
            Transaction oneTransaction;

            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Split(',');

                oneTransaction = new Transaction(Convert.ToDateTime(line[0]), 
                                                 line[1], 
                                                 Convert.ToDouble(line[2]), 
                                                 user,"R$");

                oneTransaction.my_contaCredito = "NUBANK";
                oneTransaction.print_me();
                allTransactions.Add(oneTransaction);
            }            
        }




    }


}
