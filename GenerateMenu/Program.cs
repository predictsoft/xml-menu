using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GenerateMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            /*XElement xelement1 = XElement.Load("../../menu1.xml");
            var item1 = from nm in xelement1.Elements("item")
                /*where (string)nm.Element("Sex") == "Female"#1#
                select nm;
            Console.WriteLine("item:");
            foreach (XElement xEle in item1)
                Console.WriteLine(xEle);*/


            if (args.Length != 2)
            {
                Console.WriteLine("Invalid input parameters. Please provide XML filename and active path to match. This program will now close.");
                return;
            }

            Console.WriteLine("Parameters: " + args[0] + " " + args[1]);

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File not found ("+args[0]+"). Please make sure file exists in the given location. This program will now exit");
                return;
            }

            XElement xelement = XElement.Load(args[0]);

            StringBuilder outString = new StringBuilder("");
            IEnumerable<XElement> items = xelement.Elements();
            try
            {
                foreach (var item in items)
                {
                    outString.Append(item.Element("displayName").Value + ", " + item.Element("path").Attribute("value").Value);
                    //TODO: active if true
                    if (item.Element("path").Attribute("value").Value == args[1])
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
                            if (submenuItem.Element("path").Attribute("value").Value == args[1])
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

                Console.WriteLine(outString.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception encountered. Details:\r\n\t" + e.Message);
                return;
            }

            

        }
    }
}
