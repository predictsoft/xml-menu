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
    //in-memory object that represents a menu (or submenu) node
    class ItemNode
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public List<ItemNode> SubItems { get; set; }
        public ItemNode()
        {
            this.IsActive = false;
        }
    }

    class Program
    {
        private static string _activePath = "";

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

            _activePath = args[1];

            PrintXmlMenu(args[0]);
        }

        //main function that creates an in-memory XML tree and prints the nodes, respecting the hierarchy
        private static void PrintXmlMenu(string xmlFile)
        {
            var menu = new List<ItemNode>();

            var xDoc = XElement.Load(xmlFile);

            foreach (var node in xDoc.Elements())
            {
                menu.Add(BuildTree(node));
            }
            menu.RemoveAll(node => node == null);   //clean null items

            foreach (var menuItem in menu)
            {
                PrintNode(menuItem,"");
            }
        }

        //Creates an itemnode object from a given <item> node
        private static ItemNode CreateNode(XElement node)
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
                if (retNode.Path.Equals(_activePath))
                {
                    
                    retNode.IsActive = true;            //mark this node as active
                }
            }

            return retNode;
        }
        
        //builds a menu tree structure in memory recursively
        private static ItemNode BuildTree(XElement rootNode)
        {
            ItemNode tmpNode, tmpChildNode;
            if (rootNode.Name == "item")
            {
                tmpNode = CreateNode(rootNode); //add to menu tree
                if (rootNode.Elements("subMenu").Any())
                {
                    tmpNode.SubItems = new List<ItemNode>();
                    foreach (var childNode in rootNode.Elements("subMenu").Elements("item"))
                    {
                        tmpChildNode = BuildTree(childNode);
                        if (tmpChildNode.IsActive)
                        {
                            //child is an active node, so the parent will be one as well
                            tmpNode.IsActive = true;
                        }
                        tmpNode.SubItems.Add(tmpChildNode);
                    }
                }
                return tmpNode;
            }
            else
            {
                return null;
            }
        }

        //prints the XML node and any subnode(s) to the console recursively
        private static void PrintNode(ItemNode menuItem,string tab)
        {
            Console.WriteLine(tab + menuItem.Name + ", " + menuItem.Path + (menuItem.IsActive ? "  ACTIVE" : ""));
            if (menuItem.SubItems != null) { 
                foreach (var subItem in menuItem.SubItems)
                {
                    PrintNode(subItem, tab + "\t");
                }
            }
            
        }
    }
}
