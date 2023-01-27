using System.Xml;

namespace AFIP_API.Tools.Ticket
{
	public class Ticket
	{
		private static XmlDocument _mXmlDocument;
		public string PathTicket { get; set; }
		public DateTime ExpirationTime { get; private set; }

		public void DeleteTicketExpired()
		{
			try
			{
				File.Delete(PathTicket);
			}
			catch { throw; }
		}


		public string SaveTicket(string pLoginTicketXml)
		{
			try
			{
				File.WriteAllText(PathTicket, pLoginTicketXml);
				return "OK";
			}
			catch { throw; }
		}
		public FileInfo FindXmlInDirectory(string path)
		{
			var ldirectoryTicket = new DirectoryInfo(path);

			if (!ldirectoryTicket.Exists)
				throw new Exception("La carpeta [" + path + "] que se indico no existe");

			if (ldirectoryTicket.GetFiles().Length == 0)
				throw new Exception("La carpeta [" + path + "] no contiene archivos");

			if (!ldirectoryTicket.GetFiles().Where(x => x.Extension == ".xml").Any())
				return null;

			var myFile = ldirectoryTicket.GetFiles()
			 .Where(x => x.Extension == ".xml")
			 .OrderByDescending(f => f.LastWriteTime).First();

			return myFile;
		}
		public TicketCasesDirectory SearchAvailableTicket(string pTicketFolder)
		{
			try
			{
				FileInfo myFile = FindXmlInDirectory(pTicketFolder);

				//ticket no existe
				if (myFile == null)
					return TicketCasesDirectory.TicketNotFound;

				PathTicket = myFile.FullName;

				_mXmlDocument = GetXmlDocument(myFile.FullName);

				//if ticket exist so find the expiration time of the node 
				long TicksOfTicket = GetTicksOfDateNode(_mXmlDocument, "//expirationTime");

				//ticket expirado
				if (TicksOfTicket < 0)
					return TicketCasesDirectory.TicketExpiredExist;

				//ticket activo
				return TicketCasesDirectory.TicketActiveExist;
			}
			catch { throw; }
		}
		public string GetPathAvailableTicket(string pTicketFolder)
		{
			FileInfo myFile = FindXmlInDirectory(pTicketFolder);
			return myFile.FullName;
		}
		public long GetTicksOfDateNode(XmlDocument xmlDocument, string nodeName)
		{
			XmlNode lxmlExpirationTime = default;
			lxmlExpirationTime = xmlDocument.SelectSingleNode(nodeName);

			var timeSpan = CalculateSpanTime(lxmlExpirationTime.InnerText);

			return timeSpan.Ticks;
		}
		private TimeSpan CalculateSpanTime(string expirationTime)
		{
			ExpirationTime = DateTime.Parse(expirationTime);
			DateTime TimeNow = DateTime.Now;
			TimeSpan timeSpan = ExpirationTime - TimeNow;
			return timeSpan;
		}
		public string GetSingleNodeText(string xmlFilePath, string pXmlNodeName)
		{
			try
			{
				_mXmlDocument = GetXmlDocument(xmlFilePath);
				XmlNode? lXmlNode = default;
				lXmlNode = _mXmlDocument.SelectSingleNode("//" + pXmlNodeName);
				return lXmlNode.InnerText;
			}
			catch { throw; }
		}

		private string GetSingleNodeOuterXml(string pNodeName)
		{
			XmlNode lXmlNode = default;
			lXmlNode = _mXmlDocument.SelectSingleNode("//" + pNodeName);
			return lXmlNode.OuterXml;
		}

		private static XmlDocument GetXmlDocument(string pPathFile)
		{
			try
			{
				_mXmlDocument = new XmlDocument();
				_mXmlDocument.LoadXml(File.ReadAllText(pPathFile));
				return _mXmlDocument;
			}
			catch { throw; }
		}

		public string GetElementInNode(string pXmlPath, string pXmlNodeName, string pXmlElementName)
		{
			try
			{
				_mXmlDocument = GetXmlDocument(pXmlPath);
				string lstrNode = GetSingleNodeText(pXmlPath, pXmlNodeName);

				if (lstrNode.Contains(pXmlElementName))
					return lstrNode.Substring(lstrNode.IndexOf(pXmlElementName), 16).Split(' ').LastOrDefault();

				return "";
			}
			catch { throw; }
		}

		public bool EqualCuitTicket(string pTicketPath, string pCuit)
		{
			string lstrCuit = GetElementInNode(pTicketPath, "destination", "CUIT ");
			return pCuit == lstrCuit;
		}

		public enum TicketCasesDirectory
		{
			TicketNotFound = 1,
			TicketExpiredExist = 2,
			TicketActiveExist = 3
		}

	}
}