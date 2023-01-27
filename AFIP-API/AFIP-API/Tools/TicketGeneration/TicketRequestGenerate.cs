using System.Xml;

namespace AFIP_API.Tools.Ticket
{
	internal class TicketRequestGenerate
	{
		private XmlDocument XmlLoginTicketRequest = null;
		private string XmlLoginTicketRequestTemplate = "<loginTicketRequest><header><uniqueId></uniqueId><generationTime></generationTime><expirationTime></expirationTime></header><service></service></loginTicketRequest>";

		public XmlDocument CreateXmlTicket(string service, int expiresIn)
		{
			try
			{
				XmlNode? xmlNodoUniqueId = default;
				XmlNode? xmlNodoGenerationTime = default;
				XmlNode? xmlNodoExpirationTime = default;
				XmlNode? xmlNodoService = default;

				XmlLoginTicketRequest = new XmlDocument();
				XmlLoginTicketRequest.LoadXml(XmlLoginTicketRequestTemplate);

				xmlNodoUniqueId = XmlLoginTicketRequest.SelectSingleNode("//uniqueId");
				xmlNodoGenerationTime = XmlLoginTicketRequest.SelectSingleNode("//generationTime");
				xmlNodoExpirationTime = XmlLoginTicketRequest.SelectSingleNode("//expirationTime");
				xmlNodoService = XmlLoginTicketRequest.SelectSingleNode("//service");

				xmlNodoGenerationTime.InnerText = DateTime.Now.AddMinutes(-2).ToString("s");
				xmlNodoExpirationTime.InnerText = DateTime.Now.AddHours(expiresIn).ToString("s");
				xmlNodoUniqueId.InnerText = "123123";
				xmlNodoService.InnerText = service;

				return XmlLoginTicketRequest;
			}
			catch { throw; }			
		}
	}
}

