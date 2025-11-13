using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace sajidan_ukk_dekstop
{
    public partial class login : Form
    {
        MySqlCommand cmd;
        MySqlConnection koneksi = Connections.Connect();
        MySqlDataReader rd;

        public login()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Username dan password tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }

                cmd = new MySqlCommand("SELECT * FROM users WHERE username = @username AND password = @password", koneksi);
                cmd.Parameters.AddWithValue("@username", username.Text.Trim());
                cmd.Parameters.AddWithValue("@password", password.Text.Trim());

                rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    rd.Read();
                    string username = rd["username"].ToString();
                    int id = Convert.ToInt32(rd["id_user"]);
                    Sessions.start(username, id);

                    MessageBox.Show("Login berhasil!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rd.Close();
                    KelolaProduk k = new KelolaProduk();
                    k.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username atau password salah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saat login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin keluar?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Event handler untuk Enter key pada password field
        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button1_Click(sender, e);
            }
        }

        // Method untuk clear form
        private void ClearForm()
        {
            username.Text = "";
            password.Text = "";
            username.Focus();
        }

        // Event handlers kosong (biarkan seperti ini untuk kompatibilitas designer)
        private void guna2HtmlLabel1_Click(object sender, EventArgs e) { }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e) { }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2PictureBox1_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel4_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel5_Click(object sender, EventArgs e) { }
        private void guna2TextBox2_TextChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel6_Click(object sender, EventArgs e) { }
    }
}