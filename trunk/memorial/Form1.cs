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
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        void readCsv(string arquivo)
        {           
            using (CachedCsvReader csv = new
            // Separado por virgula
            //CachedCsvReader(new StreamReader(arquivo), true))
            // Separado por tab
            //CachedCsvReader(new StreamReader(arquivo), true, '\t'))
            // Separado por ponto-e-vírgula
            CachedCsvReader(new StreamReader(arquivo), true, ';'))

            {
                dt.Load(csv);

                dt2 = dt.Clone();
                dt2.Columns[1].DataType = typeof(Decimal);
                dt2.Columns[2].DataType = typeof(Decimal);
                foreach (DataRow row in dt.Rows)
                {
                    dt2.ImportRow(row);
                }

                dataGridView1.DataSource = dt2;
            }           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                arquivo = openFileDialog1.FileName;
                readCsv(arquivo);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dt2.Columns.Add(new DataColumn("Distância", typeof(decimal)));
            dt2.Columns.Add(new DataColumn("Azimute", typeof(decimal)));

            for (int i = 1; i < dt2.Rows.Count; i++)
            {
                //Cálculo da distância
                double X = Convert.ToDouble(dt2.Rows[i]["X"]);
                double Xant = Convert.ToDouble(dt2.Rows[i-1]["X"]);
                double difX = X - Xant;
                double Y = Convert.ToDouble(dt2.Rows[i]["Y"]);
                double Yant = Convert.ToDouble(dt2.Rows[i - 1]["Y"]);
                double difY = Y - Yant;                
                dt2.Rows[i]["Distância"] = Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2));

                //Cálculo do azimute
                double rprov1 = Math.Atan(difX / difY) * (180/Math.PI) * Math.Sign(difX/difY);
                double azprov1 = rprov1;
                double azprov2 = 180 - rprov1;
                double azprov3 = 180 + rprov1;
                double azprov4 = 360 - rprov1;
                double azprov1b = ((Math.Truncate(azprov1 * Math.Sign(azprov1)) + (Math.Truncate((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1))) / 100) + (((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1) - Math.Truncate((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1))) * 60 / 10000)) * Math.Sign(azprov1);
                double azprov2b = ((Math.Truncate(azprov2 * Math.Sign(azprov2)) + (Math.Truncate((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2))) / 100) + (((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2) - Math.Truncate((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2))) * 60 / 10000)) * Math.Sign(azprov2);
                double azprov3b = ((Math.Truncate(azprov3 * Math.Sign(azprov3)) + (Math.Truncate((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3))) / 100) + (((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3) - Math.Truncate((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3))) * 60 / 10000)) * Math.Sign(azprov3);
                double azprov4b = ((Math.Truncate(azprov4 * Math.Sign(azprov4)) + (Math.Truncate((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4))) / 100) + (((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4) - Math.Truncate((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4))) * 60 / 10000)) * Math.Sign(azprov4);
                if (difY > 0 && difX > 0)
                {
                    dt2.Rows[i]["Azimute"] = azprov1b;
                }
                if (difY < 0 && difX > 0)
                {
                    dt2.Rows[i]["Azimute"] = azprov2b;
                }
                if (difY < 0 && difX < 0)
                {
                    dt2.Rows[i]["Azimute"] = azprov3b;
                }
                if (difY > 0 && difX < 0)
                {
                    dt2.Rows[i]["Azimute"] = azprov4b;
                }
            }

            dataGridView1.DataSource = dt2;            
            dataGridView1.Columns[3].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[4].DefaultCellStyle.Format = "0° .00´ 00','00´´";
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            dt3 = dt.Clone();
            dt3.Columns[1].DataType = typeof(Decimal);
            dt3.Columns[2].DataType = typeof(Decimal);
            foreach (DataRow row in dt2.Rows)
            {
                dt3.ImportRow(row);
            }
            richTextBox1.Text = "";
            richTextBox1.Font = new Font("Helvetica", 10);

            richTextBox1.AppendLine();
            richTextBox1.AppendLine();
            richTextBox1.AppendTitulo(textBox9.Text);

            if (textBox10.TextLength > 0)
            {               
                richTextBox1.AppendRegular("Imóvel: " + textBox10.Text);
                richTextBox1.AppendLine();
            }
            if (textBox12.TextLength > 0)
            {
                richTextBox1.AppendRegular("Município: " + textBox12.Text);
                richTextBox1.AppendLine();
            }
            if (textBox13.TextLength > 0)
            {
                richTextBox1.AppendRegular("Matrícula: " + textBox13.Text);
                richTextBox1.AppendLine();
            }
            if (textBox14.TextLength > 0)
            {
                richTextBox1.AppendRegular("Comarca: " + textBox14.Text);
                richTextBox1.AppendLine();
            }
            if (textBox11.TextLength > 0)
            {
                richTextBox1.AppendRegular("Proprietário: " + textBox11.Text);
                richTextBox1.AppendLine();
            }

            richTextBox1.AppendLine();
            richTextBox1.AppendRegular(textBox1.Text + " ");
            richTextBox1.AppendRegular(Convert.ToString(dt3.Rows[0][0]) + " ");
            richTextBox1.AppendRegular(textBox2.Text + " ");
            richTextBox1.AppendRegular(textBox3.Text + " " + Convert.ToString(dt3.Rows[0][1]) + " ");
            richTextBox1.AppendRegular(" e ");
            richTextBox1.AppendRegular(textBox4.Text + " " + Convert.ToString(dt3.Rows[0][2]) + ", ");
            for (int i = 1; i < dt2.Rows.Count; i++)
            {
                string ponto = Convert.ToString(dt2.Rows[i][0]);
                string X = Convert.ToDecimal(dt2.Rows[i][1]).ToString("0.0000");
                string Y = Convert.ToDecimal(dt2.Rows[i][2]).ToString("0.0000");
                string dist = Convert.ToDecimal(dt2.Rows[i][3]).ToString("0.00");
                string azi = Convert.ToDecimal(dt2.Rows[i][4]).ToString("0° .00´ 00','00´´");
                richTextBox1.AppendRegular(textBox5.Text + " " + ponto + 
                    " " + textBox2.Text +
                    " " + textBox3.Text +
                    " " + X + " e " +
                    " " + textBox4.Text +
                    " " + Y + ", " +
                    " " + textBox6.Text +
                    " " + azi +
                    " " + textBox7.Text +
                    " " + dist + 
                    " " + textBox8.Text +"; ");
            }
            richTextBox1.AppendRegular(" " + textBox15.Text);
            
        }

    }

    public static class RichTextBoxExtensions
    {
        const string NewLine = "\r\n";

        public static void AppendLine(this RichTextBox ed)
        {
            ed.AppendText(NewLine);
        }

        public static void AppendLine(this RichTextBox ed, string s)
        {
            ed.AppendText(s + NewLine);
        }

        public static void AppendBold(this RichTextBox ed, string s)
        {
            int ss = ed.SelectionStart;
            ed.AppendText(s);
            int sl = ed.SelectionStart - ss + 1;

            Font bold = new Font(ed.Font, FontStyle.Bold);
            ed.Select(ss, sl);
            ed.SelectionFont = bold;
            ed.SelectionAlignment = HorizontalAlignment.Left;
        }

        public static void AppendTitulo(this RichTextBox ed, string s)
        {
            int ss = ed.SelectionStart;
            ed.AppendText(s);
            int sl = ed.SelectionStart - ss + 1;

            Font bold = new Font(ed.Font, FontStyle.Bold);
            ed.Select(ss, sl);
            ed.SelectionFont = bold;
            ed.SelectionAlignment = HorizontalAlignment.Center;
            ed.AppendText(NewLine);
            ed.AppendText(NewLine);
        }

        public static void AppendRegular(this RichTextBox ed, string s)
        {
            int ss = ed.SelectionStart;
            ed.AppendText(s);
            int sl = ed.SelectionStart - ss + 1;

            Font regular = new Font(ed.Font, FontStyle.Regular);
            ed.Select(ss, sl);
            ed.SelectionFont = regular;
            ed.SelectionAlignment = HorizontalAlignment.Left;
        }
    }
}

//Links:
// http://stackoverflow.com/questions/8852863/datatable-foreach-row-except-first-one
// http://stackoverflow.com/questions/5467860/how-to-calculate-an-expression-for-each-row-of-a-datatable-and-add-new-column-to
// http://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader
// http://stackoverflow.com/questions/9107916/sorting-rows-in-a-data-table
// http://stackoverflow.com/questions/3751929/save-text-from-rich-text-box-with-c-sharp
// http://www.c-sharpcorner.com/uploadfile/mahesh/richtextbox-in-C-Sharp/