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
    public partial class PersonelIslemleri : DevExpress.XtraEditors.XtraForm
    {
        public PersonelIslemleri()
        {
            InitializeComponent();
            sqlDataSource1.FillAsync();
        }
        private void LoadGridData()
        {

            SqlConnection connectionFillPersonelData = DBConnection.GetConnection();

            try
            {
                connectionFillPersonelData.Open();

                // işlem yaptıktan sonra bağlantıyı kapat
                //conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (connectionFillPersonelData.State != ConnectionState.Closed)
                    connectionFillPersonelData.Close();
            }

            string sql = "SELECT KullaniciAdi, Ad, Soyad,GirisTarihi FROM Kullanicilar ";
            using (connectionFillPersonelData)
            {
                connectionFillPersonelData.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, connectionFillPersonelData))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gridControl1.DataSource = dt;
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            SqlConnection connectionFillPersonelData2 = DBConnection.GetConnection();

            try
            {
                connectionFillPersonelData2.Open();

                // işlem yaptıktan sonra bağlantıyı kapat
                //conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (connectionFillPersonelData2.State != ConnectionState.Closed)
                    connectionFillPersonelData2.Close();
            }

            string kullaniciAdi = textEdit1.Text;
            string sifre = textEdit2.Text;
            string ad = textEdit4.Text;
            string soyad = textEdit5.Text;
            DateTime girisTarihi = DateTime.Now;

           
            string sifreMD5 = CalculateMD5Hash(sifre);

            
            string ikinciSifre = textEdit3.Text;
            string ikinciSifreMD5 = CalculateMD5Hash(ikinciSifre);

            
            

            
            string kullaniciAdiCheckQuery = "SELECT COUNT(*) FROM [dbo].[Kullanicilar] WHERE [KullaniciAdi] = @KullaniciAdi";
            string kullaniciEkleQuery = "INSERT INTO [dbo].[Kullanicilar] ([KullaniciAdi], [Sifre], [Ad], [Soyad], [GirisTarihi]) " +
                                         "VALUES (@KullaniciAdi, @Sifre, @Ad, @Soyad, @GirisTarihi)";

            using (connectionFillPersonelData2)
            {
                connectionFillPersonelData2.Open();

                
                using (SqlCommand checkCommand = new SqlCommand(kullaniciAdiCheckQuery, connectionFillPersonelData2))
                {
                    checkCommand.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    int existingUserCount = (int)checkCommand.ExecuteScalar();

                    if (existingUserCount > 0)
                    {
                        Console.WriteLine("Bu kullanıcı adı zaten alınmış.");
                    }
                    else
                    {
                        
                        if (sifreMD5 != ikinciSifreMD5)
                        {
                            MessageBox.Show("Şifreler uyuşmuyor.");
                         
                        }
                        else
                        {
                           
                            using (SqlCommand insertCommand = new SqlCommand(kullaniciEkleQuery, connectionFillPersonelData2))
                            {
                                insertCommand.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                                insertCommand.Parameters.AddWithValue("@Sifre", sifreMD5);
                                insertCommand.Parameters.AddWithValue("@Ad", ad);
                                insertCommand.Parameters.AddWithValue("@Soyad", soyad);
                                insertCommand.Parameters.AddWithValue("@GirisTarihi", girisTarihi);

                                int affectedRows = insertCommand.ExecuteNonQuery();
                                if (affectedRows > 0)
                                {
                                    LoadGridData();
                                    MessageBox.Show("Kullanıcı eklendi.");
                                   
                                }
                                else
                                {
                                    MessageBox.Show("Kullanıcı eklenirken bir hata oluştu.");
                                    
                                }
                            }
                        }
                    }
                }
            }
           
        }
        static string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SqlConnection connectionFillPersonelData3 = DBConnection.GetConnection();

            try
            {
                connectionFillPersonelData3.Open();

                // işlem yaptıktan sonra bağlantıyı kapat
                //conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (connectionFillPersonelData3.State != ConnectionState.Closed)
                    connectionFillPersonelData3.Close();
            }

            string kullaniciAdi = textEdit10.Text;
            string yeniSifre = textEdit9.Text;
            string yeniAd = textEdit7.Text;
            string yeniSoyad = textEdit6.Text;
            DateTime yeniGirisTarihi = DateTime.Now;
            string yeniSifreTekrar = textEdit8.Text;

            
            if (!string.Equals(yeniSifre, yeniSifreTekrar))
            {
                MessageBox.Show("Yeni şifreler uyuşmuyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           
            using (connectionFillPersonelData3)
            {
                connectionFillPersonelData3.Open();

                
                using (SqlCommand command = new SqlCommand("UPDATE [dbo].[Kullanicilar] SET [Sifre] = @yeniSifre, [Ad] = @yeniAd, [Soyad] = @yeniSoyad, [GirisTarihi] = @yeniGirisTarihi WHERE [KullaniciAdi] = @kullaniciAdi", connectionFillPersonelData3))
                {
                    
                    command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@yeniSifre", CalculateMD5Hash(yeniSifre));
                    command.Parameters.AddWithValue("@yeniAd", yeniAd);
                    command.Parameters.AddWithValue("@yeniSoyad", yeniSoyad);
                    command.Parameters.AddWithValue("@yeniGirisTarihi", yeniGirisTarihi);

                    
                    int affectedRows = command.ExecuteNonQuery();

                   
                    if (affectedRows > 0)
                    {
                        LoadGridData();
                        MessageBox.Show("Kullanıcı güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SqlConnection connectionFillPersonelData4 = DBConnection.GetConnection();

            try
            {
                connectionFillPersonelData4.Open();

                // işlem yaptıktan sonra bağlantıyı kapat
                //conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (connectionFillPersonelData4.State != ConnectionState.Closed)
                    connectionFillPersonelData4.Close();
            }


            string kullaniciAdi = textEdit15.Text;



            using (connectionFillPersonelData4)
            {
                connectionFillPersonelData4.Open();

                
                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Kullanicilar] WHERE [KullaniciAdi] = @kullaniciAdi", connectionFillPersonelData4))
                {
                    checkCommand.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    int kullaniciAdiVarMi = (int)checkCommand.ExecuteScalar();

                    if (kullaniciAdiVarMi > 0)
                    {
                        
                        using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM [dbo].[Kullanicilar] WHERE [KullaniciAdi] = @kullaniciAdi", connectionFillPersonelData4))
                        {
                            deleteCommand.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                            
                            int affectedRows = deleteCommand.ExecuteNonQuery();

                           
                            if (affectedRows > 0)
                            {
                                LoadGridData();
                                MessageBox.Show("Kullanıcı silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Belirtilen kullanıcı adı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PersonelIslemleri_Load(object sender, EventArgs e)
        {

        }
    }
}