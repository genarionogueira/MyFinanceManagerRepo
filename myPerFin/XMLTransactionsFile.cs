using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace myPerFin
{
    class XMLTransactionsFile
    {
        XmlDocument doc = new XmlDocument();
        XmlElement controleFinanceiro;
        XmlElement transactionsNode;        
        XmlElement accountsNode;

        public List<Transaction> transactionsList { get; set; }


        public void create_basic_structure()
        {
            //create main structure
            controleFinanceiro = doc.CreateElement("controle_financeiro");
            doc.AppendChild(controleFinanceiro);
            transactionsNode = doc.CreateElement("transactions");
            accountsNode = doc.CreateElement("accounts");
            controleFinanceiro.AppendChild(transactionsNode);
            controleFinanceiro.AppendChild(accountsNode);
        }
        public XMLTransactionsFile(string filePath)
        {
            this.transactionsList = new List<Transaction>();
            if (File.Exists(filePath))
            {
                try
                {
                    doc.Load(filePath);
                    transactionsNode = (XmlElement)doc.SelectSingleNode("controle_financeiro/transactions");
                    accountsNode = (XmlElement)doc.SelectSingleNode("controle_financeiro/accounts");
                    foreach (XmlNode transactionNode in transactionsNode.ChildNodes)
                    {
                        load_transactions_from_xml(transactionsNode);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erro loading xml file " + filePath);
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                create_basic_structure();
            }
            
        }

        public void save_xml_file(string path)
        {
            doc.Save(path);
        }
        public void load_transactions_from_xml(XmlNode transactionNode)
        {
            Transaction trans = new Transaction();
            trans.my_contaDebito = transactionNode.SelectSingleNode("contaDebito").InnerText;
            trans.my_contaCredito = transactionNode.SelectSingleNode("contaCredito").InnerText;
            trans.my_valor = Convert.ToDouble( transactionNode.SelectSingleNode("valor").InnerText);
            trans.my_descricao = transactionNode.SelectSingleNode("descricao").InnerText;
            trans.my_geo_local = transactionNode.SelectSingleNode("geo_local").InnerText;
            trans.my_outras_info= transactionNode.SelectSingleNode("outras_info").InnerText;
            trans.my_usuario = transactionNode.SelectSingleNode("usuario").InnerText;
            trans.my_currency = transactionNode.SelectSingleNode("currency").InnerText;
            trans.my_dateLancamento = DateTime.Parse( transactionNode.SelectSingleNode("dateLancamento").InnerText);
            trans.my_dateCompra = DateTime.Parse( transactionNode.SelectSingleNode("dateCompra").InnerText);
            this.transactionsList.Add(trans);
        }     

        public void add_one_transaction(Transaction trans)
        {

            transactionsList.Add(trans);

            XmlNode transactions = doc.SelectSingleNode("controle_financeiro/transactions");
            XmlElement oneTransaction = doc.CreateElement("transaction");
            transactions.AppendChild(oneTransaction);

            XmlElement my_contaDebito_node = doc.CreateElement("contaDebito") ;
            my_contaDebito_node.InnerText = trans.my_contaDebito;
            oneTransaction.AppendChild(my_contaDebito_node);

            XmlElement my_contaCredito = doc.CreateElement("contaCredito");
            my_contaCredito.InnerText = trans.my_contaCredito;
            oneTransaction.AppendChild(my_contaCredito);

            XmlElement my_valor = doc.CreateElement("valor");
            my_valor.InnerText = trans.my_valor.ToString();
            oneTransaction.AppendChild(my_valor);

            XmlElement my_descricao = doc.CreateElement("descricao");
            my_descricao.InnerText = trans.my_descricao;
            oneTransaction.AppendChild(my_descricao);

            XmlElement my_geo_local = doc.CreateElement("geo_local");
            my_geo_local.InnerText = trans.my_geo_local;
            oneTransaction.AppendChild(my_geo_local);

            XmlElement my_outras_info = doc.CreateElement("outras_info");
            my_outras_info.InnerText = trans.my_outras_info;
            oneTransaction.AppendChild(my_outras_info);

            XmlElement my_usuario = doc.CreateElement("usuario");
            my_usuario.InnerText = trans.my_usuario;
            oneTransaction.AppendChild(my_usuario);

            XmlElement my_currency = doc.CreateElement("currency");
            my_currency.InnerText = trans.my_currency;
            oneTransaction.AppendChild(my_currency);

            XmlElement my_dateLancamento = doc.CreateElement("dateLancamento");
            my_dateLancamento.InnerText = trans.my_dateLancamento.ToString();
            oneTransaction.AppendChild(my_dateLancamento);

            XmlElement my_dateCompra = doc.CreateElement("dateCompra");
            my_dateCompra.InnerText = trans.my_dateCompra.ToString();
            oneTransaction.AppendChild(my_dateCompra);

        }
    }




    
}
