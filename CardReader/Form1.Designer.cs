namespace CardReader
{
    partial class Form1
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.patientInfo1 = new MSIT155_E_MID.PatientInfo();
            this.SuspendLayout();
            // 
            // patientInfo1
            // 
            this.patientInfo1.BackColor = System.Drawing.Color.MistyRose;
            this.patientInfo1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.patientInfo1.info = null;
            this.patientInfo1.Location = new System.Drawing.Point(12, 12);
            this.patientInfo1.Name = "patientInfo1";
            this.patientInfo1.Size = new System.Drawing.Size(210, 232);
            this.patientInfo1.TabIndex = 0;
            this.patientInfo1.txtBirth = "";
            this.patientInfo1.txtGender = "";
            this.patientInfo1.txtID = "";
            this.patientInfo1.txtName = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 256);
            this.Controls.Add(this.patientInfo1);
            this.Name = "Form1";
            this.Text = "健保卡讀卡機";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MSIT155_E_MID.PatientInfo patientInfo1;
    }
}

