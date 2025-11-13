using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using MySql.Data.MySqlClient;

namespace sajidan_ukk_dekstop
{
    public partial class KelolaProduk : Form
    {
        MySqlConnection koneksi = Connections.Connect();
        MySqlCommand cmd;
        DataTable dt;
        MySqlDataAdapter sda;
        MySqlDataReader rd;
        string filename = "";
        string lokasi = "C:\\Sajidan\\C#\\sajidan_ukk-dekstop\\assets";

        public KelolaProduk()
        {
            InitializeComponent();
            show();
        }

        public void show()
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new MySqlCommand("SELECT * FROM products", koneksi);
                sda = new MySqlDataAdapter(cmd);
                dt = new DataTable("products");
                sda.Fill(dt);
                dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow rw = dgv.Rows[e.RowIndex];
                nama.Text = rw.Cells["nama_produk"].Value?.ToString() ?? "";
                deskripsi.Text = rw.Cells["deskripsi"].Value?.ToString() ?? "";
                harga.Text = rw.Cells["harga"].Value?.ToString() ?? "";
                stok.Value = Convert.ToInt32(rw.Cells["stok"].Value ?? 0);
                idp.Text = rw.Cells["id_produk"].Value?.ToString() ?? "";

                string image = rw.Cells["gambar_produk"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(image))
                {
                    string fullpath = Path.Combine(lokasi, image);
                    if (File.Exists(fullpath))
                    {
                        pictureBox.Image = Image.FromFile(fullpath);
                    }
                    else
                    {
                        pictureBox.Image = null;
                    }
                }
                else
                {
                    pictureBox.Image = null;
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idp.Text))
            {
                MessageBox.Show("Pilih produk yang akan dihapus terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menghapus data ini?", "Peringatan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    koneksi.Open();
                    cmd = new MySqlCommand("DELETE FROM products WHERE id_produk = @id", koneksi);
                    cmd.Parameters.AddWithValue("@id", idp.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil dihapus!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    show();
                    chartLoad();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (koneksi.State == ConnectionState.Open)
                    {
                        koneksi.Close();
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nama.Text) || string.IsNullOrEmpty(deskripsi.Text) || string.IsNullOrEmpty(harga.Text))
            {
                MessageBox.Show("Harap isi semua kolom!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                koneksi.Open();
                cmd = new MySqlCommand("INSERT INTO products (id_user, nama_produk, harga, stok, deskripsi, gambar_produk, tanggal_upload) VALUES (@id, @nama, @harga, @stok, @deskripsi, @gambar, @tgl)", koneksi);
                cmd.Parameters.AddWithValue("@id", Sessions.id);
                cmd.Parameters.AddWithValue("@nama", nama.Text.Trim());
                cmd.Parameters.AddWithValue("@harga", harga.Text.Trim());
                cmd.Parameters.AddWithValue("@stok", stok.Value);
                cmd.Parameters.AddWithValue("@deskripsi", deskripsi.Text.Trim());
                cmd.Parameters.AddWithValue("@gambar", filename);
                cmd.Parameters.AddWithValue("@tgl", DateTime.Now);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Input berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                show();
                chartLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idp.Text))
            {
                MessageBox.Show("Pilih produk yang akan diupdate terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(nama.Text) || string.IsNullOrEmpty(deskripsi.Text) || string.IsNullOrEmpty(harga.Text))
            {
                MessageBox.Show("Harap isi semua kolom!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                koneksi.Open();
                cmd = new MySqlCommand("UPDATE products SET id_user = @id, nama_produk = @nama, harga = @harga, stok = @stok, deskripsi = @deskripsi, gambar_produk = @gambar, tanggal_upload = @tgl WHERE id_produk = @idp", koneksi);
                cmd.Parameters.AddWithValue("@idp", idp.Text);
                cmd.Parameters.AddWithValue("@id", Sessions.id);
                cmd.Parameters.AddWithValue("@nama", nama.Text.Trim());
                cmd.Parameters.AddWithValue("@harga", harga.Text.Trim());
                cmd.Parameters.AddWithValue("@stok", stok.Value);
                cmd.Parameters.AddWithValue("@deskripsi", deskripsi.Text.Trim());
                cmd.Parameters.AddWithValue("@gambar", filename);
                cmd.Parameters.AddWithValue("@tgl", DateTime.Now);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Update berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                show();
                chartLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filename = Path.GetFileName(opf.FileName);
                    pictureBox.Image = Image.FromFile(opf.FileName);

                    // Pastikan direktori exists
                    if (!Directory.Exists(lokasi))
                    {
                        Directory.CreateDirectory(lokasi);
                    }

                    string fullPath = Path.Combine(lokasi, filename);
                    if (!File.Exists(fullPath))
                    {
                        File.Copy(opf.FileName, fullPath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void KelolaProduk_Load(object sender, EventArgs e)
        {
            show();
            chartLoad();
        }

        public void chartLoad()
        {
            try
            {
                koneksi.Open();
                cmd = new MySqlCommand("SELECT nama_produk, stok FROM products", koneksi);
                rd = cmd.ExecuteReader();

                chart1.Series.Clear();
                chart1.Series.Add("Stok Produk");
                chart1.Series["Stok Produk"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                while (rd.Read())
                {
                    string namaProduk = rd["nama_produk"].ToString();
                    int stokProduk = Convert.ToInt32(rd["stok"]);
                    chart1.Series["Stok Produk"].Points.AddXY(namaProduk, stokProduk);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading chart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                rd?.Close();
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void guna2TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }
                cmd = new MySqlCommand("SELECT * FROM products WHERE nama_produk LIKE @nama", koneksi);
                cmd.Parameters.AddWithValue("@nama", "%" + cari.Text + "%");
                dt = new DataTable();
                sda = new MySqlDataAdapter(cmd);
                sda.Fill(dt);
                dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin logout?", "Peringatan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Sessions.id = 0;
                Sessions.username = "";
                login login = new login();
                MessageBox.Show("Anda telah logout", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                login.Show();
                this.Hide();
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (XLWorkbook xl = new XLWorkbook())
                {
                    string exportLokasi = @"C:\Users\Axioo Pongo\Music\dataProducts.xlsx";
                    xl.Worksheets.Add(dt, "products");
                    xl.SaveAs(exportLokasi);
                    MessageBox.Show("Data berhasil diexport ke: " + exportLokasi, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            nama.Text = "";
            deskripsi.Text = "";
            harga.Text = "";
            stok.Value = 0;
            idp.Text = "";
            filename = "";
            pictureBox.Image = null;
        }

        // Event handlers kosong yang tidak digunakan bisa dihapus
        private void guna2TextBox1_TextChanged(object sender, EventArgs e) { }
        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2PictureBox1_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel4_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel5_Click(object sender, EventArgs e) { }
        private void guna2NumericUpDown2_ValueChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel6_Click(object sender, EventArgs e) { }
        private void guna2TextBox1_TextChanged_1(object sender, EventArgs e) { }
        private void guna2TextBox1_TextChanged_2(object sender, EventArgs e) { }
        private void guna2TextBox1_TextChanged_3(object sender, EventArgs e) { }
        private void guna2TextBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) { }
        private void chart1_Click(object sender, EventArgs e) { }
    }
}