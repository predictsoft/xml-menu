using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GenerateMenu
{

    class ItemNode
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public List<ItemNode> SubItems { get; set; }
    }

    class Program
    {
        static List<ItemNode> menuTree = new List<ItemNode>();

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid input parameters. Please provide XML filename and active path to match. This program will now close.");
                return;
            }

            Console.WriteLine("File: " + args[0] + " \r\nTarget Path: " + args[1]);
            Console.WriteLine("=======================================\r\n");

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File not found (" + args[0] + "). Please make sure file exists in the given location. This program will now exit");
                return;
            }

            //Console.WriteLine(parseXMLBruteforce(args[0], args[1]));
            //Console.WriteLine(ParseXmlRecursive(args[0], args[1]));
            Console.WriteLine(PrintXMLMenu(args[0],args[1]));
        }

        private static string PrintXMLMenu(string xmlFile, string activePath)
        {
            //var menuTree = new List<ItemNode>();
            var tmpNode = new ItemNode();
            tmpNode.SubItems = new List<ItemNode>();

            var xDoc = XElement.Load(xmlFile);

            var rootChildren = xDoc.Elements();
            recursionHell(xDoc);
            return parseMenuTree(menuTree,activePath);
        }

        private static string PrintXMLMenu2(string xmlFile, string activePath)
        {
            var menuTree = new List<ItemNode>();
            var tmpNode = new ItemNode();
            tmpNode.SubItems = new List<ItemNode>();

            var xDoc = XElement.Load(xmlFile);

            var rootChildren = xDoc.Elements();

            foreach (var node in rootChildren)
            {
                if (node.Name == "item")
                {
                    tmpNode = createNode(node);
                    if (node.Elements("subMenu").Any())
                    {
                        foreach (var smItem in node.Elements("subMenu").Elements("item"))
                        {
                            var newNode = createNode(smItem);
                            tmpNode.SubItems.Add(newNode);
                        }
                    }

                    tmpNode.IsActive = false;
                    //tmpNode = new ItemNode() { Name = node.Element("displayName").ToString(), Path = }


                }
                var i = node.Name;
                var v = node.Value;
                //if(node.Name == "")
            }
            return "";
        }

        /*
         * Creates an itemnode object from a given <item> node
         */
        private static ItemNode createNode(XElement node)
        {
            var retNode = new ItemNode();
            var elName = node.Element("displayName");
            var elPath = node.Element("path");

            if (elName != null)
            {
                retNode.Name = elName.Value;
            }

            if (elPath != null && elPath.Attribute("value") != null)
            {
                retNode.Path = elPath.Attribute("value").Value;
            }

            return retNode;
        }

        /* 
         * Prints menu tree to console (with active item)
         */
        private static string parseMenuTree(List<ItemNode> menuTree, string activeNode)
        {

            return "TODO: print the tree after iterating through each item";
        }

        private static void recursionHell(XElement rootNode)
        {
            var tmpNode =new ItemNode();
            //analyze each node and either add to menu tree (if item node), or call this function with child element recursively
            if (rootNode.Name == "item")
            {
                tmpNode = createNode(rootNode);
                //add to list
                menuTree.Add(tmpNode);//(createNode(rootNode));
                Console.WriteLine(tmpNode.Name + ", " + tmpNode.Path);
            }
            if (rootNode.Name == "menu")
            {
                foreach (var node in rootNode.Elements())
                {
                    recursionHell(node);
                }
            }
            if ( rootNode.Element("subMenu")!= null)
            {
                //recursionHell(rootNode.Elements("subMenu").GetEnumerator());
                foreach (var x in rootNode.Element("subMenu").Elements())
                {
                    recursionHell(x);
                }
            }
        }

        private static string ParseXmlRecursive(string xmlFile, string targetPath)
        {
            var doc = new XmlDocument();
            StringBuilder menuList = new StringBuilder();
            try
            {
                doc.Load(xmlFile);
                Console.WriteLine(TraverseNodes(doc.FirstChild, menuList).ToString());
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception encountered while trying to parse XML document into menu. Details:\r\n\t" + e.Message);
                return "";
            }
        }

        private static StringBuilder TraverseNodes(XmlNode rootNode, StringBuilder menuListString)
        {
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                /*if (!node.HasChildNodes)
                {
                    return menuListString;
                }*/
                //if (node.Name == "item")
                {
                    menuListString.Append(node.Name.ToString());
                }
                //else if(node.Name)
                
                return TraverseNodes(node.FirstChild, menuListString);
            }
            
            return menuListString;
        }

        private static string ParseXmlBruteforce(string xmlFile, string menuPath)
        {
            /*XElement xelement1 = XElement.Load("../../menu1.xml");
            var item1 = from nm in xelement1.Elements("item")
                /*where (string)nm.Element("Sex") == "Female"#1#
                select nm;
            Console.WriteLine("item:");
            foreach (XElement xEle in item1)
                Console.WriteLine(xEle);*/

            XElement xelement = XElement.Load(xmlFile);

            StringBuilder outString = new StringBuilder("");
            IEnumerable<XElement> items = xelement.Elements();
            try
            {
                foreach (var item in items)
                {
                    outString.Append(item.Element("displayName").Value + ", " + item.Element("path").Attribute("value").Value);
                    //TODO: active if true
                    if (item.Element("path").Attribute("value").Value == menuPath)
                    {
                        outString.Append(" ACTIVE");
                    }
                    outString.Append("\r\n");


                    var submenu = item.Element("subMenu");
                    if (submenu != null)
                    {
                        foreach (var submenuItem in submenu.Elements())
                        {
                            outString.Append("\t" + submenuItem.Element("displayName").Value + ", " +
                                             submenuItem.Element("path").Attribute("value").Value);
                            if (submenuItem.Element("path").Attribute("value").Value == menuPath)
                            {
                                outString.Append(" ACTIVE");
                            }
                            outString.Append("\r\n");
                            //TODO: check and add ACTIVE here
                        }
                    }

                    /*
                        if (submenu != null)
                        {
                            outString.Append("\t");
                            var subItem = submenu.Element("item");
                            if (subItem != null)
                            {
                                outString.Append(subItem.Element("displayName").Value + subItem.Attribute("path").Value + "\r\n");
                            }
                        }
   
                 */

                    //outString.Append()
                }

                return outString.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception encountered. Details:\r\n\t" + e.Message);
                return "";
            }
        }
    }
}
