using System;
using System.Xml;
using System.Xml.Serialization;


namespace CVM_WsAfip.Tools
{
	public class Xml
	{
		public static string Serialize<T>(T pDataToSerialize, string pXmlPropertie)
		{
			try
			{
				using (var stringwriter = new System.IO.StringWriter())
				{
					var serializer = new XmlSerializer(typeof(T));

					serializer.Serialize(stringwriter, pDataToSerialize);

					if (pXmlPropertie != "")
					{
						XmlNode XmlNode = default;
						XmlDocument xmlDocument = new XmlDocument();

						xmlDocument.LoadXml(stringwriter.ToString());
						XmlNode = xmlDocument.SelectSingleNode("//" + pXmlPropertie);
						return XmlNode.InnerText;
					}

					return stringwriter.ToString();
				}
			}
			catch(Exception ex)
			{
				throw new Exception($"{ex.Message}\n" + "Error in Serialize method" + "StackTrace : " + ex.StackTrace);
			}
		}
	}
}
