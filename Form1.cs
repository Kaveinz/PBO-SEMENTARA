using System;
using System;
using System.Windows.Forms;


namespace Project_PBO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnReservasiBaru_Click(object sender, EventArgs e)
        {
            FormReservasiBaru form = new FormReservasiBaru();
            form.ShowDialog();
        }

        private void btnEditReservasi_Click(object sender, EventArgs e)
        {
            FormEditReservasi form = new FormEditReservasi();
            form.ShowDialog();
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT id, nama_pelanggan, nomor_hp, tanggal, waktu, jumlah_orang, kursi, status
                FROM reservations";
            var dt = DatabaseHelper.GetData(query);

            using (Form form = new Form { Text = "Laporan Reservasi", Size = new System.Drawing.Size(800, 400) })
            {
                DataGridView dgv = new DataGridView
                {
                    Dock = DockStyle.Top,
                    Height = 300,
                    DataSource = dt
                };

                Button btnExport = new Button
                {
                    Text = "Export to Text",
                    Location = new System.Drawing.Point(10, 310),
                    Size = new System.Drawing.Size(120, 30)
                };
                btnExport.Click += (s2, e2) =>
                {
                    DatabaseHelper.ExportToTextFile(dt, "Laporan_Reservasi.txt");
                    MessageBox.Show("Laporan telah diekspor ke Laporan_Reservasi.txt");
                };

                form.Controls.Add(dgv);
                form.Controls.Add(btnExport);
                form.ShowDialog();
            }
        }
    }
}