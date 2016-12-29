using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myPerFin
{

    class AmexFileReader
    {
        public List<Transaction> allTransactions { get; set; }

        public AmexFileReader(string htmlPath)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlNodeCollection allTrs;
            HtmlAgilityPack.HtmlDocument doc = web.Load(htmlPath);
            IEnumerable<HtmlNode> col = doc.DocumentNode.Descendants("table");
            this.allTransactions = new List<Transaction>();
                        
            foreach (HtmlNode table in col)
            {
                if(table.Id!=""){
                    allTrs = table.SelectSingleNode("tbody").SelectNodes("tr");
                    for (int i = 1; i < allTrs.Count-1; i++)
                    {
                        if (!allTrs[i].SelectNodes("td")[0].SelectNodes("span")[0].InnerText.Contains("Total")& !allTrs[i].SelectNodes("td")[0].SelectNodes("span")[0].InnerText.Contains("Data"))
                        {
                            this.allTransactions.Add(create_one_Transaction(allTrs[i], get_table_owner(table)));
                        }
                        
                    }

                }
            }
        }
        private string get_table_owner(HtmlNode table)
        {
            return table.SelectNodes("thead")[0].SelectNodes("tr")[0].SelectNodes("td")[0].SelectNodes("span")[1].InnerText;
        }

        public Transaction create_one_Transaction(HtmlNode tr, string usuario)
        {
            Transaction myTrans = new Transaction();

            try
            {
                myTrans.my_usuario = usuario;
                myTrans.my_dateLancamento = DateTime.ParseExact(tr.SelectNodes("td")[0].SelectSingleNode("span").InnerText, "dd/mm/yyyy", CultureInfo.InvariantCulture);
                if (tr.SelectNodes("td")[1].SelectNodes("span").Count == 4)
                {
                    myTrans.my_descricao = tr.SelectNodes("td")[1].SelectNodes("span")[0].InnerText;
                    myTrans.my_geo_local = tr.SelectNodes("td")[1].SelectNodes("span")[1].InnerText;
                    myTrans.my_outras_info = tr.SelectNodes("td")[1].SelectNodes("span")[2].InnerText;
                    myTrans.my_dateLancamento = DateTime.ParseExact(tr.SelectNodes("td")[1].SelectNodes("span")[3].InnerText.Replace("DATA - ", ""), "dd.mm.yy", CultureInfo.InvariantCulture);
                    myTrans.my_currency = tr.SelectNodes("td")[2].SelectNodes("span")[0].InnerText;
                    myTrans.my_valor = Convert.ToDouble(tr.SelectNodes("td")[2].SelectNodes("span")[1].InnerText.Replace(',', '.'));
                }
                else
                {
                    myTrans.my_descricao = tr.SelectNodes("td")[1].SelectNodes("span")[0].InnerText;                    
                }

                if (tr.SelectNodes("td")[2].SelectNodes("span")[0].InnerText == "-")
                {
                    myTrans.my_currency = tr.SelectNodes("td")[2].SelectNodes("span")[1].InnerText;
                    myTrans.my_valor = Convert.ToDouble(tr.SelectNodes("td")[2].SelectNodes("span")[2].InnerText.Replace(".","").Replace(',', '.'));
                    myTrans.my_valor = myTrans.my_valor * -1;
                }
                else
                {
                    myTrans.my_currency = tr.SelectNodes("td")[2].SelectNodes("span")[0].InnerText;
                    myTrans.my_valor = Convert.ToDouble(tr.SelectNodes("td")[2].SelectNodes("span")[1].InnerText.Replace(',', '.'));

                }
                myTrans.my_contaCredito = "AMEX";
                //myTrans.print_me();

        

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            return myTrans;


        }



    }
}
