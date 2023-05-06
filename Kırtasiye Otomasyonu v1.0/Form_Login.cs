using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kırtasiye_Otomasyonu_v1._0
{
    

    public partial class Form_Login : DevExpress.XtraEditors.XtraForm
    {
        public Form_Login()
        {
            InitializeComponent();
        }

        public static string kad = "";

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            SqlConnection connectionLoginButton = DBConnection.GetConnection();

            try
            {
                connectionLoginButton.Open();

                // işlem yaptıktan sonra bağlantıyı kapat
                //conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (connectionLoginButton.State != ConnectionState.Closed)
                    connectionLoginButton.Close();
            }

            string kullaniciAdi = tb_ad.Text;
                string sifre = tb_sif.Text;
                

            string md5Sifre = GetMd5Hash(sifre);
               

                string query = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi AND Sifre = @Sifre";

                using (connectionLoginButton)
                {
                    using (SqlCommand command = new SqlCommand(query, connectionLoginButton))
                    {
                       
                        command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                        command.Parameters.AddWithValue("@Sifre", md5Sifre);

                    
                    connectionLoginButton.Open();

                       
                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                        kad = "Personel: "+tb_ad.Text;

                        Form_AnaEkran anaForm = new Form_AnaEkran();
                        anaForm.Show();
                        
                            string updateQuery = "UPDATE Kullanicilar SET GirisTarihi = @GirisTarihi WHERE KullaniciAdi = @KullaniciAdi";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connectionLoginButton))
                            {
                                updateCommand.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                                updateCommand.Parameters.AddWithValue("@GirisTarihi", DateTime.Now);
                                updateCommand.ExecuteNonQuery();
                            }

                        this.Hide();
                        
                    }
                        else
                        {
                            // Giriş başarısız
                            MessageBox.Show("Hatalı kullanıcı adı veya şifre! Lütfen tekrar deneyiniz.");
                        }
                    }
                }
            }

            
            private static string GetMd5Hash(string input)
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        builder.Append(data[i].ToString("x2"));
                    }

                    return builder.ToString();
                }
            }

        private void Form_Login_Load(object sender, EventArgs e)
        {

        }
    }
    }
