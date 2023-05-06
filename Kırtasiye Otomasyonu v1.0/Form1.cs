using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Export.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Kırtasiye_Otomasyonu_v1._0
{
    public partial class Form_AnaEkran : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        
        private int saniye = 10;

        public decimal nonvert;

        public static ReportFormWithQuery ReportServiceForm = new ReportFormWithQuery();

        public Form_AnaEkran()
        {
            InitializeComponent();
            sqlDataSource1.FillAsync();
            sqlDataSource2.FillAsync();
            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateBarStaticItems();
            barStaticItem11.Caption = Form_Login.kad;

            SqlConnection connectionFormLoad = DBConnection.GetConnection();

            try
            {
                connectionFormLoad.Open();

                // işlem yaptıktan sonra bağlantıyı kapat
                //conn.Close();
            }
            catch (Exception ex)
            {
               
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (connectionFormLoad.State != ConnectionState.Closed)
                    connectionFormLoad.Close();
            }


        }

        public void UpdateBarStaticItems()
        {
            SqlConnection connectionUpdateBarStaticItems = DBConnection.GetConnection();

            try
            {
                connectionUpdateBarStaticItems.Open();

                // işlem yaptıktan sonra bağlantıyı kapat
                //conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (connectionUpdateBarStaticItems.State != ConnectionState.Closed)
                    connectionUpdateBarStaticItems.Close();
            }

            string query = "SELECT * FROM Stok";

            SqlConnection connectionUpdateBarStaticItems2 = DBConnection.GetConnection();
            SqlCommand command = new SqlCommand(query, connectionUpdateBarStaticItems2);
            connectionUpdateBarStaticItems2.Open();
            SqlDataReader reader = command.ExecuteReader();

            gridControl2.DataSource = reader;
            gridControl2.RefreshDataSource();
            

            connectionUpdateBarStaticItems2.Close();



            string queryToplamFiyat = "SELECT SUM(ToplamFiyat) AS ToplamFiyat FROM Stok";
            string queryUrunSayisi = "SELECT COUNT(*) AS UrunSayisi FROM Stok";

        
            using (connectionUpdateBarStaticItems)
            {
                using (SqlCommand commandToplamFiyat = new SqlCommand(queryToplamFiyat, connectionUpdateBarStaticItems))
                using (SqlCommand commandUrunSayisi = new SqlCommand(queryUrunSayisi, connectionUpdateBarStaticItems))
                {
                  
                    connectionUpdateBarStaticItems.Open();

                   
                    object resultToplamFiyat = commandToplamFiyat.ExecuteScalar();
                    object resultUrunSayisi = commandUrunSayisi.ExecuteScalar();

                  
                    if (resultToplamFiyat != null && resultToplamFiyat != DBNull.Value)
                    {
                        decimal toplamFiyat = Convert.ToDecimal(resultToplamFiyat);
                        lbl_toptl.Caption = toplamFiyat.ToString()+" TL";
                    }
                    else
                    {
                        lbl_toptl.Caption = "Stokta ürün bulunmamaktadır.";
                    }

                    if (resultUrunSayisi != null && resultUrunSayisi != DBNull.Value)
                    {
                        int urunSayisi = Convert.ToInt32(resultUrunSayisi);
                        lbl_topurun.Caption = urunSayisi.ToString()+" Ürün";
                    }
                    else
                    {
                        lbl_topurun.Caption = "Stokta ürün bulunmamaktadır.";
                    }
                }
            }
        }
   

        private void Form_AnaEkran_FormClosing(object sender, FormClosingEventArgs e)
        {
            
              
                DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show("Çıkış yapmak istiyor musunuz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                //Application.Exit();
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SatisYap satisForm = new SatisYap();
            satisForm.ShowDialog();
        }
        public static int count_i = 1;
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            ++count_i;

            if (count_i % 2 == 0)
            {

                Console.WriteLine(count_i);

                SqlConnection connectionRefreshAllData = DBConnection.GetConnection();

                try
                {
                    connectionRefreshAllData.Open();

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    if (connectionRefreshAllData.State != ConnectionState.Closed)
                        connectionRefreshAllData.Close();
                }

              
                string query = "SELECT * FROM Satislar ORDER BY SatisTarihi DESC";

                
                SqlCommand command = new SqlCommand(query, connectionRefreshAllData);
                connectionRefreshAllData.Open();
                SqlDataReader reader = command.ExecuteReader();

                gridControl1.DataSource = reader; 
                gridControl1.RefreshDataSource();
                barButtonItem2.Caption = "Son 100 Satışı Listele";
                lbl_sonsatislar.Text = "Tüm Satışlar";

                connectionRefreshAllData.Close();

            }
            if (count_i % 2 != 0)
            {
                Console.WriteLine(count_i + "Else if");
                barButtonItem2.Caption = "Tüm Satışları Listele";
                lbl_sonsatislar.Text = "Son 100 Satış";

                SqlConnection connectionRefreshAllData2 = DBConnection.GetConnection();

                try
                {
                    connectionRefreshAllData2.Open();

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    if (connectionRefreshAllData2.State != ConnectionState.Closed)
                        connectionRefreshAllData2.Close();
                }
                
                connectionRefreshAllData2.Open();
                string query = "SELECT TOP 100 * FROM Satislar ORDER BY SatisTarihi DESC;";

            
                SqlCommand command = new SqlCommand(query, connectionRefreshAllData2);
                SqlDataReader reader = command.ExecuteReader();

             
                gridControl1.DataSource = reader; 
                gridControl1.RefreshDataSource();

               
                connectionRefreshAllData2.Close();



            }
          
            
        }

        private void Form_AnaEkran_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            StokIslemleri formStokIslemleri = new StokIslemleri();
            formStokIslemleri.Show();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateBarStaticItems();
        }

       
        private void timer1_Tick(object sender, EventArgs e)
        {
            saniye--;
            barButtonItem7.Caption = saniye+" Saniye sonra güncelleniyor.";
            if(saniye <= 0)
            {
                saniye = 11;
            }
            UpdateBarStaticItems();


            timer1.Start();

        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PersonelIslemleri persnAc =     new PersonelIslemleri();
            persnAc.Show();

        }

        private void barSubItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void btn_tarihList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ReportFormWithQuery.setKeyQuery = 1;
            string First, Last;
            First = barTarih1.EditValue.ToString();
            Last = barTarih2.EditValue.ToString();
            string FirstDateSp, LastDateSp;
            string[] tarihParcalari = First.Split(' '); 
            string tarihPart = tarihParcalari[0];
            string[] tarihParcalari2 = tarihPart.Split('.');
            string gun = tarihParcalari2[0];
            string ay = tarihParcalari2[1];
            string yil = tarihParcalari2[2];
            string[] tarihParcalariLast = Last.Split(' '); 
            string tarihPartLast = tarihParcalariLast[0]; 
            string[] tarihParcalari2Last = tarihPartLast.Split('.');
            string gunLast = tarihParcalari2Last[0];
            string ayLast = tarihParcalari2Last[1];
            string yilLast = tarihParcalari2Last[2];

            FirstDateSp = $"{yil}-{ay}-{gun}";
            LastDateSp = $"{yilLast}-{ayLast}-{gunLast}";
            Console.WriteLine(LastDateSp);

            try
            {
                DateTime tarih1 = DateTime.ParseExact(FirstDateSp, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime tarih2 = DateTime.ParseExact(LastDateSp, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                int sonuc = DateTime.Compare(tarih1, tarih2);
                if (sonuc > 0)
                {
                    ReportFormWithQuery.noRowsMsg = tarih1.ToString() + "- " + tarih2.ToString() + " Tarihleri arasında satış bulunamadı.";
                    ReportServiceForm.reportlabel.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("Resources/delete.svg");
                    ReportServiceForm.ShowDialog();
                }
                else if (sonuc < 0)
                {
                    ReportFormWithQuery.FirstDate = FirstDateSp;
                    ReportFormWithQuery.LastDate = LastDateSp;
                    ReportServiceForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tarihleri Doğru Seçiniz.");
            }
           



           
           
        }

        private void barMaksSatis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            

        }

        private void barMaksSatis_EditValueChanged(object sender, EventArgs e)
        {
   

        }

        private void barMaksSatis_ItemDoubleClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string setVl;
            ReportFormWithQuery.setKeyQuery = 2;
            setVl = barMaksSatis.EditValue.ToString();
            //MessageBox.Show(setVl.ToString());
            ReportFormWithQuery.MaxPriceQuery = Int32.Parse(setVl);
            ReportServiceForm.ShowDialog();

        }

        private void barUrunList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string setVl2;
            ReportFormWithQuery.setKeyQuery = 3;
            setVl2 = barUrunAd.EditValue.ToString();
            ReportFormWithQuery.UrunAd = setVl2;
            ReportServiceForm.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string setVl3;
            ReportFormWithQuery.setKeyQuery = 4;
            setVl3 = barEditItem7.EditValue.ToString();
            ReportFormWithQuery.StockQu = setVl3;
            ReportServiceForm.ShowDialog();

        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateBarStaticItems();
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
        }
    }
    }
