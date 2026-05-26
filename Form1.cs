using System.Globalization;

namespace CarSalesApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetUpUI();
        }

        private Button loadButton;
        private DataGridView dataGridView;

        private void SetUpUI()
        {
            this.Text = "Car Weekend Sales";
            this.Size = new Size(600, 400);

            loadButton = new Button
            {
                Text = "Import XML",
                Dock = DockStyle.Top,
                Height = 40
            };

            loadButton.Click += loadButtonClick;

            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.Controls.Add(dataGridView);
            this.Controls.Add(loadButton);
        }

        private void loadButtonClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML files (*.xml)|*.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var service = new CarSalesService();
                        var results = service.getWeekendSales(openFileDialog.FileName);
                        dataGridView.DataSource = results;

                        dataGridView.Columns["Model"]!.HeaderText = "Název modelu";
                        dataGridView.Columns["TotalWithoutDPH"]!.HeaderText = "Cena bez DPH";
                        dataGridView.Columns["TotalWithDPH"]!.HeaderText = "Cena s DPH";

                        dataGridView.Columns["TotalWithoutDPH"]!.DefaultCellStyle.Format = dataGridView.Columns["TotalWithDPH"]!.DefaultCellStyle.Format = "N2";
                        dataGridView.Columns["TotalWithoutDPH"]!.DefaultCellStyle.FormatProvider = dataGridView.Columns["TotalWithDPH"]!.DefaultCellStyle.FormatProvider = new CultureInfo("cs-CZ");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during loading file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
