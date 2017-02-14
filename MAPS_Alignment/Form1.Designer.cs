using System;

namespace MAPS_Alignment
{
    partial class Form1
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
            this.Start_Stop = new System.Windows.Forms.Button();
            this.flydisp = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.flydisp)).BeginInit();
            this.SuspendLayout();
            // 
            // Start_Stop
            // 
            this.Start_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Start_Stop.Location = new System.Drawing.Point(11, 500);
            this.Start_Stop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Start_Stop.Name = "Start_Stop";
            this.Start_Stop.Size = new System.Drawing.Size(85, 36);
            this.Start_Stop.TabIndex = 0;
            this.Start_Stop.Text = "Start";
            this.Start_Stop.UseVisualStyleBackColor = true;
            this.Start_Stop.Click += new System.EventHandler(this.Start_Stop_Click);
            // 
            // flydisp
            // 
            this.flydisp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flydisp.Location = new System.Drawing.Point(12, 10);
            this.flydisp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flydisp.Name = "flydisp";
            this.flydisp.Size = new System.Drawing.Size(560, 246);
            this.flydisp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.flydisp.TabIndex = 1;
            this.flydisp.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.label1.Location = new System.Drawing.Point(11, 258);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 76);
            this.label1.TabIndex = 2;
            this.label1.Text = "X Offset = ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.label2.Location = new System.Drawing.Point(11, 331);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(351, 76);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y Offset = ";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.label3.Location = new System.Drawing.Point(11, 414);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(390, 76);
            this.label3.TabIndex = 4;
            this.label3.Text = "Eucl Dist. = ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 546);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flydisp);
            this.Controls.Add(this.Start_Stop);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "MAPS Alignment";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.flydisp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button Start_Stop;
        private System.Windows.Forms.PictureBox flydisp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

