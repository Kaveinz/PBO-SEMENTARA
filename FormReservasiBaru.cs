using Npgsql;
using Npgsql;
using System;
using System.Windows.Forms;

namespace Project_PBO
{
    public partial class FormReservasiBaru : Form
    {
        public FormReservasiBaru()
        {
            InitializeComponent();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNamaPelanggan.Text) || string.IsNullOrWhiteSpace(tbNomorHP.Text) ||
                string.IsNullOrWhiteSpace(tbJumlahOrang.Text))
            {
                MessageBox.Show("Harap lengkapi semua kolom!");
                return;
            }

            // Buka FormPilihKursi untuk memilih kursi
            using (FormPilihKursi formPilihKursi = new FormPilihKursi())
            {
                if (formPilihKursi.ShowDialog() == DialogResult.OK)
                {
                    string kursiDipilih = formPilihKursi.SelectedKursi;
                    if (string.IsNullOrEmpty(kursiDipilih))
                    {
                        MessageBox.Show("Pilih kursi terlebih dahulu!");
                        return;
                    }

                    string query = @"
                        INSERT INTO reservations (nama_pelanggan, nomor_hp, tanggal, waktu, jumlah_orang, status, kursi)
                        VALUES (@nama, @nomor_hp, @tanggal, @waktu, @jumlah_orang, 'Menunggu', @kursi)";

                    var parameters = new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("@nama", tbNamaPelanggan.Text),
                        new NpgsqlParameter("@nomor_hp", tbNomorHP.Text),
                        new NpgsqlParameter("@tanggal", dtpTanggal.Value.Date),
                        new NpgsqlParameter("@waktu", dtpWaktu.Value.TimeOfDay),
                        new NpgsqlParameter("@jumlah_orang", int.Parse(tbJumlahOrang.Text)),
                        new NpgsqlParameter("@kursi", kursiDipilih)
                    };

                    try
                    {
                        DatabaseHelper.ExecuteNonQuery(query, parameters);
                        MessageBox.Show("Reservasi berhasil disimpan!");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Terjadi kesalahan: {ex.Message}");
                    }
                }
            }
        }
    }
}