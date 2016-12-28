using OFXSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myPerFin
{
    class Program
    {
        static void Main(string[] args)
        {
            TransactionsManager man = new TransactionsManager();
            man.read_NuBank("F:\\seguro\\nubank\\nubank-2017-01.csv","genario nogueira");
            man.read_americanExpress("F:\\seguro\\fatura amex\\13122016.html");
            man.read_itauFile("F:\\seguro\\itau\\extrato.txt","genario nogueira");
            Console.Read();
        }



    }
}
