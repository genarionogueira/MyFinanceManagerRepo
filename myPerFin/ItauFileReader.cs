using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myPerFin
{
    class ItauFileReader
    {
        public List<Transaction> alltransactions { get; set; }
        public ItauFileReader(string filePath,string user)
        {
            alltransactions = new List<Transaction>();
            StreamReader sr = new StreamReader(File.OpenRead(filePath));
            Transaction oneTrans;
            String[] oneLine;
            while (!sr.EndOfStream)
            {
                oneLine = sr.ReadLine().Split(';');
                oneTrans = new Transaction( DateTime.ParseExact(oneLine[0],"dd/mm/yyyy", CultureInfo.InvariantCulture), 
                                            oneLine[1], 
                                            Convert.ToDouble(oneLine[2].Replace(',','.')),
                                            user, "R$");
                oneTrans.my_contaCredito = "CC ITAU";
                oneTrans.print_me();
                alltransactions.Add(oneTrans);
            }


        }

    }

}
