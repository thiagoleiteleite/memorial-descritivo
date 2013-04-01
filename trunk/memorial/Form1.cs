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
            dataGridView1.Columns[3].DefaultCellStyle.Format = "0.0000";
            dataGridView1.Columns[4].DefaultCellStyle.Format = "0° .00´ 00','00´´";
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