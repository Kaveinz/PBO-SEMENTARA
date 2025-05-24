using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Project_PBO
{
    public partial class FormPilihKursi : Form
    {
        private string[] kursiTerpesan;
        private string selectedKursi = null;

        public string SelectedKursi => selectedKursi;

        public FormPilihKursi()
        {
            InitializeComponent();
            LoadKursiTerpesan();
        }

        private void LoadKursiTerpesan()
        {
            string query = "SELECT kursi FROM reservations WHERE kursi IS NOT NULL";
            var dt = DatabaseHelper.GetData(query);
            kursiTerpesan = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                kursiTerpesan[i] = dt.Rows[i]["kursi"].ToString();
            }
        }

        private void FormPilihKursi_Load(object sender, EventArgs e)
        {
            CekReservasiKursi(meja1, "A1");
            CekReservasiKursi(meja2, "A2");
            CekReservasiKursi(meja3, "A3");
            CekReservasiKursi(meja4, "A4");
            CekReservasiKursi(meja5, "A5");
            CekReservasiKursi(meja6, "A6");
            CekReservasiKursi(meja7, "A7");
        }

        private void CekReservasiKursi(Button meja, string kodeKursi)
        {
            if (Array.Exists(kursiTerpesan, k => k == kodeKursi))
            {
                meja.BackColor = Color.Red;
                meja.Enabled = false;
            }
            else
            {
                meja.BackColor = SystemColors.Control;
                meja.Click += Kursi_Click;
            }
        }

        private void Kursi_Click(object sender, EventArgs e)
        {
            Button meja = sender as Button;
            if (meja.BackColor == Color.Green)
            {
                meja.BackColor = SystemColors.Control;
                selectedKursi = null;
            }
            else
            {
                // Hapus warna hijau dari tombol lain
                foreach (Control ctrl in groupBox1.Controls)
                {
                    if (ctrl is Button btn && btn.BackColor == Color.Green)
                    {
                        btn.BackColor = SystemColors.Control;
                    }
                }
                meja.BackColor = Color.Green;
                selectedKursi = meja.Text; // Simpan kode kursi yang dipilih
            }
        }

        private void btnKonfirmasi_Click(object sender, EventArgs e)
        {
            if (selectedKursi != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Pilih satu kursi terlebih dahulu!");
            }
        }
    }
}