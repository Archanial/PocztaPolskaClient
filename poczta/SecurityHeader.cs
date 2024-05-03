namespace poczta;

public sealed class SecurityHeader(string userName, string password) : System.ServiceModel.Channels.MessageHeader
{
    public override string Name => "Security";

    public override string Namespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
    
    protected override void OnWriteStartHeader(System.Xml.XmlDictionaryWriter writer,
        System.ServiceModel.Channels.MessageVersion messageVersion)
    {
        writer.WriteStartElement("wsse", Name, Namespace);
        writer.WriteXmlnsAttribute("wsse", Namespace);
    }

    protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer,
        System.ServiceModel.Channels.MessageVersion messageVersion)
    {
        writer.WriteStartElement("wsse", "UsernameToken", Namespace);
        writer.WriteStartElement("wsse", "Username", Namespace);
        writer.WriteValue(userName);
        writer.WriteEndElement();
        writer.WriteStartElement("wsse", "Password", Namespace);
        writer.WriteValue(password);
        writer.WriteEndElement();
        writer.WriteEndElement();
    }
}