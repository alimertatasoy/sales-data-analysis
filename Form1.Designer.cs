namespace satisTahmin
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
            txtMonth = new TextBox();
            btnNextMonthPrediction = new Button();
            lblPredictionResult = new Label();
            txtSales = new TextBox();
            btnAddData = new Button();
            lstSalesData = new ListBox();
            salesPlotView = new OxyPlot.WindowsForms.PlotView();
            label1 = new Label();
            label2 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            btnForecast12Months = new Button();
            btnForecast5Months = new Button();
            btnShowActualData = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // txtMonth
            // 
            txtMonth.Font = new Font("Segoe UI", 13.875F, FontStyle.Regular, GraphicsUnit.Point);
            txtMonth.Location = new Point(29, 106);
            txtMonth.Name = "txtMonth";
            txtMonth.Size = new Size(478, 57);
            txtMonth.TabIndex = 0;
            // 
            // btnNextMonthPrediction
            // 
            btnNextMonthPrediction.BackColor = Color.FromArgb(255, 255, 192);
            btnNextMonthPrediction.Location = new Point(26, 844);
            btnNextMonthPrediction.Name = "btnNextMonthPrediction";
            btnNextMonthPrediction.Size = new Size(531, 103);
            btnNextMonthPrediction.TabIndex = 1;
            btnNextMonthPrediction.Text = "Sonraki Ay Tahmin";
            btnNextMonthPrediction.UseVisualStyleBackColor = false;
            btnNextMonthPrediction.Click += btnPredict_Click;
            // 
            // lblPredictionResult
            // 
            lblPredictionResult.AutoSize = true;
            lblPredictionResult.Location = new Point(66, 832);
            lblPredictionResult.Name = "lblPredictionResult";
            lblPredictionResult.Size = new Size(78, 32);
            lblPredictionResult.TabIndex = 2;
            lblPredictionResult.Text = "label1";
            // 
            // txtSales
            // 
            txtSales.Font = new Font("Segoe UI", 13.875F, FontStyle.Regular, GraphicsUnit.Point);
            txtSales.Location = new Point(31, 229);
            txtSales.Name = "txtSales";
            txtSales.Size = new Size(476, 57);
            txtSales.TabIndex = 3;
            // 
            // btnAddData
            // 
            btnAddData.BackColor = Color.FromArgb(192, 255, 192);
            btnAddData.Location = new Point(29, 356);
            btnAddData.Name = "btnAddData";
            btnAddData.Size = new Size(478, 77);
            btnAddData.TabIndex = 4;
            btnAddData.Text = "Veriyi Veritabanına Ekle";
            btnAddData.UseVisualStyleBackColor = false;
            btnAddData.Click += btnAdd_Click;
            // 
            // lstSalesData
            // 
            lstSalesData.FormattingEnabled = true;
            lstSalesData.ItemHeight = 32;
            lstSalesData.Location = new Point(26, 542);
            lstSalesData.Name = "lstSalesData";
            lstSalesData.Size = new Size(531, 260);
            lstSalesData.TabIndex = 5;
            // 
            // salesPlotView
            // 
            salesPlotView.BackColor = SystemColors.ActiveCaption;
            salesPlotView.Location = new Point(66, 64);
            salesPlotView.Name = "salesPlotView";
            salesPlotView.PanCursor = Cursors.Hand;
            salesPlotView.Size = new Size(985, 540);
            salesPlotView.TabIndex = 6;
            salesPlotView.Text = "plotView1";
            salesPlotView.ZoomHorizontalCursor = Cursors.SizeWE;
            salesPlotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            salesPlotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.125F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            label1.Location = new Point(31, 64);
            label1.Name = "label1";
            label1.Size = new Size(130, 37);
            label1.TabIndex = 7;
            label1.Text = "Hangi Ay";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.125F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            label2.Location = new Point(31, 189);
            label2.Name = "label2";
            label2.Size = new Size(118, 37);
            label2.TabIndex = 8;
            label2.Text = "Net Ciro";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.ControlDark;
            groupBox1.Controls.Add(txtSales);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtMonth);
            groupBox1.Controls.Add(btnAddData);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(26, 36);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(531, 490);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Net Ciro Giriş";
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.FromArgb(192, 255, 255);
            groupBox2.Controls.Add(btnForecast12Months);
            groupBox2.Controls.Add(btnForecast5Months);
            groupBox2.Controls.Add(btnShowActualData);
            groupBox2.Controls.Add(salesPlotView);
            groupBox2.Controls.Add(lblPredictionResult);
            groupBox2.Location = new Point(608, 36);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1128, 911);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "groupBox2";
            // 
            // btnForecast12Months
            // 
            btnForecast12Months.BackColor = Color.FromArgb(255, 255, 192);
            btnForecast12Months.Location = new Point(789, 663);
            btnForecast12Months.Name = "btnForecast12Months";
            btnForecast12Months.Size = new Size(262, 103);
            btnForecast12Months.TabIndex = 9;
            btnForecast12Months.Text = "Gelecek 12 Ay Tahmin";
            btnForecast12Months.UseVisualStyleBackColor = false;
            btnForecast12Months.Click += btnShowForecast12Months_Click;
            // 
            // btnForecast5Months
            // 
            btnForecast5Months.BackColor = Color.FromArgb(255, 255, 192);
            btnForecast5Months.Location = new Point(426, 663);
            btnForecast5Months.Name = "btnForecast5Months";
            btnForecast5Months.Size = new Size(262, 103);
            btnForecast5Months.TabIndex = 8;
            btnForecast5Months.Text = "Gelecek 5 Ay Tahmin";
            btnForecast5Months.UseVisualStyleBackColor = false;
            btnForecast5Months.Click += btnShowForecast5Months_Click;
            // 
            // btnShowActualData
            // 
            btnShowActualData.BackColor = Color.FromArgb(255, 192, 128);
            btnShowActualData.Location = new Point(66, 663);
            btnShowActualData.Name = "btnShowActualData";
            btnShowActualData.Size = new Size(273, 103);
            btnShowActualData.TabIndex = 7;
            btnShowActualData.Text = "Şuanki Grafik";
            btnShowActualData.UseVisualStyleBackColor = false;
            btnShowActualData.Click += btnShowActualData_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1789, 1008);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(lstSalesData);
            Controls.Add(btnNextMonthPrediction);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtMonth;
        private Button btnNextMonthPrediction;
        private Label lblPredictionResult;
        private TextBox txtSales;
        private Button btnAddData;
        private ListBox lstSalesData;
        private OxyPlot.WindowsForms.PlotView salesPlotView;
        private Label label1;
        private Label label2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button btnForecast12Months;
        private Button btnForecast5Months;
        private Button btnShowActualData;
    }
}