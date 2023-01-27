using AFIP_API.Models;
using ServiceFEEAFIP;
using AFIP_API.Services.Authentication;
using AFIP_API.Tools.Ticket;
using Newtonsoft.Json;
using CVM_WsAfip.Tools;

namespace AFIP_API.Controller
{
	public class FEEController : PropertiesFEE
	{
		TicketResult _ticketResult;
		private FEERequest _mFEERequestModel;
		private ServiceSoapClient _mServiceSoap;
		private ClsFEXAuthRequest _mClsFEXAuthRequest;
		private ClsFEXRequest _mClsFEXRequest;
		private Ticket _mTicket;
		public FEEController()
		{
			_mClsFEXRequest = new ClsFEXRequest();
			_mTicket = new Ticket();
			_mFEERequestModel = new FEERequest()
			{
				Opcionales = new List<Models.Opcional>(),
				Permisos = new List<Models.Permiso>(),
				Cmps_asoc = new List<Models.Cmp_asoc>(),
				Items = new List<Models.Item>()
			};
			_mServiceSoap = new ServiceSoapClient(0);
		}

		public void CreateFEE(FEERequest _pFEERequest) =>
			_mFEERequestModel = _pFEERequest;

		//Overload
		public string CreateFEE(long pId, string pFecha_cbte, short pCbte_Tipo, int pPunto_vta, long pCbte_nro, short pTipo_expo, string pPermiso_existente, short pDst_cmp, string pCliente, long pCuit_pais_cliente, string pDomicilio_cliente, string pId_impositivo, string pMoneda_Id, decimal pMoneda_ctz, string pObs_comerciales, decimal pImp_total, string pObs, string pForma_pago, string pIncoterms, string pIncoterms_Ds, short pIdioma_cbte, string pFecha_pago)
		{
			if (pMoneda_Id == "PES" && pMoneda_ctz == 0)
				pMoneda_ctz = 1;

			if (pMoneda_Id != "PES" && pMoneda_ctz == 0)
				return "Si la moneda no es pesos, se requiere Cotizacion";

			_mFEERequestModel.Id = pId;
			_mFEERequestModel.Fecha_cbte = pFecha_cbte;
			_mFEERequestModel.Cbte_Tipo = pCbte_Tipo;
			_mFEERequestModel.Punto_vta = pPunto_vta;
			_mFEERequestModel.Cbte_nro = pCbte_nro;
			_mFEERequestModel.Tipo_expo = pTipo_expo;
			_mFEERequestModel.Permiso_existente = pPermiso_existente;
			_mFEERequestModel.Dst_cmp = pDst_cmp;
			_mFEERequestModel.Cliente = pCliente;
			_mFEERequestModel.Cuit_pais_cliente = pCuit_pais_cliente;
			_mFEERequestModel.Domicilio_cliente = pDomicilio_cliente;
			_mFEERequestModel.Id_impositivo = pId_impositivo;
			_mFEERequestModel.Moneda_Id = pMoneda_Id;
			_mFEERequestModel.Moneda_ctz = pMoneda_ctz;
			_mFEERequestModel.Obs_comerciales = pObs_comerciales;
			_mFEERequestModel.Imp_total = pImp_total;
			_mFEERequestModel.Obs = pObs;
			_mFEERequestModel.Forma_pago = pForma_pago;
			_mFEERequestModel.Incoterms = pIncoterms;
			_mFEERequestModel.Incoterms_Ds = pIncoterms_Ds;
			_mFEERequestModel.Idioma_cbte = pIdioma_cbte;
			_mFEERequestModel.Fecha_pago = pFecha_pago;

			return "OK";
		}
		public string AddItems(string pPro_codigo, string pPro_ds, decimal pPro_qty, int pPro_umed, decimal pPro_precio_uni, decimal pPro_bonificacion, decimal pPro_total_item)
		{
			_mFEERequestModel.Items.Add(new Models.Item(pPro_codigo, pPro_ds, pPro_qty, pPro_umed, pPro_precio_uni, pPro_bonificacion, pPro_total_item));
			return "OK";
		}
		public string AddCmp_asoc(short pCbte_tipo, int pCbte_punto_vta, long pCbte_nro, long pCbte_cuit)
		{
			_mFEERequestModel.Cmps_asoc.Add(new Models.Cmp_asoc(pCbte_tipo, pCbte_punto_vta, pCbte_nro, pCbte_cuit));
			return "OK";
		}
		public string AddPermiso(string pId_permiso, int pDst_merc)
		{
			_mFEERequestModel.Permisos.Add(new Models.Permiso(pId_permiso, pDst_merc));
			return "OK";
		}

		//Auth
		public async Task<string> AuthenticationWithPFX(string pCuit, string pCertPfxPath, string pTicketFolder, int expiresIn = 12, string pTicketName = "TicketRequest")
		{
			string lstr_res = "OK";

			try
			{
				_ticketResult = await new AfipConnection().CreateTicketWithPFX("FEEController", pTicketName, pTicketFolder, pCuit, pCertPfxPath, "wsfe", expiresIn);

				if (_ticketResult.result == "OK")
				{
					if (!_mTicket.EqualCuitTicket(_ticketResult.path, pCuit))
					{
						lstr_res = "El ticket enviado por parametros no es el mismo que el del certificado autorizado";
					}

					_mClsFEXAuthRequest = new ClsFEXAuthRequest()
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
		public async Task<string> AuthenticationWithCrtAndPk(string pCuit, string pCertFolder, string pCrtName, string pPrivKeyName, string pTicketFolder, int expiresIn = 12, string pTicketName = "TicketRequest")
		{
			string lstr_res = "OK";

			try
			{
				_ticketResult = await new AfipConnection().CreateTicketWithCrtAndKey("ControllerFE", pTicketName, pTicketFolder, pCertFolder + "/" + pPrivKeyName, pCertFolder + "/" + pCrtName, pCertFolder, pCuit, "wsfex", expiresIn);

				if (_ticketResult.result == "OK")
				{
					if (!_mTicket.EqualCuitTicket(_ticketResult.path, pCuit))
						lstr_res = "El ticket enviado por parametros no es el mismo que el del certificado autorizado";

					_mClsFEXAuthRequest = new ClsFEXAuthRequest()
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
				lstr_res = $"{ex.Message}" + " \n Error in AuthenticationWithCrtAndPk" + "\n StackTrace: " + ex.StackTrace;
			}
			return lstr_res;
		}
		public async Task<string> Autorizar()
		{
			string lstr_res = "OK";

			try
			{
				_mClsFEXRequest = JsonConvert.DeserializeObject<ClsFEXRequest>(JsonConvert.SerializeObject(_mFEERequestModel));

				_mFEXResponseAuthorize = await _mServiceSoap.FEXAuthorizeAsync(_mClsFEXAuthRequest, _mClsFEXRequest);
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in Autorizar" + "\n StackTrace: " + ex.StackTrace;
			}
			return lstr_res;
		}

		//Functions
		public string GetXmlRequest(string pXmlPropertie = "") => Xml.Serialize(_mFEERequestModel, pXmlPropertie);
		public string GetXmlResponse(string pXmlPropertie = "") => Xml.Serialize(_mFEXResponseAuthorize, pXmlPropertie);
		public string GetXmlAuth(string pXmlPropertie = "") => Xml.Serialize(_mClsFEXAuthRequest, pXmlPropertie);
		public string GetXmlResponseUltimoCoomprobante(string pXmlPropertie = "") =>
			Xml.Serialize(_mFEXResponseLast_CMP, pXmlPropertie);
		public async Task<string> GetLastFEXCompUltAut(int pPtoVta, short pCbteTipo)
		{
			string lstr_res = "OK";
			try
			{
				_mFEXResponseLast_CMP = await _mServiceSoap.FEXGetLast_CMPAsync(new ClsFEX_LastCMP()
				{
					Cbte_Tipo = pCbteTipo,
					Pto_venta = pPtoVta,
					Token = _mClsFEXAuthRequest.Token,
					Sign = _mClsFEXAuthRequest.Sign,
					Cuit = _mClsFEXAuthRequest.Cuit
				});
			}
			catch (Exception ex)
			{
				lstr_res = $"{ex.Message}" + " \n Error in GetLastFEXCompUltAut" + "\n StackTrace: " + ex.StackTrace;
			}

			return lstr_res;
		}

	}
	public class PropertiesFEE
	{
		protected FEXResponseAuthorize _mFEXResponseAuthorize;
		protected FEXResponseLast_CMP? _mFEXResponseLast_CMP;
		public long Id { get => _mFEXResponseAuthorize.FEXResultAuth == null ? 0 : _mFEXResponseAuthorize.FEXResultAuth.Id; }

		public long Cuit { get => _mFEXResponseAuthorize.FEXResultAuth == null ? 0 : _mFEXResponseAuthorize.FEXResultAuth.Cuit; }

		public short Cbte_tipo { get => (short)(_mFEXResponseAuthorize.FEXResultAuth == null ? 0 : _mFEXResponseAuthorize.FEXResultAuth.Cbte_tipo); }

		public int Punto_vta { get => _mFEXResponseAuthorize.FEXResultAuth == null ? 0 : _mFEXResponseAuthorize.FEXResultAuth.Punto_vta; }

		public long Cbte_nro { get => _mFEXResponseAuthorize.FEXResultAuth == null ? 0 : _mFEXResponseAuthorize.FEXResultAuth.Cbte_nro; }

		public string Cae { get => _mFEXResponseAuthorize.FEXResultAuth == null ? "" : _mFEXResponseAuthorize.FEXResultAuth.Cae; }

		public string Fch_venc_Cae { get => _mFEXResponseAuthorize.FEXResultAuth == null ? "" : _mFEXResponseAuthorize.FEXResultAuth.Fch_venc_Cae; }

		public string Fch_cbte { get => _mFEXResponseAuthorize.FEXResultAuth == null ? "" : _mFEXResponseAuthorize.FEXResultAuth.Fch_cbte; }

		public string Resultado { get => _mFEXResponseAuthorize.FEXResultAuth == null ? "" : _mFEXResponseAuthorize.FEXResultAuth.Resultado; }

		public string Reproceso { get => _mFEXResponseAuthorize.FEXResultAuth == null ? "" : _mFEXResponseAuthorize.FEXResultAuth.Reproceso; }

		public string Motivos_Obs { get => _mFEXResponseAuthorize.FEXResultAuth == null ? "" : _mFEXResponseAuthorize.FEXResultAuth.Motivos_Obs; }

		public int ErrCode { get => _mFEXResponseAuthorize.FEXResultAuth == null && _mFEXResponseAuthorize.FEXErr == null ? 0 : _mFEXResponseAuthorize.FEXErr.ErrCode; }

		public string ErrMsg { get => _mFEXResponseAuthorize.FEXResultAuth == null && _mFEXResponseAuthorize.FEXErr == null ? "" : _mFEXResponseAuthorize.FEXErr.ErrMsg; }

		public int EventCode { get => _mFEXResponseAuthorize.FEXResultAuth == null && _mFEXResponseAuthorize.FEXEvents == null ? 0 : _mFEXResponseAuthorize.FEXEvents.EventCode; }

		public string EventMsg { get => _mFEXResponseAuthorize.FEXResultAuth == null && _mFEXResponseAuthorize.FEXEvents == null ? "" : _mFEXResponseAuthorize.FEXEvents.EventMsg; }


		//Ultimo comprobante
		public string UltCompPtoVta { get => _mFEXResponseLast_CMP.FEXResult_LastCMP == null || _mFEXResponseLast_CMP.FEXResult_LastCMP.Cbte_fecha == null ? "" : _mFEXResponseLast_CMP.FEXResult_LastCMP.Cbte_fecha; }
		public long UltCompCbteNroField { get => _mFEXResponseLast_CMP.FEXResult_LastCMP == null || _mFEXResponseLast_CMP.FEXResult_LastCMP.Cbte_nro == null ? 0 : _mFEXResponseLast_CMP.FEXResult_LastCMP.Cbte_nro; }
		public ListClientResponse UltCompErrors
		{
			get
			{
				if (_mFEXResponseLast_CMP.FEXErr == null)
					return new ListClientResponse { Lista = new List<ListaObservaDetalle> { new ListaObservaDetalle { detalle = "No hay errores" } } };

				return new ListClientResponse
				{
					Lista = new List<ListaObservaDetalle>
					{
						new ListaObservaDetalle
						{
							detalle = _mFEXResponseLast_CMP.FEXErr.ErrMsg,
							codigo = _mFEXResponseLast_CMP.FEXErr.ErrCode.ToString()
						}
					},
				};

			}
		}
		public ListClientResponse UltCompEvents
		{
			get
			{
				if (_mFEXResponseLast_CMP.FEXEvents == null)
					return new ListClientResponse() { Lista = new List<ListaObservaDetalle> { new ListaObservaDetalle { detalle = "No hay eventos" } } };

				return new ListClientResponse
				{
					Lista = new List<ListaObservaDetalle>
					{
						new ListaObservaDetalle
						{
							codigo = _mFEXResponseLast_CMP.FEXEvents.EventCode.ToString(),
							detalle = _mFEXResponseLast_CMP.FEXEvents.EventMsg
						}
					}
				};
			}
		}


		//Ticket StackTrace
		public string TicketStackTrace { get; protected set; }

	}
}

