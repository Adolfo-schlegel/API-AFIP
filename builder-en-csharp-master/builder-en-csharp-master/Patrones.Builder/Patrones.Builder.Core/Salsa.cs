using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patrones.Builder.Core
{
    public abstract class Salsa
    {
        protected string _descripcion;
        public string Descripcion { get { return _descripcion; } }
    }


    public class Tomate : Salsa
    {
        public Tomate()
        {
            _descripcion = "Salsa de tomate clásica";
        }
    }

    public class Oliva : Salsa
    {
        public Oliva()
        {
            _descripcion = "Salsa de tomate a la oliva";
        }
    }

    public class Light : Salsa
    {
        public Light()
        {
            _descripcion = "Salsa sin condimentos ni sal";
        }
    }


    public class AjoAlOleo : Salsa
    {
        public AjoAlOleo()
        {
            _descripcion = "Salsa de ajo y aceite";
        }
    }

}
