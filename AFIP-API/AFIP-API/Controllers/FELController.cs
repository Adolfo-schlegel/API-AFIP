using AFIP_API.Models;
using AFIP_API.Services.Authentication;
using AFIP_API.Tools.Ticket;
using CVM_WsAfip.Tools;
using Newtonsoft.Json;
using ServiceFEAFIP;

namespace AFIP_API.Controllers
{
	public class FELController : PropertiesFE
	{
		private TicketResult _ticketResult;
		private FeCAEReq _mFeCAEReqModel;
		private Ticket _mTicket;
		private FEAuthRequest _mFEAuthRequest;
		private FECAERequest _mFECAERequest;
		private ServiceSoapClient _mServiceSoap;
		public FELController()
		{
			_mFeCAEReqModel = new FeCAEReq()
			{
				FeCabReq = new Models.FECAECabRequest(),
				FeDetReq = new List<Models.FECAEDetRequest>()
			};
			_mTicket = new Ticket();
			_mServiceSoap = new ServiceSoapClient(0);
		}

		//Add values to model
		public void CreateComprobante(FeCAEReq pFeCAEReq) =>
			_mFeCAEReqModel = pFeCAEReq;
		public void AddIVA(Models.AlicIva pAlicIva, int pIndexFeDetReq = 0) =>
			_mFeCAEReqModel.FeDetReq[pIndexFeDetReq].Iva.Add(pAlicIva);
		public void AddOpcional(Models.Opcional pOpcional, int pIndexFeDetReq = 0) =>
			_mFeCAEReqModel.FeDetReq[pIndexFeDetReq].Opcionales.Add(pOpcional);
		public void AddCompAsoc(Models.CbteAsoc pCbteAsoc, int pIndexCbteAsoc = 0) =>
			_mFeCAEReqModel.FeDetReq[pIndexCbteAsoc].CbtesAsoc.Add(pCbteAsoc);
		public void AddTributo(Models.Tributo pTributo, int pIndexTributo) =>
			_mFeCAEReqModel.FeDetReq[pIndexTributo].Tributos.Add(pTributo);

		//Overload model
		public string CreateComprobante(int pCantRegCAB, int pPtoVtaCAB, int pCbteTipoCAB, int pConcepto, int pDocTipo, long pDocNro, long pCbteDesde, long pCbteHasta, string pCbteFch, double pImpTotal, double pImpTotConc, double pImpNeto, double pImpOpEx, double pImpTrib, double pImpIVA, string pFchServDesde, string pFchServHasta, string pFchVtoPago, string pMonId, double pMonCotiz)
		{
			if (pMonId == "PES" && pMonCotiz == 0)
				pMonCotiz = 1;

			if (pMonId != "PES" && pMonCotiz == 0)
				return "Si la moneda no es pesos, se requiere Cotizacion";

			try
			{
				//CAB
				_mFeCAEReqModel.FeCabReq.PtoVta = pPtoVtaCAB;
				_mFeCAEReqModel.FeCabReq.CantReg = pCantRegCAB;
				_mFeCAEReqModel.FeCabReq.CbteTipo = pCbteTipoCAB;

				//Body/DET
				_mFeCAEReqModel.FeDetReq.Add(
				new Models.FECAEDetRequest()
				{
					MonId = pMonId,
					FchServDesde = pFchServDesde,
					CbteDesde = pCbteDesde,
					CbteHasta = pCbteHasta,
					CbteFch = pCbteFch,
					FchVtoPago = pFchVtoPago,
					DocTipo = pDocTipo,
					FchServHasta = pFchServHasta,
					Concepto = pConcepto,
					DocNro = pDocNro,
					ImpIVA = pImpIVA,
					ImpNeto = pImpNeto,
					ImpOpEx = pImpOpEx,
					ImpTotal = pImpTotal,
					ImpTotConc = pImpTotConc,
					ImpTrib = pImpTrib,
					MonCotiz = pMonCotiz,
					Iva = new List<Models.AlicIva>(),
					Opcionales = new List<Models.Opcional>(),
					CbtesAsoc = new List<Models.CbteAsoc>(),
					Tributos = new List<Models.Tributo>()
				});

				return "OK";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public string AddIVA(int pId, double pBaseImp, double pImporte, int pIndexFeDetReq = 0)
		{
			try
			{
				_mFeCAEReqModel.FeDetReq[pIndexFeDetReq].Iva.Add(
					new Models.AlicIva()
					{
						Id = pId,
						BaseImp = pBaseImp,
						Importe = pImporte
					});
				return "OK";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public string AddOpcional(string pId, string pValor, int pIndexFeDetReq = 0)
		{
			try
			{
				_mFeCAEReqModel.FeDetReq[pIndexFeDetReq].Opcionales.Add(
					new Models.Opcional()
					{
						Id = pId,
						Valor = pValor
					});
				return "OK";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public string AddCompAsoc(int pTipo, int pPtoVta, long pNro, string pCuit, string pCbteFch, int pIndexCbteAsoc = 0)
		{
			try
			{
				_mFeCAEReqModel.FeDetReq[pIndexCbteAsoc].CbtesAsoc.Add(
					new Models.CbteAsoc()
					{
						Tipo = pTipo,
						PtoVta = pPtoVta,
						Nro = pNro,
						Cuit = pCuit,
						CbteFch = pCbteFch
					});
				return "OK";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public string AddTributo(short pId, string pDesc, double pBaseImp, double pAlic, double pImporte, int pindexTributo = 0)
		{
			try
			{
				_mFeCAEReqModel.FeDetReq[pindexTributo].Tributos.Add(
					new Models.Tributo()
					{
						Desc = pDesc,
						Importe = pImporte,
						Alic = pAlic,
						BaseImp = pBaseImp,
						Id = pId,
					}
				);


				return "OK";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		//Auth service
		public async Task<string> AuthenticationWithPFX(string pCuit, string pCertPfxPath, string pTicketFolder, int expiresIn = 12, string pTicketName = "TicketRequest")
		{
			string lstr_res = "OK";

			if (expiresIn >= 24)
			{
				expiresIn = 23;
			}

			try
			{
				_ticketResult = await new AfipConnection().CreateTicketWithPFX("FEController", pTicketName, pTicketFolder, pCuit, pCertPfxPath, "wsfe", expiresIn);

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

					TicketStackTrace = _ticketResult.stackTrace;
					lstr_res = _ticketResult.result;
				}
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in AuthenticationWithPFX" + "\n StackTrace: " + ex.StackTrace;
			}
			return lstr_res;
		}
		public async Task<string> AuthenticationWithCrtAndPk(string pCuit, string pCertFolder, string pCrtName, string pPrivKeyName, string pTicketFolder, int expiresIn = 12, string pTicketName = "TicketRequest")
		{
			string lstr_res = "OK";

			if (expiresIn >= 24)
			{
				expiresIn = 23;
			}
			try
			{
				_ticketResult = await new AfipConnection().CreateTicketWithCrtAndKey("FE", pTicketName, pTicketFolder, pCertFolder + "/" + pPrivKeyName, pCertFolder + "/" + pCrtName, pCertFolder, pCuit, "wsfe", expiresIn);

				if (_ticketResult.result == "OK")
				{
					if (!_mTicket.EqualCuitTicket(_ticketResult.path, pCuit))
						return "El ticket enviado por parametros no es el mismo que el del certificado autorizado";

					_mFEAuthRequest = new FEAuthRequest()
					{
						Cuit = long.Parse(pCuit),
						Token = _mTicket.GetSingleNodeText(_ticketResult.path, "token"),
						Sign = _mTicket.GetSingleNodeText(_ticketResult.path, "sign")
					};

					TicketStackTrace = _ticketResult.stackTrace;
					lstr_res = _ticketResult.result;
				}
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in AuthenticationWithCrtAndPk" + "\n StackTrace: " + ex.StackTrace;
			}
			return lstr_res;
		}
		public async Task<string> Autorizar()
		{
			string lstr_res = "OK";

			try
			{
				_mFECAERequest = JsonConvert.DeserializeObject<FECAERequest>(JsonConvert.SerializeObject(_mFeCAEReqModel).ToString());

				FECAESolicitarResponse fECAESolicitarResponse = await _mServiceSoap.FECAESolicitarAsync(_mFEAuthRequest, _mFECAERequest);

				_mFECAEResponse = fECAESolicitarResponse.Body.FECAESolicitarResult;
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in Autorizar" + "\n StackTrace: " + ex.StackTrace;
			}

			return lstr_res;
		}

		//Functions 
		public string GetXmlRequest(string pXmlPropertie = "") => Xml.Serialize(_mFeCAEReqModel, pXmlPropertie);
		public string GetXmlResponse(string pXmlPropertie = "") => Xml.Serialize(_mFECAEResponse, pXmlPropertie);
		public string GetXmlAuth(string pXmlPropertie = "") => Xml.Serialize(_mFEAuthRequest, pXmlPropertie);
		public string GetXmlResponseUltimoCoomprobante(string pXmlPropertie = "") => Xml.Serialize(_mLastCbteResponse, pXmlPropertie);
		public async Task<string> GetLastFECompUltAut(int pPtoVta, int pCbteTipo)
		{
			string lstr_res = "OK";

			try
			{
				FECompUltimoAutorizadoResponse fECompUltimoAutorizadoResponse = await _mServiceSoap.FECompUltimoAutorizadoAsync(_mFEAuthRequest, pPtoVta, pCbteTipo);
				_mLastCbteResponse = fECompUltimoAutorizadoResponse.Body.FECompUltimoAutorizadoResult;

				if (_mLastCbteResponse.Errors.Length != 0)
					lstr_res += _mLastCbteResponse.Errors.First();
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in GetLastFECompUltAut" + "\n StackTrace: " + ex.StackTrace;
			}
			return lstr_res;
		}
	}

	public class PropertiesFE
	{
		protected FECAEResponse _mFECAEResponse;
		protected FERecuperaLastCbteResponse _mLastCbteResponse;

		//cab
		public long FECabCuit { get => _mFECAEResponse.FeCabResp.Cuit; }
		public int FECabCuitPtoVta { get => _mFECAEResponse.FeCabResp.PtoVta; }
		public int FECabCbteTipo { get => _mFECAEResponse.FeCabResp.CbteTipo; }
		public string FECabFchProceso { get => _mFECAEResponse.FeCabResp.FchProceso; }
		public int FECabCantReg { get => _mFECAEResponse.FeCabResp.CantReg; }
		public string CFEabResultado { get => _mFECAEResponse.FeCabResp.Resultado; }
		public string FECabReproceso { get => _mFECAEResponse.FeCabResp.Reproceso; }

		//body
		public string FEDetCAE { get => _mFECAEResponse.FeDetResp.First().CAE; }
		public string FEDetCAEFchVto { get => _mFECAEResponse.FeDetResp.First().CAEFchVto; }
		public int FEDetConcepto { get => _mFECAEResponse.FeDetResp.First().Concepto; }
		public int FEDetDocTipo { get => _mFECAEResponse.FeDetResp.First().DocTipo; }
		public long FEDetDocNro { get => _mFECAEResponse.FeDetResp.First().DocNro; }
		public long FEDetCbteDesde { get => _mFECAEResponse.FeDetResp.First().CbteDesde; }
		public long FEDetCbteHasta { get => _mFECAEResponse.FeDetResp.First().CbteHasta; }
		public string FEDetCbteFch { get => _mFECAEResponse.FeDetResp.First().CbteFch; }
		public string FEDetResultado { get => _mFECAEResponse.FeDetResp.First().Resultado; }
		public ListClientResponse FEDetObservaciones
		{
			get
			{
				if (_mFECAEResponse.FeDetResp == null || _mFECAEResponse.FeDetResp.First().Observaciones == null)
					return new ListClientResponse() { Lista = new List<ListaObservaDetalle>() { new ListaObservaDetalle() { detalle = "No hay observaciones" } } };

				return new ListClientResponse()
				{
					Cantidad = _mFECAEResponse.FeDetResp.First().Observaciones.Length,
					Lista = _mFECAEResponse.FeDetResp.First().Observaciones.Select((x) =>
					new ListaObservaDetalle()
					{

						codigo = x.Code.ToString(),
						detalle = x.Msg

					}).ToList<ListaObservaDetalle>()
				};
			}
		}
		public ListClientResponse FEDetErrors
		{
			get
			{
				if (_mFECAEResponse.Errors == null)
					return new ListClientResponse { Lista = new List<ListaObservaDetalle> { new ListaObservaDetalle { detalle = "No hay errores" } } };

				return new ListClientResponse
				{
					Cantidad = _mFECAEResponse.Errors.Length,
					Lista = _mFECAEResponse.Errors.Select((a) =>
					new ListaObservaDetalle
					{

						codigo = a.Code.ToString(),
						detalle = a.Msg

					}).ToList<ListaObservaDetalle>()
				};
			}
		}
		public ListClientResponse FEDetEven
		{
			get
			{
				if (_mFECAEResponse.Events == null)
					return new ListClientResponse { Lista = new List<ListaObservaDetalle> { new ListaObservaDetalle { detalle = "No hay Eventos" } } };

				return new ListClientResponse
				{
					Cantidad = _mFECAEResponse.Events.Length,
					Lista = _mFECAEResponse.Events.Select((x) =>
					new ListaObservaDetalle
					{
						codigo = x.Code.ToString(),
						detalle = x.Msg
					}).ToList<ListaObservaDetalle>()
				};
			}
		}

		//Ultimo comprobante emitido
		public int UltCompPtoVta { get => _mLastCbteResponse.PtoVta; }
		public int UltCompCbteTipoField { get => _mLastCbteResponse.CbteTipo; }
		public int UltCompCbteNroField { get => _mLastCbteResponse.CbteNro; }
		public ListClientResponse UltCompErrors
		{
			get
			{
				if (_mLastCbteResponse.Errors == null)
					return new ListClientResponse { Lista = new List<ListaObservaDetalle> { new ListaObservaDetalle { detalle = "No hay errores" } } };

				return new ListClientResponse
				{
					Cantidad = _mLastCbteResponse.Errors.Length,
					Lista = _mLastCbteResponse.Errors.Select((a) =>
					new ListaObservaDetalle
					{

						codigo = a.Code.ToString(),
						detalle = a.Msg

					}).ToList<ListaObservaDetalle>()
				};
			}
		}

		//Ticket StackTrace
		public string? TicketStackTrace { get; protected set; }

	}
}

