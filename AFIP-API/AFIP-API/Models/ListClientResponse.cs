namespace AFIP_API.Models
{
    public class ListClientResponse
    {
        private int mint_cantidad = 0;
        private List<ListaObservaDetalle> mlst_detalle = new List<ListaObservaDetalle>();

        public int Cantidad
        {
            set
            {
                mint_cantidad = value;
            }
            get
            {
                return mint_cantidad;
            }
        }

        public List<ListaObservaDetalle> Lista
        {
            set
            {
                mlst_detalle = value;
            }
            get
            {
                return mlst_detalle;
            }
        }
    }

    public class ListaObservaDetalle
    {
        private string mstr_codigo = "";
        private string mstr_detalle = "";

        public string codigo
        {
            set
            {
                mstr_codigo = value;
            }
            get
            {
                return mstr_codigo;
            }
        }

        public string detalle
        {
            set
            {
                mstr_detalle = value;
            }
            get
            {
                return mstr_detalle;
            }
        }
    }
}
