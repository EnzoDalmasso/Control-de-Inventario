using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control_de_Inventario
{
    public class Movimiento
    {
        
            public string Producto { get; set; } = string.Empty;
            public int Cantidad { get; set; }
            public string Tipo { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
        

    }
}
