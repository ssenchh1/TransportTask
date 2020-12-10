using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transport
{
    class SeveroZapad
    {
        public static double[,] goods;
        public static double[,] cost;
        public static double[] need;
        public static double[] stock;
        public static double[] Vpotent;
        public static double[] Upotent;
        public static DataGridView dataGridView;
        public static DataGridView dataGridView2;
        public static Label summa;
        public static double CurrentSum = 0;
        public SeveroZapad(DataGridView _dataGridView,DataGridView _dataGridView2, Label label)
        {
            summa = label;
            dataGridView = _dataGridView;
            dataGridView2 = _dataGridView2;
            cost = new double[dataGridView.RowCount - 1, dataGridView.ColumnCount - 1];
            for (int i = 0; i < dataGridView.RowCount - 1; i++)
            {
                for (int j = 0; j < dataGridView.ColumnCount - 1; j++)
                {
                    cost[i, j] = Double.Parse((String)dataGridView.Rows[i].Cells[j].Value);
                }
            }
            need = new double[dataGridView.RowCount - 1];
            for (int i = 0; i < dataGridView.RowCount - 1; i++)
            {
                need[i] = Double.Parse((String)dataGridView.Rows[i].Cells[dataGridView.ColumnCount - 1].Value);
                dataGridView2.Rows[i].Cells[dataGridView.ColumnCount - 1].Value = need[i];
            }
            stock = new double[dataGridView.ColumnCount - 1];
            for (int i = 0; i < dataGridView.ColumnCount - 1; i++)
            {
                stock[i] = Double.Parse((String)dataGridView.Rows[dataGridView.RowCount - 1].Cells[i].Value);
                dataGridView2.Rows[dataGridView.RowCount - 1].Cells[i].Value = stock[i];
            }
        }

        public void Show()
        {
            for (int i = 0; i < dataGridView.RowCount - 1; i++)
            {
                for (int j = 0; j < dataGridView.ColumnCount - 1; j++)
                {
                    MessageBox.Show((cost[i, j]).ToString());
                }
            }
        }

        public void Showneed()
        {
            foreach (var item in need)
            {
                MessageBox.Show(item.ToString());
            }
        }

        public void Showstock()
        {
            foreach (var item in stock)
            {
                MessageBox.Show(item.ToString());
            }
        }

        public void FormPlan()
        {
            int rawsInCost = cost.GetLength(0);
            int columnsInCost = cost.GetLength(1);
            goods = new double[rawsInCost, columnsInCost];
            for(int i=0; i< rawsInCost; i++)
            {
                while (need[i] > 0)
                {
                    for(int j = 0; j < columnsInCost; j++)
                    {
                        if (need[i] >= stock[j])
                        {
                            goods[i, j] = stock[j];
                            need[i] -= stock[j];
                            stock[j] = 0;
                        }
                        else
                        {
                            goods[i, j] = need[i];
                            stock[j] -= need[i];
                            need[i] = 0;
                        }
                    }
                }
            }
            for (int i = 0; i < dataGridView2.RowCount - 1; i++)
            {
                for (int j = 0; j < dataGridView2.ColumnCount - 1; j++)
                {
                    dataGridView2.Rows[i].Cells[j].Value = goods[i, j].ToString() + "(" + cost[i, j] + ")";
                }
            }
            //выводим общую стоимость перевозок
            double Sum = 0;
            for (int i = 0; i < rawsInCost; i++)
            {
                for (int j = 0; j < columnsInCost; j++)
                {
                    if (goods[i, j] > 0)
                        Sum += goods[i, j] * cost[i, j];
                }
            }
            summa.Text += Sum.ToString();
            summa.Visible = true;
        }

        public static void FindSolution()
        {
            
            bool isOptimal = false;
            double[] Vpotent = new double[goods.GetLength(0)];
            for (int i = 0; i < Vpotent.Length; i++)
            {
                Vpotent[i] = 9999;
            }
            Upotent = new double[dataGridView.ColumnCount - 1];
            for (int i = 0; i < Upotent.Length; i++)
            {
                Upotent[i] = 9999;
            }

            //находим максимальный тариф в заполненных ячейках
            double MaxCost = 0;
            int maxV = 0;
            int maxU = 0;
            for (int i = 0; i < cost.GetLength(0); i++)
            {
                for (int j = 0; j < cost.GetLength(1); j++)
                {
                    if (goods[i, j] != 0 && cost[i, j] > MaxCost)
                    {
                        MaxCost = cost[i, j];
                        maxV = i;
                        maxU = j;
                    }
                }
            }
            Vpotent[maxV] = 0;
            Upotent[maxU] = cost[maxV, maxU] - Vpotent[maxV];
            for (int sania = 0; sania < cost.GetLength(1); sania++)
            {
                for (int i = 0; i < cost.GetLength(0); i++)
                {
                    for (int j = 0; j < cost.GetLength(1); j++)
                    {
                        if (goods[i, j] != 0 && (Vpotent[i] == 9999 || Upotent[j] == 9999))
                        {
                            if (Vpotent[i] == 9999 & Upotent[j] == 9999)
                                continue;
                            if (Vpotent[i] != 9999)
                            {
                                for (int k = 0; k < cost.GetLength(1); k++)
                                {
                                    if (goods[i, k] != 0)
                                    {
                                        Upotent[k] = cost[i, k] - Vpotent[i];
                                    }
                                }
                            }
                            if (Upotent[j] != 9999)
                            {
                                for (int k = 0; k < cost.GetLength(0); k++)
                                {
                                    if (goods[k, j] != 0)
                                    {
                                        Vpotent[k] = cost[k, j] - Upotent[j];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            //посчитали потенциалы, приступаем к анализу данного плана
            //создаем матрицу дельта
            double[,] delta = new double[cost.GetLength(0), cost.GetLength(1)];
            double Maxdelta = 0;
            int maxi = int.MaxValue, maxj = int.MaxValue;
            //
            for (int i = 0; i < cost.GetLength(0); i++)
            {
                for (int j = 0; j < cost.GetLength(1); j++)
                {
                    if (goods[i, j] == 0)
                    {
                        delta[i, j] = Vpotent[i] + Upotent[j] - cost[i, j];
                        if (delta[i, j] > 0)
                        {
                            isOptimal = false;
                            if (delta[i, j] > Maxdelta)
                            {
                                Maxdelta = delta[i, j];
                                maxi = i;
                                maxj = j;
                            }
                        }
                    }
                }
            }
            if (maxi == int.MaxValue && maxj == int.MaxValue)
            {
                MessageBox.Show("План оптимален");
                isOptimal = true;
                summa.Text = "" + CurrentSum;
            }
            else
            {
                double MaxCostInRowWithDelta = 0;
                int MaxCostInRowWithDeltaI = 0, MaxCostInRowWithDeltaJ = 0;
                //находим ячейку с товаром и максимальным тарифом в строке с максимальным дельта
                for (int j = 0; j < goods.GetLength(1); j++)
                {
                    if (goods[maxi, j] != 0 && cost[maxi, j] > MaxCostInRowWithDelta)
                    {
                        MaxCostInRowWithDelta = cost[maxi, j];
                        MaxCostInRowWithDeltaI = maxi;
                        MaxCostInRowWithDeltaJ = j;
                    }
                }
                //находим ячейку с товаром и максимальным тарифом в столбце с максимальным дельта
                double MaxCostInCOLUMNWithDelta = 0;
                int MaxCostInColumnWithDeltaI = 0, MaxCostInColumnWithDeltaJ = 0;

                for (int i = 0; i < goods.GetLength(0); i++)
                {
                    if (goods[i, maxj] != 0 && cost[i, maxj] > MaxCostInCOLUMNWithDelta)
                    {
                        MaxCostInCOLUMNWithDelta = cost[i, maxj];
                        MaxCostInColumnWithDeltaI = i;
                        MaxCostInColumnWithDeltaJ = maxj;
                    }
                }
                //находим, сколько товара мы можем переместить
                double MaxAmountWeCanAfford ;
                if (goods[MaxCostInColumnWithDeltaI, MaxCostInColumnWithDeltaJ] > goods[MaxCostInRowWithDeltaI, MaxCostInRowWithDeltaJ])
                {
                    MaxAmountWeCanAfford = goods[MaxCostInRowWithDeltaI, MaxCostInRowWithDeltaJ];
                }
                else
                {
                    MaxAmountWeCanAfford = goods[MaxCostInColumnWithDeltaI, MaxCostInColumnWithDeltaJ];
                }
                //переносим
                goods[MaxCostInRowWithDeltaI, MaxCostInRowWithDeltaJ] -= MaxAmountWeCanAfford;
                goods[maxi, maxj] += MaxAmountWeCanAfford;
                goods[MaxCostInColumnWithDeltaI, MaxCostInColumnWithDeltaJ] -= MaxAmountWeCanAfford;
                goods[MaxCostInColumnWithDeltaI, MaxCostInRowWithDeltaJ] += MaxAmountWeCanAfford;

                //выводим результат
                for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    for (int j = 0; j < dataGridView2.ColumnCount - 1; j++)
                    {
                        dataGridView2.Rows[i].Cells[j].Value = goods[i, j].ToString() + "(" + cost[i, j] + ")";
                    }
                }


                CurrentSum = 0;
                for (int i = 0; i < goods.GetLength(0); i++)
                    for (int j = 0; j < goods.GetLength(1); j++)
                        if (goods[i, j] > 0)
                            CurrentSum += goods[i, j] * cost[i, j];

                summa.Text = "План приближен к оптимальному. Сейчас перевозка стоит: " + CurrentSum;

            }


        }
    }
}
