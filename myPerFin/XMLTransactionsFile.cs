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
        XmlDocument doc;
        XmlElement controleFinanceiro;
        XmlElement transactionsNode;        
        XmlElement accountsNode;
        String myXmlPath;

        
        public void create_basic_structure()
        {
            //create xml structure                     
            doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);            

            controleFinanceiro = create_element("controle_financeiro");
            doc.AppendChild(controleFinanceiro);

            transactionsNode = create_element("transactions");
            controleFinanceiro.AppendChild(transactionsNode);

            accountsNode = create_element("accounts");            
            controleFinanceiro.AppendChild(accountsNode);

        }

        public XmlElement create_element( string elementName)
        {
            return doc.CreateElement(string.Empty, elementName, string.Empty);
        }


        public XMLTransactionsFile(string filePath)
        {
            doc = new XmlDocument();
            this.myXmlPath = filePath;
            if (File.Exists(filePath))
            {
                try
                {
                    doc.Load(filePath);
                    transactionsNode = (XmlElement)doc.SelectSingleNode("controle_financeiro/transactions");
                    accountsNode = (XmlElement)doc.SelectSingleNode("controle_financeiro/accounts");                                

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

        public void save_xml_file()
        {
            doc.Save(this.myXmlPath);
        }
        public void save_xml_file(string path)
        {
            doc.Save(path);
        }
        public List<Transaction> get_transaction_list()
        {
            List<Transaction> allTrans = new List<Transaction>();
            try
            {
                foreach (XmlNode transNode in transactionsNode.ChildNodes)
                {
                    Transaction trans = new Transaction();
                    foreach (XmlNode transProp in transNode.ChildNodes)
                    {
                        System.Reflection.PropertyInfo propInfo = trans.GetType().GetProperty(transProp.Name);
                        insert_xmlNode_Into_Property(propInfo, transProp, trans);                   
                    }
                    allTrans.Add(trans);
                }
                return allTrans;
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                throw e;
            }
        }    
        
        private void insert_xmlNode_Into_Property(System.Reflection.PropertyInfo propInfo,XmlNode transProp, Transaction trans)
        {
            if (propInfo.CanWrite)
            {
                if (propInfo.PropertyType.Name == "String")
                {
                    propInfo.SetValue(trans, transProp.InnerText, null);
                }
                else if (propInfo.PropertyType.Name == "Double")
                {
                    propInfo.SetValue(trans, Convert.ToDouble(transProp.InnerText), null);
                }
                else if (propInfo.PropertyType.Name == "DateTime")
                {
                    propInfo.SetValue(trans, Convert.ToDateTime(transProp.InnerText), null);
                }
                else
                {
                    Console.WriteLine(propInfo.PropertyType.Name);
                }
            }
        } 

        public void add_one_transaction(Transaction trans)
        {
            try
            {
                XmlNode transactions = doc.SelectSingleNode("controle_financeiro/transactions");
                XmlElement oneTransaction = create_element("transaction");
                transactions.AppendChild(oneTransaction);
                foreach (System.Reflection.PropertyInfo property in trans.GetType().GetProperties())
                {
                    XmlElement oneProperty = create_element(property.Name);
                    oneProperty.InnerText = Convert.ToString(property.GetValue(trans, null));
                    oneTransaction.AppendChild(oneProperty);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("error adding transaction, converting obj to xml");
                Console.WriteLine(e.Message);
            }

        }
    }




    
}
