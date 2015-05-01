namespace DataSorter
{
    partial class DataSorter
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLoadData = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.resortButton = new System.Windows.Forms.Button();
            this.labelImageNumber = new System.Windows.Forms.Label();
            this.numericUpDownImNum = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImNum)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLoadData
            // 
            this.buttonLoadData.Location = new System.Drawing.Point(207, 40);
            this.buttonLoadData.Name = "buttonLoadData";
            this.buttonLoadData.Size = new System.Drawing.Size(103, 28);
            this.buttonLoadData.TabIndex = 0;
            this.buttonLoadData.Text = "load";
            this.buttonLoadData.UseVisualStyleBackColor = true;
            this.buttonLoadData.Click += new System.EventHandler(this.buttonLoadData_Click);
            // 
            // resortButton
            // 
            this.resortButton.Location = new System.Drawing.Point(208, 98);
            this.resortButton.Name = "resortButton";
            this.resortButton.Size = new System.Drawing.Size(102, 28);
            this.resortButton.TabIndex = 1;
            this.resortButton.Text = "resort data";
            this.resortButton.UseVisualStyleBackColor = true;
            this.resortButton.Click += new System.EventHandler(this.resortButton_Click);
            // 
            // labelImageNumber
            // 
            this.labelImageNumber.AutoSize = true;
            this.labelImageNumber.Location = new System.Drawing.Point(12, 48);
            this.labelImageNumber.Name = "labelImageNumber";
            this.labelImageNumber.Size = new System.Drawing.Size(92, 13);
            this.labelImageNumber.TabIndex = 2;
            this.labelImageNumber.Text = "Number of images";
            // 
            // numericUpDownImNum
            // 
            this.numericUpDownImNum.Location = new System.Drawing.Point(110, 46);
            this.numericUpDownImNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownImNum.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownImNum.Name = "numericUpDownImNum";
            this.numericUpDownImNum.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownImNum.TabIndex = 3;
            this.numericUpDownImNum.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // DataSorter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 161);
            this.Controls.Add(this.numericUpDownImNum);
            this.Controls.Add(this.labelImageNumber);
            this.Controls.Add(this.resortButton);
            this.Controls.Add(this.buttonLoadData);
            this.Name = "DataSorter";
            this.Text = "Data Sorter";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadData;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button resortButton;
        private System.Windows.Forms.Label labelImageNumber;
        private System.Windows.Forms.NumericUpDown numericUpDownImNum;
    }
}

