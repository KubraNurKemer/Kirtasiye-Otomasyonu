using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ButtonPanel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kırtasiye_Otomasyonu_v1._0
{

    public partial class ReportFormWithQuery : DevExpress.XtraEditors.XtraForm
    {
        public static int setKeyQuery = 0;

        public static string FirstDate, LastDate, UrunAd;

        public static int MaxPriceQuery;

        public static string StockQu;


        public static string noRowsMsg, noRowsImg;

        private void reportlabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        public ReportFormWithQuery()
        {
            InitializeComponent();
        }

        private void ReportFormWithQuery_Load(object sender, EventArgs e)
        {
           

            if (setKeyQuery == 1) // Satış Raporu: Tarih Aralığı Raporu
            {
                SqlConnection connectionRefreshAllData = DBConnection.GetConnection();
                string query = "SELECT * FROM Satislar WHERE SatisTarihi BETWEEN '" + FirstDate + "' AND '" + LastDate + "' ORDER BY SatisTarihi DESC;";
                SqlCommand command = new SqlCommand(query, connectionRefreshAllData);
                connectionRefreshAllData.Open();
                SqlDataReader reader = command.ExecuteReader();
                gridControl1.DataSource = reader;
                gridControl1.RefreshDataSource();
                reportlabel.Caption = FirstDate + " - " + LastDate + " Tarihleri arasındaki tüm satışlar";
            }
            if (setKeyQuery == 2) // Satış Raporu: Maks. Satış Fiyatı
            {
                SqlConnection connectionRefreshAllData = DBConnection.GetConnection();
                string query = "SELECT * FROM Satislar WHERE ToplamFiyat= '" + MaxPriceQuery +"'; ";
                SqlCommand command = new SqlCommand(query, connectionRefreshAllData);
                connectionRefreshAllData.Open();
                SqlDataReader reader = command.ExecuteReader();
                gridControl1.DataSource = reader;
                gridControl1.RefreshDataSource();
                reportlabel.Caption = "Toplam Fiyatı "+MaxPriceQuery + " TL olan satışlar";

            }
            if (setKeyQuery == 3) // Satış Raporu: Ürün Adına Göre
            {
                gridControl1.RefreshDataSource();
                SqlConnection connectionRefreshAllData22 = DBConnection.GetConnection();
                string query = "SELECT * FROM Satislar WHERE UrunAdi= '" + UrunAd + "'; ";
                SqlCommand command = new SqlCommand(query, connectionRefreshAllData22);
                connectionRefreshAllData22.Open();
                SqlDataReader reader = command.ExecuteReader();
                gridControl1.DataSource = reader;
                gridControl1.RefreshDataSource();
                reportlabel.Caption = "Ürün Adı: " + UrunAd + " Olan Satışlar";

            }
             if (setKeyQuery == 4) // Girilen Stok Miktarına Göre Listeleme
            {
                gridControl1.RefreshDataSource();
                SqlConnection connectionRefreshAllData222 = DBConnection.GetConnection();
                string query = "SELECT * FROM Stok WHERE Miktar= '" + StockQu + "'; ";
                SqlCommand command = new SqlCommand(query, connectionRefreshAllData222);
                connectionRefreshAllData222.Open();
                SqlDataReader reader = command.ExecuteReader();
                gridControl1.DataSource = reader;
                gridControl1.RefreshDataSource();
                reportlabel.Caption = "Stok Miktarı: " + StockQu + " Kalan Ürünler";
            }
            
           




        }
    }
}