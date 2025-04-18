using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetodoJacobi
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void lblTiempo_Click(object sender, EventArgs e)
        {

        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            //Aqui convierte el texto ingresado en un numero int
            if (int.TryParse(txtTamaño.Text, out int n) && n > 0 && n <= 10)
            {
                //Limpiar el dataGridView
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();
                //Agrega las columnas al dataGridView
                for (int i = 0; i < n; i++)
                {
                    dataGridView.Columns.Add($"col{i}", "");
                    dataGridView.Columns[i].Width = 50;
                }
                //Agrega las filas al dataGridView
                for (int i = 0; i < n; i++)
                {
                    dataGridView.Rows.Add();
                }
            }
            else
            {
                //Manejo de errores
                MessageBox.Show("Ingresa un número válido (máximo 10).");
            }
        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            //Sin esta linea el metodo calcular no leera bien la matriz ya que lo tomara como
            //un indice fuera del intevalo
            int n = dataGridView.Rows.Count - 1;  // Restamos 1 por la fila extra de DataGridView
            double[,] matriz = new double[n, n];
            //Leer los valores de la matris desde el dataGridView
            try
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (j >= dataGridView.Rows[i].Cells.Count)
                        {
                            MessageBox.Show($"Error: La celda ({i + 1},{j + 1}) no existe.");
                            return;
                        }

                        string cellValue = dataGridView.Rows[i].Cells[j].Value?.ToString();
                        if (string.IsNullOrEmpty(cellValue) || !double.TryParse(cellValue,
                            out matriz[i, j]))
                        {
                            MessageBox.Show($"Valor inválido en la celda ({i + 1},{j + 1}).");
                            return;
                        }
                    }
                }
            }
            //Manejo y control de posibles errores al ingresar los datos dentro de la matriz 
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la matriz: {ex.Message}");
                return;
            }
            //Verifica si la metriz es simetrica
            if (!EsSimetrica(matriz))
            {
                MessageBox.Show("La matriz no es simétrica.");
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            //Calculo de lso auto valores con el metodo Jacobi
            try
            {
                double[] autovalores = Jacobi.CalcularAutovalores(matriz, 1e-10);
                sw.Stop();
                txtResultados.Text = string.Join(Environment.NewLine, autovalores.Select((v, i)
                    => $"λ{i + 1} = {v:F3}"));
                lblTiempo.Text = $"Tiempo: {sw.Elapsed.TotalSeconds:F4} s";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en el cálculo: {ex.Message}");
            }

        }

        //Metodo para verificar si la matriz es simetrica
        private bool EsSimetrica(double[,] A)
        {
            int n = A.GetLength(0);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (A[i, j] != A[j, i]) return false;
            return true;
        }
        //Metodo Jacobi
        public static class Jacobi
        {
            public static double[] CalcularAutovalores(double[,] A, double tolerancia)
            {
                int n = A.GetLength(0);
                while (true)
                {
                    //Encontramos el elemento fuera de la diagonal con mayor magnitud
                    int p = 0, q = 1;
                    double max = 0.0;
                    for (int i = 0; i < n - 1; i++)
                        for (int j = i + 1; j < n; j++)
                            if (Math.Abs(A[i, j]) > max)
                            {
                                max = Math.Abs(A[i, j]);
                                p = i;
                                q = j;
                            }
                    //Verificamos si la matriz ya es diagonal
                    if (max < tolerancia) break;
                    //Calculamos los angulos de rotacion
                    double theta = 0.5 * Math.Atan2(2 * A[p, q], A[q, q] - A[p, p]);
                    double cos = Math.Cos(theta);
                    double sin = Math.Sin(theta);
                    //Aplicar la transformación de Jacobi   
                    double app = cos * cos * A[p, p] - 2 * sin * cos * A[p, q] + sin * sin * A[q, q];
                    double aqq = sin * sin * A[p, p] + 2 * sin * cos * A[p, q] + cos * cos * A[q, q];

                    A[p, p] = app;
                    A[q, q] = aqq;
                    A[p, q] = A[q, p] = 0.0;

                    //Actualizar los valores de la matriz
                    for (int i = 0; i < n; i++)
                    {
                        if (i != p && i != q)
                        {
                            double aip = A[i, p];
                            double aiq = A[i, q];
                            A[i, p] = A[p, i] = cos * aip - sin * aiq;
                            A[i, q] = A[q, i] = sin * aip + cos * aiq;
                        }
                    }
                }
                //Extraer los auto valores      
                double[] autovalores = new double[n];
                for (int i = 0; i < n; i++)
                    autovalores[i] = A[i, i];

                return autovalores;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
