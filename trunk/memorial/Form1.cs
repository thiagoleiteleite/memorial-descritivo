using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace memorial
{
    public partial class Form1 : Form
    {
        string arquivo;

        public Form1()
        {
            InitializeComponent();
        }

        void readCsv(string arquivo)
        {
            DataTable dt = new DataTable();

            //Fonte: http://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader            
            using (CachedCsvReader csv = new
            //CachedCsvReader(new StreamReader(arquivo), true))
            CachedCsvReader(new StreamReader(arquivo), true))
            {
                dt.Load(csv);
                dataGridView1.DataSource = dt;
            }           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                arquivo = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            readCsv(arquivo);
        }
    }
}
