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
    public partial class Kategoriler : DevExpress.XtraEditors.XtraForm
    {
        public Kategoriler()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection();
        OleDbCommand komut = new OleDbCommand();

        private void Kategoriler_Load(object sender, EventArgs e)
        {
            baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + System.Windows.Forms.Application.StartupPath.ToString() + "\\db.mdb;");
            
            kategorileriGetir();
        }
        void kategorileriGetir()
        {
            try
            {
                listBoxControl1.Items.Clear();
                baglanti.Open();
                komut = new OleDbCommand("Select * From Kategoriler", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    listBoxControl1.Items.Add(oku["KategoriAd"].ToString());
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
                baglanti.Open();
                komut = new OleDbCommand("INSERT INTO Kategoriler(KategoriAd,KategoriAciklama) VALUES('" + textEdit2.Text + "','" + memoEdit1.Text + "')", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                kategorileriGetir();
                MessageBox.Show("Kategori eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                komut = new OleDbCommand("DELETE FROM Kategoriler WHERE KategoriAd='" + listBoxControl1.SelectedItem + "'", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                kategorileriGetir();
                MessageBox.Show("Kategori silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
            }
        }
    }
}