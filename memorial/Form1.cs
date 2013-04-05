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
using System.Globalization;

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

        //abrir arquivo
        private void button1_Click(object sender, EventArgs e)
            {
            button2.Enabled = false;
            button6.Enabled = false;

            openFileDialog1.Filter = "Arquivo CSV (*.csv)|*.csv|Arquivo TXT (*.txt)|*.txt";
            openFileDialog1.FileName = "";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                {
                arquivo = openFileDialog1.FileName;
                readCsv(arquivo);
                button2.Enabled = true;
                }
            }

        //Ler arquivo
        void readCsv(string arquivo)
            {
            //verificar o caractere separador do arquivo csv ou texto
            char separador = ' ';
            if (radioButton1.Checked == true)
                {
                separador = ';';
                }
            if (radioButton2.Checked == true)
                {
                separador = '\t';
                }

            using (CachedCsvReader csv = new CachedCsvReader(new StreamReader(arquivo), true, separador))
                {
                dt.Clear();
                dt.Columns.Clear();
                dt.Load(csv);

                //Caso o arquivo seja apenas com Ponto;X;Y
                if (dt.Columns.Count == 3)
                    {
                    dt2.Clear();
                    dt2.Columns.Clear();
                    dt2 = dt.Clone();
                    dt2.Columns[1].DataType = typeof(Decimal);
                    dt2.Columns[2].DataType = typeof(Decimal);
                    foreach (DataRow row in dt.Rows)
                        {
                        dt2.ImportRow(row);
                        }

                    //adiciona as colunas
                    if (dt2.Columns.Contains("Distância") == true)
                        {
                        dt2.Columns.Remove("Distância");
                        dt2.Columns.Add(new DataColumn("Distância", typeof(decimal)));
                        }
                    else if (dt2.Columns.Contains("Distância") == false)
                        {
                        dt2.Columns.Add(new DataColumn("Distância", typeof(decimal)));
                        }

                    if (dt2.Columns.Contains("Azimute") == true)
                        {
                        dt2.Columns.Remove("Azimute");
                        dt2.Columns.Add(new DataColumn("Azimute", typeof(string)));
                        }
                    else if (dt2.Columns.Contains("Azimute") == false)
                        {
                        dt2.Columns.Add(new DataColumn("Azimute", typeof(string)));
                        }

                    if (dt2.Columns.Contains("Confrontante") == true)
                        {
                        dt2.Columns.Remove("Confrontante");
                        dt2.Columns.Add(new DataColumn("Confrontante", typeof(string)));
                        }
                    else if (dt2.Columns.Contains("Confrontante") == false)
                        {
                        dt2.Columns.Add(new DataColumn("Confrontante", typeof(string)));
                        }

                    if (dt2.Columns.Contains("Divisa") == true)
                        {
                        dt2.Columns.Remove("Divisa");
                        dt2.Columns.Add(new DataColumn("Divisa", typeof(string)));
                        }
                    else if (dt2.Columns.Contains("Divisa") == false)
                        {
                        dt2.Columns.Add(new DataColumn("Divisa", typeof(string)));
                        }

                    dataGridView1.DataSource = dt2;

                    //formata as casas decimais das coordenadas
                    formataCoordenadaTabela();
                    }
                else
                    {
                    MessageBox.Show("Tabela fora da especificação");
                    }

                //Caso o arquivo seja apenas com Ponto;X;Y;Distância;Azimute;Confrontante;Divisa
                if (dt.Columns.Count == 7)
                    {
                    dt2.Clear();
                    dt2.Columns.Clear();
                    dt2 = dt.Clone();
                    dt2.Columns[1].DataType = typeof(Decimal);
                    dt2.Columns[2].DataType = typeof(Decimal);
                    foreach (DataRow row in dt.Rows)
                        {
                        dt2.ImportRow(row);
                        }

                    //Deixando as coluas editáveis - Parei aqui, não descobri como deixar a coluna editável
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                        if (column.Name == "Distância")
                            {
                            column.ReadOnly = false;
                            }
                        }

                    dataGridView1.DataSource = dt2;

                    //formata as casas decimais das coordenadas
                    formataCoordenadaTabela();
                    }
                }
            }

        //===Não precisa deste código agora===
        //else if (radioButton2.Checked == true)
        //    {
        //    using (CachedCsvReader csv = new
        //    CachedCsvReader(new StreamReader(arquivo), true, '\t'))
        //        {
        //        dt.Clear();
        //        dt.Columns.Clear();
        //        dt.Load(csv);

        //        if (dt.Columns.Count == 3)
        //            {
        //            dt2.Clear();
        //            dt2.Columns.Clear();
        //            dt2 = dt.Clone();
        //            dt2.Columns[1].DataType = typeof(Decimal);
        //            dt2.Columns[2].DataType = typeof(Decimal);
        //            foreach (DataRow row in dt.Rows)
        //                {
        //                dt2.ImportRow(row);
        //                }

        //            dataGridView1.DataSource = dt2;

        //            //Coodernadas
        //            if (numericUpDown2.Value == 0)
        //                {
        //                dataGridView1.Columns[1].DefaultCellStyle.Format = "0";
        //                dataGridView1.Columns[2].DefaultCellStyle.Format = "0";
        //                }
        //            if (numericUpDown2.Value == 1)
        //                {
        //                dataGridView1.Columns[1].DefaultCellStyle.Format = "0.0";
        //                dataGridView1.Columns[2].DefaultCellStyle.Format = "0.0";
        //                }
        //            if (numericUpDown2.Value == 2)
        //                {
        //                dataGridView1.Columns[1].DefaultCellStyle.Format = "0.00";
        //                dataGridView1.Columns[2].DefaultCellStyle.Format = "0.00";
        //                }
        //            if (numericUpDown2.Value == 3)
        //                {
        //                dataGridView1.Columns[1].DefaultCellStyle.Format = "0.000";
        //                dataGridView1.Columns[2].DefaultCellStyle.Format = "0.000";
        //                }
        //            if (numericUpDown2.Value == 4)
        //                {
        //                dataGridView1.Columns[1].DefaultCellStyle.Format = "0.0000";
        //                dataGridView1.Columns[2].DefaultCellStyle.Format = "0.0000";
        //                }
        //            }
        //        else
        //            {
        //            MessageBox.Show("Tabela fora da especificação");
        //            }
        //        }
        //    }

        //Função para converter Grau decimal para grau sexagesimal - Meio tosco!
        private string dd2dms(double valor)
            {
            string CasaDec = Convert.ToString(numericUpDown3.Value);
            switch (CasaDec)
                {
                case "0":
                    return valor.ToString("0° .00´ 00´´").Replace("° ,", "° ");
                    break;
                case "1":
                    return valor.ToString("0° .00´ 00','0´´").Replace("° ,", "° ");
                    break;
                case "2":
                    return valor.ToString("0° .00´ 00','00´´").Replace("° ,", "° ");
                    break;
                case "3":
                    return valor.ToString("0° .00´ 00','000´´").Replace("° ,", "° ");
                    break;
                case "4":
                    return valor.ToString("0° .00´ 00','0000´´").Replace("° ,", "° ");
                    break;
                default:
                    return valor.ToString("0° .00´ 00´´").Replace("° ,", "° ");
                    break;
                }
            }

        //formatação de casas decimais na tabela de coordenada
        void formataCoordenadaTabela()
            {
            if (numericUpDown2.Value == 0)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "0";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "0";
                }
            if (numericUpDown2.Value == 1)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "0.0";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "0.0";
                }
            if (numericUpDown2.Value == 2)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "0.00";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "0.00";
                }
            if (numericUpDown2.Value == 3)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "0.000";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "0.000";
                }
            if (numericUpDown2.Value == 4)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "0.0000";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "0.0000";
                }
            }

        //formatação de cadas decimais nas coordenadas
        private decimal formataCoordenada(decimal valor)
            {
            string CasaDec = Convert.ToString(numericUpDown2.Value);
            switch (CasaDec)
                {
                case "0":
                    return Convert.ToDecimal(valor.ToString("0"));
                    break;
                case "1":
                    return Convert.ToDecimal(valor.ToString("0.0"));
                    break;
                case "2":
                    return Convert.ToDecimal(valor.ToString("0.00"));
                    break;
                case "3":
                    return Convert.ToDecimal(valor.ToString("0.000"));
                    break;
                case "4":
                    return Convert.ToDecimal(valor.ToString("0.0000"));
                    break;
                default:
                    return Convert.ToDecimal(valor.ToString("0.000"));
                    break;
                }
            }

        //formatação de casas decimais da distância
        private decimal formataDist(double valor)
            {
            string CasaDec = Convert.ToString(numericUpDown1.Value);
            switch (CasaDec)
                {
                case "0":
                    return Convert.ToDecimal(valor.ToString("0"));
                    break;
                case "1":
                    return Convert.ToDecimal(valor.ToString("0.0"));
                    break;
                case "2":
                    return Convert.ToDecimal(valor.ToString("0.00"));
                    break;
                case "3":
                    return Convert.ToDecimal(valor.ToString("0.000"));
                    break;
                case "4":
                    return Convert.ToDecimal(valor.ToString("0.0000"));
                    break;
                default:
                    return Convert.ToDecimal(valor.ToString("0.000"));
                    break;
                }
            }

        //Cálculo de Azimute e distância
        private void button2_Click(object sender, EventArgs e)
            {



            //dt2.Columns.Add(new DataColumn("Distância", typeof(decimal)));
            //dt2.Columns.Add(new DataColumn("Azimute", typeof(decimal)));

            for (int i = 0; i < dt2.Rows.Count; i++)
                {
                if (i == 0)
                    {

                    //Cálculo da distância
                    double X = Convert.ToDouble(dt2.Rows[i][1]);
                    double Xant = Convert.ToDouble(dt2.Rows[dt2.Rows.Count - 1][1]);
                    double difX = X - Xant;
                    double Y = Convert.ToDouble(dt2.Rows[i][2]);
                    double Yant = Convert.ToDouble(dt2.Rows[dt2.Rows.Count - 1][2]);
                    double difY = Y - Yant;
                    dt2.Rows[i]["Distância"] = formataDist(Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2)));

                    //Cálculo do azimute
                    double rprov1 = Math.Atan(difX / difY) * (180 / Math.PI) * Math.Sign(difX / difY);
                    double azprov1 = rprov1;
                    double azprov2 = 180 - rprov1;
                    double azprov3 = 180 + rprov1;
                    double azprov4 = 360 - rprov1;
                    double azprov1b = ((Math.Truncate(azprov1 * Math.Sign(azprov1)) + (Math.Truncate((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1))) / 100) + (((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1) - Math.Truncate((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1))) * 60 / 10000)) * Math.Sign(azprov1);
                    double azprov2b = ((Math.Truncate(azprov2 * Math.Sign(azprov2)) + (Math.Truncate((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2))) / 100) + (((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2) - Math.Truncate((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2))) * 60 / 10000)) * Math.Sign(azprov2);
                    double azprov3b = ((Math.Truncate(azprov3 * Math.Sign(azprov3)) + (Math.Truncate((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3))) / 100) + (((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3) - Math.Truncate((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3))) * 60 / 10000)) * Math.Sign(azprov3);
                    double azprov4b = ((Math.Truncate(azprov4 * Math.Sign(azprov4)) + (Math.Truncate((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4))) / 100) + (((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4) - Math.Truncate((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4))) * 60 / 10000)) * Math.Sign(azprov4);

                    //formatação do azimute
                    //format(azprov2b, "0° .00´ 00','00´´").ToString.Replace(",", "");
                    string azprov1bf = dd2dms(azprov1b);
                    string azprov2bf = dd2dms(azprov2b);
                    string azprov3bf = dd2dms(azprov3b);
                    string azprov4bf = dd2dms(azprov4b);

                    if (difY > 0 && difX > 0)
                        {
                        dt2.Rows[i]["Azimute"] = azprov1bf;
                        }
                    if (difY < 0 && difX > 0)
                        {
                        dt2.Rows[i]["Azimute"] = azprov2bf;
                        }
                    if (difY < 0 && difX < 0)
                        {
                        dt2.Rows[i]["Azimute"] = azprov3bf;
                        }
                    if (difY > 0 && difX < 0)
                        {
                        dt2.Rows[i]["Azimute"] = azprov4bf;
                        }

                    //formataçao de coordenadas na tabela
                    formataCoordenadaTabela();
                    }
                else
                    {
                    //Cálculo da distância
                    double X = Convert.ToDouble(dt2.Rows[i][1]);
                    double Xant = Convert.ToDouble(dt2.Rows[i - 1][1]);
                    double difX = X - Xant;
                    double Y = Convert.ToDouble(dt2.Rows[i][2]);
                    double Yant = Convert.ToDouble(dt2.Rows[i - 1][2]);
                    double difY = Y - Yant;
                    dt2.Rows[i]["Distância"] = formataDist(Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2)));

                    //Cálculo do azimute
                    double rprov1 = Math.Atan(difX / difY) * (180 / Math.PI) * Math.Sign(difX / difY);
                    double azprov1 = rprov1;
                    double azprov2 = 180 - rprov1;
                    double azprov3 = 180 + rprov1;
                    double azprov4 = 360 - rprov1;
                    double azprov1b = ((Math.Truncate(azprov1 * Math.Sign(azprov1)) + (Math.Truncate((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1))) / 100) + (((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1) - Math.Truncate((azprov1 - (Math.Truncate(azprov1 * Math.Sign(azprov1)) * Math.Sign(azprov1))) * 60 * Math.Sign(azprov1))) * 60 / 10000)) * Math.Sign(azprov1);
                    double azprov2b = ((Math.Truncate(azprov2 * Math.Sign(azprov2)) + (Math.Truncate((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2))) / 100) + (((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2) - Math.Truncate((azprov2 - (Math.Truncate(azprov2 * Math.Sign(azprov2)) * Math.Sign(azprov2))) * 60 * Math.Sign(azprov2))) * 60 / 10000)) * Math.Sign(azprov2);
                    double azprov3b = ((Math.Truncate(azprov3 * Math.Sign(azprov3)) + (Math.Truncate((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3))) / 100) + (((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3) - Math.Truncate((azprov3 - (Math.Truncate(azprov3 * Math.Sign(azprov3)) * Math.Sign(azprov3))) * 60 * Math.Sign(azprov3))) * 60 / 10000)) * Math.Sign(azprov3);
                    double azprov4b = ((Math.Truncate(azprov4 * Math.Sign(azprov4)) + (Math.Truncate((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4))) / 100) + (((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4) - Math.Truncate((azprov4 - (Math.Truncate(azprov4 * Math.Sign(azprov4)) * Math.Sign(azprov4))) * 60 * Math.Sign(azprov4))) * 60 / 10000)) * Math.Sign(azprov4);

                    //formatação do azimute
                    string azprov1bf = dd2dms(azprov1b);
                    string azprov2bf = dd2dms(azprov2b);
                    string azprov3bf = dd2dms(azprov3b);
                    string azprov4bf = dd2dms(azprov4b);

                    if (difY > 0 && difX > 0)
                        {
                        dt2.Rows[i]["Azimute"] = azprov1bf;
                        }
                    if (difY < 0 && difX > 0)
                        {
                        dt2.Rows[i]["Azimute"] = azprov2bf;
                        }
                    if (difY < 0 && difX < 0)
                        {
                        dt2.Rows[i]["Azimute"] = azprov3bf;
                        }
                    if (difY > 0 && difX < 0)
                        {
                        dt2.Rows[i]["Azimute"] = azprov4bf;
                        }
                    //formataçao de coordenadas na tabela
                    formataCoordenadaTabela();
                    }
                button6.Enabled = true;
                }


            //Distância
            //if (numericUpDown1.Value == 0)
            //    {
            //    dataGridView1.Columns[3].DefaultCellStyle.Format = "0";
            //    }
            //if (numericUpDown1.Value == 1)
            //    {
            //    dataGridView1.Columns[3].DefaultCellStyle.Format = "0.0";
            //    }
            //if (numericUpDown1.Value == 2)
            //    {
            //    dataGridView1.Columns[3].DefaultCellStyle.Format = "0.00";
            //    }
            //if (numericUpDown1.Value == 3)
            //    {
            //    dataGridView1.Columns[3].DefaultCellStyle.Format = "0.000";
            //    }
            //if (numericUpDown1.Value == 4)
            //    {
            //    dataGridView1.Columns[3].DefaultCellStyle.Format = "0.0000";
            //    }

            //Azimute - Não precisa mais
            //if (numericUpDown3.Value == 0)
            //{
            //  //  dataGridView1.Columns[4].DefaultCellStyle.Format = "0° .00´ 00´´";           
            //}
            //if (numericUpDown3.Value == 1)
            //{
            ////    dataGridView1.Columns[4].DefaultCellStyle.Format = "0° .00´ 00','0´´";
            //}
            //if (numericUpDown3.Value == 2)
            //{
            //  //  dataGridView1.Columns[4].DefaultCellStyle.Format = "0° .00´ 00','00´´";
            //}
            //if (numericUpDown3.Value == 3)
            //{
            //   // dataGridView1.Columns[4].DefaultCellStyle.Format = "0° .00´ 00','000´´";
            //}
            //if (numericUpDown3.Value == 4)
            //{
            ////    dataGridView1.Columns[4].DefaultCellStyle.Format = "0° .00´ 00','0000´´";
            //}

            }

        //Gerar memorial descritivo
        private void button3_Click(object sender, EventArgs e)
            {
            if (dt2.Columns.Count == 6)
                {
                richTextBox1.Text = "";
                richTextBox1.Font = new Font("Times", 12);

                //Título
                richTextBox1.AppendLine();
                richTextBox1.AppendLine();
                if (checkBox3.Checked == true)
                    {
                    richTextBox1.AppendTituloNegrito(textBox9.Text);
                    }
                else
                    {
                    richTextBox1.AppendTitulo(textBox9.Text);
                    }

                //Cálculo perímetro
                double per = 0;
                for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                    per = per + Convert.ToDouble(dt2.Rows[i][3]);
                    }

                //Cabeçalho
                if (textBox10.TextLength > 0)
                    {
                    if (checkBox4.Checked == true)
                        {
                        richTextBox1.AppendBold("Imóvel: ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular("Imóvel: ");
                        }
                    if (checkBox6.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox10.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox10.Text);
                        }
                    richTextBox1.AppendLine();
                    }
                if (textBox12.TextLength > 0)
                    {
                    if (checkBox5.Checked == true)
                        {
                        richTextBox1.AppendBold("Município: ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular("Município: ");
                        }
                    if (checkBox10.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox12.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox12.Text);
                        }
                    richTextBox1.AppendLine();
                    }
                if (textBox13.TextLength > 0)
                    {
                    if (checkBox9.Checked == true)
                        {
                        richTextBox1.AppendBold("Matrícula: ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular("Matrícula: ");
                        }
                    if (checkBox8.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox13.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox13.Text);
                        }
                    richTextBox1.AppendLine();
                    }
                if (textBox14.TextLength > 0)
                    {
                    if (checkBox7.Checked == true)
                        {
                        richTextBox1.AppendBold("Comarca: ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular("Comarca: ");
                        }
                    if (checkBox16.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox14.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox14.Text);
                        }
                    richTextBox1.AppendLine();
                    }
                if (textBox19.TextLength > 0)
                    {
                    if (checkBox15.Checked == true)
                        {
                        richTextBox1.AppendBold("Código INCRA: ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular("Código INCRA: ");
                        }
                    if (checkBox14.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox19.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox19.Text);
                        }
                    richTextBox1.AppendLine();
                    }
                if (textBox20.TextLength > 0)
                    {
                    if (checkBox4.Checked == true)
                        {
                        richTextBox1.AppendBold("Área: ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular("Área: ");
                        }
                    if (checkBox6.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox20.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox20.Text);
                        }
                    richTextBox1.AppendLine();
                    }
                if (checkBox1.Checked == true)
                    {
                    if (checkBox11.Checked == true)
                        {
                        richTextBox1.AppendBold("Perímetro: ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular("Perímetro: ");
                        }

                    if (numericUpDown1.Value == 0)
                        {
                        if (checkBox22.Checked == true)
                            {
                            richTextBox1.AppendBold(per.ToString("0") + " " + textBox8.Text);
                            }
                        else
                            {
                            richTextBox1.AppendRegular(per.ToString("0") + " " + textBox8.Text);
                            }
                        }
                    if (numericUpDown1.Value == 1)
                        {
                        if (checkBox22.Checked == true)
                            {
                            richTextBox1.AppendBold(per.ToString("0.0") + " " + textBox8.Text);
                            }
                        else
                            {
                            richTextBox1.AppendRegular(per.ToString("0.0") + " " + textBox8.Text);
                            }
                        }
                    if (numericUpDown1.Value == 2)
                        {
                        if (checkBox22.Checked == true)
                            {
                            richTextBox1.AppendBold(per.ToString("0.00") + " " + textBox8.Text);
                            }
                        else
                            {
                            richTextBox1.AppendRegular(per.ToString("0.00") + " " + textBox8.Text);
                            }
                        }
                    if (numericUpDown1.Value == 3)
                        {
                        if (checkBox22.Checked == true)
                            {
                            richTextBox1.AppendBold(per.ToString("0.000") + " " + textBox8.Text);
                            }
                        else
                            {
                            richTextBox1.AppendRegular(per.ToString("0.000") + " " + textBox8.Text);
                            }
                        }
                    if (numericUpDown1.Value == 4)
                        {
                        if (checkBox22.Checked == true)
                            {
                            richTextBox1.AppendBold(per.ToString("0.0000") + " " + textBox8.Text);
                            }
                        else
                            {
                            richTextBox1.AppendRegular(per.ToString("0.0000") + " " + textBox8.Text);
                            }
                        }
                    richTextBox1.AppendLine();
                    }

                if (textBox11.TextLength > 0)
                    {
                    if (checkBox21.Checked == true)
                        {
                        richTextBox1.AppendBold("Proprietário: ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular("Proprietário: ");
                        }
                    if (checkBox20.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox11.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox11.Text);
                        }
                    richTextBox1.AppendLine();
                    }

                decimal X1 = 0;
                decimal Y1 = 0;
                X1 = formataCoordenada(Convert.ToDecimal(dt2.Rows[0][1]));
                Y1 = formataCoordenada(Convert.ToDecimal(dt2.Rows[0][2]));

                //string X1 = null;
                //string Y1 = null;
                //X1 = Convert.ToString(dt2.Rows[0][1]);
                //Y1 = Convert.ToString(dt2.Rows[0][2]);
                //if (numericUpDown2.Value == 0)
                //    {
                //    X1 = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0");
                //    Y1 = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0");
                //    }
                //if (numericUpDown2.Value == 1)
                //    {
                //    X1 = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.0");
                //    Y1 = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.0");
                //    }
                //if (numericUpDown2.Value == 2)
                //    {
                //    X1 = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.00");
                //    Y1 = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.00");
                //    }
                //if (numericUpDown2.Value == 3)
                //    {
                //    X1 = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.000");
                //    Y1 = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.000");
                //    }
                //if (numericUpDown2.Value == 4)
                //    {
                //    X1 = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.0000");
                //    Y1 = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.0000");
                //    }

                //Começo do texto do memorial

                richTextBox1.AppendLine();
                //Parágrafo
                richTextBox1.AppendRegular("\t" + textBox1.Text + " ");
                //Ponto
                richTextBox1.AppendRegular(Convert.ToString(dt2.Rows[0][0]) + " ");
                //Texto coordenada
                richTextBox1.AppendRegular(textBox2.Text + " ");
                //Coord X
                if (checkBox17.Checked == true)
                    {
                    richTextBox1.AppendBold(textBox3.Text + " " + X1 + " " + textBox8.Text);
                    }
                else
                    {
                    richTextBox1.AppendRegular(textBox3.Text + " " + X1 + " " + textBox8.Text);
                    }
                richTextBox1.AppendRegular(" e ");
                //Coord Y
                if (checkBox17.Checked == true)
                    {
                    richTextBox1.AppendBold(textBox4.Text + " " + Y1 + " " + textBox8.Text);
                    }
                else
                    {
                    richTextBox1.AppendRegular(textBox4.Text + " " + Y1 + " " + textBox8.Text);
                    }
                richTextBox1.AppendRegular(", ");

                //Confrontante
                if (Convert.ToString(dt2.Rows[0][5]).Length > 0)
                    {
                    richTextBox1.AppendRegular(textBox21.Text);

                    if (checkBox23.Checked == true)
                        {
                        richTextBox1.AppendBold(" " + dt2.Rows[0][5]);
                        richTextBox1.AppendRegular(", ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular(" " + dt2.Rows[0][5] + ", ");
                        }
                    }

                //Percorre os pontos
                for (int i = 1; i < dt2.Rows.Count; i++)
                    {
                    string ponto = Convert.ToString(dt2.Rows[i][0]);

                    //Coodernadas
                    decimal X = 0;
                    decimal Y = 0;
                    X = formataCoordenada(Convert.ToDecimal(dt2.Rows[i][1]));
                    Y = formataCoordenada(Convert.ToDecimal(dt2.Rows[i][2]));

                    //if (numericUpDown2.Value == 0)
                    //    {
                    //    X = Convert.ToDecimal(dt2.Rows[i][1]).ToString("0");
                    //    Y = Convert.ToDecimal(dt2.Rows[i][2]).ToString("0");
                    //    }
                    //if (numericUpDown2.Value == 1)
                    //    {
                    //    X = Convert.ToDecimal(dt2.Rows[i][1]).ToString("0.0");
                    //    Y = Convert.ToDecimal(dt2.Rows[i][2]).ToString("0.0");
                    //    }
                    //if (numericUpDown2.Value == 2)
                    //    {
                    //    X = Convert.ToDecimal(dt2.Rows[i][1]).ToString("0.00");
                    //    Y = Convert.ToDecimal(dt2.Rows[i][2]).ToString("0.00");
                    //    }
                    //if (numericUpDown2.Value == 3)
                    //    {
                    //    X = Convert.ToDecimal(dt2.Rows[i][1]).ToString("0.000");
                    //    Y = Convert.ToDecimal(dt2.Rows[i][2]).ToString("0.000");
                    //    }
                    //if (numericUpDown2.Value == 4)
                    //    {
                    //    X = Convert.ToDecimal(dt2.Rows[i][1]).ToString("0.0000");
                    //    Y = Convert.ToDecimal(dt2.Rows[i][2]).ToString("0.0000");
                    //    }


                    //Distância
                    string dist = null;
                    dist = Convert.ToString(dt2.Rows[i][3]);
                    //if (numericUpDown1.Value == 0)
                    //{
                    //    dist = Convert.ToDecimal(dt2.Rows[i][3]).ToString("0");
                    //}
                    //if (numericUpDown1.Value == 1)
                    //{
                    //    dist = Convert.ToDecimal(dt2.Rows[i][3]).ToString("0.0");
                    //}
                    //if (numericUpDown1.Value == 2)
                    //{
                    //    dist = Convert.ToDecimal(dt2.Rows[i][3]).ToString("0.00");
                    //}
                    //if (numericUpDown1.Value == 3)
                    //{
                    //    dist = Convert.ToDecimal(dt2.Rows[i][3]).ToString("0.000");
                    //}
                    //if (numericUpDown1.Value == 4)
                    //{
                    //    dist = Convert.ToDecimal(dt2.Rows[i][3]).ToString("0.0000");
                    //}

                    //Azimute
                    string azi = null;
                    azi = Convert.ToString(dt2.Rows[i][4]);
                    //if (numericUpDown3.Value == 0)
                    //{
                    ////azi = Convert.ToString(dt2.Rows[i][4]);
                    //}
                    //if (numericUpDown3.Value == 1)
                    //{
                    // //   azi = Convert.ToDecimal(dt2.Rows[i][4]).ToString("0° .00´ 00','0´´");
                    //}
                    //if (numericUpDown3.Value == 2)
                    //{
                    // //   azi = Convert.ToDecimal(dt2.Rows[i][4]).ToString("0° .00´ 00','00´´");
                    //}
                    //if (numericUpDown3.Value == 3)
                    //{
                    ////    azi = Convert.ToDecimal(dt2.Rows[i][4]).ToString("0° .00´ 00','000´´");
                    //}
                    //if (numericUpDown3.Value == 4)
                    //{
                    ////    azi = Convert.ToDecimal(dt2.Rows[i][4]).ToString("0° .00´ 00','0000´´");
                    //}


                    richTextBox1.AppendRegular(
                        //Ligação
                        textBox5.Text +
                        //Ponto
                        " " + ponto +
                        //Coordenadas
                        " " + textBox2.Text + " ");
                    //Coord X
                    if (checkBox17.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox3.Text + " " + X + " " + textBox8.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox3.Text + " " + X + " " + textBox8.Text);
                        }
                    richTextBox1.AppendRegular(" e ");
                    //Coord Y
                    if (checkBox17.Checked == true)
                        {
                        richTextBox1.AppendBold(textBox4.Text + " " + Y + " " + textBox8.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(textBox4.Text + " " + Y + " " + textBox8.Text);
                        }
                    richTextBox1.AppendRegular(", ");

                    ////Tipo coordenada X
                    //" " + textBox3.Text +
                    //" " + X + " " + textBox8.Text + " e " +
                    ////Tipo coordenada Y
                    //" " + textBox4.Text +
                    //" " + Y + " " + textBox8.Text + ", "+ 

                    //Azimute
                    richTextBox1.AppendRegular(" " + textBox6.Text +
                    " " + azi +
                        //Distância
                    " " + textBox7.Text +
                    " " + dist +
                    " " + textBox8.Text + "; ");

                    if (Convert.ToString(dt2.Rows[i][5]).Length > 0)
                        {
                        richTextBox1.AppendRegular(textBox21.Text + " " + dt2.Rows[i][5] + ", ");
                        }

                    }
                string pontof = Convert.ToString(dt2.Rows[0][0]);

                //Coodernadas
                decimal Xf = 0;
                decimal Yf = 0;
                Xf = formataCoordenada(Convert.ToDecimal(dt2.Rows[0][1]));
                Yf = formataCoordenada(Convert.ToDecimal(dt2.Rows[0][2]));

                //string Xf = null;
                //string Yf = null;
                //if (numericUpDown2.Value == 0)
                //    {
                //    Xf = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.0000");
                //    Yf = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.0000");
                //    }
                //if (numericUpDown2.Value == 1)
                //    {
                //    Xf = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.0");
                //    Yf = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.0");
                //    }
                //if (numericUpDown2.Value == 2)
                //    {
                //    Xf = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.00");
                //    Yf = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.00");
                //    }
                //if (numericUpDown2.Value == 3)
                //    {
                //    Xf = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.000");
                //    Yf = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.000");
                //    }
                //if (numericUpDown2.Value == 4)
                //    {
                //    Xf = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.0000");
                //    Yf = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.0000");
                //    }

                //Distância
                string distf = null;
                distf = Convert.ToString(dt2.Rows[0][3]);

                //if (numericUpDown1.Value == 0)
                //    {
                //    distf = Convert.ToDecimal(dt2.Rows[0][3]).ToString("0.00");
                //    }
                //if (numericUpDown1.Value == 1)
                //    {
                //    distf = Convert.ToDecimal(dt2.Rows[0][3]).ToString("0.0");
                //    }
                //if (numericUpDown1.Value == 2)
                //    {
                //    distf = Convert.ToDecimal(dt2.Rows[0][3]).ToString("0.00");
                //    }
                //if (numericUpDown1.Value == 3)
                //    {
                //    distf = Convert.ToDecimal(dt2.Rows[0][3]).ToString("0.000");
                //    }
                //if (numericUpDown1.Value == 4)
                //    {
                //    distf = Convert.ToDecimal(dt2.Rows[0][3]).ToString("0.0000");
                //    }

                //Azimute
                string azif = null;
                azif = Convert.ToString(dt2.Rows[0][4]);

                //if (numericUpDown3.Value == 0)
                //{
                //azif = Convert.ToString(dt2.Rows[0][4]);
                //}
                //if (numericUpDown3.Value == 1)
                //{
                //azif = Convert.ToString(dt2.Rows[0][4]);
                //}
                //if (numericUpDown3.Value == 2)
                //{
                //azif = Convert.ToString(dt2.Rows[0][4]);
                //}
                //if (numericUpDown3.Value == 3)
                //{
                //azif = Convert.ToString(dt2.Rows[0][4]);
                //}
                //if (numericUpDown3.Value == 4)
                //{
                //azif = Convert.ToString(dt2.Rows[0][4]);
                //}

                //string Xf = Convert.ToDecimal(dt2.Rows[0][1]).ToString("0.0000");
                //string Yf = Convert.ToDecimal(dt2.Rows[0][2]).ToString("0.0000");
                //string distf = Convert.ToDecimal(dt2.Rows[0][3]).ToString("0.00");
                //string azif = Convert.ToDecimal(dt2.Rows[0][4]).ToString("0° .00´ 00','00´´");

                richTextBox1.AppendRegular(textBox5.Text + " " + pontof +
                        " " + textBox2.Text + " ");

                //Coord X
                if (checkBox17.Checked == true)
                    {
                    richTextBox1.AppendBold(textBox3.Text + " " + Xf + " " + textBox8.Text);
                    }
                else
                    {
                    richTextBox1.AppendRegular(textBox3.Text + " " + Xf + " " + textBox8.Text);
                    }
                richTextBox1.AppendRegular(" e ");
                //Coord Y
                if (checkBox17.Checked == true)
                    {
                    richTextBox1.AppendBold(textBox4.Text + " " + Yf + " " + textBox8.Text);
                    }
                else
                    {
                    richTextBox1.AppendRegular(textBox4.Text + " " + Yf + " " + textBox8.Text);
                    }
                richTextBox1.AppendRegular(", ");

                //" " + Xf + " " + textBox8.Text + " e " +
                //" " + textBox4.Text +
                //" " + Yf + " " + textBox8.Text + ", " +

                richTextBox1.AppendRegular(" " + textBox6.Text +
                " " + azif +
                " " + textBox7.Text +
                " " + distf +
                " " + textBox8.Text + "; ");

                richTextBox1.AppendRegular(textBox15.Text);

                //Perímetro
                if (numericUpDown1.Value == 0)
                    {
                    richTextBox1.AppendRegular(" " + per.ToString("0") + " " + textBox8.Text + ".");
                    }
                if (numericUpDown1.Value == 1)
                    {
                    richTextBox1.AppendRegular(" " + per.ToString("0.0") + " " + textBox8.Text + ".");
                    }
                if (numericUpDown1.Value == 2)
                    {
                    richTextBox1.AppendRegular(" " + per.ToString("0.00") + " " + textBox8.Text + ".");
                    }
                if (numericUpDown1.Value == 3)
                    {
                    richTextBox1.AppendRegular(" " + per.ToString("0.000") + " " + textBox8.Text + ".");
                    }
                if (numericUpDown1.Value == 4)
                    {
                    richTextBox1.AppendRegular(" " + per.ToString("0.0000") + " " + textBox8.Text + ".");
                    }

                if (textBox22.TextLength > 0 || checkBox2.Checked == true || radioButton3.Checked == true || radioButton4.Checked == true || textBox16.TextLength > 0 || textBox17.TextLength > 0)
                    {
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    }

                //Cidade
                if (textBox22.TextLength > 0)
                    {
                    richTextBox1.AppendCenter(textBox22.Text);
                    }

                //Data
                if (checkBox2.Checked == true)
                    {
                    //richTextBox1.AppendLine();                  

                    if (radioButton3.Checked == true)
                        {
                        CultureInfo culture = new CultureInfo("pt-BR");
                        DateTimeFormatInfo dtfi = culture.DateTimeFormat;

                        int dia = DateTime.Now.Day;
                        int ano = DateTime.Now.Year;
                        string mes = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(DateTime.Now.Month));
                        string diasemana = culture.TextInfo.ToTitleCase(dtfi.GetDayName(DateTime.Now.DayOfWeek));
                        string data = diasemana + ", " + dia + " de " + mes + " de " + ano;

                        if (textBox22.TextLength > 0)
                            {
                            richTextBox1.AppendCenter(", " + data);
                            }
                        else
                            {
                            richTextBox1.AppendCenter(data);
                            }

                        }
                    if (radioButton4.Checked == true)
                        {
                        richTextBox1.AppendCenter(textBox23.Text);
                        }
                    }

                //Profissional
                if (textBox16.TextLength > 0)
                    {
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendCenter(textBox18.Text);
                    }
                if (textBox16.TextLength > 0)
                    {
                    richTextBox1.AppendLine();
                    richTextBox1.AppendCenter(textBox16.Text);
                    }
                if (textBox17.TextLength > 0)
                    {
                    richTextBox1.AppendLine();
                    richTextBox1.AppendCenter(textBox17.Text);
                    }

                richTextBox1.AppendLine();



                //    //Cálculo da área
                //    double p1 = 0;
                //    for (int i = 0; i < dt2.Rows.Count-1; i++)
                //    {
                //        p1 = p1 + Convert.ToDouble(dt2.Rows[i][1]) * Convert.ToDouble(dt2.Rows[i + 1][2]);
                //    }

                //    double p2 = 0;
                //    for (int i = 0; i < dt2.Rows.Count-1; i++)
                //    {
                //        p2 = p2 + Convert.ToDouble(dt2.Rows[i][2]) * Convert.ToDouble(dt2.Rows[i + 1][1]);
                //    }

                //    double area = (p1 - p2)/2;            
                //    richTextBox1.AppendRegular(" " + area.ToString("0.0000") + " ha.");
                }
            else
                {
                MessageBox.Show("Necessário importar arquivo CSV e cálcular distâncias e azimutes para escrever o memorial descritivo");
                }


            }

        private void button5_Click(object sender, EventArgs e)
            {
            Clipboard.SetDataObject(richTextBox1.Text, true);
            }

        private void button4_Click(object sender, EventArgs e)
            {
            saveFileDialog1.Filter = "Formato Rich Text (*.rtf)|*.rtf";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                richTextBox1.SaveFile(saveFileDialog1.FileName);
                }

            }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
            }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
            }

        private void Form1_Load(object sender, EventArgs e)
            {
            linkLabel1.Text = "http://code.google.com/p/memorial-descritivo/";
            linkLabel1.Links.Add(0, 100, "http://code.google.com/p/memorial-descritivo/");

            linkLabel2.Text = "- LumenWorks.Framework.IO.Csv";
            linkLabel2.Links.Add(0, 100, "http://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader");
            }

        private void button6_Click(object sender, EventArgs e)
            {
            saveFileDialog2.Filter = "CSV separado por ponto e vírgula|*.csv";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(saveFileDialog2.FileName);

                string strHeader = "";

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {

                    strHeader += dataGridView1.Columns[i].HeaderText + ";";

                    }

                streamWriter.WriteLine(strHeader);

                for (int m = 0; m < dataGridView1.Rows.Count; m++)
                    {

                    string strRowValue = "";

                    for (int n = 0; n < dataGridView1.Columns.Count; n++)
                        {

                        strRowValue += dataGridView1.Rows[m].Cells[n].Value + ";";

                        }

                    streamWriter.WriteLine(strRowValue);

                    }

                streamWriter.Close();
                }


            }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
            {
            if (checkBox2.Checked == true)
                {
                radioButton3.Enabled = true;
                radioButton3.Checked = true;
                radioButton4.Enabled = true;
                }
            if (checkBox2.Checked == false)
                {
                radioButton3.Enabled = false;
                radioButton3.Checked = false;
                radioButton4.Enabled = false;
                textBox23.Enabled = false;
                }
            }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
            {
            if (radioButton4.Checked == true)
                {
                textBox23.Enabled = true;
                }

            if (radioButton4.Checked == false)
                {
                textBox23.Enabled = false;
                }
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
            //int ss = ed.SelectionStart;
            //ed.AppendText(s);
            //int sl = ed.SelectionStart - ss + 1;

            Font bold = new Font(ed.Font, FontStyle.Bold);
            Font regular = new Font(ed.Font, FontStyle.Regular);
            //ed.Select(ss, sl);
            ed.SelectionFont = bold;
            ed.AppendText(s);
            ed.SelectionFont = regular;
            //ed.SelectionAlignment = HorizontalAlignment.Left;
            }

        public static void AppendTitulo(this RichTextBox ed, string s)
            {
            Font regular = new Font(ed.Font, FontStyle.Regular);
            ed.SelectionFont = regular;
            ed.SelectionAlignment = HorizontalAlignment.Center;
            ed.AppendText(s);
            ed.SelectionFont = regular;
            //ed.SelectionAlignment = HorizontalAlignment.Left;
            ed.AppendText(NewLine);
            ed.AppendText(NewLine);
            }

        public static void AppendTituloNegrito(this RichTextBox ed, string s)
            {
            Font bold = new Font(ed.Font, FontStyle.Bold);
            Font regular = new Font(ed.Font, FontStyle.Regular);
            ed.SelectionFont = bold;
            ed.SelectionAlignment = HorizontalAlignment.Center;
            ed.AppendText(s);
            ed.SelectionFont = regular;
            //ed.SelectionAlignment = HorizontalAlignment.Left;
            ed.AppendText(NewLine);
            ed.AppendText(NewLine);
            }

        public static void AppendCenter(this RichTextBox ed, string s)
            {
            int ss = ed.SelectionStart;
            ed.AppendText(s);
            int sl = ed.SelectionStart - ss + 1;

            Font regular = new Font(ed.Font, FontStyle.Regular);
            ed.Select(ss, sl);
            ed.SelectionFont = regular;
            ed.SelectionAlignment = HorizontalAlignment.Center;
            //ed.AppendText(NewLine);
            }

        public static void AppendRegular(this RichTextBox ed, string s)
            {
            //int ss = ed.SelectionStart;
            //ed.AppendText(s);
            //int sl = ed.SelectionStart - ss + 1;

            //Font regular = new Font(ed.Font, FontStyle.Regular);
            //ed.Select(ss, sl);
            //ed.SelectionFont = regular;
            //ed.SelectionAlignment = HorizontalAlignment.Left;
            Font regular = new Font(ed.Font, FontStyle.Regular);
            ed.SelectionFont = regular;
            ed.AppendText(s);
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