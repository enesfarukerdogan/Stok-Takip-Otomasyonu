using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.OleDb;

namespace Stop_Takip_Uygulaması
{
    public partial class Yazdir : DevExpress.XtraEditors.XtraForm
    {
        public Yazdir()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection();

        public int SatirSayisi = 0;

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                AnaForm a = (AnaForm)Application.OpenForms["AnaForm"];

                int i = 0;

                //ÇİZİM BAŞLANGICI
                Font myFont = new Font("Calibri", 7);
                SolidBrush sbrush = new SolidBrush(Color.Black);
                Pen myPen = new Pen(Color.Black);

                e.Graphics.DrawString("Düzenlenme Tarihi: " + DateTime.Now.ToLongDateString() + "   " + DateTime.Now.ToLongTimeString(), myFont, sbrush, 50, 25);
                //e.Graphics.DrawString("Uygulanan filtre: " + a.buttonEdit1.Text, myFont, sbrush, 650, 25);

                e.Graphics.DrawLine(myPen, 50, 45, 770, 45); // 1. Kalem, 2. X, 3. Y Koordinatı, 4. Uzunluk, 5. BitişX 

                myFont = new Font("Calibri", 15, FontStyle.Bold);
                e.Graphics.DrawString("Ürün Listesi", myFont, sbrush, 350, 65);
                e.Graphics.DrawLine(myPen, 50, 95, 770, 95);

                myFont = new Font("Calibri", 10, FontStyle.Bold);
                e.Graphics.DrawString("Stok No", myFont, sbrush, 50, 110);
                e.Graphics.DrawString("Ürün Adı", myFont, sbrush, 120, 110);
                e.Graphics.DrawString("Kategori", myFont, sbrush, 350, 110);
                e.Graphics.DrawString("Stok Adedi", myFont, sbrush, 500, 110);
                e.Graphics.DrawString("Birim Adı", myFont, sbrush, 600, 110);
                e.Graphics.DrawString("Birim Fiyatı", myFont, sbrush, 700, 110);

                e.Graphics.DrawLine(myPen, 50, 125, 770, 125);

                int y = 150;

                myFont = new Font("Calibri", 10);

                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("Select * From Urunler ORDER BY UrunID", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();


                while (oku.Read())
                {
                    e.Graphics.DrawString(oku["UrunID"].ToString(), myFont, sbrush, 50, y);
                    e.Graphics.DrawString(oku["UrunAd"].ToString(), myFont, sbrush, 120, y);
                    e.Graphics.DrawString(oku["KategoriAd"].ToString(), myFont, sbrush, 350, y);
                    e.Graphics.DrawString(oku["UrunAdet"].ToString(), myFont, sbrush, 500, y);
                    e.Graphics.DrawString(oku["UrunBirim"].ToString(), myFont, sbrush, 600, y);
                    e.Graphics.DrawString(Convert.ToDouble(oku["UrunFiyat"]).ToString("c"), myFont, sbrush, 700, y);

                    y += 20;

                    i += 1;


                    //yeni sayfaya geçme kontrolü
                    if (y > 1000)
                    {
                        e.Graphics.DrawString("(Devamı -->)", myFont, sbrush, 700, y + 50);
                        y = 50;
                        break; //burada yazdırma sınırına ulaştığımız için while döngüsünden çıkıyoruz
                        //çıktığımızda whil baştan başlıyor i değişkeni değer almaya devam ediyor
                        //yazdırma yeni sayfada başlamış oluyor
                    }
                }

                //çoklu sayfa kontrolü
                if (i < SatirSayisi)
                {
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                    i = 0;
                }


                StringFormat myStringFormat = new StringFormat();
                myStringFormat.Alignment = StringAlignment.Far;
            }
            catch
            {
            }
        }

        private void Yazdir_Load(object sender, EventArgs e)
        {
            baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + System.Windows.Forms.Application.StartupPath.ToString() + "\\db.mdb;");
            
        }
    }
}