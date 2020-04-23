using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"xml11.xml");
            //root element
            XmlElement xRoot = xDoc.DocumentElement;
            Company company = new Company();
            //object record
            foreach (XmlElement xnode in xRoot)
            {
                switch (xnode.Name)
                {
                    case "provider":
                        if (xnode.InnerText != "")
                            company.Provider = Int32.Parse(xnode.InnerText);
                        break;
                    case "account":

                        company.Account = xnode.InnerText;
                        break;
                    case "counters":

                        company.Counter = new List<Counter>();
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            Counter counter = new Counter();
                            foreach (XmlNode count in childnode.ChildNodes)
                            {
                                if (count.Name == "type")
                                {
                                    counter.Type = Int32.Parse(count.InnerText);
                                }
                                if (count.Name == "value")
                                {
                                    counter.Value = count.InnerText;
                                }
                            }
                            company.Counter.Add(counter);

                        }
                        break;
                    case "pay_summ":
                        if(xnode.InnerText != "")
                        Math.Round(company.Pay_summ = Convert.ToDouble(xnode.InnerText.Replace(".", ",")), 2);
                        break;
                    default:
                        Console.WriteLine("incorrect file");
                        break;
                }

            }

            //output
            switch (company.Provider)
            {
                case 1:
                    StringBuilder sb = new StringBuilder();
                    JsonWriter jw = new JsonTextWriter(new StringWriter(sb));
                    jw.Formatting = Newtonsoft.Json.Formatting.Indented;
                    jw.WriteStartObject();
                    jw.WritePropertyName("login");
                    jw.WriteValue("1mbank");
                    jw.WritePropertyName("parol");
                    jw.WriteValue("123456");
                    jw.WritePropertyName("data");
                    jw.WriteStartArray();
                    jw.WriteStartObject();
                    jw.WritePropertyName("schet");
                    jw.WriteValue(company.Account);
                    jw.WritePropertyName("pokazaniya");
                    jw.WriteValue(company.Counter[0].Value);
                    jw.WritePropertyName("summa");
                    jw.WriteValue(company.Pay_summ);
                    jw.WriteEndObject();
                    jw.WriteEndArray();
                    jw.WriteEndObject();

                    Console.WriteLine(sb);
                    //  string json = System.Text.Json.JsonSerializer.Serialize<Company>(company);

                    break;
                case 2:
                    StringBuilder sb1 = new StringBuilder();
                    JsonWriter jw1 = new JsonTextWriter(new StringWriter(sb1));

                    jw1.Formatting = Newtonsoft.Json.Formatting.Indented;
                    jw1.WriteStartObject();
                    jw1.WritePropertyName("jsonrpc");
                    jw1.WriteValue(company.Account);
                    jw1.WritePropertyName("method");
                    jw1.WriteValue("fpay");
                    jw1.WritePropertyName("params");
                    jw1.WriteStartArray();
                    jw1.WriteStartObject();
                    jw1.WritePropertyName("session");
                    jw1.WriteStartObject();
                    jw1.WritePropertyName("login");
                    jw1.WriteValue("1mbank");
                    jw1.WritePropertyName("pass");
                    jw1.WriteValue("33333");
                    jw1.WriteEndObject();
                    jw1.WritePropertyName("data");
                    jw1.WriteStartArray();
                    jw1.WriteStartObject();
                    jw1.WritePropertyName("account");
                    jw1.WriteValue(company.Account);

                    jw1.WritePropertyName("counters");
                    jw1.WriteStartArray();
                    jw1.WriteStartObject();
                    int i;
                    i = 0;
                    for (i = 0; i < company.Counter.Count; i++)
                    {
                        jw1.WritePropertyName("counter");
                        jw1.WriteStartObject();
                        jw1.WritePropertyName("type");
                        jw1.WriteValue(company.Counter[i].Type);
                        jw1.WritePropertyName("value");
                        jw1.WriteValue(company.Counter[i].Value);
                        jw1.WriteEndObject();
                    }
                    jw1.WriteEndObject();
                    jw1.WriteEndArray();
                    jw1.WriteEndObject();
                    jw1.WriteEndArray();
                    jw1.WriteEndObject();
                    jw1.WriteEndArray();
                    jw1.WriteEndObject();
                    Console.WriteLine(sb1);
                    break;

                case 3:
                    XDocument xDocument =
                new XDocument(
                    new XElement("xml",
                        new XElement("dogovor", company.Account),
                        new XElement("date", DateTime.Now),
                        new XElement("summa", company.Pay_summ)));
                    Console.WriteLine(xDocument.ToString());
                    break;
                case 4:
                    List<int> someList = new List<int>();
                    for (i = 0; i < company.Counter.Count; i++)
                    {
                        someList.Add(i);
                    }
                    XDocument xDocument1 =
            new XDocument(
                new XElement("xml",
                    new XElement("c",
                        new XAttribute("n", "ses"),
                            new XElement("a",
                                new XAttribute("n", "log"), "1mbank"),
                            new XElement("a",
                                new XAttribute("n", "pas"), "11111"),
                            new XElement("a",
                                new XAttribute("n", "acc"), company.Account)
                                ),
                    new XElement("s",
                        from n in someList
                        select new XElement("c", new XAttribute("n", "cnt"), new XElement("a",
                                                                                              new XAttribute("n", "typ"), company.Counter[n].Type),
                                                                             new XElement("a",
                                                                                              new XAttribute("n", "val"), company.Counter[n].Value))
                                ),
                    new XElement("c",
                            new XAttribute("n", "sum"), company.Pay_summ)));
                    Console.WriteLine(xDocument1.ToString());
                    break;
                default:
                    Console.WriteLine("no provider");
                    break;
            }

            Console.ReadLine();
        }

        public class Company
        {
            public int Provider { get; set; }
            public string Account { get; set; }
            public List<Counter> Counter { get; set; }
            public double Pay_summ { get; set; }
        }

        public class Counter
        {
            public int Type { get; set; }
            public string Value { get; set; }
        }

    }
}
