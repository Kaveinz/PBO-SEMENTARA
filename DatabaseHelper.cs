using Npgsql;
using System;
using System.Data;
using System.IO;

namespace Project_PBO
{
    public static class DatabaseHelper
    {
        private static string connectionString = "Host=localhost;Username=postgres;Password=Renpersona5;Database=Warung_Makan_Mbok_Wo";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public static DataTable GetData(string query)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var adapter = new NpgsqlDataAdapter(query, conn))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static void ExecuteNonQuery(string query, params NpgsqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ExportToTextFile(DataTable dt, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("Laporan Reservasi");
                sw.WriteLine("=================");
                sw.WriteLine($"Tanggal Cetak: {DateTime.Now}");
                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    sw.WriteLine($"ID Reservasi: {row["id"]}");
                    sw.WriteLine($"Nama Pelanggan: {row["nama_pelanggan"]}");
                    sw.WriteLine($"Nomor HP: {row["nomor_hp"]}");
                    sw.WriteLine($"Tanggal: {row["tanggal"]}");
                    sw.WriteLine($"Waktu: {row["waktu"]}");
                    sw.WriteLine($"Jumlah Orang: {row["jumlah_orang"]}");
                    sw.WriteLine($"Kursi: {row["kursi"]}");
                    sw.WriteLine($"Status: {row["status"]}");
                    sw.WriteLine("-----------------");
                }
            }
        }
    }
}