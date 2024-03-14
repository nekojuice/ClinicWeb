namespace MSIT155_E_MID
{
    partial class PatientInfo
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label17 = new System.Windows.Forms.Label();
            this.txtCardGender = new System.Windows.Forms.TextBox();
            this.lbCardId = new System.Windows.Forms.Label();
            this.lbCardBirth = new System.Windows.Forms.Label();
            this.txtCardBirth = new System.Windows.Forms.TextBox();
            this.txtCardId = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbCardName = new System.Windows.Forms.Label();
            this.lbCardGender = new System.Windows.Forms.Label();
            this.txtCardName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.LightCoral;
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label17.Cursor = System.Windows.Forms.Cursors.Default;
            this.label17.Dock = System.Windows.Forms.DockStyle.Top;
            this.label17.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label17.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label17.Location = new System.Drawing.Point(0, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(210, 29);
            this.label17.TabIndex = 31;
            this.label17.Text = "病患資訊";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCardGender
            // 
            this.txtCardGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCardGender.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCardGender.Location = new System.Drawing.Point(158, 28);
            this.txtCardGender.Name = "txtCardGender";
            this.txtCardGender.ReadOnly = true;
            this.txtCardGender.Size = new System.Drawing.Size(49, 35);
            this.txtCardGender.TabIndex = 35;
            // 
            // lbCardId
            // 
            this.lbCardId.AutoSize = true;
            this.lbCardId.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbCardId.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbCardId.Location = new System.Drawing.Point(3, 3);
            this.lbCardId.Name = "lbCardId";
            this.lbCardId.Size = new System.Drawing.Size(96, 27);
            this.lbCardId.TabIndex = 32;
            this.lbCardId.Text = "身分證號";
            // 
            // lbCardBirth
            // 
            this.lbCardBirth.AutoSize = true;
            this.lbCardBirth.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbCardBirth.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbCardBirth.Location = new System.Drawing.Point(3, 65);
            this.lbCardBirth.Name = "lbCardBirth";
            this.lbCardBirth.Size = new System.Drawing.Size(54, 27);
            this.lbCardBirth.TabIndex = 39;
            this.lbCardBirth.Text = "生日";
            // 
            // txtCardBirth
            // 
            this.txtCardBirth.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtCardBirth.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCardBirth.Location = new System.Drawing.Point(3, 92);
            this.txtCardBirth.Name = "txtCardBirth";
            this.txtCardBirth.ReadOnly = true;
            this.txtCardBirth.Size = new System.Drawing.Size(204, 35);
            this.txtCardBirth.TabIndex = 38;
            // 
            // txtCardId
            // 
            this.txtCardId.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtCardId.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCardId.Location = new System.Drawing.Point(3, 30);
            this.txtCardId.Name = "txtCardId";
            this.txtCardId.ReadOnly = true;
            this.txtCardId.Size = new System.Drawing.Size(204, 35);
            this.txtCardId.TabIndex = 37;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtCardGender, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbCardName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbCardGender, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtCardName, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 29);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(210, 64);
            this.tableLayoutPanel1.TabIndex = 41;
            // 
            // lbCardName
            // 
            this.lbCardName.AutoSize = true;
            this.lbCardName.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbCardName.Location = new System.Drawing.Point(3, 0);
            this.lbCardName.Name = "lbCardName";
            this.lbCardName.Size = new System.Drawing.Size(54, 25);
            this.lbCardName.TabIndex = 35;
            this.lbCardName.Text = "姓名";
            // 
            // lbCardGender
            // 
            this.lbCardGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCardGender.AutoSize = true;
            this.lbCardGender.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbCardGender.Location = new System.Drawing.Point(153, 0);
            this.lbCardGender.Name = "lbCardGender";
            this.lbCardGender.Size = new System.Drawing.Size(54, 25);
            this.lbCardGender.TabIndex = 36;
            this.lbCardGender.Text = "性別";
            // 
            // txtCardName
            // 
            this.txtCardName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCardName.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCardName.Location = new System.Drawing.Point(3, 28);
            this.txtCardName.Name = "txtCardName";
            this.txtCardName.ReadOnly = true;
            this.txtCardName.Size = new System.Drawing.Size(144, 35);
            this.txtCardName.TabIndex = 36;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtCardBirth);
            this.panel1.Controls.Add(this.lbCardBirth);
            this.panel1.Controls.Add(this.txtCardId);
            this.panel1.Controls.Add(this.lbCardId);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 93);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(210, 131);
            this.panel1.TabIndex = 43;
            // 
            // PatientInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label17);
            this.Name = "PatientInfo";
            this.Size = new System.Drawing.Size(210, 232);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtCardGender;
        private System.Windows.Forms.Label lbCardId;
        private System.Windows.Forms.Label lbCardBirth;
        private System.Windows.Forms.TextBox txtCardBirth;
        private System.Windows.Forms.TextBox txtCardId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbCardName;
        private System.Windows.Forms.Label lbCardGender;
        private System.Windows.Forms.TextBox txtCardName;
        private System.Windows.Forms.Panel panel1;
    }
}
