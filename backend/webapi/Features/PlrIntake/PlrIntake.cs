namespace Pidp.Features.PlrIntake;

using System.Xml;

public class PlrIntake
{

    private readonly XmlNamespaceManager nsManager;

    public PlrIntake()
    {
        this.nsManager = new XmlNamespaceManager(new NameTable());
        this.nsManager.AddNamespace("plr", "urn:hl7-org:v3");
    }

    public void ReadDistributionMessage(string messageconent)
    {
        var doc = new XmlDocument();
        var strReader = new StringReader(messageconent);
        doc.Load(strReader);
        XmlNode docRoot = doc.DocumentElement!;
        var messageId = this.ReadNodeData($"//plr:id[@root='2.16.840.1.113883.3.40.1.5']/@extension", docRoot);
        Console.WriteLine("messageId : {0}", messageId);
    }

    private string? ReadNodeData(string xPath, XmlNode documentRoot, string? messageId = null)
    {
        var node = documentRoot.SelectSingleNode(xPath, this.nsManager);
        if (node != null)
        {
            return node.InnerXml;
        }
        else
        {
            Console.WriteLine("No Data Found for message id : {0}", messageId);
            return null;
        }
    }
}