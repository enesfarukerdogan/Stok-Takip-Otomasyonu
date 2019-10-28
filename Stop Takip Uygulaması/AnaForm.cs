using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Stop_Takip_Uygulaması
{
    public partial class AnaForm : DevExpress.XtraEditors.XtraForm
    {
        public AnaForm()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection();
        OleDbCommand komut = new OleDbCommand();
        OleDbDataAdapter adaptor = new OleDbDataAdapter();
        OleDbCommandBuilder build = new OleDbCommandBuilder();
        DataSet ds = new DataSet();

        private void AnaForm_Load(object sender, EventArgs e)
        {
            baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + System.Windows.Forms.Application.StartupPath.ToString() + "\\db.mdb;");
            dataGridDoldur("Select * From Urunler ORDER BY UrunID");
            label3.Caption = DateTime.Now.ToShortDateString() + "   " + DateTime.Now.ToLongTimeString();

        }
        void azalanVeyaKalmayanUrunler()
        {
            try
            {
                listBoxControl1.Items.Clear();
                baglanti.Open();
                komut = new OleDbCommand("Select * From Urunler Where UrunAdet<=" + spinEdit1.Text, baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    listBoxControl1.Items.Add(oku["UrunAd"].ToString());
                }
                baglanti.Close();
            }
            catch
            {
            }
        }

        public void dataGridDoldur(string komut)
        {
            try
            {
                ds.Clear();
                baglanti.Open();
                adaptor = new OleDbDataAdapter(komut,baglanti);
                adaptor.Fill(ds,"Urunler");
                bindingSource1.DataSource=ds.Tables["Urunler"];
                gridControl1.DataSource = bindingSource1;
                //dataNavigator1.DataSource = bindingSource1;
                baglanti.Close();

                gridView1.Columns[0].Caption = "Stok No";
                gridView1.Columns[1].Caption = "Ürün Adı";
                gridView1.Columns[2].Caption = "Kategori";
                gridView1.Columns[3].Caption = "Ürün Açıklaması";
                gridView1.Columns[4].Caption = "Stok Adedi";
                gridView1.Columns[5].Caption = "Birim Adı";
                gridView1.Columns[6].Caption = "Birim Fiyatı";
                gridView1.Columns[7].Caption = "Ekleme Tarihi";
                gridView1.Columns[8].Caption = "Güncelleme Tarihi";
                gridView1.Columns[9].Caption = "Ürün Resmi";

                gridView1.Columns[0].Width = 50;
                gridView1.Columns[1].Width = 150;
                gridView1.Columns[3].Width = 250;

                OleDbCommandBuilder builder = new OleDbCommandBuilder(adaptor);
                adaptor.InsertCommand = builder.GetInsertCommand();
                adaptor.UpdateCommand = builder.GetUpdateCommand();
                adaptor.DeleteCommand = builder.GetDeleteCommand();

                label1.Caption = "Toplam Kayıt: " + gridView1.RowCount.ToString();
                paraHesapla();
                azalanVeyaKalmayanUrunler();
            }
            catch
            {
            }
        }

        public double toplam=0;
        void paraHesapla()
        {
            try
            {
                toplam = 0;

                baglanti.Open();
                komut = new OleDbCommand("Select UrunAdet,UrunFiyat From Urunler", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    if (oku["UrunAdet"].ToString() != "" && oku["UrunFiyat"].ToString() != "")
                    {
                        double deger = Convert.ToDouble(oku["UrunAdet"]) * Convert.ToDouble(oku["UrunFiyat"]);
                        toplam += deger;
                    }
                }
                baglanti.Close();
                label2.Caption = "Toplam Ürün Tutarı: " + toplam.ToString("c");
            }
            catch
            {
            }
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            adaptor.Update(ds.Tables["Urunler"]);
            dataGridDoldur("Select * From Urunler ORDER BY UrunID");
            MessageBox.Show("Tüm değişiklikleriniz kaydedildi.","Başarılı",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            Urunler u = new Urunler();
            u.ShowDialog();
        }

        private void btnGoruntule_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Columns.GridColumn col=new DevExpress.XtraGrid.Columns.GridColumn();//sanal sütun tanımladık
                col=gridView1.Columns[0];//varolan sütünü sanala atadık.
                int[] i = gridView1.GetSelectedRows();//seçili satırın numarasını getirdik
                Urunler u = new Urunler();
                u.durum = 1;
                u.urunID = Convert.ToInt32(gridView1.GetRowCellValue(i[0], col));//seçili satır ve sütunun bilgisini aktardık.
                u.ShowDialog();
            }
            catch
            {
            }
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            Yazdir y = new Yazdir();
            y.SatirSayisi = gridView1.RowCount;
            y.ShowDialog();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            dataGridDoldur("Select * From Urunler ORDER BY UrunID");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Caption = DateTime.Now.ToShortDateString()+"   "+DateTime.Now.ToLongTimeString();
        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit1.SelectedIndex==0)
            {
                dataGridDoldur("Select * from Urunler WHERE UrunAd Like'" + textEdit1.Text + "%'");
            }
            else if (comboBoxEdit1.SelectedIndex == 1)
            {
                dataGridDoldur("Select * from Urunler WHERE KategoriAd Like'" + textEdit1.Text + "%'");
            }
            else if (comboBoxEdit1.SelectedIndex == 2)
            {
                dataGridDoldur("Select * from Urunler WHERE UrunEklemeTarih Like'" + textEdit1.Text + "%'");
            }
            else
            {
                MessageBox.Show("Lütfen bir kriter seçiniz.","Hata",MessageBoxButtons.OK,MessageBoxIcon.Hand);
            }
        }

        private void spinEdit1_TextChanged(object sender, EventArgs e)
        {
            azalanVeyaKalmayanUrunler();
        }

        private void barStaticItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("www.kodbankasi.gen.tr");
        }

        private void btnKategori_Click(object sender, EventArgs e)
        {
            Kategoriler k = new Kategoriler();
            k.ShowDialog();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExceleAktar();
        }
        void ExceleAktar()
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet sheet1 = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                int StartCol = 1;
                int StartRow = 1;
                int j = 0, i = 0;

                DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();

                //başlıkları yazdırıyoruz...
                for (j = 0; j < gridView1.Columns.Count; j++)
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow, StartCol + j];
                    myRange.Value2 = gridView1.Columns[j].Caption;

                    myRange.Font.Bold = true;
                    myRange.Font.Underline = true;
                    myRange.Worksheet.StandardWidth = 18;
                    System.Drawing.Font f = new System.Drawing.Font("Calibri", 12);
                    myRange.Cells.Font.Size = f.Size;
                }

                StartRow++;

                //datagridview içeriğini yazdırıyoruz...
                for (i = 0; i < gridView1.RowCount; i++)
                {
                    for (j = 0; j < gridView1.Columns.Count; j++)
                    {
                        col = gridView1.Columns[j];
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow + i, StartCol + j];
                        myRange.Value2 = gridView1.GetRowCellValue(i, col);
                    }
                }
            }
            catch
            {
            }
        }
        private void btnGrafik_Click(object sender, EventArgs e)
        {
            Raporlama r = new Raporlama();
            r.ShowDialog();
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("www.kodbankasi.gen.tr");
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("www.kodbankasi.gen.tr");
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Yardim y = new Yardim();
            y.ShowDialog();
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FirmaAd = "STOK TAKİP UYGULAMASI ||| kodbankasi.gen.tr ||| Tamamen ücretsizdir. ||| Kullanıcı Firma: " + barEditItem1.EditValue.ToString();
            this.Text = "STOK TAKİP UYGULAMASI ||| kodbankasi.gen.tr ||| Tamamen ücretsizdir. ||| Kullanıcı Firma: " + barEditItem1.EditValue.ToString();
            Properties.Settings.Default.Save();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();//sanal sütun tanımladık
                col = gridView1.Columns[0];//varolan sütünü sanala atadık.
                int[] i = gridView1.GetSelectedRows();//seçili satırın numarasını getirdik
                Urunler u = new Urunler();
                u.durum = 1;
                u.urunID = Convert.ToInt32(gridView1.GetRowCellValue(i[0], col));//seçili satır ve sütunun bilgisini aktardık.
                u.ShowDialog();
            }
            catch 
            {
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Kategoriler k = new Kategoriler();
            k.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Raporlama r = new Raporlama();
            r.ShowDialog();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExceleAktar();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Yazdir y = new Yazdir();
            y.SatirSayisi = gridView1.RowCount;
            y.ShowDialog();
        }

        private void btnYazdir_Click_1(object sender, EventArgs e)
        {
            Yazdir y = new Yazdir();
            y.SatirSayisi = gridView1.RowCount;
            y.ShowDialog();
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Veritabani v = new Veritabani();
            v.xtraTabControl1.SelectedTabPageIndex = 0;
            v.ShowDialog();
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Veritabani v = new Veritabani();
            v.xtraTabControl1.SelectedTabPageIndex = 2;
            v.ShowDialog();
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DialogResult sorgu = MessageBox.Show("Veritabanını tamamen sıfırlamak istediğinize emin misiniz? Bu işlemi bir daha geri alamayacaksınız.", "Lütfen Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (sorgu == DialogResult.Yes)
                {
                    baglanti.Open();
                    OleDbCommand komut = new OleDbCommand("DELETE FROM Urunler", baglanti);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Veritabanı sıfırlama işlemi başarılı.", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dataGridDoldur("SELECT * FROM Urunler ORDER BY UrunID");
                }
            }
            catch 
            {
                
            }
        }
    }
}
