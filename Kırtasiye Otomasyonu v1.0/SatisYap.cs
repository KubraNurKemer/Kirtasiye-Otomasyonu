﻿using DevExpress.Xpo.DB;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kırtasiye_Otomasyonu_v1._0
{
    public partial class SatisYap : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dt2 = new DataTable();
        public SatisYap()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill the SqlDataSource asynchronously
            sqlDataSource1.FillAsync();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill the SqlDataSource asynchronously
            sqlDataSource1.FillAsync();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill the SqlDataSource asynchronously
            sqlDataSource1.FillAsync();
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            
            int selectedRowHandle = gridView1.FocusedRowHandle;
            int id = Convert.ToInt32(gridView1.GetRowCellValue(selectedRowHandle, "ID"));
            string ad = gridView1.GetRowCellValue(selectedRowHandle, "Ad").ToString();
            string soyad = gridView1.GetRowCellValue(selectedRowHandle, "Soyad").ToString();
            string telefon = gridView1.GetRowCellValue(selectedRowHandle, "Telefon").ToString();

           
        }
       
       

        private void repositoryItemButtonEdit1_ButtonClick_1(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int selectedRowHandle = gridView1.FocusedRowHandle;
            if (selectedRowHandle < 0)
                return;

            string urunAdi = gridView1.GetRowCellValue(selectedRowHandle, "UrunAdi")?.ToString();
            string birimFiyat = gridView1.GetRowCellValue(selectedRowHandle, "BirimFiyat")?.ToString();

            List<string> selectedRowDataList = new List<string>();
            selectedRowDataList.Add(urunAdi);
            selectedRowDataList.Add(birimFiyat);
            DataRow newRow = dt2.NewRow();
            newRow["UrunAdi"] = urunAdi;
            newRow["BirimFiyat"] = birimFiyat;
            dt2.Rows.Add(newRow);
            gridControl2.RefreshDataSource();
            gCRefs(0);
        }

        private void SatisYap_Load(object sender, EventArgs e)
        {
            timer1.Start();
            GridColumn gridColumnUrunAdi = new GridColumn();
            gridColumnUrunAdi.FieldName = "UrunAdi";
            gridColumnUrunAdi.Caption = "Ürün Adı";
            gridControl2.MainView = gridView2;
            gridView2.Columns.Add(gridColumnUrunAdi);

            GridColumn gridColumnBirimFiyat = new GridColumn();
            gridColumnBirimFiyat.FieldName = "BirimFiyat";
            gridColumnBirimFiyat.Caption = "Birim Fiyat";
            gridView2.Columns.Add(gridColumnBirimFiyat);

           
            dt2.Columns.Add("UrunAdi");
            dt2.Columns.Add("BirimFiyat");

       
            gridControl2.DataSource = dt2;
            
        }

        private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
           
            int rowIndex = gridView2.FocusedRowHandle;
            if (rowIndex >= 0)
            {
               
                gridView2.DeleteRow(rowIndex);
                gCRefs(0);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gCRefs(0);
        }
        public void gCRefs(int nu)
        {
            if (nu == 0)
            {
               
                decimal toplamBirimFiyat = 0;

                
                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    decimal birimFiyat = Convert.ToDecimal(gridView2.GetRowCellValue(i, "BirimFiyat"));
                    toplamBirimFiyat += birimFiyat;
                }

                
                DataTable dt3 = new DataTable(); 
                dt3.Columns.Add("UrunAdi");
                dt3.Columns.Add("ToplamFiyat");

                DataRow newRow = dt3.NewRow();
                newRow["UrunAdi"] = "Toplam";
                newRow["ToplamFiyat"] = toplamBirimFiyat;
                dt3.Rows.Add(newRow);

                gridControl3.DataSource = dt3;

                
                gridControl3.RefreshDataSource();
            }
            else
            {
                gridControl3.RefreshDataSource();
            }
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (gridView2.RowCount > 0)
            {
                if (!string.IsNullOrEmpty(tb_musteri.Text))
                {
                    for (int i = 0; i < gridView2.RowCount; i++)
                    {
                        string musteriAdi = tb_musteri.Text;
                        string urunAdi = gridView2.GetRowCellValue(i, "UrunAdi").ToString();
                        decimal fiyat = Convert.ToDecimal(gridView2.GetRowCellValue(i, "BirimFiyat"));
                        decimal toplamFiyat = Convert.ToDecimal(gridView3.GetRowCellValue(0, "ToplamFiyat"));
                        DateTime satisTarihi = DateTime.Now;

                        using (SqlConnection connectionString = DBConnection.GetConnection())
                        {
                            connectionString.Open();
                            using (SqlCommand command = new SqlCommand("INSERT INTO Satislar (MusteriAdi, UrunAdi, Miktar, Fiyat, ToplamFiyat, SatisTarihi) VALUES (@MusteriAdi, @UrunAdi, @Miktar, @Fiyat, @ToplamFiyat, @SatisTarihi)", connectionString))
                            {
                                command.Parameters.AddWithValue("@MusteriAdi", musteriAdi);
                                command.Parameters.AddWithValue("@UrunAdi", urunAdi);
                                command.Parameters.AddWithValue("@Miktar", 1);
                                command.Parameters.AddWithValue("@Fiyat", fiyat);
                                command.Parameters.AddWithValue("@ToplamFiyat", toplamFiyat);
                                command.Parameters.AddWithValue("@SatisTarihi", satisTarihi);
                                command.ExecuteNonQuery();
                            }
                            connectionString.Close();

                            
                            using (SqlConnection stockConnectionString = DBConnection.GetConnection())
                            {
                                stockConnectionString.Open();
                                using (SqlCommand stockCommand = new SqlCommand("UPDATE Stok SET Miktar = Miktar - @Miktar WHERE UrunAdi = @UrunAdi", stockConnectionString))
                                {
                                    stockCommand.Parameters.AddWithValue("@Miktar", 1);
                                    stockCommand.Parameters.AddWithValue("@UrunAdi", urunAdi);
                                    stockCommand.ExecuteNonQuery();
                                }
                                stockConnectionString.Close();
                            }
                        }
                    }
                    MessageBox.Show("Satış onaylandı.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Müşteri adını doldurmadan satışı kapatamazsınız.");
                }
            }
            else
            {
                MessageBox.Show("Sepette ürün bulunmamaktadır, boş satış yapılamaz. Satışa ürün ekleyiniz.");
            }






        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelControl3.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
    }
    

   


