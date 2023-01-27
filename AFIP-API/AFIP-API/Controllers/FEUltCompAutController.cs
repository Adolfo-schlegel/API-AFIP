using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AFIP_API.Models;
using AFIP_API.Services.Authentication;
using AFIP_API.Tools;
using AFIP_API.Tools.Ticket;
using CVM_WsAfip.Tools;
using ServiceFEAFIP;

namespace AFIP_API.Controller
{
	public class FEUltCompAutController
	{
		TicketResult _ticketResult;
		private ServiceSoapClient _mServiceSoap;
		private FEAuthRequest? _mFEAuthRequest;
		private Ticket? _mTicket;
		private FERecuperaLastCbteResponse _mFELastCbteResponse;

		public FEUltCompAutController()
		{
			_mServiceSoap = new ServiceSoapClient(0);
			_mTicket = new Ticket();
		}
		public int UltCompCbteNro { get => _mFELastCbteResponse.CbteNro; }
		public ListClientResponse Errors
		{
			get
			{
				if (_mFELastCbteResponse.Errors == null)
					return new ListClientResponse { Lista = new List<ListaObservaDetalle> { new ListaObservaDetalle { detalle = "No hay errores" } } };


				return new ListClientResponse
				{
					Cantidad = _mFELastCbteResponse.Errors.Length,
					Lista = _mFELastCbteResponse.Errors.Select((x) =>
					new ListaObservaDetalle
					{
						codigo = x.Code.ToString(),
						detalle = x.Msg
					}).ToList<ListaObservaDetalle>()
				};


			}
		}
		public ListClientResponse Even
		{
			get
			{
				if (_mFELastCbteResponse.Events == null)
					return new ListClientResponse { Lista = new List<ListaObservaDetalle> { new ListaObservaDetalle { detalle = "No hay Eventos" } } };

				return new ListClientResponse
				{
					Cantidad = _mFELastCbteResponse.Events.Length,
					Lista = _mFELastCbteResponse.Events.Select((x) => new ListaObservaDetalle
					{
						codigo = x.Code.ToString(),
						detalle = x.Msg
					}).ToList<ListaObservaDetalle>()
				};
			}
		}

		//Ticket StackTrace
		public string TicketStackTrace { get; protected set; }

		public async Task<string> AuthenticationWithPFX(string pCuit, string pCertPfxPath, string pTicketFolder, int expiresIn = 12, string pTicketName = "TicketRequest")
		{
			string lstr_res = "OK";

			try
			{

				_ticketResult = await new AfipConnection().CreateTicketWithPFX("UltCompAuth", pTicketName, pTicketFolder, pCuit, pCertPfxPath, "wsfe", expiresIn);

				if (_ticketResult.result == "OK")
				{
					if (!_mTicket.EqualCuitTicket(_ticketResult.path, pCuit))
					{
						lstr_res = "El ticket enviado por parametros no es el mismo que el del certificado autorizado";
					}

					_mFEAuthRequest = new FEAuthRequest()
					{
						Cuit = long.Parse(pCuit),
						Token = _mTicket.GetSingleNodeText(_ticketResult.path, "token"),
						Sign = _mTicket.GetSingleNodeText(_ticketResult.path, "sign")
					};

					lstr_res = _ticketResult.result;
					TicketStackTrace = _ticketResult.stackTrace;
				}
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in AuthenticationWithPFX" + "\n StackTrace: " + ex.StackTrace;
			}
			return lstr_res;
		}
		public async Task<string> AuthenticationWithCrtAndPkAsync(string pCuit, string pCertFolder, string pCrtName, string pPrivKeyName, string pTicketFolder, int expiresIn = 12, string pTicketName = "TicketRequest")
		{
			string lstr_res = "OK";

			try
			{
				_ticketResult = await new AfipConnection().CreateTicketWithCrtAndKey("UltCompAuth", pTicketName, pTicketFolder, pCertFolder + "/" + pPrivKeyName, pCertFolder + "/" + pCrtName, pCertFolder, pCuit, "wsfe", expiresIn);

				if (_ticketResult.result == "OK")
				{
					if (!_mTicket.EqualCuitTicket(_ticketResult.path, pCuit))
						lstr_res = "El ticket enviado por parametros no es el mismo que el del certificado autorizado";

					_mFEAuthRequest = new FEAuthRequest()
					{
						Cuit = long.Parse(pCuit),
						Token = _mTicket.GetSingleNodeText(_ticketResult.path, "token"),
						Sign = _mTicket.GetSingleNodeText(_ticketResult.path, "sign")
					};

					lstr_res = _ticketResult.result;
					TicketStackTrace = _ticketResult.stackTrace;
				}
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in AuthenticationWithCrtAndPkAsync" + "\n StackTrace: " + ex.StackTrace;
			}
			return lstr_res;
		}
		public async Task<string> GetLastFECompUltAut(int pPtoVta, int pCbteTipo)
		{
			string lstr_res = "OK";

			try
			{
				FECompUltimoAutorizadoResponse fECompUltimoAutorizadoResponse = await _mServiceSoap.FECompUltimoAutorizadoAsync(_mFEAuthRequest, pPtoVta, pCbteTipo);
				_mFELastCbteResponse = fECompUltimoAutorizadoResponse.Body.FECompUltimoAutorizadoResult;
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in GetLastFECompUltAut" + "\n StackTrace: " + ex.StackTrace;
			}

			return lstr_res;
		}
			
		public string GetXmlResponseUltimoCoomprobante(string pXmlPropertie = "") => Xml.Serialize(_mFELastCbteResponse, pXmlPropertie);
	}
}
