using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CVA.Core.CriadorDeCampos
{
    public static class XmlHelper
    {
        public static IEnumerable<Connection> StreamConnections(string uri)
        {
            var xmlDoc = new XmlDocument();
            var ret = new List<Connection>();
            xmlDoc.Load(uri);

            var xnList = xmlDoc.GetElementsByTagName("company");

            for (var i = 0; i < xnList.Count; i++)
            {
                var conn = new Connection
                {
                    CompanyDb = xnList[i]["companyDb"].InnerText,
                    DbPassword = xnList[i]["dbPassword"].InnerText,
                    DbServer = xnList[i]["dbServer"].InnerText,
                    DbServerType = xnList[i]["dbServerType"].InnerText,
                    DbUsername = xnList[i]["dbUserName"].InnerText,
                    LicenseServer = xnList[i]["licenseServer"].InnerText,
                    Password = xnList[i]["password"].InnerText,
                    Username = xnList[i]["userName"].InnerText
                };
                ret.Add(conn);
            }

            return ret;
        }

        public static IEnumerable<UserField> StreamUserFields(string uri)
        {
            var xmlDoc = new XmlDocument();
            var ret = new List<UserField>();
            xmlDoc.Load(uri);

            var xnList = xmlDoc.GetElementsByTagName("field");

            for (var i = 0; i < xnList.Count; i++)
            {
                var field = new UserField
                {
                    DefaultValue = xnList[i]["defaultValue"].InnerText,
                    Description = xnList[i]["description"].InnerText,
                    LinkedTable = xnList[i]["linkedTable"].InnerText,
                    Mandatory = xnList[i]["mandatory"].InnerText,
                    Name = xnList[i]["name"].InnerText,
                    Size = xnList[i]["size"].InnerText,
                    SubType = xnList[i]["subType"].InnerText,
                    TableName = xnList[i]["tableName"].InnerText,
                    Type = xnList[i]["type"].InnerText
                };

                var vvList = new List<ValidValue>();

                var vvInner = xnList[i]["validValues"].InnerXml;
                var vvXdoc = new XmlDocument();
                vvXdoc.LoadXml(vvInner);
                var vvXnList = vvXdoc.GetElementsByTagName("validValue");
                for(var j = 0; j < vvXnList.Count; j++)
                {
                    var vv = new ValidValue
                    {
                        Description = vvXnList[j]["description"].InnerText,
                        Value = vvXnList[j]["value"].InnerText
                    };
                    vvList.Add(vv);
                }

                field.ValidValues = vvList;
                ret.Add(field);
            }

            return ret;
        }
    }

    public class Connection
    {
        public string DbServerType { get; set; }
        public string DbServer { get; set; }
        public string LicenseServer { get; set; }
        public string CompanyDb { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
    }

    public class UserField
    {
        public string TableName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string SubType { get; set; }
        public string Description { get; set; }
        public string Mandatory { get; set; }
        public string LinkedTable { get; set; }
        public string DefaultValue { get; set; }
        public List<ValidValue> ValidValues { get; set; }

        public UserField()
        {
            ValidValues = new List<ValidValue>();
        }
    }

    public class ValidValue
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }
}