using AFIP_API.Models;
using AFIP_API.Tools.PfxGeneration;
using AFIP_API.Tools.Ticket;
using ServiceAuthenticationAFIP;
using System.Xml;
using static AFIP_API.Tools.Ticket.Ticket;

namespace AFIP_API.Services.Authentication
{
	public class AfipConnection
	{
		private loginCmsResponse? _mLoginCmsResponse;
		private LoginCMSClient? _mLoginCMSClient;
		private TicketRequestSigner? _mTicketRequestSigner;
		private TicketRequestGenerate? _mTicketRequestGenerate;
		private Ticket? _mLoginTicket;
		private PfxMaker? pfxService;

		public async Task<TicketResult> CreateTicketWithPFX(string pControllerName, string pTicketName, string pTicketFolder, string pPfxPassword, string pPfxPath, string pService, int expiresIn = 12)
		{
			string lstrResult = "OK";
			string ticketPath = "";
			string stackTraceTicket = "";

			try
			{
				_mLoginTicket = new Ticket();
				TicketCasesDirectory casesDirectory = _mLoginTicket.SearchAvailableTicket(pTicketFolder);

				if (casesDirectory == TicketCasesDirectory.TicketActiveExist)
				{
					ticketPath = _mLoginTicket.GetPathAvailableTicket(pTicketFolder);
					stackTraceTicket += "\n Ticket Existente y activo \n Fecha de vencimiento: " + _mLoginTicket.ExpirationTime;
				}
				else
				{
					if (casesDirectory == TicketCasesDirectory.TicketNotFound)
						stackTraceTicket += "\n -> Ticket Inexistente";

					if (casesDirectory == TicketCasesDirectory.TicketExpiredExist)
					{
						stackTraceTicket += "\n -> Ticket Existente pero vencido \n -> Fecha vencimiento " + _mLoginTicket.ExpirationTime;

						_mLoginTicket.DeleteTicketExpired();

						stackTraceTicket += "\n -> Se elimino el anterior ticket expirado";
					}

					lstrResult = new PfxMaker().CheckExistisPfx(pPfxPath);

					if (lstrResult == "OK")
					{
						lstrResult = await ticketRequest(pService, pPfxPath, pPfxPassword, expiresIn);
					}
					if (lstrResult == "OK")
					{
						_mLoginTicket.PathTicket = pTicketFolder + @"\" + pTicketName + "_" + pControllerName + ".xml";

						lstrResult = _mLoginTicket.SaveTicket(_mLoginCmsResponse.loginCmsReturn);
						ticketPath = _mLoginTicket.PathTicket;

						if (lstrResult == "OK")
							stackTraceTicket += "\n -> Se creo un nuevo Ticket de acceso \n -> Nombre: " + Path.GetFileNameWithoutExtension(_mLoginTicket.PathTicket) + " \n -> Fecha vencimiento: " + _mLoginTicket.GetSingleNodeText(_mLoginTicket.PathTicket, "expirationTime");
					}
				}
			}
			catch (Exception ex) { throw new Exception($"{ex.Message}\n" + "Error in CreateTicketWithPFX" + "\nStackTrace: " + ex.StackTrace); }

			return new TicketResult { result = lstrResult, path = ticketPath, stackTrace = stackTraceTicket };
		}
		public async Task<TicketResult> CreateTicketWithCrtAndKey(string pControllerName, string pTicketName, string pTicketFolder, string pPrivateKey, string pCrtPath, string pPfxPathToSave, string pPfxPassword, string pService, int expiresIn = 12)
		{
			string lstrResult = "OK";
			string ticketPath = "";
			string stackTraceTicket = "";

			try
			{
				_mLoginTicket = new Ticket();
				TicketCasesDirectory casesDirectory = _mLoginTicket.SearchAvailableTicket(pTicketFolder);

				if (casesDirectory == TicketCasesDirectory.TicketActiveExist)
				{
					ticketPath = _mLoginTicket.GetPathAvailableTicket(pTicketFolder);
					stackTraceTicket += "\n Ticket Existente y activo \n Fecha de vencimiento: " + _mLoginTicket.ExpirationTime;
				}
				else
				{
					if (casesDirectory == TicketCasesDirectory.TicketNotFound)
						stackTraceTicket += "\n -> Ticket Inexistente";

					if (casesDirectory == TicketCasesDirectory.TicketExpiredExist)
					{
						stackTraceTicket += "\n -> Ticket Existente pero vencido \n -> Fecha vencimiento " + _mLoginTicket.ExpirationTime;

						_mLoginTicket.DeleteTicketExpired();

						stackTraceTicket += "\n -> Se elimino el anterior ticket expirado";
					}

					pfxService = new PfxMaker();

					lstrResult = await pfxService.CreatePFXFile(pCrtPath, pPrivateKey, pPfxPathToSave, pPfxPassword, Path.GetFileNameWithoutExtension(pCrtPath));

					if (lstrResult == "OK")
					{
						lstrResult = await ticketRequest(pService, pfxService.mCertPfxPath, pfxService.mCertPfxPassword, expiresIn);
					}

					if (lstrResult == "OK")
					{
						_mLoginTicket.PathTicket = pTicketFolder + @"\" + pTicketName + "_" + pControllerName + ".xml";

						lstrResult = _mLoginTicket.SaveTicket(_mLoginCmsResponse.loginCmsReturn);
						ticketPath = _mLoginTicket.PathTicket;

						if (lstrResult == "OK")
							stackTraceTicket += "\n -> Se creo un nuevo Ticket de acceso \n -> Nombre: " + Path.GetFileNameWithoutExtension(_mLoginTicket.PathTicket) + " \n -> Fecha vencimiento: " + _mLoginTicket.GetSingleNodeText(_mLoginTicket.PathTicket, "expirationTime");
					}
				}
			}
			catch (Exception ex) { throw new Exception($"{ex.Message}\n" + "Error in CreateTicketWithCrtAndKey" + "\nStackTrace: " + ex.StackTrace); }

			return new TicketResult { result = lstrResult, path = ticketPath, stackTrace = stackTraceTicket };
		}

		private async Task<string> ticketRequest(string pService, string pPfxPath, string pPfxPassword, int expiresIn = 12)
		{
			string lstrResult = "OK";
			try
			{
				_mTicketRequestGenerate = new TicketRequestGenerate();
				_mTicketRequestSigner = new TicketRequestSigner();
				_mLoginCmsResponse = new loginCmsResponse();
				_mLoginCMSClient = new LoginCMSClient();

				//xml ticket creation
				XmlDocument XmlTicket = _mTicketRequestGenerate.CreateXmlTicket(pService, expiresIn);

				//sign xml with the pfx file
				string cmsSignToBase64 = _mTicketRequestSigner.signDocWithPFX(pPfxPath, pPfxPassword, XmlTicket);

				//if was succeful, set the cms with  				
				_mLoginCmsResponse = await _mLoginCMSClient.loginCmsAsync(cmsSignToBase64);
			}
			catch (Exception ex)
			{
				lstrResult = ex.Message;
			}
			return lstrResult;
		}
	}
}


