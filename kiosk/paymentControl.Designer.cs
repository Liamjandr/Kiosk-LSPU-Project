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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.qrPictureBox = new Guna.UI2.WinForms.Guna2PictureBox();
            this.exitBox = new System.Windows.Forms.PictureBox();
            this.print = new System.Windows.Forms.Button();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.guna2Shapes1 = new Guna.UI2.WinForms.Guna2Shapes();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.receiptID = new System.Windows.Forms.Label();
            this.itemNames = new System.Windows.Forms.TextBox();
            this.itemQTY = new System.Windows.Forms.TextBox();
            this.itemCosts = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.Subtotal = new System.Windows.Forms.Label();
            this.discount = new System.Windows.Forms.Label();
            this.total = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.qrPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Arial", 12F);
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBox1.Location = new System.Drawing.Point(571, 557);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(290, 27);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "lorem";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.button1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(50, 617);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(383, 50);
            this.button1.TabIndex = 3;
            this.button1.Text = "Link Here";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // qrPictureBox
            // 
            this.qrPictureBox.ImageRotate = 0F;
            this.qrPictureBox.Location = new System.Drawing.Point(112, 88);
            this.qrPictureBox.Name = "qrPictureBox";
            this.qrPictureBox.Size = new System.Drawing.Size(258, 248);
            this.qrPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.qrPictureBox.TabIndex = 22;
            this.qrPictureBox.TabStop = false;
            this.qrPictureBox.WaitOnLoad = true;
            // 
            // exitBox
            // 
            this.exitBox.ErrorImage = null;
            this.exitBox.Image = ((System.Drawing.Image)(resources.GetObject("exitBox.Image")));
            this.exitBox.Location = new System.Drawing.Point(882, 12);
            this.exitBox.Name = "exitBox";
            this.exitBox.Size = new System.Drawing.Size(30, 33);
            this.exitBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitBox.TabIndex = 24;
            this.exitBox.TabStop = false;
            this.exitBox.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // print
            // 
            this.print.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.print.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.print.ForeColor = System.Drawing.SystemColors.Control;
            this.print.Location = new System.Drawing.Point(502, 626);
            this.print.Name = "print";
            this.print.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.print.Size = new System.Drawing.Size(383, 50);
            this.print.TabIndex = 25;
            this.print.Text = "Print Receipt";
            this.print.UseVisualStyleBackColor = false;
            this.print.Click += new System.EventHandler(this.print_Click);
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox2.Image")));
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(0, 0);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(932, 717);
            this.guna2PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox2.TabIndex = 27;
            this.guna2PictureBox2.TabStop = false;
            this.guna2PictureBox2.UseTransparentBackground = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 22);
            this.label1.TabIndex = 28;
            this.label1.Text = "Scan QR Code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 596);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 18);
            this.label2.TabIndex = 29;
            this.label2.Text = "Or pay at the website link:";
            // 
            // guna2Shapes1
            // 
            this.guna2Shapes1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Shapes1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.guna2Shapes1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.guna2Shapes1.Location = new System.Drawing.Point(502, 409);
            this.guna2Shapes1.Name = "guna2Shapes1";
            this.guna2Shapes1.PolygonSkip = 1;
            this.guna2Shapes1.Rotate = 0F;
            this.guna2Shapes1.RoundedRadius = 12;
            this.guna2Shapes1.Shape = Guna.UI2.WinForms.Enums.ShapeType.Rounded;
            this.guna2Shapes1.Size = new System.Drawing.Size(383, 194);
            this.guna2Shapes1.TabIndex = 30;
            this.guna2Shapes1.Text = "guna2Shapes1";
            this.guna2Shapes1.UseTransparentBackground = true;
            this.guna2Shapes1.Zoom = 100;
            this.guna2Shapes1.Click += new System.EventHandler(this.guna2Shapes1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.label3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(510, 419);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 22);
            this.label3.TabIndex = 33;
            this.label3.Text = "Payment Reminder";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(511, 557);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 19);
            this.label4.TabIndex = 34;
            this.label4.Text = "Status:";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Arial", 12F);
            this.textBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBox2.Location = new System.Drawing.Point(514, 453);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(325, 87);
            this.textBox2.TabIndex = 35;
            this.textBox2.Text = "- Check payment details carefully\r\n- Take a picture for your proof of payment\r\n- " +
    "Contact support if payment is not reflected on the application\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(47, 379);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 18);
            this.label5.TabIndex = 36;
            this.label5.Text = "How to Pay:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(81, 409);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 18);
            this.label6.TabIndex = 37;
            this.label6.Text = "Scan the QR Code";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(81, 557);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 18);
            this.label10.TabIndex = 41;
            this.label10.Text = "Print the receipt";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(81, 522);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(274, 18);
            this.label9.TabIndex = 40;
            this.label9.Text = "Review the payment details and cofirm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(81, 486);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(187, 18);
            this.label8.TabIndex = 39;
            this.label8.Text = "Enter required credentials";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(81, 451);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(237, 18);
            this.label7.TabIndex = 38;
            this.label7.Text = "Enter preferred mode of payment";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(498, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(158, 22);
            this.label11.TabIndex = 42;
            this.label11.Text = "Order Summary";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(499, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(138, 19);
            this.label12.TabIndex = 43;
            this.label12.Text = "Receipt Number:";
            // 
            // receiptID
            // 
            this.receiptID.AutoSize = true;
            this.receiptID.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.receiptID.Location = new System.Drawing.Point(499, 115);
            this.receiptID.Name = "receiptID";
            this.receiptID.Size = new System.Drawing.Size(107, 18);
            this.receiptID.TabIndex = 44;
            this.receiptID.Text = "Order Number";
            // 
            // itemNames
            // 
            this.itemNames.BackColor = System.Drawing.Color.White;
            this.itemNames.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.itemNames.Font = new System.Drawing.Font("Arial", 12F);
            this.itemNames.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.itemNames.Location = new System.Drawing.Point(503, 158);
            this.itemNames.Multiline = true;
            this.itemNames.Name = "itemNames";
            this.itemNames.Size = new System.Drawing.Size(201, 99);
            this.itemNames.TabIndex = 45;
            // 
            // itemQTY
            // 
            this.itemQTY.BackColor = System.Drawing.Color.White;
            this.itemQTY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.itemQTY.Font = new System.Drawing.Font("Arial", 12F);
            this.itemQTY.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.itemQTY.Location = new System.Drawing.Point(711, 158);
            this.itemQTY.Multiline = true;
            this.itemQTY.Name = "itemQTY";
            this.itemQTY.Size = new System.Drawing.Size(62, 99);
            this.itemQTY.TabIndex = 46;
            // 
            // itemCosts
            // 
            this.itemCosts.BackColor = System.Drawing.Color.White;
            this.itemCosts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.itemCosts.Font = new System.Drawing.Font("Arial", 12F);
            this.itemCosts.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.itemCosts.Location = new System.Drawing.Point(789, 158);
            this.itemCosts.Multiline = true;
            this.itemCosts.Name = "itemCosts";
            this.itemCosts.Size = new System.Drawing.Size(96, 99);
            this.itemCosts.TabIndex = 47;
            this.itemCosts.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(499, 276);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 19);
            this.label13.TabIndex = 48;
            this.label13.Text = "Subtotal";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(499, 307);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 19);
            this.label14.TabIndex = 49;
            this.label14.Text = "Discount";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(499, 357);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(46, 19);
            this.label15.TabIndex = 50;
            this.label15.Text = "Total";
            // 
            // Subtotal
            // 
            this.Subtotal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Subtotal.AutoSize = true;
            this.Subtotal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Subtotal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Subtotal.Location = new System.Drawing.Point(790, 277);
            this.Subtotal.Name = "Subtotal";
            this.Subtotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Subtotal.Size = new System.Drawing.Size(98, 18);
            this.Subtotal.TabIndex = 51;
            this.Subtotal.Text = "0000000000";
            this.Subtotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // discount
            // 
            this.discount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.discount.AutoSize = true;
            this.discount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.discount.Location = new System.Drawing.Point(798, 307);
            this.discount.Name = "discount";
            this.discount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.discount.Size = new System.Drawing.Size(89, 18);
            this.discount.TabIndex = 52;
            this.discount.Text = "000000000";
            this.discount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // total
            // 
            this.total.AutoSize = true;
            this.total.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.total.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.total.Location = new System.Drawing.Point(798, 358);
            this.total.Name = "total";
            this.total.Size = new System.Drawing.Size(89, 18);
            this.total.TabIndex = 53;
            this.total.Text = "000000000";
            this.total.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // paymentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.total);
            this.Controls.Add(this.discount);
            this.Controls.Add(this.Subtotal);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.itemCosts);
            this.Controls.Add(this.itemQTY);
            this.Controls.Add(this.itemNames);
            this.Controls.Add(this.receiptID);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.guna2Shapes1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exitBox);
            this.Controls.Add(this.qrPictureBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.print);
            this.Controls.Add(this.guna2PictureBox2);
            this.Name = "paymentControl";
            this.Size = new System.Drawing.Size(932, 717);
            ((System.ComponentModel.ISupportInitialize)(this.qrPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private Guna.UI2.WinForms.Guna2PictureBox qrPictureBox;
        private System.Windows.Forms.PictureBox exitBox;
        private System.Windows.Forms.Button print;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Shapes guna2Shapes1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label receiptID;
        private System.Windows.Forms.TextBox itemNames;
        private System.Windows.Forms.TextBox itemQTY;
        private System.Windows.Forms.TextBox itemCosts;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label Subtotal;
        private System.Windows.Forms.Label discount;
        private System.Windows.Forms.Label total;
    }
}
