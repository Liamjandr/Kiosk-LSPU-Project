namespace kiosk
{
    partial class InventoryModal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryModal));
            this.exit = new Guna.UI2.WinForms.Guna2PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.stockCount = new Guna.UI2.WinForms.Guna2TextBox();
            this.modaltext = new Guna.UI2.WinForms.Guna2TextBox();
            this.updateModal = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.exit)).BeginInit();
            this.SuspendLayout();
            // 
            // exit
            // 
            this.exit.BackColor = System.Drawing.Color.Transparent;
            this.exit.Image = ((System.Drawing.Image)(resources.GetObject("exit.Image")));
            this.exit.ImageRotate = 0F;
            this.exit.Location = new System.Drawing.Point(571, 19);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(39, 44);
            this.exit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exit.TabIndex = 0;
            this.exit.TabStop = false;
            this.exit.UseTransparentBackground = true;
            this.exit.Click += new System.EventHandler(this.guna2PictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Add Inventory";
            // 
            // stockCount
            // 
            this.stockCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.stockCount.BorderColor = System.Drawing.Color.Empty;
            this.stockCount.BorderThickness = 0;
            this.stockCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.stockCount.DefaultText = "a";
            this.stockCount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.stockCount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.stockCount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.stockCount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.stockCount.FillColor = System.Drawing.SystemColors.ActiveCaption;
            this.stockCount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.stockCount.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stockCount.ForeColor = System.Drawing.Color.Transparent;
            this.stockCount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.stockCount.Location = new System.Drawing.Point(70, 145);
            this.stockCount.Margin = new System.Windows.Forms.Padding(6);
            this.stockCount.Name = "stockCount";
            this.stockCount.PlaceholderForeColor = System.Drawing.Color.Transparent;
            this.stockCount.PlaceholderText = "";
            this.stockCount.SelectedText = "";
            this.stockCount.ShadowDecoration.Color = System.Drawing.Color.Transparent;
            this.stockCount.Size = new System.Drawing.Size(59, 34);
            this.stockCount.TabIndex = 9;
            // 
            // modaltext
            // 
            this.modaltext.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.modaltext.DefaultText = "";
            this.modaltext.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.modaltext.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.modaltext.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.modaltext.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.modaltext.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.modaltext.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.modaltext.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.modaltext.Location = new System.Drawing.Point(168, 145);
            this.modaltext.Name = "modaltext";
            this.modaltext.PlaceholderText = "";
            this.modaltext.SelectedText = "";
            this.modaltext.Size = new System.Drawing.Size(210, 88);
            this.modaltext.TabIndex = 10;
            this.modaltext.TextChanged += new System.EventHandler(this.modaltext_TextChanged);
            // 
            // updateModal
            // 
            this.updateModal.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.updateModal.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.updateModal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.updateModal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.updateModal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.updateModal.ForeColor = System.Drawing.Color.White;
            this.updateModal.Location = new System.Drawing.Point(416, 149);
            this.updateModal.Name = "updateModal";
            this.updateModal.Size = new System.Drawing.Size(155, 62);
            this.updateModal.TabIndex = 11;
            this.updateModal.Text = "update";
            this.updateModal.Click += new System.EventHandler(this.updateModal_Click);
            // 
            // InventoryModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.updateModal);
            this.Controls.Add(this.modaltext);
            this.Controls.Add(this.stockCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exit);
            this.Name = "InventoryModal";
            this.Size = new System.Drawing.Size(632, 632);
            ((System.ComponentModel.ISupportInitialize)(this.exit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox exit;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox stockCount;
        private Guna.UI2.WinForms.Guna2TextBox modaltext;
        private Guna.UI2.WinForms.Guna2Button updateModal;
    }
}
