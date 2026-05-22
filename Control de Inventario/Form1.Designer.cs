using System.Drawing;
using System.Windows.Forms;

namespace Control_de_Inventario
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtProducto;
        private TextBox txtCantidad;
        private TextBox txtBuscar;

        private Button btnAgregar;
        private Button btnIngresar;
        private Button btnDescontar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private Button btnBajoStock;

        private DataGridView dgvInventario;
        private Label lblTotal;
        private Label lblBuscar;

        private Button btnHistorial;
        private Button btnImprimir;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            txtProducto = new TextBox();
            txtCantidad = new TextBox();
            txtBuscar = new TextBox();
            btnAgregar = new Button();
            btnIngresar = new Button();
            btnDescontar = new Button();
            btnEliminar = new Button();
            btnLimpiar = new Button();
            btnBajoStock = new Button();
            dgvInventario = new DataGridView();
            lblTotal = new Label();
            lblBuscar = new Label();
            btnHistorial = new Button();
            btnImprimir = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).BeginInit();
            SuspendLayout();
            // 
            // txtProducto
            // 
            txtProducto.Location = new Point(20, 20);
            txtProducto.Name = "txtProducto";
            txtProducto.Size = new Size(200, 27);
            txtProducto.TabIndex = 0;
            // 
            // txtCantidad
            // 
            txtCantidad.Location = new Point(230, 20);
            txtCantidad.Name = "txtCantidad";
            txtCantidad.Size = new Size(100, 27);
            txtCantidad.TabIndex = 1;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(83, 430);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(200, 27);
            txtBuscar.TabIndex = 10;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(20, 60);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(75, 32);
            btnAgregar.TabIndex = 2;
            btnAgregar.Text = "Agregar";
            btnAgregar.Click += btnAgregar_Click;
            // 
            // btnIngresar
            // 
            btnIngresar.Location = new Point(110, 60);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(75, 32);
            btnIngresar.TabIndex = 3;
            btnIngresar.Text = "Ingresar";
            btnIngresar.Click += btnIngresar_Click;
            // 
            // btnDescontar
            // 
            btnDescontar.Location = new Point(200, 60);
            btnDescontar.Name = "btnDescontar";
            btnDescontar.Size = new Size(104, 32);
            btnDescontar.TabIndex = 4;
            btnDescontar.Text = "Descontar";
            btnDescontar.Click += btnDescontar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(322, 58);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 32);
            btnEliminar.TabIndex = 5;
            btnEliminar.Text = "Eliminar";
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(350, 20);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(75, 32);
            btnLimpiar.TabIndex = 6;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnBajoStock
            // 
            btnBajoStock.Location = new Point(539, 60);
            btnBajoStock.Name = "btnBajoStock";
            btnBajoStock.Size = new Size(75, 32);
            btnBajoStock.TabIndex = 7;
            btnBajoStock.Text = "Bajo Stock";
            btnBajoStock.Click += btnBajoStock_Click;
            // 
            // dgvInventario
            // 
            dgvInventario.ColumnHeadersHeight = 29;
            dgvInventario.Location = new Point(20, 120);
            dgvInventario.Name = "dgvInventario";
            dgvInventario.RowHeadersWidth = 51;
            dgvInventario.Size = new Size(650, 300);
            dgvInventario.TabIndex = 8;
            dgvInventario.CellClick += dgvInventario_CellClick;
            // 
            // lblTotal
            // 
            lblTotal.Location = new Point(20, 480);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(600, 30);
            lblTotal.TabIndex = 11;
            // 
            // lblBuscar
            // 
            lblBuscar.Location = new Point(20, 440);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(57, 23);
            lblBuscar.TabIndex = 9;
            lblBuscar.Text = "Buscar:";
            // 
            // btnHistorial
            // 
            btnHistorial.Location = new Point(620, 60);
            btnHistorial.Name = "btnHistorial";
            btnHistorial.Size = new Size(75, 32);
            btnHistorial.TabIndex = 12;
            btnHistorial.Text = "Historial";
            btnHistorial.Click += btnHistorial_Click;
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(580, 434);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(90, 32);
            btnImprimir.TabIndex = 13;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(720, 530);
            Controls.Add(txtProducto);
            Controls.Add(txtCantidad);
            Controls.Add(btnAgregar);
            Controls.Add(btnIngresar);
            Controls.Add(btnDescontar);
            Controls.Add(btnEliminar);
            Controls.Add(btnLimpiar);
            Controls.Add(btnBajoStock);
            Controls.Add(dgvInventario);
            Controls.Add(lblBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(lblTotal);
            Controls.Add(btnHistorial);
            Controls.Add(btnImprimir);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Sistema de Inventario";
            ((System.ComponentModel.ISupportInitialize)dgvInventario).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
