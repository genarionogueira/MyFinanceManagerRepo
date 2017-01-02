using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                    allTrans.Add(parse_transaction_to_Obj((XmlElement)transNode));
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
                    
        //parse one transaction to xmlNode
        private XmlElement parse_transaction_to_xmlNode(Transaction trans)
        {
            XmlElement transNode = create_element("transaction");

            XmlElement my_contaDebito = create_element("my_contaDebito"); transNode.AppendChild(my_contaDebito);
            my_contaDebito.AppendChild(parse_Conta_to_xmlNode(trans.my_contaDebito));

            XmlElement my_contaCredito = create_element("my_contaCredito"); transNode.AppendChild(my_contaCredito);
            my_contaCredito.AppendChild(parse_Conta_to_xmlNode(trans.my_contaCredito));

            XmlElement my_valor = create_element("my_valor"); transNode.AppendChild(my_valor);
            my_valor.InnerText =Convert.ToString (trans.my_valor.ToString());

            XmlElement my_descricao = create_element("my_descricao"); transNode.AppendChild(my_descricao);
            my_descricao.InnerText = trans.my_descricao;

            XmlElement my_geo_local = create_element("my_geo_local"); transNode.AppendChild(my_geo_local);
            my_geo_local.InnerText = trans.my_geo_local;

            XmlElement my_outras_info = create_element("my_outras_info"); transNode.AppendChild(my_outras_info);
            my_outras_info.InnerText = trans.my_outras_info;

            XmlElement my_usuario = create_element("my_usuario"); transNode.AppendChild(my_usuario);
            my_usuario.InnerText = trans.my_usuario;

            XmlElement my_currency = create_element("my_currency"); transNode.AppendChild(my_currency);
            my_currency.InnerText = trans.my_currency;

            XmlElement my_dateLancamento = create_element("my_dateLancamento"); transNode.AppendChild(my_dateLancamento);
            my_dateLancamento.InnerText = trans.my_dateLancamento.ToString();

            XmlElement my_dateCompra = create_element("my_dateCompra"); transNode.AppendChild(my_dateCompra);
            my_dateCompra.InnerText = trans.my_dateCompra.ToString();
            return transNode;
        }

        private Transaction parse_transaction_to_Obj(XmlElement transNode)
        {
            Transaction trans = new Transaction();
            trans.my_contaDebito = parse_Conta_to_Obj((XmlElement)transNode.SelectSingleNode("my_contaDebito"));
            trans.my_contaCredito = parse_Conta_to_Obj((XmlElement)transNode.SelectSingleNode("my_contaCredito"));
            trans.my_valor = Convert.ToDouble( transNode.SelectSingleNode("my_valor").InnerText);
            trans.my_descricao = transNode.SelectSingleNode("my_descricao").InnerText;
            trans.my_geo_local = transNode.SelectSingleNode("my_geo_local").InnerText;
            trans.my_outras_info = transNode.SelectSingleNode("my_outras_info").InnerText;
            trans.my_usuario = transNode.SelectSingleNode("my_usuario").InnerText;
            trans.my_currency = transNode.SelectSingleNode("my_currency").InnerText;
            trans.my_dateLancamento = Convert.ToDateTime(transNode.SelectSingleNode("my_dateLancamento").InnerText);
            trans.my_dateCompra = Convert.ToDateTime(transNode.SelectSingleNode("my_dateCompra").InnerText);
            return trans;
        }

        private XmlElement parse_Conta_to_xmlNode(Conta account)
        {
            XmlElement transNode = create_element("conta");
            XmlElement my_name = create_element("my_name"); transNode.AppendChild(my_name);
            XmlElement my_description = create_element("my_description"); transNode.AppendChild(my_description);
            if (account != null) {
                my_name.InnerText = Convert.ToString(account.my_Name);                
                my_description.InnerText = Convert.ToString(account.my_Description);
            }
            return transNode;
        }
        private Conta parse_Conta_to_Obj(XmlElement accountNode)
        {
            Conta myAccount = new Conta();
            myAccount.my_Name = accountNode.SelectSingleNode("conta//my_name").InnerText;
            myAccount.my_Description = accountNode.SelectSingleNode("conta//my_description").InnerText;
            return myAccount;
        }

        //parse a xmlNode to a Object
        private object parse_XmlNode_to_object(XmlNode myNode)
        {
            Object obj = GetInstance(myNode.Name);
            foreach (XmlNode propertyNode in myNode.ChildNodes)
            {
                System.Reflection.PropertyInfo prop;
                prop = obj.GetType().GetProperty(propertyNode.Name);

                if (isClass_type(prop))
                {
                    prop.SetValue(obj, parse_XmlNode_to_object(propertyNode),null);
                }
                else
                {
                    prop.SetValue(obj, propertyNode.InnerText,null);
                }               
                
            }
            return obj;
        }
        //get a instace of a object by his name
        private object GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType("myPerFin."+strFullyQualifiedName);
            return Activator.CreateInstance(t);
        }
        //parse a object to a XmlNode
        private XmlElement parse_obj_to_XmlNode(object myObj,string myObjName, string className="")
            
        {
            try
            {
                XmlElement objNode = create_element(myObjName);                
                //add class name
                if(myObj != null)
                {
                    foreach (System.Reflection.PropertyInfo propinfo in myObj.GetType().GetProperties())
                    {
                        XmlElement nodeProp;
                        if (isClass_type(propinfo))
                        {
                            Object subObj = propinfo.GetValue(myObj, null);
                            nodeProp = this.parse_obj_to_XmlNode(propinfo.GetValue(myObj, null), propinfo.Name+":"+ subObj.GetType().FullName);                            
                        }
                        else
                        {
                            nodeProp = create_element(propinfo.Name);
                            nodeProp.InnerText = Convert.ToString(propinfo.GetValue(myObj, null));                            
                        }
                        objNode.AppendChild(nodeProp);
                    }                    
                }

                return objNode;

            }
            catch(Exception e)
            {
                throw e;
            }
        }
        private Boolean isClass_type(System.Reflection.PropertyInfo  propinfo)
        {
            if (propinfo.PropertyType.Name=="String")
            {
                return false;
            }
            else if (propinfo.PropertyType.Name == "Double")
            {
                return false;
            }
            else if (propinfo.PropertyType.Name == "DateTime")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void add_one_transaction(Transaction trans)
        {
            try
            {
                XmlNode transNode = parse_transaction_to_xmlNode (trans);
                transactionsNode.AppendChild(transNode);
            }
            catch (Exception e)
            {
                Console.WriteLine("error adding transaction, converting obj to xml");
                Console.WriteLine(e.Message);
            }

        }
    }




    
}
