using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Stop_Takip_Uygulaması
{
    public partial class Yardim : DevExpress.XtraEditors.XtraForm
    {
        public Yardim()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("www.kodbankasi.gen.tr");
        }
    }
}