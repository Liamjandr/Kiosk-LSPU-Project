namespace kiosk
{
    partial class paymentControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(paymentControl));
            this.guna2Shapes1 = new Guna.UI2.WinForms.Guna2Shapes();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.qrPictureBox = new Guna.UI2.WinForms.Guna2PictureBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.exitBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.qrPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBox)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Shapes1
            // 
            this.guna2Shapes1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Shapes1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2Shapes1.BorderThickness = 5;
            this.guna2Shapes1.FillColor = System.Drawing.Color.White;
            this.guna2Shapes1.Location = new System.Drawing.Point(0, 0);
            this.guna2Shapes1.Name = "guna2Shapes1";
            this.guna2Shapes1.PolygonSkip = 1;
            this.guna2Shapes1.Rotate = 0F;
            this.guna2Shapes1.Shape = Guna.UI2.WinForms.Enums.ShapeType.Rounded;
            this.guna2Shapes1.Size = new System.Drawing.Size(700, 605);
            this.guna2Shapes1.TabIndex = 20;
            this.guna2Shapes1.Text = "guna2Shapes1";
            this.guna2Shapes1.UseTransparentBackground = true;
            this.guna2Shapes1.Zoom = 100;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(162, 313);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(370, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.button1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(40, 524);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(626, 50);
            this.button1.TabIndex = 3;
            this.button1.Text = "Pay here";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // qrPictureBox
            // 
            this.qrPictureBox.ImageRotate = 0F;
            this.qrPictureBox.Location = new System.Drawing.Point(252, 86);
            this.qrPictureBox.Name = "qrPictureBox";
            this.qrPictureBox.Size = new System.Drawing.Size(196, 196);
            this.qrPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.qrPictureBox.TabIndex = 22;
            this.qrPictureBox.TabStop = false;
            this.qrPictureBox.WaitOnLoad = true;
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(334, 351);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(58, 13);
            this.resultLabel.TabIndex = 23;
            this.resultLabel.Text = "resultLabel";
            // 
            // exitBox
            // 
            this.exitBox.ErrorImage = null;
            this.exitBox.Image = ((System.Drawing.Image)(resources.GetObject("exitBox.Image")));
            this.exitBox.Location = new System.Drawing.Point(644, 18);
            this.exitBox.Name = "exitBox";
            this.exitBox.Size = new System.Drawing.Size(40, 41);
            this.exitBox.TabIndex = 24;
            this.exitBox.TabStop = false;
            this.exitBox.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // paymentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.exitBox);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.qrPictureBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.guna2Shapes1);
            this.Name = "paymentControl";
            this.Size = new System.Drawing.Size(700, 605);
            ((System.ComponentModel.ISupportInitialize)(this.qrPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Shapes guna2Shapes1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private Guna.UI2.WinForms.Guna2PictureBox qrPictureBox;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.PictureBox exitBox;
    }
}
