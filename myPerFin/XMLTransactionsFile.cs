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
        //contructors
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
        //create basic structure of the xml
        public void create_basic_structure()
        {
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

        //save the xml
        public void save_xml_file()
        {
            doc.Save(this.myXmlPath);
        }

        //get all transactions as object
        public List<Transaction> get_transaction_list()
        {
            List<Transaction> allTrans = new List<Transaction>();
            try
            {
                foreach(XmlNode transNode in transactionsNode.ChildNodes)
                {
                    allTrans.Add((Transaction)parse_XmlNode_to_object(transNode));
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
                    
        //parse a xmlNode to a Object
        private object parse_XmlNode_to_object(XmlNode myNode)
        {
            Object obj = GetInstance(myNode.Name);
            foreach (XmlNode propertyNode in myNode.ChildNodes)
            {
                System.Reflection.PropertyInfo prop;
                prop = obj.GetType().GetProperty(propertyNode.Name);
                prop.SetValue(obj, Convert.ChangeType(propertyNode.InnerText,Type.GetType(prop.GetType().FullName)), null);
            }
            return obj;
        }
        //get a instace of a object by his name
        private object GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return Activator.CreateInstance(t);
        }
        //parse a object to a XmlNode
        private XmlElement parse_obj_to_XmlNode(object myObj, XmlNode parentNode = null)
        {
            XmlElement objNode = create_element(myObj.GetType().Name);
            if (parentNode != null)
            {
                parentNode.AppendChild(objNode);
            }

            foreach (System.Reflection.PropertyInfo propinfo in myObj.GetType().GetProperties())
            {
                XmlElement nodeProp = create_element(propinfo.Name);
                if (propinfo.PropertyType.IsClass)
                {
                    this.parse_obj_to_XmlNode(propinfo.GetValue(myObj, null), objNode);
                }
                else
                {
                    nodeProp.InnerText = Convert.ToString(propinfo.GetValue(myObj, null));
                }
                
            }
            return objNode;

        }



        public void add_one_transaction(Transaction trans)
        {
            try
            {
                XmlNode transNode = parse_obj_to_XmlNode(trans);
              
            }
            catch (Exception e)
            {
                Console.WriteLine("error adding transaction, converting obj to xml");
                Console.WriteLine(e.Message);
            }

        }
    }




    
}
