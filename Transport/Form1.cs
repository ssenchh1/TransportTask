using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = (int)(numericUpDown1.Value+1);
            for(int i=0; i< dataGridView1.ColumnCount-1; i++)
            {
                dataGridView1.Columns[i].Name = "Поставщик " + Convert.ToString(i + 1);
                dataGridView1.Columns[i].Width = 75;
            }
            dataGridView1.Columns[dataGridView1.ColumnCount-1].Name = "Потребность";
            dataGridView1.Columns[dataGridView1.ColumnCount-1].Width = 73;
            //datgrid2
            dataGridView2.ColumnCount = dataGridView1.ColumnCount;
            for (int i = 0; i < dataGridView2.ColumnCount - 1; i++)
            {
                dataGridView2.Columns[i].Name = "Поставщик " + Convert.ToString(i + 1);
                dataGridView2.Columns[i].Width = 75;
            }
            dataGridView2.Columns[dataGridView1.ColumnCount - 1].Name = "Потребность";
            dataGridView2.Columns[dataGridView1.ColumnCount - 1].Width = 73;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.RowCount = (int)(numericUpDown2.Value+1);
            for(int i =0; i< dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = "Магазин " + Convert.ToString(i + 1);
                dataGridView1.Rows[i].Height = 30;
                dataGridView1.AutoResizeRowHeadersWidth(0, DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            }
            dataGridView1.Rows[dataGridView1.RowCount - 1].HeaderCell.Value = "Наличие";
            //datagrid2
            dataGridView2.RowCount = dataGridView1.RowCount;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                dataGridView2.Rows[i].HeaderCell.Value = "Магазин " + Convert.ToString(i + 1);
                dataGridView2.Rows[i].Height = 30;
                dataGridView2.AutoResizeRowHeadersWidth(0, DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            }
            dataGridView2.Rows[dataGridView1.RowCount - 1].HeaderCell.Value = "Наличие";
        }
        
        
        private void button1_Click(object sender, EventArgs e)
        {
            //добавляем кнопочки и датагридвью
            button1.Visible = false;
            button2.Visible = true;
            button2.Location = new Point(200, dataGridView2.Height + dataGridView2.Location.Y + 15);
            //построим таблицу с опорным планом в datagridview2
            //dataGridView2.ColumnCount = dataGridView1.ColumnCount;
            //for (int i = 0; i < dataGridView2.ColumnCount - 1; i++)
            //{
            //    dataGridView2.Columns[i].Name = "Поставщик " + Convert.ToString(i + 1);
            //    dataGridView2.Columns[i].Width = 75;
            //}
            //dataGridView2.Columns[dataGridView1.ColumnCount - 1].Name = "Потребность";
            //dataGridView2.Columns[dataGridView1.ColumnCount - 1].Width = 73;

            //dataGridView2.RowCount = dataGridView1.RowCount;
            //for (int i = 0; i < dataGridView2.RowCount; i++)
            //{
            //    dataGridView2.Rows[i].HeaderCell.Value = "Потреб. " + Convert.ToString(i + 1);
            //    dataGridView2.Rows[i].Height = 30;
            //    dataGridView2.AutoResizeRowHeadersWidth(0, DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            //}
            //dataGridView2.Rows[dataGridView1.RowCount - 1].HeaderCell.Value = "Наличие";
            
            
            if (comboBox1.SelectedIndex == 0)
            {
                MinZatrataMethod mzm = new MinZatrataMethod(dataGridView1, dataGridView2, label5);
                mzm.FormPlan();
            }
            else
            {
                SeveroZapad szm = new SeveroZapad(dataGridView1,dataGridView2 ,label5);
                szm.FormPlan();
            }
            
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                MinZatrataMethod.FindSolution();
            }
            else
            {
                SeveroZapad.FindSolution();
                
                button2.Text = "Следующая итерация";
            }
        }
    }

       
}



