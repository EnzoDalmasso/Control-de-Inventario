using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Control_de_Inventario
{
    public partial class Form1 : Form
    {
        private List<Producto> inventario = new();
        private List<Movimiento> movimientos = new();
        private readonly InventoryApiClient apiClient = new();

        private string archivoInventario = "inventario.json";
        private string archivoMovimientos = "movimientos.json";
        private bool mostrandoBajoStock = false;

        private PrintDocument printDocument = new PrintDocument();

        private IEnumerable<Producto> ObtenerInventarioFiltrado()
        {
            IEnumerable<Producto> lista = inventario;

            // Filtro buscador
            string filtro = txtBuscar.Text?.ToLower() ?? "";

            if (!string.IsNullOrWhiteSpace(filtro))
                lista = lista.Where(p => p.Nombre.ToLower().Contains(filtro));

            // Filtro bajo stock
            if (mostrandoBajoStock)
                lista = lista.Where(p => p.Stock < 5);

            return lista.OrderBy(p => p.Nombre);
        }


        public Button btnExcel = null!;
        public Button btnPDF = null!;


        public Form1()
        {
            InitializeComponent();

            CrearBotonesExportacion();


            InicializarDataGrid();
            lblTotal.Text = "Cargando inventario...";
            Shown += async (_, _) => await CargarDatosDesdeApiAsync();



            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void InicializarDataGrid()
        {
            dgvInventario.Columns.Clear();
            dgvInventario.Columns.Add("Nombre", "Producto");
            dgvInventario.Columns.Add("Stock", "Stock");
            dgvInventario.Columns.Add("Estado", "Estado");

            dgvInventario.ReadOnly = true;
            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.AllowUserToResizeColumns = false;
            dgvInventario.AllowUserToResizeRows = false;
            dgvInventario.AllowUserToOrderColumns = false;
            dgvInventario.RowHeadersVisible = false;
            dgvInventario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvInventario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventario.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvInventario.Columns["Nombre"].FillWeight = 60;
            dgvInventario.Columns["Stock"].FillWeight = 20;
            dgvInventario.Columns["Estado"].FillWeight = 20;

            dgvInventario.Columns["Nombre"].Resizable = DataGridViewTriState.False;
            dgvInventario.Columns["Stock"].Resizable = DataGridViewTriState.False;
            dgvInventario.Columns["Estado"].Resizable = DataGridViewTriState.False;
        }

        private void CargarInventario()
        {
            if (File.Exists(archivoInventario))
                inventario = JsonConvert.DeserializeObject<List<Producto>>
                    (File.ReadAllText(archivoInventario)) ?? new();
        }

        private void GuardarInventario()
        {
            inventario = inventario.OrderBy(p => p.Nombre).ToList();

            File.WriteAllText(archivoInventario,
                JsonConvert.SerializeObject(inventario, Formatting.Indented));
        }

        private void CargarMovimientos()
        {
            if (File.Exists(archivoMovimientos))
                movimientos = JsonConvert.DeserializeObject<List<Movimiento>>
                    (File.ReadAllText(archivoMovimientos)) ?? new();
        }

        private void GuardarMovimientos()
        {
            File.WriteAllText(archivoMovimientos,
                JsonConvert.SerializeObject(movimientos, Formatting.Indented));
        }

        private void ActualizarDataGrid()
        {
            dgvInventario.Rows.Clear();

            var datos = ObtenerInventarioFiltrado().ToList();

            foreach (var p in datos)
            {
                string estado = p.Stock == 0 ? "❌ SIN STOCK"
                                : p.Stock < 5 ? "⚠️ BAJO"
                                : "✅ OK";

                dgvInventario.Rows.Add(p.Nombre, p.Stock, estado);
            }

            lblTotal.Text = $"Productos: {datos.Count} | Unidades: {datos.Sum(p => p.Stock)}";
        }

        private async void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtProducto.Text.Trim();

            if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad) || cantidad < 0)
            {
                MessageBox.Show("Cantidad inválida");
                return;
            }

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Ingrese producto");
                return;
            }

            if (inventario.Any(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("El producto ya existe");
                return;
            }

            try
            {
                await apiClient.CrearProductoAsync(nombre, cantidad);
                await CargarDatosDesdeApiAsync();
                LimpiarCampos();
            }
            catch (Exception exception)
            {
                MostrarErrorApi(exception);
            }
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            await ModificarStockAsync("Ingreso");
        }

        private async void btnDescontar_Click(object sender, EventArgs e)
        {
            await ModificarStockAsync("Egreso");
        }

        private async Task ModificarStockAsync(string tipo)
        {
            string nombre = txtProducto.Text.Trim();

            var producto = inventario
                .FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            if (producto == null)
            {
                MessageBox.Show("Producto no existe");
                return;
            }

            if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Cantidad inválida");
                return;
            }

            if (tipo == "Egreso" && cantidad > producto.Stock)
            {
                MessageBox.Show("Stock insuficiente");
                return;
            }

            try
            {
                if (tipo == "Ingreso")
                    await apiClient.IngresarStockAsync(producto.Id, cantidad);
                else
                    await apiClient.DescontarStockAsync(producto.Id, cantidad);

                await CargarDatosDesdeApiAsync();
                LimpiarCampos();
            }
            catch (Exception exception)
            {
                MostrarErrorApi(exception);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            string nombre = txtProducto.Text.Trim();

            var producto = inventario
                .FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            if (producto == null)
            {
                MessageBox.Show("Producto no existe");
                return;
            }

            try
            {
                await apiClient.EliminarProductoAsync(producto.Id);
                await CargarDatosDesdeApiAsync();
                LimpiarCampos();
            }
            catch (Exception exception)
            {
                MostrarErrorApi(exception);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtProducto.Clear();
            txtCantidad.Clear();
            txtProducto.Focus();
        }

        private void dgvInventario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtProducto.Text = dgvInventario.Rows[e.RowIndex].Cells[0].Value?.ToString();
                txtCantidad.Text = dgvInventario.Rows[e.RowIndex].Cells[1].Value?.ToString();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            ActualizarDataGrid();
        }

        private void btnBajoStock_Click(object sender, EventArgs e)
        {
            mostrandoBajoStock = !mostrandoBajoStock;

            btnBajoStock.Text = mostrandoBajoStock ? "Mostrar Todos" : "Bajo Stock";

            ActualizarDataGrid();
        }
        

        private async void btnHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                movimientos = await apiClient.ObtenerMovimientosAsync();
                Form2 historial = new Form2(movimientos);
                historial.ShowDialog();
            }
            catch (Exception exception)
            {
                MostrarErrorApi(exception);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDocument;
            preview.ShowDialog();
        }

        private void PrintDocument_PrintPage(object? sender, PrintPageEventArgs e)
        {
            int y = 100;
            int x = 50;

            Font font = new Font("Arial", 11);

            e.Graphics!.DrawString("LISTADO DE INVENTARIO",
                new Font("Arial", 14, FontStyle.Bold),
                Brushes.Black, x, 50);

            var datos = ObtenerInventarioFiltrado().ToList();

            foreach (var p in datos)
            {
                string linea = $"{p.Nombre} - Stock: {p.Stock}";
                e.Graphics.DrawString(linea, font, Brushes.Black, x, y);
                y += 25;
            }
        }

        private void CrearBotonesExportacion()
        {
            btnPDF = new Button();
            btnPDF.Text = "Exportar PDF";
            btnPDF.Size = new Size(110, 32);
            btnPDF.Location = new Point(330, 434);
            btnPDF.BackColor = Color.RoyalBlue;
            btnPDF.ForeColor = Color.White;
            btnPDF.Click += btnPDF_Click;
            this.Controls.Add(btnPDF);

            btnExcel = new Button();
            btnExcel.Text = "Exportar Excel";
            btnExcel.Size = new Size(110, 32);
            btnExcel.Location = new Point(450, 434);
            btnExcel.BackColor = Color.ForestGreen;
            btnExcel.ForeColor = Color.White;
            btnExcel.Click += btnExcel_Click;
            this.Controls.Add(btnExcel);
        }

        private void ExportarExcel()
        {
            var datos = ObtenerInventarioFiltrado().ToList();

            if (!datos.Any())
            {
                MessageBox.Show("No hay datos para exportar.");
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Workbook|*.xlsx";
            sfd.FileName = "Stock.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Inventario");

                worksheet.Cell(1, 1).Value = "Producto";
                worksheet.Cell(1, 2).Value = "Stock";
                worksheet.Cell(1, 3).Value = "Estado";

                int fila = 2;

                foreach (var p in datos)
                {
                    string estado = p.Stock == 0 ? "SIN STOCK"
                                    : p.Stock < 5 ? "BAJO"
                                    : "OK";

                    worksheet.Cell(fila, 1).Value = p.Nombre;
                    worksheet.Cell(fila, 2).Value = p.Stock;
                    worksheet.Cell(fila, 3).Value = estado;
                    fila++;
                }

                workbook.SaveAs(sfd.FileName);

                MessageBox.Show("Excel exportado correctamente.");
            }
        }

        private void ExportarPDF()
        {
            var datos = ObtenerInventarioFiltrado().ToList();

            if (!datos.Any())
            {
                MessageBox.Show("No hay datos para exportar.");
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF|*.pdf";
            sfd.FileName = "Stock.pdf";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                PdfWriter writer = new PdfWriter(sfd.FileName);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("LISTADO DE INVENTARIO")
                    .SetFontSize(16));

                document.Add(new Paragraph(" "));

                Table table = new Table(3);

                table.AddHeaderCell("Producto");
                table.AddHeaderCell("Stock");
                table.AddHeaderCell("Estado");

                foreach (var p in datos)
                {
                    string estado = p.Stock == 0 ? "SIN STOCK"
                                    : p.Stock < 5 ? "BAJO"
                                    : "OK";

                    table.AddCell(p.Nombre);
                    table.AddCell(p.Stock.ToString());
                    table.AddCell(estado);
                }

                document.Add(table);
                document.Close();

                MessageBox.Show("PDF exportado correctamente.");
            }
        }

        private void btnExcel_Click(object? sender, EventArgs e)
        {
            ExportarExcel();
        }

        private void btnPDF_Click(object? sender, EventArgs e)
        {
            ExportarPDF();
        }

        private async Task CargarDatosDesdeApiAsync()
        {
            try
            {
                inventario = await apiClient.ObtenerProductosAsync();
                movimientos = await apiClient.ObtenerMovimientosAsync();
                ActualizarDataGrid();
            }
            catch (Exception exception)
            {
                lblTotal.Text = "No se pudo conectar con la API";
                MostrarErrorApi(exception);
            }
        }

        private static void MostrarErrorApi(Exception exception)
        {
            MessageBox.Show(
                $"No se pudo completar la operacion.\n\nDetalle: {exception.Message}\n\nVerifique que la API este ejecutandose en http://localhost:5071.",
                "Error de conexion con la API",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }


    }
}
