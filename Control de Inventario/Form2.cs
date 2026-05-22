using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_de_Inventario
{
    
    public partial class Form2 : Form
    {
        private List<Movimiento> movimientos;

        public Form2(List<Movimiento> listaMovimientos)
        {
            InitializeComponent();
            movimientos = listaMovimientos;
            InicializarGrid();
            CargarMovimientos();
        }

        private void InicializarGrid()
        {
            dgvMovimientos.Columns.Clear();
            dgvMovimientos.Columns.Add("Producto", "Producto");
            dgvMovimientos.Columns.Add("Cantidad", "Cantidad");
            dgvMovimientos.Columns.Add("Tipo", "Tipo");
            dgvMovimientos.Columns.Add("Fecha", "Fecha");

            dgvMovimientos.ReadOnly = true;
            dgvMovimientos.AllowUserToAddRows = false;
            dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 🔒 Bloqueos
            dgvMovimientos.AllowUserToResizeColumns = false;
            dgvMovimientos.AllowUserToResizeRows = false;
            dgvMovimientos.AllowUserToOrderColumns = false;
            dgvMovimientos.RowHeadersVisible = false;
            dgvMovimientos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 🔒 Bloqueo individual por columna (más seguro)
            foreach (DataGridViewColumn col in dgvMovimientos.Columns)
            {
                col.Resizable = DataGridViewTriState.False;
            }

        }

        private void CargarMovimientos()
        {
            dgvMovimientos.Rows.Clear();

            foreach (var mov in movimientos.OrderByDescending(m => m.Fecha))
            {
                dgvMovimientos.Rows.Add(
                    mov.Producto,
                    mov.Cantidad,
                    mov.Tipo,
                    mov.Fecha.ToString("dd/MM/yyyy HH:mm:ss")
                );
            }
        }
    }
}

