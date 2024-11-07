using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace LAB4_MO
{
    public class lab4_balash
    {
        private double[] c;
        private double[] a;
        private double b;
        private string[] rasp_x; // расположение иксов
        private double[] c_no_sort;
        private double[] a_no_sort;

        public lab4_balash(double[] c, double[] a, double b)
        {
            int n = c.Length;
            c_no_sort = (double[])c.Clone(); // если напишем просто c_no_sort = с, то мы приравняем ссылку на массив c, в итоге у нас c_no_sort и c будут совпадать.
            a_no_sort = (double[])a.Clone();
            string[] nabor_x = new string[] { "X1", "X2", "X3", "X4", "X5", "X6", "X7", "X8" };
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (c[j] < c[j + 1]) // Сравниваем для убывания
                    {
                        // Меняем местами c[j] и c[j + 1]
                        double tempC = c[j];
                        c[j] = c[j + 1];
                        c[j + 1] = tempC;

                        // Также меняем местами соответствующие элементы в массиве a
                        double tempA = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = tempA;

                        string tempX = nabor_x[j];
                        nabor_x[j] = nabor_x[j + 1];
                        nabor_x[j + 1] = tempX;
                    }
                }
            }
            this.c = c;
            this.a = a;
            this.b = b;
            this.rasp_x = nabor_x;
        }

        public List<double[]> Solution_Balash()
        {
            List<double[]> continue_roots = new List<double[]> { }; // узлы у которых есть продолжение в следующем слое
            List<double[]> solve_roots = new List<double[]> { }; // узлы решения

            double[] pred_el = new double[8];


            if (check_root(pred_el, a)) 
            {
                solve_roots.Add(pred_el);
                return solve_roots;
            }
                
            else continue_roots.Add(pred_el);


            while (continue_roots.Any()) // пока у нас остаются узлы, которые могут разветвлятся 
            {
                int length_c_r = continue_roots.Count;
                for (int i = 0; i<length_c_r; i++) 
                {
                    int k = -1; // позиция первой единички слева в векторе решения
                    int flag = -1; // флаг для остановки поиска решения на этом уровне

                    if (solve_roots.Any())
                    {
                        for (int j = 0; j < 8; j++) if (solve_roots[solve_roots.Count - 1][j] == 1) k = j;
                    }

                    

                    for (int t = 7; t >= k+1; t--) 
                    {

                        pred_el = new double[8];
                        for (int j = 0; j < 8; j++) pred_el[j] = continue_roots[i][j];

                        if (pred_el[t] == 1) continue;

                        pred_el[t] = 1;

                        if (check_root(pred_el,a))
                        {
                            solve_roots.Add(pred_el);
                            flag = 1;
                            break;
                        }

                        else continue_roots.Add(pred_el);
                    }

                    if (flag == 1) { flag = -1; break; }
                }
                continue_roots.RemoveRange(0,length_c_r);

                int count_clone = 0; // удаление дубликатов узлов у которых есть продолжение
                for(int i = 0; i < continue_roots.Count; i++) 
                {
                    for(int j = 0; j < continue_roots.Count; j++) 
                    {
                        for(int z = 0; z < 8; z++) 
                        {
                            if(i == j) continue;
                            if (continue_roots[i][z] == continue_roots[j][z]) count_clone++;
                        }

                        if (count_clone == 8) continue_roots.RemoveAt(i);

                        count_clone = 0;
                    }
                }

            }

            List<double[]> itog_solve = new List<double[]> { };
            for(int i = 0; i < solve_roots.Count; i++) 
            {
                double[] temp = new double[8];
                for(int j = 0;j < 8; j++) 
                {
                    string x = $"X{j+1}";  
                    int k = Array.IndexOf(rasp_x, x);
                    temp[j] = solve_roots[i][k];   
                }
                itog_solve.Add(temp);
            }
            return itog_solve;
        }

        private bool check_root(double[] root, double[] a_)
        {
            double sum = 0;
            for (int i = 0; i < c.Length; i++)
            {
                sum += a_[i] * root[i];
            }
            return sum<=b;
        }

        public void perebor(double[] root, int pos = 0) // метод полного перебора
        {
            if (pos == root.Length)
            {
                if (check_root(root, a_no_sort))
                {
                    double F = 0;
                    for (int j = 0; j < root.Length; j++)
                    {
                        Console.Write($"X{j + 1}: {root[j]}   ");
                        F += c_no_sort[j] * root[j];
                        if (j == root.Length - 1) Console.Write($"   F: {F}");
                    }
                    Console.WriteLine();
                }
                return;
            }
            root[pos] = 0; 
            perebor(root, pos + 1); 
            root[pos] = 1; 
            perebor(root, pos + 1);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            double[] c = new double[] { 7, 7, 9, 8, 10, 2, 10, 3 };
            double[] dubl_c = new double[] { 7, 7, 9, 8, 10, 2, 10, 3 }; // дубликат массива c, нужен из-за того что c изменяется и не сохраняет первоначальный вид
            double[] a = new double[] { 4, 7, -2, -6, 5, 2, 4, -3 };
            double b = -10;
            lab4_balash k = new lab4_balash(c, a, b);

            List<double[]> list = k.Solution_Balash();
            
            Console.WriteLine("Метод Балаша:");
            for (int i = 0; i < list.Count; i++)
            {
                double F = 0;
                for (int j = 0; j < 8; j++)
                {
                    Console.Write($"X{j + 1}: {list[i][j]}   ");
                    F += dubl_c[j] * list[i][j];
                    if (j == 7) Console.Write($"   F: {F}");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Метод полного перебора:");
            k.perebor(new double[8]);
            Console.WriteLine();
        }
    }
}
