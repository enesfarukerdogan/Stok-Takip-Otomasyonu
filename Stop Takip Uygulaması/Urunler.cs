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
    public partial class Urunler : DevExpress.XtraEditors.XtraForm
    {
        public Urunler()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection();
        OleDbCommand komut = new OleDbCommand();

        public int durum = 0;
        public int urunID=0;

        private void Urunler_Load(object sender, EventArgs e)
        {
            baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + System.Windows.Forms.Application.StartupPath.ToString() + "\\db.mdb;");
            if (durum==1)
            {
                bilgileriGetir();
            }
            kategorileriGetir();
        }
        void kategorileriGetir()
        {
            try
            {
                textEdit3.Properties.Items.Clear();
                baglanti.Open();
                komut = new OleDbCommand("Select * From Kategoriler", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    textEdit3.Properties.Items.Add(oku["KategoriAd"].ToString());
                }
                baglanti.Close();
            }
            catch
            {
            }
        }
        void bilgileriGetir()
        {
            try
            {
                baglanti.Open();
                komut = new OleDbCommand("Select * From Urunler Where UrunID=" + urunID, baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    textEdit1.Text = oku["UrunID"].ToString();
                    textEdit2.Text = oku["UrunAd"].ToString();
                    textEdit3.Text = oku["KategoriAd"].ToString();
                    textEdit4.Text = oku["UrunAdet"].ToString();
                    textEdit5.Text = oku["UrunBirim"].ToString();
                    textEdit6.Text = oku["UrunFiyat"].ToString();
                    memoEdit1.Text = oku["UrunAciklama"].ToString();
                }
                baglanti.Close();
            }
            catch
            {
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (durum == 1)
                {
                    baglanti.Open();
                    komut = new OleDbCommand("UPDATE Urunler SET UrunAd='" + textEdit2.Text + "',KategoriAd='" + textEdit3.Text + "',UrunAdet=" + textEdit4.Text + ",UrunBirim='" + textEdit5.Text + "',UrunFiyat='" + textEdit6.Text + "',UrunAciklama='" + memoEdit1.Text + "',UrunGuncellemeTarih='" + DateTime.Now.ToString() + "' WHERE UrunID=" + urunID, baglanti);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Değişiklikleriniz kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();

                    AnaForm a = (AnaForm)Application.OpenForms["Anaform"];
                    a.dataGridDoldur("Select * From Urunler ORDER BY UrunID");
                }
                else
                {
                    baglanti.Open();
                    komut = new OleDbCommand("INSERT INTO Urunler(UrunAd,KategoriAd,UrunAdet,UrunBirim,UrunFiyat,UrunAciklama,UrunEklemeTarih) VALUES('" + textEdit2.Text + "','" + textEdit3.Text + "'," + textEdit4.Text + ",'" + textEdit5.Text + "','" + textEdit6.Text + "','" + memoEdit1.Text + "','" + DateTime.Now.ToString() + "')", baglanti);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Ürününüz kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    AnaForm a = (AnaForm)Application.OpenForms["Anaform"];
                    a.dataGridDoldur("Select * From Urunler ORDER BY UrunID");

                    textEdit6.Text = "";
                    textEdit5.Text = "";
                    textEdit4.Text = "";
                    textEdit3.Text = "";
                    textEdit2.Text = "";
                    textEdit1.Text = "";
                    memoEdit1.Text = "";
                }
            }
            catch
            {
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Şu anda aktif değildir.","Mesaj",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}