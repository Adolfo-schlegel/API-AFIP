namespace AFIP_API.Models
{
	public class CAEResponse
	{
        public FECAECabResponse? FeCabResp {get; set;}
        
        public FECAEDetResponse[]? FeDetResp {get; set;}
        
        public Evt[]? Events {get; set;}
        
        public Err[]? Errors {get; set;}
	}

    public class FECAECabResponse : FECabResponse
	{
        public string? CAE {get; set;}

        public string? CAEFchVto {get; set;}
    }
    public class FECAEDetResponse : FEDetResponse
    {
        public string? CAE {get; set;}

        public string? CAEFchVto {get; set;}
    }

    public class FECabResponse
	{

        public long? Cuit{ get; set; }

        public int? PtoVta{get; set;}

        public int? CbteTipo{get; set;}

        public string? FchProceso{get; set;}

        public int? CantReg{get; set;}

        public string? Resultado{get; set;}

        public string? Reproceso{get; set;}
    }
    public class FEDetResponse
	{
        public int? Concepto  {get; set;}

        public int? DocTipo  {get; set;}

        public long? DocNro  {get; set;}

        public long? CbteDesde  {get; set;}

        public long? CbteHasta  {get; set;}

        public string? CbteFch  {get; set;}

        public string? Resultado  {get; set;}

        public Obs[]? Observaciones  {get; set;}
    }

    public class Obs
	{
        public int? Code { get; set; }
        public string? Msg { get; set; }
    }
    public class Evt
	{
        public int? Code {get; set;}

        public string? Msg {get; set;}
    }
    public class Err
	{
        public int? Code {get; set;}

        public string? Msg {get; set;}
    }

}
