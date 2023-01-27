using AFIP_API.Models;

namespace AFIP_API.Services.Facturacion.Interface
{
	public interface IFacturaBuilder
	{
		IFacturaBuilder CreateFactura();
		IFacturaBuilder AddIva(List<AlicIva> lst_alicIva);
		IFacturaBuilder AddOpcional(List<Opcional> lst_opcional);
		IFacturaBuilder AddItem(List<Item> lst_item);
		IFacturaBuilder AddPermiso(List<Permiso> lst_permiso);
		IFacturaBuilder AddCompAsociado(List<CbteAsoc> lst_cbteAsoc);
		IFacturaBuilder AddTributo(List<Tributo> lst_tributo);
		IFacturaBuilder WithAuthPfx(string Cuit, string CertPfxPath, string TicketFolder, int TicketExpirationTime, string TicketName);
		IFacturaBuilder WithAuthCertAndKey(string Cuit, string CertFolder, string CrtName, string PrivKeyName, string TicketFolder, int TicketExpirationTime , string TicketName);
		IFacturaBuilder Build();
	}
}
