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
using System.IO;

namespace Stop_Takip_Uygulaması
{
    public partial class Veritabani : DevExpress.XtraEditors.XtraForm
    {
        public Veritabani()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            simpleButton2.Enabled = false;
            simpleButton1.Enabled = true;

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                simpleButton2.Enabled = true;
                kayityeri2 = saveFileDialog1.FileName + ".kodbankasi";
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            simpleButton1.Enabled = false;
            simpleButton2.Enabled = true;
        }

        string filenameMusteriler = Application.StartupPath.ToString()+"\\db.mdb", kayityeri1;
        string filenameMusteriler2 = Application.StartupPath.ToString() + "\\db.mdb", kayityeri2;
        string adres = DateTime.Now.ToShortDateString();

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                bool x = true;

                Directory.CreateDirectory(@"C:\Stok Takip Uygulaması\Yedek\" + adres);
                kayityeri1 = "C:\\Stok Takip Uygulaması\\Yedek\\" + adres + "\\Veritabani_yedek_" + adres + ".kodbankasi";
                try
                {
                    System.IO.File.Copy(filenameMusteriler, kayityeri1);
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Bugünkü yedekleme işlemini yapmışsınız. Bir günde birden fazla yedekleme işlemi yapmak sabit diskinizde birikmeye yol açar.       " + hata.Message, "Hata ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    x = false;
                }
                if (x)
                {
                    timer1.Enabled = true;
                    progressPanel1.Visible = true;
                    simpleButton2.Enabled = false;
                }

            }
            else
            {
                bool x = true;
                try
                {
                    System.IO.File.Copy(filenameMusteriler2, kayityeri2);
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata Oluştu : " + hata.Message, "Hata ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    x = false;
                }
                if (x)
                {
                    timer1.Enabled = true;
                    progressPanel1.Visible = true;
                    simpleButton2.Enabled = false;
                }
            }
        }

        int tick = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            tick++;
            if (tick == 5)
            {
                tick = 0;
                timer1.Enabled = false;
                progressPanel1.Visible = false;
                MessageBox.Show("Yedekleme işlemi başarılı.", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
            }
        }



        ///////////////////////////GERİ YÜKLEME KODLARI BAŞLADI ////////////////////////////

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked == true)
            {
                simpleButton4.Enabled = true;
            }
            else
            {
                simpleButton4.Enabled = false;
                simpleButton3.Enabled = false;
            }
        }

        string filenameMusteriler3 = "";
        string kayityeri3 = "";

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                filenameMusteriler3 = openFileDialog1.FileName;
                simpleButton3.Enabled = true;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            bool x = true;
            try
            {
                System.IO.File.Delete(Application.StartupPath.ToString() + "\\db.mdb");

                kayityeri3 = Application.StartupPath.ToString() + "\\db.mdb";

                System.IO.File.Copy(filenameMusteriler3, kayityeri3);
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Oluştu : " + hata.Message, "Hata ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                x = false;
            }
            if (x)
            {
                timer2.Enabled = true;
                progressPanel2.Visible = true;
                simpleButton3.Enabled = false;
                simpleButton4.Enabled = false;
                checkEdit1.Checked = false;
            }
        }

        int tick2 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            tick2++;
            if (tick2==5)
            {
                tick2 = 0;
                timer2.Enabled = false;
                simpleButton4.Enabled = true;
                progressPanel2.Visible = false;
                MessageBox.Show("Geri yükleme işlemi başarılı.", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        OleDbConnection baglanti = new OleDbConnection();
        private void VeritabaniIslemleri_Load(object sender, EventArgs e)
        {
            AnaForm a = (AnaForm)Application.OpenForms["AnaForm"];
            baglanti.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+Application.StartupPath.ToString()+"\\db.mdb";

        }

    }
}