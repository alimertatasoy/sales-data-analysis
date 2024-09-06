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
                InitializePlot(); // Ba�lang��ta ger�ek verileri g�ster
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Uygulama ba�lat�l�rken bir hata olu�tu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Veritaban� olu�turulurken bir hata olu�tu: {ex.Message}", "Veritaban� Hatas�", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Veritaban�ndan veriler y�klenirken bir hata olu�tu: {ex.Message}", "Veri Y�kleme Hatas�", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializePlot()
        {
            ShowActualData(); // Ba�lang��ta ger�ek verileri g�steren grafi�i �a��r
        }

        private void ShowActualData()
        {
            // Mevcut verileri g�steren grafi�i olu�tur
            var plotModel = new PlotModel { Title = "�uanki Grafik" };  // Ba�l�k "�uanki Grafik" olarak ayarland�
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Ay" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Sat��" });

            var series = new LineSeries
            {
                Title = "Sat��",
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
            salesPlotView.Model = plotModel;  // Grafi�i g�ncelle
            salesPlotView.InvalidatePlot(true);  // Grafi�i yenile
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            try
            {
                // E�er daha �nce 5 veya 12 ayl�k tahmin yap�ld�ysa uyar� g�ster
                if (salesPlotView.Model != null && salesPlotView.Model.Title != "�uanki Grafik" && salesPlotView.Model.Title != "Sonraki Ay Tahmini")
                {
                    MessageBox.Show("�nceki tahminleri temizleyin ya da '�uanki Grafik' butonuna basarak mevcut veriyi g�r�nt�leyin.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Veritaban�ndan mevcut ay ve sat�� verilerini al
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

                lblPredictionResult.Text = $"Gelecek ay ({nextMonth}. Ay) i�in tahmin edilen sat��: {predictedSales:F2} birim.";

                // Mevcut verileri ve tahmini i�eren grafi�i olu�tur
                var plotModel = new PlotModel { Title = "Sonraki Ay Tahmini" };
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Ay" });
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Sat��" });

                var series = new LineSeries
                {
                    Title = "Sat��",
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
                salesPlotView.Model = plotModel; // Modeli g�ncelle
                salesPlotView.InvalidatePlot(true); // Grafi�i yenile
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tahmin yap�l�rken bir hata olu�tu: {ex.Message}", "Tahmin Hatas�", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ShowForecast(int monthsAhead)
        {
            // Mevcut grafi�i temizle ve ba�l��� ayarla
            salesPlotView.Model = new PlotModel { Title = $"Gelecek {monthsAhead} Ay Sat�� Tahminleri" };
            salesPlotView.Model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Ay" });
            salesPlotView.Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Tahmin Edilen Sat��" });

            var forecastSeries = new LineSeries
            {
                Title = "Tahmin Edilen Sat��",
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
            salesPlotView.InvalidatePlot(true); // Grafi�i yenile
        }


        private void btnShowActualData_Click(object sender, EventArgs e)
        {
            ShowActualData(); // Ger�ek verileri g�steren grafi�i �a��r
        }

        private void btnShowForecast5Months_Click(object sender, EventArgs e)
        {
            ShowForecast(5); // 5 ayl�k tahmin grafi�ini g�ster
        }

        private void btnShowForecast12Months_Click(object sender, EventArgs e)
        {
            ShowForecast(12); // 12 ayl�k tahmin grafi�ini g�ster
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

                // Grafi�e yeni veriyi ekle
                var series = salesPlotView.Model.Series[0] as LineSeries;
                series.Points.Add(new DataPoint(month, sales));
                salesPlotView.InvalidatePlot(true); // Grafi�i yenile

                txtMonth.Clear();
                txtSales.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("L�tfen ge�erli bir say� format� girin.", "Format Hatas�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri eklenirken bir hata olu�tu: {ex.Message}", "Ekleme Hatas�", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
