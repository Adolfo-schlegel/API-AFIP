namespace Patrones.Builder.UI
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cboLineas = new System.Windows.Forms.ComboBox();
            this.btnConstruír = new System.Windows.Forms.Button();
            this.lstEntregas = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Seleccione línea de montaje";
            // 
            // cboLineas
            // 
            this.cboLineas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLineas.FormattingEnabled = true;
            this.cboLineas.Location = new System.Drawing.Point(12, 100);
            this.cboLineas.Name = "cboLineas";
            this.cboLineas.Size = new System.Drawing.Size(182, 24);
            this.cboLineas.TabIndex = 1;
            // 
            // btnConstruír
            // 
            this.btnConstruír.Location = new System.Drawing.Point(246, 96);
            this.btnConstruír.Name = "btnConstruír";
            this.btnConstruír.Size = new System.Drawing.Size(161, 30);
            this.btnConstruír.TabIndex = 2;
            this.btnConstruír.Text = "Constuír Pizza";
            this.btnConstruír.UseVisualStyleBackColor = true;
            this.btnConstruír.Click += new System.EventHandler(this.btnConstruír_Click);
            // 
            // lstEntregas
            // 
            this.lstEntregas.FormattingEnabled = true;
            this.lstEntregas.ItemHeight = 16;
            this.lstEntregas.Location = new System.Drawing.Point(12, 148);
            this.lstEntregas.Name = "lstEntregas";
            this.lstEntregas.Size = new System.Drawing.Size(962, 212);
            this.lstEntregas.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 450);
            this.Controls.Add(this.lstEntregas);
            this.Controls.Add(this.btnConstruír);
            this.Controls.Add(this.cboLineas);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Patrón Builder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboLineas;
        private System.Windows.Forms.Button btnConstruír;
        private System.Windows.Forms.ListBox lstEntregas;
    }
}

