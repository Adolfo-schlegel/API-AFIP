namespace AFIP_API.Models
{
    public class FECAECabRequest
    {
        public int? CantReg { get; set; }
        public int? PtoVta { get; set; }
        public int? CbteTipo { get; set; }
    }
    public class FECAEDetRequest
    {
		public int? Concepto { get; set; }
        public int? DocTipo { get; set; }
        public long? DocNro { get; set; }
        public long? CbteDesde { get; set; }
        public long? CbteHasta { get; set; }
        public string? CbteFch { get; set; }
        public double? ImpTotal { get; set; }
        public double? ImpTotConc { get; set; }
        public double? ImpNeto { get; set; }
        public double? ImpOpEx { get; set; }
        public double? ImpTrib { get; set; }
        public double? ImpIVA { get; set; }        
        public string? FchServDesde { get; set; }     
        public string? FchServHasta { get; set; }
        public string? FchVtoPago { get; set; }
        public string? MonId { get; set; }
        public double? MonCotiz { get; set; }
        public List<CbteAsoc>? CbtesAsoc { get; set; }
        public List<Tributo>? Tributos { get; set; }
        public List<AlicIva>? Iva { get; set; }        
        public List<Opcional>? Opcionales { get; set; }
        public bool ShouldSerializeFchServHasta() => FchServHasta.Any();
        public bool ShouldSerializeFchServDesde() => FchServDesde.Any();
        public bool ShouldSerializeCbtesAsoc() => CbtesAsoc.Any();
        public bool ShouldSerializeOpcionales() => Opcionales.Any();
        public bool ShouldSerializeTributos() => Tributos.Any();
    }
    public class CbteAsoc
	{
		public int? Tipo { get; set; }
        public int? PtoVta { get; set; }
        public long? Nro { get; set; }
        public string? Cuit { get; set; }
        public string? CbteFch { get; set; }
    }
    public class FeCAEReq
    {
        public FECAECabRequest? FeCabReq { get; set; }

        public List<FECAEDetRequest>? FeDetReq { get; set; }
    }
    public class AlicIva
    {
        public int? Id { get; set; }

        public double? BaseImp { get; set; }

        public double? Importe { get; set; }
    }
    public class Opcional
    {
        public string? Id { get; set; }

        public string? Valor { get; set; }
    }
    public class Tributo
	{
        public short? Id { get; set; }

		public string? Desc { get; set; }

        public double? BaseImp { get; set; }

        public double? Alic { get; set; }

        public double? Importe { get; set; }
    }
}
