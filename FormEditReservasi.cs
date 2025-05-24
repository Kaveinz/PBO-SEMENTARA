using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace Project_PBO
{
    public partial class FormEditReservasi : Form
    {
        public FormEditReservasi()
        {
            InitializeComponent();
            LoadReservations();
        }

        private void LoadReservations()
        {
            string query = @"
                SELECT id, nama_pelanggan, nomor_hp, tanggal, waktu, jumlah_orang, kursi, status
                FROM reservations
                WHERE status = 'Menunggu' OR status = 'Dikonfirmasi'";
            var dt = DatabaseHelper.GetData(query);
            dgvReservations.DataSource = dt;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih reservasi yang akan diedit!");
                return;
            }

            if (cbStatus.SelectedItem == null)
            {
                MessageBox.Show("Pilih status baru!");
                return;
            }

            int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["id"].Value);
            string newStatus = cbStatus.SelectedItem.ToString();

            string query = "UPDATE reservations SET status = @status WHERE id = @id";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@status", newStatus),
                new NpgsqlParameter("@id", reservationId)
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Status reservasi berhasil diperbarui!");
                LoadReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}");
            }
        }
    }
}
