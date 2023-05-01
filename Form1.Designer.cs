namespace SnakeWF
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Puntuacion = new System.Windows.Forms.Label();
            this.TiempoRestante = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Puntuacion
            // 
            this.Puntuacion.AutoSize = true;
            this.Puntuacion.Location = new System.Drawing.Point(12, 9);
            this.Puntuacion.Name = "Puntuacion";
            this.Puntuacion.Size = new System.Drawing.Size(38, 16);
            this.Puntuacion.TabIndex = 0;
            this.Puntuacion.Text = "label1";
            // 
            // TiempoRestante
            // 
            this.TiempoRestante.AutoSize = true;
            this.TiempoRestante.Location = new System.Drawing.Point(12, 47);
            this.TiempoRestante.Name = "TiempoRestante";
            this.TiempoRestante.Size = new System.Drawing.Size(38, 16);
            this.TiempoRestante.TabIndex = 1;
            this.TiempoRestante.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TiempoRestante);
            this.Controls.Add(this.Puntuacion);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label Puntuacion;
        private Label TiempoRestante;
    }
}