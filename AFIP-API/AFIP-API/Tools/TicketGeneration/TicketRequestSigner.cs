using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace AFIP_API.Tools.Ticket
{
	internal class TicketRequestSigner
	{
		public string signDocWithPFX(string pPfxPath, string pPfxPassword, XmlDocument pXmlLoginTicketReq)
		{
			try
			{
				X509Certificate2 x509Certificate = new X509Certificate2(File.ReadAllBytes(pPfxPath), pPfxPassword, X509KeyStorageFlags.PersistKeySet);

				return SignMsgToBase64(pXmlLoginTicketReq, x509Certificate);
			}
			catch { throw; }
		}

		public string SignMsgToBase64(XmlDocument xml, X509Certificate2 x509Certificate)
		{
			try
			{
				byte[] msgBytes;
				byte[] encodedSignedCms;

				Encoding mEncodedMsg = Encoding.UTF8;

				msgBytes = mEncodedMsg.GetBytes(xml.OuterXml);

				encodedSignedCms = SignBytesMessage(msgBytes, x509Certificate);

				string cmsFirmadoBase64 = Convert.ToBase64String(encodedSignedCms);

				return cmsFirmadoBase64;
			}
			catch { throw; }
		}
		private static byte[] SignBytesMessage(byte[] pBytesMsg, X509Certificate2 pCertFirmante)
		{
			try
			{
				ContentInfo infoContenido = new ContentInfo(pBytesMsg);
				SignedCms cmsFirmado = new SignedCms(infoContenido);
				CmsSigner cmsFirmante = new CmsSigner(pCertFirmante);

				cmsFirmante.IncludeOption = X509IncludeOption.EndCertOnly;
				cmsFirmado.ComputeSignature(cmsFirmante);

				return cmsFirmado.Encode();
			}
			catch { throw; }
		}
	}
}