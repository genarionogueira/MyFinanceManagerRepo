using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myPerFin
{
    class Transaction
    {
        public string my_contaDebito { get; set; }
        public string my_contaCredito { get; set; }
        public double my_valor { get; set; }
        public string my_descricao { get; set; }
        public string my_geo_local { get; set; }
        public string my_outras_info { get; set; }
        public string my_usuario { get; set; }
        public string my_currency { get; set; }
        public DateTime my_dateLancamento { get; set; }
        public DateTime my_dateCompra { get; set; }

        public Transaction()
        {

        }
        public Transaction(DateTime date, string title, double value, string user, string currency)
        {
            this.my_dateLancamento = date;
            this.my_descricao = title.ToUpper();
            this.my_valor = value;
            this.my_usuario = user.ToUpper();
            this.my_currency = currency.ToUpper();
            print_me();
        }
        
        public void print_me()
        {
            Console.WriteLine(this.my_usuario);
            Console.WriteLine (this.my_dateLancamento);
            Console.WriteLine (this.my_descricao);
            Console.WriteLine (this.my_currency + " " + this.my_valor);            
            Console.WriteLine("****************************************");

        }

    }
}
