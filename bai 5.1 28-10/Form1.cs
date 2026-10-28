using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace LoginFormApp
{
    public partial class FormLogin : Form
    {

        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LoginDB;Integrated Security=True;TrustServerCertificate=True;";
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // Thiết lập trạng thái mặc định
            txtPassword.UseSystemPasswordChar = true;
            chkShowPassword.Checked = false;

            // Đảm bảo phím Enter & Esc luôn được map đúng Button
            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;
        }

        /// <summary>
        /// Yêu cầu: Tick chọn -> Hiện mật khẩu (UseSystemPasswordChar = false)
        ///          Bỏ chọn   -> Ẩn mật khẩu (UseSystemPasswordChar = true)
        /// </summary>
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        /// <summary>
        /// Xử lý khi nhấn nút Đăng nhập hoặc gõ Enter
        /// </summary>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu hợp lệ (nếu rỗng thì ErrorProvider báo lỗi và dừng lại)
            if (!ValidateInputs())
            {
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // 2. Bắt đầu kết nối CSDL và kiểm tra tài khoản
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Lệnh SQL tìm tài khoản khớp cả Tên đăng nhập và Mật khẩu
                    string query = "SELECT HoTen FROM TaiKhoan WHERE TenDangNhap = @User AND MatKhau = @Pass";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Dùng Parameter để bảo mật chống hack SQL Injection
                        cmd.Parameters.AddWithValue("@User", username);
                        cmd.Parameters.AddWithValue("@Pass", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Nếu tìm thấy dòng khớp trong CSDL
                            {
                                string hoTen = reader["HoTen"].ToString();

                                MessageBox.Show($"Đăng nhập thành công!\nXin chào: {hoTen}",
                                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else // Sai tên đăng nhập hoặc sai mật khẩu
                            {
                                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!",
                                                "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                txtPassword.SelectAll();
                                txtPassword.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiện lỗi chi tiết nếu mất kết nối hoặc sai tên bảng
                MessageBox.Show("Lỗi kết nối CSDL:\n" + ex.Message,
                                "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý khi nhấn Thoát hoặc gõ phím Esc
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát khỏi ứng dụng?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Hàm Validation dữ liệu sử dụng ErrorProvider
        /// </summary>
        private bool ValidateInputs()
        {
            bool isValid = true;

            // 1. Kiểm tra txtUsername không để trống hoặc chỉ chứa dấu cách
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Tên đăng nhập không được để trống!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtUsername, string.Empty);
            }

            // 2. Kiểm tra txtPassword không để trống
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Mật khẩu không được để trống!");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtPassword, string.Empty);
            }

            // Tự động nhảy con trỏ về ô đầu tiên bị lỗi
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Focus();
            }

            return isValid;
        }

        /// <summary>
        /// Xóa cảnh báo lỗi ngay khi người dùng bắt đầu gõ ký tự vào ô Username
        /// </summary>
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, string.Empty);
            }
        }

        /// <summary>
        /// Xóa cảnh báo lỗi ngay khi người dùng bắt đầu gõ ký tự vào ô Password
        /// </summary>
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, string.Empty);
            }
        }
    }
}