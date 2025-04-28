namespace LR4
{
    partial class Form6
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ListBox listBoxCandidates;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.listBoxCandidates = new System.Windows.Forms.ListBox();
            this.ButtonSaveStageResults = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(20, 20);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(350, 300);
            this.chart1.TabIndex = 0;
            // 
            // listBoxCandidates
            // 
            this.listBoxCandidates.FormattingEnabled = true;
            this.listBoxCandidates.Location = new System.Drawing.Point(390, 20);
            this.listBoxCandidates.Name = "listBoxCandidates";
            this.listBoxCandidates.Size = new System.Drawing.Size(200, 290);
            this.listBoxCandidates.TabIndex = 1;
            // 
            // ButtonSaveStageResults
            // 
            this.ButtonSaveStageResults.Location = new System.Drawing.Point(413, 315);
            this.ButtonSaveStageResults.Name = "ButtonSaveStageResults";
            this.ButtonSaveStageResults.Size = new System.Drawing.Size(75, 23);
            this.ButtonSaveStageResults.TabIndex = 2;
            this.ButtonSaveStageResults.Text = "button1";
            this.ButtonSaveStageResults.UseVisualStyleBackColor = true;
            this.ButtonSaveStageResults.Click += new System.EventHandler(this.ButtonSaveStageResults_Click);
            // 
            // Form6
            // 
            this.ClientSize = new System.Drawing.Size(620, 350);
            this.Controls.Add(this.ButtonSaveStageResults);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.listBoxCandidates);
            this.Name = "Form6";
            this.Text = "Результати голосування";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button ButtonSaveStageResults;
    }
}
