namespace Control_de_Inventario
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvMovimientos;
        private Label lblTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvMovimientos = new DataGridView();
            lblTitulo = new Label();

            SuspendLayout();

            // Label título
            lblTitulo.Text = "Historial de Movimientos";
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new System.Drawing.Point(20, 15);

            // DataGrid
            dgvMovimientos.Location = new System.Drawing.Point(20, 60);
            dgvMovimientos.Size = new System.Drawing.Size(740, 350);

            // Form2
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(lblTitulo);
            Controls.Add(dgvMovimientos);
            Text = "Historial";
            StartPosition = FormStartPosition.CenterParent;

            ResumeLayout(false);
            PerformLayout();
           
        }
    }
}