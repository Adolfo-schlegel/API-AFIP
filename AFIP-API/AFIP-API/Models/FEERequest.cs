namespace AFIP_API.Models
{
	public class FEERequest
	{
		public long? Id { get; set; }
		public string? Fecha_cbte { get; set; }
		public short? Cbte_Tipo { get; set; }
		public int? Punto_vta { get; set; }
		public long? Cbte_nro { get; set; }
		public short? Tipo_expo { get; set; }
		public string? Permiso_existente { get; set; }
		public short? Dst_cmp { get; set; }
		public string? Cliente { get; set; }
		public long? Cuit_pais_cliente { get; set; }
		public string? Domicilio_cliente { get; set; }
		public string? Id_impositivo { get; set; }
		public string? Moneda_Id { get; set; }
		public decimal? Moneda_ctz { get; set; }
		public string? Obs_comerciales { get; set; }
		public decimal? Imp_total { get; set; }
		public string? Obs { get; set; }		
		public string? Forma_pago { get; set; }
		public string? Incoterms { get; set; }
		public string? Incoterms_Ds { get; set; }
		public short? Idioma_cbte { get; set; }
		public string? Fecha_pago { get; set; }
		public List<Item>? Items { get; set; }
		public List<Opcional>? Opcionales { get; set; }
		public List<Permiso>? Permisos { get; set; }
		public List<Cmp_asoc>? Cmps_asoc { get; set; }

		public bool ShouldSerializeCbtesAsoc() => Cmps_asoc.Any();
		public bool ShouldSerializeOpcionales() => Opcionales.Any();
		public bool ShouldSerializeTributos() => Items.Any();
	}
	public class Item
	{
		public Item() { }
		public Item(string pro_codigo, string pro_ds, decimal pro_qty, int pro_umed, decimal pro_precio_uni, decimal pro_bonificacion, decimal pro_total_item)
		{
			this.pro_codigo = pro_codigo;
			this.pro_ds = pro_ds;
			this.pro_qty = pro_qty;
			this.pro_umed = pro_umed;
			this.pro_precio_uni = pro_precio_uni;
			this.pro_bonificacion = pro_bonificacion;
			this.pro_total_item = pro_total_item;
		}

		public string? pro_codigo { get; set; }
		public string? pro_ds { get; set; }
		public decimal? pro_qty { get; set; }
		public int? pro_umed { get; set; }
		public decimal? pro_precio_uni { get; set; }
		public decimal? pro_bonificacion { get; set; }
		public decimal? pro_total_item { get; set; }
	}
	public class Cmp_asoc
	{
		public Cmp_asoc() { }
		public Cmp_asoc(short cbte_tipo, int cbte_punto_vta, long cbte_nro, long cbte_cuit)
		{
			this.cbte_tipo = cbte_tipo;
			this.cbte_punto_vta = cbte_punto_vta;
			this.cbte_nro = cbte_nro;
			this.cbte_cuit = cbte_cuit;
		}

		public short? cbte_tipo { get; set; }
		public int? cbte_punto_vta { get; set; }
		public long? cbte_nro { get; set; }
		public long? cbte_cuit { get; set; }
	}
    public class Permiso
	{
		public Permiso() { }
				
		public Permiso(string id_permiso, int dst_merc)
		{
			this.id_permiso = id_permiso;
			this.dst_merc = dst_merc;
		}

		public string? id_permiso { get; set; }
		public int? dst_merc { get; set; }
	}
}
