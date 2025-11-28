namespace kiosk
{
    partial class AddInventory
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
            this.Delete = new Guna.UI2.WinForms.Guna2Button();
            this.isEnable = new Guna.UI2.WinForms.Guna2Button();
            this.Cost = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.Stock = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.Description = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.Type = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.ItemID = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.historyTableHeader = new Guna.UI2.WinForms.Guna2Shapes();
            this.SuspendLayout();
            // 
            // Delete
            // 
            this.Delete.BackColor = System.Drawing.Color.Transparent;
            this.Delete.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(128)))));
            this.Delete.BorderRadius = 8;
            this.Delete.BorderThickness = 2;
            this.Delete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Delete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Delete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Delete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Delete.FillColor = System.Drawing.Color.White;
            this.Delete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Delete.ForeColor = System.Drawing.Color.Black;
            this.Delete.Location = new System.Drawing.Point(1424, 12);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(79, 37);
            this.Delete.TabIndex = 19;
            this.Delete.Text = "delete";
            this.Delete.UseTransparentBackground = true;
            // 
            // isEnable
            // 
            this.isEnable.BackColor = System.Drawing.Color.Transparent;
            this.isEnable.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(128)))));
            this.isEnable.BorderRadius = 8;
            this.isEnable.BorderThickness = 2;
            this.isEnable.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.isEnable.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.isEnable.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.isEnable.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.isEnable.FillColor = System.Drawing.Color.White;
            this.isEnable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isEnable.ForeColor = System.Drawing.Color.Black;
            this.isEnable.Location = new System.Drawing.Point(1241, 12);
            this.isEnable.Name = "isEnable";
            this.isEnable.Size = new System.Drawing.Size(138, 37);
            this.isEnable.TabIndex = 18;
            this.isEnable.Text = "btn1";
            this.isEnable.UseTransparentBackground = true;
            this.isEnable.Click += new System.EventHandler(this.isEnable_Click);
            // 
            // Cost
            // 
            this.Cost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(182)))), ((int)(((byte)(255)))));
            this.Cost.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cost.Location = new System.Drawing.Point(1048, 18);
            this.Cost.Name = "Cost";
            this.Cost.Size = new System.Drawing.Size(36, 22);
            this.Cost.TabIndex = 17;
            this.Cost.Text = "Cost";
            // 
            // Stock
            // 
            this.Stock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(182)))), ((int)(((byte)(255)))));
            this.Stock.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Stock.Location = new System.Drawing.Point(876, 18);
            this.Stock.Name = "Stock";
            this.Stock.Size = new System.Drawing.Size(44, 22);
            this.Stock.TabIndex = 16;
            this.Stock.Text = "Stock";
            // 
            // Description
            // 
            this.Description.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(182)))), ((int)(((byte)(255)))));
            this.Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Description.Location = new System.Drawing.Point(418, 18);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(83, 22);
            this.Description.TabIndex = 14;
            this.Description.Text = "Description";
            // 
            // Type
            // 
            this.Type.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(182)))), ((int)(((byte)(255)))));
            this.Type.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Type.Location = new System.Drawing.Point(224, 18);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(44, 22);
            this.Type.TabIndex = 13;
            this.Type.Text = "TYPE";
            // 
            // ItemID
            // 
            this.ItemID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(182)))), ((int)(((byte)(255)))));
            this.ItemID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemID.Location = new System.Drawing.Point(47, 18);
            this.ItemID.Name = "ItemID";
            this.ItemID.Size = new System.Drawing.Size(79, 22);
            this.ItemID.TabIndex = 12;
            this.ItemID.Text = "Receipt ID";
            // 
            // historyTableHeader
            // 
            this.historyTableHeader.BackColor = System.Drawing.Color.Transparent;
            this.historyTableHeader.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(128)))));
            this.historyTableHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(182)))), ((int)(((byte)(255)))));
            this.historyTableHeader.Location = new System.Drawing.Point(0, 0);
            this.historyTableHeader.Name = "historyTableHeader";
            this.historyTableHeader.PolygonSkip = 1;
            this.historyTableHeader.Rotate = 0F;
            this.historyTableHeader.RoundedRadius = 4;
            this.historyTableHeader.Shape = Guna.UI2.WinForms.Enums.ShapeType.Rounded;
            this.historyTableHeader.Size = new System.Drawing.Size(1533, 60);
            this.historyTableHeader.TabIndex = 11;
            this.historyTableHeader.Text = "guna2Shapes1";
            this.historyTableHeader.UseTransparentBackground = true;
            this.historyTableHeader.Zoom = 100;
            // 
            // AddInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.isEnable);
            this.Controls.Add(this.Cost);
            this.Controls.Add(this.Stock);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.ItemID);
            this.Controls.Add(this.historyTableHeader);
            this.Name = "AddInventory";
            this.Size = new System.Drawing.Size(1536, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Guna.UI2.WinForms.Guna2Button Delete;
        public Guna.UI2.WinForms.Guna2Button isEnable;
        public Guna.UI2.WinForms.Guna2HtmlLabel Cost;
        public Guna.UI2.WinForms.Guna2HtmlLabel Stock;
        public Guna.UI2.WinForms.Guna2HtmlLabel Description;
        public Guna.UI2.WinForms.Guna2HtmlLabel Type;
        public Guna.UI2.WinForms.Guna2HtmlLabel ItemID;
        public Guna.UI2.WinForms.Guna2Shapes historyTableHeader;
    }
}
