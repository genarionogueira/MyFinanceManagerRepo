/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace XML_MAN
{
    class XMLMaker
    {

        //basic info
        StaticInfo info;
        string mDbgRepName;
        string mDBGRepEmail;
        string mTemplateName;
        string mXMLAddress;

        public string DBGRepName { get {return mDbgRepName; } set { mDbgRepName = value; } }
        public string DBGRepEmail { get {return mDBGRepEmail; } set { mDBGRepEmail=value; } }
        public string TemplateName { get {return mTemplateName; } set { mTemplateName=value; } }
        public string XMLAddress { get { return mXMLAddress; } set { mXMLAddress = value; } }

        public Dictionary <int,string> regionalSettings;
        public Dictionary <int,string> coreTests;
        public Dictionary <int, SpecificTest_UIErrorFinder> specificTests_UIErrorFinder;
        public Dictionary <int, SpecificTest_StatisticalVerification> specificTests_StatisticalVerification;

        public  XMLMaker()
        {
            info = new StaticInfo();
            regionalSettings = new Dictionary<int, string>();
            coreTests = new Dictionary<int, string>();
            specificTests_UIErrorFinder = new Dictionary<int, SpecificTest_UIErrorFinder>();
            specificTests_StatisticalVerification = new Dictionary<int, SpecificTest_StatisticalVerification>();
        }
        
        public void add_one_regionalSetting(string regionalSetting)
        {
            regionalSettings.Add(regionalSettings.Count, regionalSetting);            
        }
        public void add_one_regionalSetting(XmlNode regionalSettingNode)
        {
            regionalSettings.Add(regionalSettings.Count,info.allLanguages_toUF[regionalSettingNode.InnerText]);
        }

        public void add_one_coreTest(string coreTest)
        {
            coreTests.Add(coreTests.Count, coreTest);
        }

        public void add_one_coreTest(XmlNode coreTestNode)
        {
            if (coreTestNode.Name == "vba_force_compile_test")
            {
                try{
                    XmlAttribute atb = coreTestNode.Attributes[0];
                    coreTests.Add(coreTests.Count, info.coreTests_toUF[coreTestNode.Name] + atb.InnerText + ")");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }                
                
            }
            else
            {
                coreTests.Add(coreTests.Count, info.coreTests_toUF[coreTestNode.Name]);
            }

            
        }
        public void add_one_specificTest(XmlNode xmlSpecificTestElement)
        {

            SpecificTest_UIErrorFinder sp_UIErrorFinder;
            SpecificTest_StatisticalVerification spStatisticalVerification;

            if (xmlSpecificTestElement.Name == "UI_error_finder")
            {
                sp_UIErrorFinder = new SpecificTest_UIErrorFinder(xmlSpecificTestElement);
                specificTests_UIErrorFinder.Add(specificTests_UIErrorFinder.Count, sp_UIErrorFinder);
            }
            else if (xmlSpecificTestElement.Name == "Buss_stat_ver")
            {
                spStatisticalVerification = new SpecificTest_StatisticalVerification(xmlSpecificTestElement);
                specificTests_StatisticalVerification.Add(specificTests_StatisticalVerification.Count, spStatisticalVerification);
            }

            
        }
        public void add_one_specificTest(SpecificTest_UIErrorFinder specificTest)
        {
            specificTests_UIErrorFinder.Add(specificTests_UIErrorFinder.Count, specificTest);
        }
        public void add_one_specificTest(SpecificTest_StatisticalVerification specificTest)
        {
            specificTests_StatisticalVerification.Add(specificTests_StatisticalVerification.Count, specificTest);
        }

        public void load_one_xml(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);

            this.DBGRepEmail = doc.SelectSingleNode("DBGTest/mainInfo/DBGRepEmail").InnerText; 
            this.DBGRepName = doc.SelectSingleNode("DBGTest/mainInfo/DBGRepName").InnerText;
            this.TemplateName = doc.SelectSingleNode("DBGTest/mainInfo/templatename").InnerText;
            this.XMLAddress = xmlPath;

            //get regional settings            
            foreach(XmlNode myNode in doc.SelectSingleNode("DBGTest/testRoutine/regionalTests").ChildNodes )
            {
                this.add_one_regionalSetting(myNode);
            }

            //get core testes
            foreach (XmlNode myNode in doc.SelectSingleNode("DBGTest/testRoutine/coreTests").ChildNodes)
            {
                this.add_one_coreTest(myNode);
            }
            //get specifics tests
            foreach (XmlNode myNode in doc.SelectSingleNode("DBGTest/testRoutine/specificTests").ChildNodes)
            {
                this.add_one_specificTest(myNode);
            }
        }

        public void generate_XML_File()
        {
            //create xml structure
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8",null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement DBGTestNode = doc.CreateElement(string.Empty, "DBGTest", string.Empty);
            doc.AppendChild(DBGTestNode);
            XmlElement mainInfo = doc.CreateElement(string.Empty, "mainInfo", string.Empty);
            DBGTestNode.AppendChild(mainInfo);
            XmlElement testRoutine = doc.CreateElement(string.Empty, "testRoutine", string.Empty);
            DBGTestNode.AppendChild(testRoutine);
            XmlElement coreTests = doc.CreateElement(string.Empty, "coreTests", string.Empty);
            testRoutine.AppendChild(coreTests);
            XmlElement regionalTests = doc.CreateElement(string.Empty, "regionalTests", string.Empty);
            testRoutine.AppendChild(regionalTests);
            XmlElement specificTests = doc.CreateElement(string.Empty, "specificTests", string.Empty);
            testRoutine.AppendChild(specificTests);
            
            //add basic info
            add_basic_info(doc);
            //add regional settings
            add_regional_settings(doc);
            //add core tests
            add_core_tests(doc);
            //add specifics test
            add_all_specific_tests(doc);
            //save xml
            if (this.XMLAddress == "")
            {
                System.Windows.MessageBox.Show("XML Path Empty!!");
                return;
            }
            doc.Save(this.XMLAddress);
        }
        public void add_basic_info(XmlDocument doc)
        {
            XmlElement templateNameNode = doc.CreateElement(string.Empty, "templatename", string.Empty);
            XmlElement DBGRepNameNode = doc.CreateElement(string.Empty, "DBGRepName", string.Empty);
            XmlElement DBGRepEmailNode = doc.CreateElement(string.Empty, "DBGRepEmail", string.Empty);

            XmlElement testPeriodicyNode = doc.CreateElement(string.Empty, "TestPeriodicy", string.Empty);
            XmlElement startDateNode = doc.CreateElement(string.Empty, "startDate", string.Empty);
            XmlElement periodNode = doc.CreateElement(string.Empty, "period", string.Empty);

            startDateNode.InnerText = DateTime.Today.ToString("MM/dd/yyyy");
            periodNode.InnerText = "Weekly";

            XmlNode mainInfoNode = doc.SelectSingleNode("DBGTest/mainInfo");
            mainInfoNode.AppendChild(templateNameNode);
            mainInfoNode.AppendChild(DBGRepNameNode);
            mainInfoNode.AppendChild(DBGRepEmailNode);
            mainInfoNode.AppendChild(testPeriodicyNode);
            testPeriodicyNode.AppendChild(startDateNode);
            testPeriodicyNode.AppendChild(periodNode);

            templateNameNode.InnerText = this.TemplateName;
            DBGRepNameNode.InnerText = this.DBGRepName;
            DBGRepEmailNode.InnerText = this.DBGRepEmail;
        }
        public void add_regional_settings(XmlDocument doc)
        {

            XmlNode regSettingsNode = doc.SelectSingleNode("DBGTest/testRoutine/regionalTests");

            foreach (string regSett in regionalSettings.Values )
            {
                string regSettXml = info.allLanguages_toXML[regSett];
                XmlElement oneRegSettNode = doc.CreateElement(string.Empty, "regional_test", string.Empty);
                regSettingsNode.AppendChild(oneRegSettNode);
                XmlElement oneVMLanguage = doc.CreateElement(string.Empty,"VM_language_set", string.Empty);
                oneRegSettNode.AppendChild(oneVMLanguage);
                oneVMLanguage.InnerText = regSettXml;
            }

        }
        public void add_core_tests( XmlDocument doc)
        {
            
            XmlNode coreTestsNode;
            coreTestsNode = doc.SelectSingleNode("DBGTest/testRoutine/coreTests");

            foreach (string coreTest in coreTests.Values) {
                XmlElement oneCoreTestNode;
                if (coreTest.ToUpper().IndexOf("FORCE COMPILE") == -1)
                {
                    string coreTestXmlTag = info.coreTests_toXML[coreTest];
                    if (coreTestXmlTag != "")
                    {
                        oneCoreTestNode = doc.CreateElement(string.Empty, coreTestXmlTag, string.Empty);
                        coreTestsNode.AppendChild(oneCoreTestNode);
                    }
                }
                else
                {
                    string paramStr;
                    paramStr = coreTest.Replace(coreTest.Split('(')[0], "");
                    paramStr = paramStr.Remove(0,1);
                    paramStr = paramStr.Remove(paramStr.Length -1, 1);
                    string[] forceCompParam = paramStr.Split('|');

                    string coreTestXmlTag = info.coreTests_toXML[coreTest.Split('(')[0]+'(' ];
                    oneCoreTestNode = doc.CreateElement(string.Empty, coreTestXmlTag, string.Empty);
                    oneCoreTestNode.SetAttribute("ext", forceCompParam[0]);
                    coreTestsNode.AppendChild(oneCoreTestNode);

                }
                
            }
        }

        public void add_all_specific_tests(XmlDocument doc)
        {
            XmlNode specificsTestNode = doc.SelectSingleNode("DBGTest/testRoutine/specificTests");
            //ui error finder
            foreach(SpecificTest_UIErrorFinder ui in specificTests_UIErrorFinder.Values)
            {
                XmlElement uiXmlelement = ui.parse_me_to_node(doc);
                specificsTestNode.AppendChild(uiXmlelement);
            }
            //statistical verification
            foreach(SpecificTest_StatisticalVerification sv in specificTests_StatisticalVerification.Values)
            {
                XmlElement svXmlelement = sv.parse_me_to_node(doc);
                specificsTestNode.AppendChild(svXmlelement); 
            }
        }
    }
}*/
