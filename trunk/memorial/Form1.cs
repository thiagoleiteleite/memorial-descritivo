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


            openFileDialog1.Filter = "Arquivo CSV (*.csv)|*.csv|Arquivo TXT (*.txt)|*.txt";
            openFileDialog1.FileName = "";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                {
                arquivo = openFileDialog1.FileName;
                readCsv(arquivo);
                button2.Enabled = true;
                button6.Enabled = true;
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

            System.Text.Encoding codificacao = null;
            if (radioButton6.Checked == true)
                {
                codificacao = Encoding.Default;
                }
            else if (radioButton5.Checked == true)
                {
                codificacao = Encoding.UTF8;
                }
            else if (radioButton7.Checked == true)
                {
                codificacao = System.Text.Encoding.GetEncoding("iso-8859-1");
                }

            using (CachedCsvReader csv = new CachedCsvReader(new StreamReader(arquivo, codificacao), true, separador))
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
                    dt2.Columns[1].DataType = typeof(double);
                    dt2.Columns[2].DataType = typeof(double);
                    foreach (DataRow row in dt.Rows)
                        {
                        dt2.ImportRow(row);
                        }

                    //adiciona as colunas
                    if (dt2.Columns.Contains("Distância") == true)
                        {
                        dt2.Columns.Remove("Distância");
                        dt2.Columns.Add(new DataColumn("Distância", typeof(string)));
                        }
                    else if (dt2.Columns.Contains("Distância") == false)
                        {
                        dt2.Columns.Add(new DataColumn("Distância", typeof(string)));
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

                //Caso o arquivo seja com Ponto;X;Y;Distância;Azimute;Confrontante;Divisa
                else if (dt.Columns.Count == 7)
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

                    //Deixando as colunas editáveis
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                        if ((column.Name == "Distância") || (column.Name == "Azimute") || (column.Name == "Confrontante") || (column.Name == "Divisa"))
                            {
                            column.ReadOnly = false;
                            }
                        }

                    dt2.Columns["Distância"].ReadOnly = false;
                    dt2.Columns["Azimute"].ReadOnly = false;
                    dt2.Columns["Confrontante"].ReadOnly = false;
                    dt2.Columns["Divisa"].ReadOnly = false;

                    dataGridView1.DataSource = dt2;

                    //formata as casas decimais das coordenadas
                    formataCoordenadaTabela();
                    }
                else
                    {
                    MessageBox.Show("Tabela fora da especificação");
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
                    return valor.ToString("0° .00´ 00´´").Replace("° ,", "° ").Replace("´", "'");
                    break;
                case "1":
                    return valor.ToString("0° .00´ 00','0´´").Replace("° ,", "° ").Replace("´", "'");
                    break;
                case "2":
                    return valor.ToString("0° .00´ 00','00´´").Replace("° ,", "° ").Replace("´", "'");
                    break;
                case "3":
                    return valor.ToString("0° .00´ 00','000´´").Replace("° ,", "° ").Replace("´", "'");
                    break;
                case "4":
                    return valor.ToString("0° .00´ 00','0000´´").Replace("° ,", "° ").Replace("´", "'");
                    break;
                default:
                    return valor.ToString("0° .00´ 00´´").Replace("° ,", "° ").Replace("´", "'");
                    break;
                }
            }

        //formatação de casas decimais na tabela de coordenada
        void formataCoordenadaTabela()
            {
            if (numericUpDown2.Value == 0)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "#,###.";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "#,###.";
                }
            if (numericUpDown2.Value == 1)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "#,###0.0";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "#,###0.0";
                }
            if (numericUpDown2.Value == 2)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "#,###0.00";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "#,###0.00";
                }
            if (numericUpDown2.Value == 3)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "#,###0.000";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "#,###0.000";
                }
            if (numericUpDown2.Value == 4)
                {
                dataGridView1.Columns[1].DefaultCellStyle.Format = "#,###0.0000";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "#,###0.0000";
                }
            }

        //formatação de cadas decimais nas coordenadas
        private string formataCoordenada(double valor)
            {
            string CasaDec = Convert.ToString(numericUpDown2.Value);
            switch (CasaDec)
                {
                case "0":
                    //  return Convert.ToDecimal(valor.ToString("#,###."));
                    return valor.ToString("#,###.");
                    break;
                case "1":
                    //  return Convert.ToDecimal(valor.ToString("#,###0.0"));
                    return valor.ToString("#,###.0");
                    break;
                case "2":
                    // return Convert.ToDecimal(valor.ToString("#,###0.00"));
                    return valor.ToString("#,###.00");
                    break;
                case "3":
                    // return Convert.ToDecimal(valor.ToString("#,###0.000"));
                    return valor.ToString("#,###.000");
                    break;
                case "4":
                    //return Convert.ToDecimal(valor.ToString("#,###0.0000"));
                    return valor.ToString("#,###.0000");
                    break;
                default:
                    //return Convert.ToDecimal(valor.ToString("#,###0.000"));
                    return valor.ToString("#,###.000");
                    break;
                }
            }

        //formatação de casas decimais da distância
        private string formataDist(double valor)
            {
            string CasaDec = Convert.ToString(numericUpDown1.Value);
            switch (CasaDec)
                {
                case "0":
                    return valor.ToString("#,###.");
                    break;
                case "1":
                    return valor.ToString("#,###0.0");
                    break;
                case "2":
                    return valor.ToString("#,###0.00");
                    break;
                case "3":
                    return valor.ToString("#,###0.000");
                    break;
                case "4":
                    return valor.ToString("#,###0.0000");
                    break;
                default:
                    return valor.ToString("#,###0.00");
                    //  return Convert.ToDouble(valor.ToString("#,###0.000"));
                    break;
                }
            }

        //Cálculo de Azimute e distância
        private void button2_Click(object sender, EventArgs e)
            {

            //dt2.Columns.Add(new DataColumn("Distância", typeof(decimal)));
            //dt2.Columns.Add(new DataColumn("Azimute", typeof(decimal)));

            //Número total de pontos ou número da linha do último ponto
            dataGridView1.Refresh();

            int numPontos = dt2.Rows.Count - 1;

            //Coordenadas primeiro ponto
            double xInicio = Convert.ToDouble(dt2.Rows[0][1]);
            double yInicio = Convert.ToDouble(dt2.Rows[0][2]);

            //Coordenadas último ponto
            double xUltimo = Convert.ToDouble(dt2.Rows[numPontos][1]);
            double yUlitmo = Convert.ToDouble(dt2.Rows[numPontos][2]);

            //verifica se o primeiro ponto é igual ao último. Caso for, não calcula
            if ((xInicio == xUltimo) && (yInicio == yUlitmo))
                {
                MessageBox.Show("A coordenada do último ponto é igual ao primeiro ponto, favor deletar último ponto no arquivo de origem.", "Memorial Descritivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                }

            //Variáveis de área e perímetro
            double Aream2 = 0;
            double per = 0;

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
                    double dist = Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2));

                    dt2.Rows[dt2.Rows.Count - 1]["Distância"] = formataDist(Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2)));

                    //Cálculo da área
                    Aream2 = (Xant * Y) - (Yant * X);

                    //Perimetro
                    per = dist;

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
                        dt2.Rows[dt2.Rows.Count - 1]["Azimute"] = azprov1bf;
                        }
                    if (difY < 0 && difX > 0)
                        {
                        dt2.Rows[dt2.Rows.Count - 1]["Azimute"] = azprov2bf;
                        }
                    if (difY < 0 && difX < 0)
                        {
                        dt2.Rows[dt2.Rows.Count - 1]["Azimute"] = azprov3bf;
                        }
                    if (difY > 0 && difX < 0)
                        {
                        dt2.Rows[dt2.Rows.Count - 1]["Azimute"] = azprov4bf;
                        }

                    //Casos especiais no cálculo do Azimute
                    if (difX < 0 && difY == 0)
                        {
                        dt2.Rows[dt2.Rows.Count - 1]["Azimute"] = "270°";
                        }
                    if (difX > 0 && difY == 0)
                        {
                        dt2.Rows[dt2.Rows.Count - 1]["Azimute"] = "90°";
                        }
                    if (difX == 0 && difY > 0)
                        {
                        dt2.Rows[dt2.Rows.Count - 1]["Azimute"] = "0°";
                        }
                    if (difX == 0 && difY < 0)
                        {
                        dt2.Rows[dt2.Rows.Count - 1]["Azimute"] = "180°";
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
                    double dist = (Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2)));

                    dt2.Rows[i - 1]["Distância"] = formataDist(Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2)));

                    //Cálculo da área
                    Aream2 = Aream2 + (Xant * Y) - (Yant * X);

                    //Perimetro
                    per = per + dist;

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
                        dt2.Rows[i - 1]["Azimute"] = azprov1bf;
                        }
                    if (difY < 0 && difX > 0)
                        {
                        dt2.Rows[i - 1]["Azimute"] = azprov2bf;
                        }
                    if (difY < 0 && difX < 0)
                        {
                        dt2.Rows[i - 1]["Azimute"] = azprov3bf;
                        }
                    if (difY > 0 && difX < 0)
                        {
                        dt2.Rows[i - 1]["Azimute"] = azprov4bf;
                        }

                    //Casos especiais no cálculo do Azimute
                    if (difX < 0 && difY == 0)
                        {
                        dt2.Rows[i - 1]["Azimute"] = "270°";
                        }
                    if (difX > 0 && difY == 0)
                        {
                        dt2.Rows[i - 1]["Azimute"] = "90°";
                        }
                    if (difX == 0 && difY > 0)
                        {
                        dt2.Rows[i - 1]["Azimute"] = "0°";
                        }
                    if (difX == 0 && difY < 0)
                        {
                        dt2.Rows[i - 1]["Azimute"] = "180°";
                        }

                    //formataçao de coordenadas na tabela
                    formataCoordenadaTabela();
                    }

                }//fim do for

            button6.Enabled = true;

            //Colocar o valor de área e perímetro nas Caixas de Texto

            //Área em m²
            Aream2 = Math.Abs(Aream2 / 2);
            txtArea.Text = formataDist(Aream2);
            //Área em ha
            txtAreaha.Text = formataDist(Aream2 / 10000);

            //Perimetro
            textBox2.Text = formataDist(per);

            }

        //Gerar memorial descritivo
        private void button3_Click(object sender, EventArgs e)
            {
            if (dt2.Columns.Count == 7)
                {

                //Barra de progresso
                progressBar1.Minimum = 0;
                progressBar1.Maximum = dt2.Rows.Count;
                progressBar1.Value = 0;

                richTextBox1.Text = "";
                richTextBox1.Font = new Font("Times", 12);

                //Título
                richTextBox1.AppendLine();
                richTextBox1.AppendLine();
                if (checkBox3.Checked == true)
                    {
                    richTextBox1.AppendTituloNegrito(txtTitulo.Text);
                    }
                else
                    {
                    richTextBox1.AppendTitulo(txtTitulo.Text);
                    }

                //Cálculo perímetro
                //double per = 0;
                //for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //    per = per + Convert.ToDouble(dt2.Rows[i][3]);
                //    }

                //Cabeçalho
                if (txtImovel.TextLength > 0)
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
                        richTextBox1.AppendBold(txtImovel.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(txtImovel.Text);
                        }
                    richTextBox1.AppendLine();
                    }

                //Município
                if (txtMunicipio.TextLength > 0)
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
                        richTextBox1.AppendBold(txtMunicipio.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(txtMunicipio.Text);
                        }
                    richTextBox1.AppendLine();
                    }

                //Matrícula
                if (txtMatricula.TextLength > 0)
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
                        richTextBox1.AppendBold(txtMatricula.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(txtMatricula.Text);
                        }
                    richTextBox1.AppendLine();
                    }

                //Comarca
                if (txtComarca.TextLength > 0)
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
                        richTextBox1.AppendBold(txtComarca.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(txtComarca.Text);
                        }
                    richTextBox1.AppendLine();
                    }

                //Cód. Incra
                if (txtCodIncra.TextLength > 0)
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
                        richTextBox1.AppendBold(txtCodIncra.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(txtCodIncra.Text);
                        }
                    richTextBox1.AppendLine();
                    }

                //Área m²
                if (chkArea.Checked == true)
                    {
                    if (txtArea.TextLength > 0)
                        {
                        if (checkBox13.Checked == true)
                            {
                            richTextBox1.AppendBold("Área: ");
                            }
                        else
                            {
                            richTextBox1.AppendRegular("Área: ");
                            }

                        //Área ha
                        string TextoArea = "";
                        if (chkAreaha.Checked == true)
                            {
                            TextoArea = txtArea.Text + " m²" + " ou " + txtAreaha.Text + " ha";
                            }
                        else
                            {
                            TextoArea = txtArea.Text + " m²";
                            }

                        if (checkBox12.Checked == true)
                            {
                            richTextBox1.AppendBold(TextoArea);
                            }
                        else
                            {
                            richTextBox1.AppendRegular(TextoArea);
                            }
                        richTextBox1.AppendLine();
                        }
                    }

                //Perímetro
                if (chkPer.Checked == true)
                    {
                    if (textBox2.TextLength > 0)
                        {
                        if (checkBox13.Checked == true)
                            {
                            richTextBox1.AppendBold("Perímetro: ");
                            }
                        else
                            {
                            richTextBox1.AppendRegular("Perímetro: ");
                            }
                        if (checkBox12.Checked == true)
                            {
                            richTextBox1.AppendBold(textBox2.Text + " m");
                            }
                        else
                            {
                            richTextBox1.AppendRegular(textBox2.Text + " m");
                            }
                        richTextBox1.AppendLine();
                        }
                    }

                //if (numericUpDown1.Value == 0)
                //    {
                //    if (checkBox22.Checked == true)
                //        {
                //        richTextBox1.AppendBold(per.ToString("#,###.") + " " + txtUnidade.Text);
                //        }
                //    else
                //        {
                //        richTextBox1.AppendRegular(per.ToString("#,###.") + " " + txtUnidade.Text);
                //        }
                //    }
                //if (numericUpDown1.Value == 1)
                //    {
                //    if (checkBox22.Checked == true)
                //        {
                //        richTextBox1.AppendBold(per.ToString("#,###0.0") + " " + txtUnidade.Text);
                //        }
                //    else
                //        {
                //        richTextBox1.AppendRegular(per.ToString("#,###0.0") + " " + txtUnidade.Text);
                //        }
                //    }
                //if (numericUpDown1.Value == 2)
                //    {
                //    if (checkBox22.Checked == true)
                //        {
                //        richTextBox1.AppendBold(per.ToString("#,###0.00") + " " + txtUnidade.Text);
                //        }
                //    else
                //        {
                //        richTextBox1.AppendRegular(per.ToString("#,###0.00") + " " + txtUnidade.Text);
                //        }
                //    }
                //if (numericUpDown1.Value == 3)
                //    {
                //    if (checkBox22.Checked == true)
                //        {
                //        richTextBox1.AppendBold(per.ToString("#,###0.000") + " " + txtUnidade.Text);
                //        }
                //    else
                //        {
                //        richTextBox1.AppendRegular(per.ToString("#,###0.000") + " " + txtUnidade.Text);
                //        }
                //    }
                //if (numericUpDown1.Value == 4)
                //    {
                //    if (checkBox22.Checked == true)
                //        {
                //        richTextBox1.AppendBold(per.ToString("#,###0.0000") + " " + txtUnidade.Text);
                //        }
                //    else
                //        {
                //        richTextBox1.AppendRegular(per.ToString("#,###0.0000") + " " + txtUnidade.Text);
                //        }
                //    }


                //Proprietário
                if (txtProprietario.TextLength > 0)
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
                        richTextBox1.AppendBold(txtProprietario.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(txtProprietario.Text);
                        }
                    richTextBox1.AppendLine();
                    }


                //Começo do texto do memorial
                richTextBox1.AppendLine();

                //Parágrafo
                richTextBox1.AppendRegular("\t" + txtInicio.Text + " ");

                //Nome ponto
                string ponto = null;

                //Coodernadas
                string X = null;
                string Y = null;

                //Azimute Inicial
                string azi = null;

                //Distância Inicial
                string dist = null;

                //Confrontante
                string nome = null;
                string nome2 = null;

                //Divisa
                string divisa = null;
                string divisa2 = null;

                //Percorre os pontos                
                for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                    //Nome ponto
                    ponto = Convert.ToString(dt2.Rows[i][0]);
                    richTextBox1.AppendRegular(txtLigacao.Text + " ");
                    if (checkBox24.Checked == true)
                        {
                        richTextBox1.AppendBold(ponto);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(ponto);
                        }
                    richTextBox1.AppendRegular(", " + txtCoord.Text + " ");

                    //Coodernadas
                    X = formataCoordenada(Convert.ToDouble(dt2.Rows[i][1]));
                    Y = formataCoordenada(Convert.ToDouble(dt2.Rows[i][2]));

                    //Coord X
                    if (checkBox17.Checked == true)
                        {
                        richTextBox1.AppendBold(txtEste.Text + " " + X + " " + txtUnidade.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(txtEste.Text + " " + X + " " + txtUnidade.Text);
                        }
                    richTextBox1.AppendRegular(" e ");

                    //Coord Y
                    if (checkBox17.Checked == true)
                        {
                        richTextBox1.AppendBold(txtNorte.Text + " " + Y + " " + txtUnidade.Text);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(txtNorte.Text + " " + Y + " " + txtUnidade.Text);
                        }
                    //richTextBox1.AppendRegular(", ");

                    //Confrontante
                    nome = Convert.ToString(dt2.Rows[i][5]);

                    //Divisa
                    divisa = Convert.ToString(dt2.Rows[i][6]);

                    if (i < dt2.Rows.Count)
                        {
                        if (i == 0)
                            {
                            nome2 = Convert.ToString(dt2.Rows[i][5]);
                            divisa2 = Convert.ToString(dt2.Rows[i][6]);
                            }
                        else
                            {
                            nome2 = Convert.ToString(dt2.Rows[i - 1][5]);
                            divisa2 = Convert.ToString(dt2.Rows[i - 1][6]);
                            }
                        }
                    else
                        {
                        nome2 = Convert.ToString(dt2.Rows[0][5]);
                        divisa2 = Convert.ToString(dt2.Rows[0][6]);
                        }
                    if (nome.Length > 0)
                        {
                        if (i == 0)
                            {
                            richTextBox1.AppendRegular(", " + txtConfronto.Text);
                            if (checkBox23.Checked == true) //negrito
                                {
                                richTextBox1.AppendBold(" " + dt2.Rows[i][5]);
                                richTextBox1.AppendRegular(", ");
                                richTextBox1.AppendRegular(txtDivisa.Text);
                                richTextBox1.AppendBold(" " + dt2.Rows[i][6]);
                                }
                            else
                                {
                                richTextBox1.AppendRegular(" " + dt2.Rows[i][5] + ", ");
                                richTextBox1.AppendRegular(txtDivisa.Text);
                                richTextBox1.AppendRegular(" " + dt2.Rows[i][6]);
                                }
                            }
                        //Verifica se o nome anterior é igual, para não repetir o nome no texto
                        if (nome == nome2)
                            {
                            if (divisa.Length > 0)
                                {
                                if (divisa == divisa2)
                                    {
                                    //divisa
                                    if (checkBox25.Checked == true)
                                        {
                                        //richTextBox1.AppendBold(", ");
                                        }
                                    else
                                        {
                                        //richTextBox1.AppendRegular(", ");
                                        }
                                    }
                                else
                                    {
                                    richTextBox1.AppendRegular(", " + txtDivisa.Text);
                                    if (checkBox25.Checked == true)
                                        {
                                        richTextBox1.AppendBold(" " + dt2.Rows[i][6]);
                                        }
                                    else
                                        {
                                        richTextBox1.AppendRegular(" " + dt2.Rows[i][6]);
                                        }
                                    }

                                //Nome
                                if (checkBox23.Checked == true) //negrito
                                    {
                                    richTextBox1.AppendBold("");
                                    }
                                else
                                    {
                                    richTextBox1.AppendRegular(" " + dt2.Rows[i][5] + ", ");
                                    }
                                }
                            }
                        else
                            {
                            richTextBox1.AppendRegular("; " + txtConfronto.Text);
                            if (checkBox23.Checked == true) //negrito
                                {
                                richTextBox1.AppendBold(" " + dt2.Rows[i][5]);
                                richTextBox1.AppendRegular(", ");
                                richTextBox1.AppendRegular(txtDivisa.Text);
                                richTextBox1.AppendBold(" " + dt2.Rows[i][6]);
                                }
                            else
                                {
                                richTextBox1.AppendRegular(" " + dt2.Rows[i][5] + ", ");
                                richTextBox1.AppendRegular(" " + dt2.Rows[i][6]);
                                }
                            }
                        }

                    //Divisa
                    //divisa = Convert.ToString(dt2.Rows[i][6]);
                    //if (divisa.Length > 0)
                    //    {
                    //    richTextBox1.AppendRegular(txtDivisa.Text);
                    //    if (checkBox25.Checked == true)
                    //        {
                    //        richTextBox1.AppendBold(" " + dt2.Rows[i][6]);
                    //        }
                    //    else
                    //        {
                    //        richTextBox1.AppendRegular(" " + dt2.Rows[i][6]);
                    //        }
                    //    }

                    //Azimute
                    azi = Convert.ToString(dt2.Rows[i][4]);
                    richTextBox1.AppendRegular(" " + txtAzimute.Text + " ");
                    if (checkBox19.Checked == true)
                        {
                        richTextBox1.AppendBold(azi);
                        }
                    else
                        {
                        richTextBox1.AppendRegular(azi);
                        }

                    //Distância
                    dist = Convert.ToString(dt2.Rows[i][3]) + " " + txtUnidade.Text;
                    richTextBox1.AppendRegular(" " + txtDist.Text + " ");
                    if (checkBox18.Checked == true)
                        {
                        richTextBox1.AppendBold(dist + " ");
                        }
                    else
                        {
                        richTextBox1.AppendRegular(dist + " ");
                        }

                    progressBar1.Value = i;

                    } //fim do for

                //Volta ao primeiro ponto, para fechar o perímetro
                ponto = Convert.ToString(dt2.Rows[0][0]);

                //Nome ponto
                richTextBox1.AppendRegular(txtLigacao.Text + " ");
                if (checkBox24.Checked == true)
                    {
                    richTextBox1.AppendBold(ponto);
                    }
                else
                    {
                    richTextBox1.AppendRegular(ponto);
                    }

                richTextBox1.AppendRegular(", " + txtFim.Text);

                //Perímetro
                //string pers = null;
                //if (numericUpDown1.Value == 0)
                //    {
                //    pers = per.ToString("#,###.");
                //    //richTextBox1.AppendRegular(" " + per.ToString("0") + " " + textBox8.Text + ".");
                //    }
                //if (numericUpDown1.Value == 1)
                //    {
                //    pers = per.ToString("#,###0.0");
                //    //richTextBox1.AppendRegular(" " + per.ToString("0.0") + " " + textBox8.Text + ".");
                //    }
                //if (numericUpDown1.Value == 2)
                //    {
                //    //pers = per.ToString("0.00");
                //    pers = per.ToString("#,###0.00");
                //    //richTextBox1.AppendRegular(" " + per.ToString("0.00") + " " + textBox8.Text + ".");
                //    }
                //if (numericUpDown1.Value == 3)
                //    {
                //    pers = per.ToString("#,###0.000");
                //    //richTextBox1.AppendRegular(" " + per.ToString("0.000") + " " + textBox8.Text + ".");
                //    }
                //if (numericUpDown1.Value == 4)
                //    {
                //    pers = per.ToString("#,###0.0000");
                //    //richTextBox1.AppendRegular(" " + per.ToString("0.0000") + " " + textBox8.Text + ".");
                //    }

                //Perímetro no fim do texto - Acho que não precisa estar no texto
                //richTextBox1.AppendRegular(" ");
                //if (checkBox18.Checked == true)
                //    {
                //    richTextBox1.AppendBold(pers);
                //    }
                //else
                //    {
                //    richTextBox1.AppendRegular(pers);
                //    }
                //richTextBox1.AppendRegular(" " + txtUnidade.Text + ".");

                richTextBox1.AppendLine();

                string paragFinal = "Todas as coordenadas aqui descritas estão georreferenciadas ao Sistema Geodésico Brasileiro e encontram-se representadas no Sistema UTM, referenciadas ao Meridiano Central " + txtMC.Text + ", tendo como datum o " + txtDatum.Text + ". Todos os azimutes e distâncias, área e perímetro foram calculados no plano de projeção UTM.";

                richTextBox1.AppendRegular("\t" + paragFinal);

                //Espaço entre texto e assinatura
                if (txtCidade.TextLength > 0 || checkBox2.Checked == true || radioButton3.Checked == true || radioButton4.Checked == true || textBox16.TextLength > 0 || txtRegistro.TextLength > 0)
                    {
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    }

                //Cidade
                if (txtCidade.TextLength > 0)
                    {
                    if (checkBox26.Checked == true)
                        {
                        richTextBox1.AppendCenterBold(txtCidade.Text);
                        }
                    else
                        {
                        richTextBox1.AppendCenter(txtCidade.Text);
                        }
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

                        if (txtCidade.TextLength > 0)
                            {
                            if (checkBox27.Checked == true)
                                {
                                richTextBox1.AppendCenterBold(", " + data);
                                }
                            else
                                {
                                richTextBox1.AppendCenter(", " + data);
                                }
                            }
                        else
                            {
                            if (checkBox27.Checked == true)
                                {
                                richTextBox1.AppendCenterBold(data);
                                }
                            else
                                {
                                richTextBox1.AppendCenter(data);
                                }
                            }
                        }
                    if (radioButton4.Checked == true)
                        {
                        if (checkBox27.Checked == true)
                            {
                            richTextBox1.AppendCenterBold(txtData.Text);
                            }
                        else
                            {
                            richTextBox1.AppendCenter(txtData.Text);
                            }
                        }
                    }

                //Profissional
                if (textBox16.TextLength > 0)
                    {
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendLine();
                    richTextBox1.AppendCenter(txtAssinatura.Text);
                    }
                if (textBox16.TextLength > 0)
                    {
                    richTextBox1.AppendLine();
                    if (checkBox28.Checked == true)
                        {
                        richTextBox1.AppendCenterBold(textBox16.Text);
                        }
                    else
                        {
                        richTextBox1.AppendCenter(textBox16.Text);
                        }
                    }
                if (txtRegistro.TextLength > 0)
                    {
                    richTextBox1.AppendLine();
                    if (checkBox29.Checked == true)
                        {
                        richTextBox1.AppendCenterBold(txtRegistro.Text);
                        }
                    else
                        {
                        richTextBox1.AppendCenter(txtRegistro.Text);
                        }
                    }

                richTextBox1.AppendLine();

                //Reseta barra de progresso
                progressBar1.Value = 0;

                //Cálculo da área
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

        //Exportar tabela para CSV
        private void button6_Click(object sender, EventArgs e)
            {

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(saveFileDialog2.FileName);

                string strHeader = "";

                int numcolunas = dataGridView1.Columns.Count;
                for (int i = 0; i < numcolunas; i++)
                    {
                    if (i == (numcolunas - 1))
                        {
                        strHeader += dataGridView1.Columns[i].HeaderText;
                        }
                    else
                        {
                        strHeader += dataGridView1.Columns[i].HeaderText + ";";
                        }
                    }

                streamWriter.WriteLine(strHeader);

                for (int m = 0; m < dataGridView1.Rows.Count; m++)
                    {
                    string strRowValue = "";

                    for (int n = 0; n < numcolunas; n++)
                        {
                        if (n == (numcolunas - 1))
                            {
                            strRowValue += dataGridView1.Rows[m].Cells[n].Value;
                            }
                        else
                            {
                            strRowValue += dataGridView1.Rows[m].Cells[n].Value + ";";
                            }
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
                txtData.Enabled = false;
                }
            }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
            {
            if (radioButton4.Checked == true)
                {
                txtData.Enabled = true;
                }

            if (radioButton4.Checked == false)
                {
                txtData.Enabled = false;
                }
            }

        private void txtArea_TextChanged(object sender, EventArgs e)
            {
            if (txtArea.TextLength > 0)
                {
                txtAreaha.Text = formataDist(Convert.ToDouble(txtArea.Text) / 10000);
                }
            else
                {
                txtAreaha.Text = formataDist(0);
                }
            }

        private void txtArea_KeyPress(object sender, KeyPressEventArgs e)
            {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
                {
                e.Handled = true;
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
            Font regular = new Font(ed.Font, FontStyle.Regular);
            ed.SelectionFont = regular;
            ed.SelectionAlignment = HorizontalAlignment.Center;
            ed.AppendText(s);
            ed.SelectionFont = regular;
            //ed.SelectionAlignment = HorizontalAlignment.Left;
            //ed.AppendText(NewLine);
            //ed.AppendText(NewLine);
            }

        public static void AppendCenterBold(this RichTextBox ed, string s)
            {
            Font bold = new Font(ed.Font, FontStyle.Bold);
            Font regular = new Font(ed.Font, FontStyle.Regular);
            ed.SelectionFont = bold;
            ed.SelectionAlignment = HorizontalAlignment.Center;
            ed.AppendText(s);
            ed.SelectionFont = regular;
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