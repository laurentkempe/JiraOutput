using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace JiraOutput
{
    public class JiraPreferences
    {
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string ServerUrl { get; set; }

        public static void Setup()
        {
            if (!Directory.Exists(JiraOutput.PreferenceDirectory))
                Directory.CreateDirectory(JiraOutput.PreferenceDirectory + "\\");
            if (File.Exists(JiraOutput.PrefsPathWithFileName))
                return;
            WriteDefaults();
        }

        private static void WriteDefaults()
        {
            Write();
        }

        public static void Write()
        {
            var xmlConfig = new XElement("Jira",
                                new XElement("ServerUrl", ServerUrl),
                                new XElement("Username", Username),
                                new XElement("Password", Password));

            using (var xmlWriter = XmlWriter.Create(JiraOutput.PrefsPathWithFileName, new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = " "
            }))
            {
                xmlConfig.WriteTo(xmlWriter);
            }
        }

        public static void Read()
        {
            var xElement = XElement.Load(JiraOutput.PrefsPathWithFileName);
            var element = xElement.Element("ServerUrl");
            if (element != null) ServerUrl = element.Value;
            element = xElement.Element("Username");
            if (element != null) Username = element.Value;
            element = xElement.Element("Password");
            if (element != null) Password = element.Value;
        }
    }
}