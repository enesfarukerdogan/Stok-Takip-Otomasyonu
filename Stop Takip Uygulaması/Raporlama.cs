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
    public partial class Raporlama : DevExpress.XtraEditors.XtraForm
    {
        public Raporlama()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection();

        private void Raporlama_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\db.mdb";
            raporOlustur();
        }
        void raporOlustur()
        {
            try
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("Select UrunID From Urunler", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                int toplamUrunCesidi = 0;
                while (oku.Read())
                {
                    toplamUrunCesidi++;
                }
                ///////////////////////////////////////////////////
                double toplamTutar = 0;
                OleDbCommand komut2 = new OleDbCommand("Select UrunAdet,UrunFiyat From Urunler", baglanti);
                OleDbDataReader oku2 = komut2.ExecuteReader();
                while (oku2.Read())
                {
                    if (oku2["UrunAdet"].ToString() != "" && oku2["UrunFiyat"].ToString() != "")
                    {
                        double deger = Convert.ToDouble(oku2["UrunAdet"]) * Convert.ToDouble(oku2["UrunFiyat"]);
                        toplamTutar += deger;
                    }
                }
                ////////////////////////////////////////////////////
                int toplamKategori = 0;
                OleDbCommand komut3 = new OleDbCommand("Select KategoriID From Kategoriler", baglanti);
                OleDbDataReader oku3 = komut3.ExecuteReader();
                while (oku3.Read())
                {
                    toplamKategori++;
                }
                ///////////////////////////////////////////////////
                string sonEklenenUrun = "";
                OleDbCommand komut4 = new OleDbCommand("Select * From Urunler ORDER BY UrunID", baglanti);
                OleDbDataReader oku4 = komut4.ExecuteReader();
                int urunID = 0;
                while (oku4.Read())
                {
                    urunID = Convert.ToInt32(oku4["UrunID"].ToString());
                }
                OleDbCommand komut5 = new OleDbCommand("Select UrunAd From Urunler WHERE UrunID=" + urunID, baglanti);
                OleDbDataReader oku5 = komut5.ExecuteReader();
                while (oku5.Read())
                {
                    sonEklenenUrun = oku5["UrunAd"].ToString();
                }
                ///////////////////////////////////////////////////
                string sonGuncellenenUrun = "";
                OleDbCommand komut6 = new OleDbCommand("Select * From Urunler Order BY UrunGuncellemeTarih", baglanti);
                OleDbDataReader oku6 = komut6.ExecuteReader();
                int urunID2 = 0;
                while (oku6.Read())
                {
                    urunID2 = Convert.ToInt32(oku6["UrunID"].ToString());
                }
                OleDbCommand komut7 = new OleDbCommand("Select UrunAd From Urunler WHERE UrunID=" + urunID2, baglanti);
                OleDbDataReader oku7 = komut7.ExecuteReader();
                while (oku7.Read())
                {
                    sonGuncellenenUrun = oku7["UrunAd"].ToString();
                }
                ////////////////////////////////////////////////////
                string sonGuncellemeTarih = "";
                OleDbCommand komut8 = new OleDbCommand("Select * From Urunler Order By UrunGuncellemeTarih", baglanti);
                OleDbDataReader oku8 = komut8.ExecuteReader();
                while (oku8.Read())
                {
                    sonGuncellemeTarih = oku8["UrunGuncellemeTarih"].ToString();
                }
                ////////////////////////////////////////////////////
                baglanti.Close();

                labelControl1.Text = toplamUrunCesidi.ToString() + " Adet";
                labelControl2.Text = toplamTutar.ToString("c");
                labelControl3.Text = toplamKategori.ToString() + " Adet";
                labelControl4.Text = sonEklenenUrun;
                labelControl5.Text = sonGuncellenenUrun;
                labelControl6.Text = sonGuncellemeTarih;
            }
            catch
            {
            }
        }
    }
}