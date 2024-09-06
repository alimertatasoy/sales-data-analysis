using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;

namespace satisTahmin
{
    public partial class Form1 : Form
    {
        private string dbFile = "sales_data.db";
        private SQLiteConnection connection;

        public Form1()
        {
            InitializeComponent();
            try
            {
                InitializeDatabase();
                LoadDataFromDatabase();
                InitializePlot(); // Baþlangýçta gerçek verileri göster
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Uygulama baþlatýlýrken bir hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                if (!File.Exists(dbFile))
                {
                    SQLiteConnection.CreateFile(dbFile);
                }

                connection = new SQLiteConnection($"Data Source={dbFile};Version=3;");
                connection.Open();

                string createTableQuery = "CREATE TABLE IF NOT EXISTS SalesData (Month INTEGER, Sales REAL)";
                using (var cmd = new SQLiteCommand(createTableQuery, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabaný oluþturulurken bir hata oluþtu: {ex.Message}", "Veritabaný Hatasý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                string selectQuery = "SELECT Month, Sales FROM SalesData";
                using (var cmd = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int month = reader.GetInt32(0);
                            double sales = reader.GetDouble(1);
                            lstSalesData.Items.Add($"{month}: {sales}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanýndan veriler yüklenirken bir hata oluþtu: {ex.Message}", "Veri Yükleme Hatasý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializePlot()
        {
            ShowActualData(); // Baþlangýçta gerçek verileri gösteren grafiði çaðýr
        }

        private void ShowActualData()
        {
            // Mevcut verileri gösteren grafiði oluþtur
            var plotModel = new PlotModel { Title = "Þuanki Grafik" };  // Baþlýk "Þuanki Grafik" olarak ayarlandý
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Ay" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Satýþ" });

            var series = new LineSeries
            {
                Title = "Satýþ",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                LabelFormatString = "{1:F2}"
            };

            string selectQuery = "SELECT Month, Sales FROM SalesData";
            using (var cmd = new SQLiteCommand(selectQuery, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int month = reader.GetInt32(0);
                        double sales = reader.GetDouble(1);
                        series.Points.Add(new DataPoint(month, sales));
                    }
                }
            }

            plotModel.Series.Add(series);
            salesPlotView.Model = plotModel;  // Grafiði güncelle
            salesPlotView.InvalidatePlot(true);  // Grafiði yenile
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            try
            {
                // Eðer daha önce 5 veya 12 aylýk tahmin yapýldýysa uyarý göster
                if (salesPlotView.Model != null && salesPlotView.Model.Title != "Þuanki Grafik" && salesPlotView.Model.Title != "Sonraki Ay Tahmini")
                {
                    MessageBox.Show("Önceki tahminleri temizleyin ya da 'Þuanki Grafik' butonuna basarak mevcut veriyi görüntüleyin.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Veritabanýndan mevcut ay ve satýþ verilerini al
                string selectQuery = "SELECT Month, Sales FROM SalesData";
                var months = new System.Collections.Generic.List<int>();
                var sales = new System.Collections.Generic.List<double>();

                using (var cmd = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            months.Add(reader.GetInt32(0));
                            sales.Add(reader.GetDouble(1));
                        }
                    }
                }

                // Lineer regresyon hesaplama
                double slope = CalculateSlope(months.ToArray(), sales.ToArray());
                double intercept = CalculateIntercept(months.ToArray(), sales.ToArray(), slope);

                // Gelecek ay tahmini
                int nextMonth = months.Max() + 1;
                double predictedSales = slope * nextMonth + intercept;

                lblPredictionResult.Text = $"Gelecek ay ({nextMonth}. Ay) için tahmin edilen satýþ: {predictedSales:F2} birim.";

                // Mevcut verileri ve tahmini içeren grafiði oluþtur
                var plotModel = new PlotModel { Title = "Sonraki Ay Tahmini" };
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Ay" });
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Satýþ" });

                var series = new LineSeries
                {
                    Title = "Satýþ",
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 4,
                    MarkerStroke = OxyColors.White,
                    LabelFormatString = "{1:F2}"
                };

                // Mevcut verileri seriye ekle
                for (int i = 0; i < months.Count; i++)
                {
                    series.Points.Add(new DataPoint(months[i], sales[i]));
                }

                // Tahmin edilen veriyi de seriye ekle
                series.Points.Add(new DataPoint(nextMonth, predictedSales));

                plotModel.Series.Add(series);
                salesPlotView.Model = plotModel; // Modeli güncelle
                salesPlotView.InvalidatePlot(true); // Grafiði yenile
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tahmin yapýlýrken bir hata oluþtu: {ex.Message}", "Tahmin Hatasý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ShowForecast(int monthsAhead)
        {
            // Mevcut grafiði temizle ve baþlýðý ayarla
            salesPlotView.Model = new PlotModel { Title = $"Gelecek {monthsAhead} Ay Satýþ Tahminleri" };
            salesPlotView.Model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Ay" });
            salesPlotView.Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Tahmin Edilen Satýþ" });

            var forecastSeries = new LineSeries
            {
                Title = "Tahmin Edilen Satýþ",
                MarkerType = MarkerType.Diamond,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Blue,
                LabelFormatString = "{1:F2}"
            };

            string selectQuery = "SELECT Month, Sales FROM SalesData ORDER BY Month";
            var months = new System.Collections.Generic.List<int>();
            var sales = new System.Collections.Generic.List<double>();

            using (var cmd = new SQLiteCommand(selectQuery, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        months.Add(reader.GetInt32(0));
                        sales.Add(reader.GetDouble(1));
                    }
                }
            }

            int lastMonth = months.Last();
            double lastSales = sales.Last();

            for (int i = 1; i <= monthsAhead; i++)
            {
                months.Add(lastMonth);
                sales.Add(lastSales);

                double slope = CalculateSlope(months.ToArray(), sales.ToArray());
                double intercept = CalculateIntercept(months.ToArray(), sales.ToArray(), slope);

                int nextMonth = lastMonth + 1;
                double predictedSales = slope * nextMonth + intercept;

                forecastSeries.Points.Add(new DataPoint(nextMonth, predictedSales));

                lastMonth = nextMonth;
                lastSales = predictedSales;
            }

            salesPlotView.Model.Series.Add(forecastSeries);
            salesPlotView.InvalidatePlot(true); // Grafiði yenile
        }


        private void btnShowActualData_Click(object sender, EventArgs e)
        {
            ShowActualData(); // Gerçek verileri gösteren grafiði çaðýr
        }

        private void btnShowForecast5Months_Click(object sender, EventArgs e)
        {
            ShowForecast(5); // 5 aylýk tahmin grafiðini göster
        }

        private void btnShowForecast12Months_Click(object sender, EventArgs e)
        {
            ShowForecast(12); // 12 aylýk tahmin grafiðini göster
        }

        private double CalculateSlope(int[] x, double[] y)
        {
            int n = x.Length;
            double xSum = x.Sum();
            double ySum = y.Sum();
            double xySum = 0;
            double xxSum = 0;

            for (int i = 0; i < n; i++)
            {
                xySum += x[i] * y[i];
                xxSum += x[i] * x[i];
            }

            double slope = (n * xySum - xSum * ySum) / (n * xxSum - xSum * xSum);
            return slope;
        }

        private double CalculateIntercept(int[] x, double[] y, double slope)
        {
            double xMean = x.Average();
            double yMean = y.Average();

            double intercept = yMean - slope * xMean;
            return intercept;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int month = int.Parse(txtMonth.Text);
                double sales = double.Parse(txtSales.Text);

                lstSalesData.Items.Add($"{month}: {sales}");

                string insertQuery = "INSERT INTO SalesData (Month, Sales) VALUES (@month, @sales)";
                using (var cmd = new SQLiteCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@sales", sales);
                    cmd.ExecuteNonQuery();
                }

                // Grafiðe yeni veriyi ekle
                var series = salesPlotView.Model.Series[0] as LineSeries;
                series.Points.Add(new DataPoint(month, sales));
                salesPlotView.InvalidatePlot(true); // Grafiði yenile

                txtMonth.Clear();
                txtSales.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Lütfen geçerli bir sayý formatý girin.", "Format Hatasý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri eklenirken bir hata oluþtu: {ex.Message}", "Ekleme Hatasý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
