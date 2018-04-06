using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            StringBuilder outString = new StringBuilder("");
            Console.WriteLine("==================");
            XElement xelement = XElement.Load("../../menu1.xml");
            IEnumerable<XElement> items = xelement.Elements();
            try
            {
                foreach (var item in items)
                {
                    outString.Append(item.Element("displayName").Value + ", " + item.Element("path").Attribute("value").Value + "\r\n");
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
