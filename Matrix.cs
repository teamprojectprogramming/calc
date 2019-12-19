using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixOperations.Models
{
    public class Matrix
    {
        public string Name { get; set; }

        public int RowsCount { get; private set; }
        public int ColumnsCount { get; private set; }


        public double this[int i, int j]
        {
            get { return mass[i, j]; }
            set { mass[i, j] = value; }
        }

        public double[,] mass;


        #region Constructors

        public Matrix()
        {
            mass = new double[RowsCount, ColumnsCount];            
        }

        public Matrix(int N)
        {
            RowsCount = N;
            ColumnsCount = N;
            mass = new double[RowsCount, ColumnsCount];
        }

        public Matrix(int rowsCount, int columnsCount)
        {
            RowsCount = rowsCount;
            ColumnsCount = columnsCount;
            mass = new double[RowsCount, ColumnsCount]; 
        }

        public Matrix(double[,] mass)
        {
            this.mass = mass;
            RowsCount = mass.GetLength(0);
            ColumnsCount = mass.GetLength(1);
        }

        public Matrix(Matrix matrix)
        {
            mass = new double[matrix.RowsCount, matrix.ColumnsCount];
            RowsCount = matrix.RowsCount;
            ColumnsCount = matrix.ColumnsCount;

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                    mass[i, j] = matrix[i, j];
            }

        }

        #endregion

        public void RandomFill(int from, int to)
        {
            Random rand = new Random();
            
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j=0; j< ColumnsCount; j++)
                {
                    mass[i, j] = rand.Next(from, to);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    mass[i, j] = 0;
                }
            }
        }

        public static Matrix Mult(Matrix A, Matrix B)
        {
            if (!(A.ColumnsCount == B.RowsCount))
            {
                throw new ArgumentException("Произведение матрицы А на В не имеет смысла, т.к. число столбцов матрицы A не равно числу строк матрицы В");
            }

            Matrix res = new Matrix(A.RowsCount, B.ColumnsCount);

            for (int i = 0; i < A.RowsCount; i++)
                for (int j = 0; j < B.ColumnsCount; j++)
                    for (int k = 0; k < B.RowsCount; k++)
                        res[i, j] += A[i, k] * B[k, j];

            return res;
        }

        public static Matrix Mult(Matrix A, int scalar)
        {
            Matrix temp = new Matrix(A);
            for (int i = 0; i < A.RowsCount; i++)
            {
                for (int j = 0; j < A.ColumnsCount; j++)
                {
                    temp[i, j] = temp[i, j] * scalar;
                }
            }
            return temp;
        }

        public static Matrix Div(Matrix A, Matrix B)
        {
            return A * B.Invert();
        }

        public static Matrix Div(Matrix A, double num)
        {
            Matrix res = new Matrix(A.RowsCount, A.RowsCount);

            for (int i = 0; i < A.RowsCount; i++)
                for (int j = 0; j < A.RowsCount; j++)
                    res[i, j] += A[i, j] * 1 / num;

            return res;
        }

        public static Matrix Add(Matrix A, Matrix B)
        {
            if (!(A.RowsCount == B.RowsCount && A.ColumnsCount == B.ColumnsCount))
            {
                throw new ArgumentException("Сложение не имеет смысла, т.к. размеры матриц не одинаковы");
            }

            Matrix res = new Matrix(A.RowsCount, A.ColumnsCount);

            for (int i = 0; i < A.RowsCount; i++)
            {
                for (int j = 0; j < A.ColumnsCount; j++)
                {
                    res[i, j] = A[i, j] + B[i, j];
                }
            }
            return res;
        }

        public static Matrix Sub(Matrix A, Matrix B)
        {
            if (!(A.RowsCount == B.RowsCount && A.ColumnsCount == B.ColumnsCount))
            {
                throw new ArgumentException("Вычитание не имеет смысла, т.к. размеры матриц не одинаковы");
            }

            Matrix res = new Matrix(A.RowsCount, A.ColumnsCount);

            for (int i = 0; i < A.RowsCount; i++)
            {
                for (int j = 0; j < A.ColumnsCount; j++)
                {
                    res[i, j] = A[i, j] - B[i, j];
                }
            }
            return res;
        }

        public Matrix Invert()
        {
            if (RowsCount != ColumnsCount)
            {
                throw new ArgumentException("Обратная матрица существует только для квадратных");
            }

            double determinant = DetRec();
            if (determinant == 0)
            {
                throw new ArgumentException("Детерминант матрицы равен нулю");
            }

            Matrix matrix = new Matrix(RowsCount);

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    Matrix tmp = Exclude(i, j);  //получаем матрицу без строки i и столбца t
                    //(1 / determinant) * Determinant(tmp) - формула поределения элемента обратной матрицы
                    matrix[j, i] = (1 / determinant) * Math.Pow(-1, i + j) * detRec(tmp.mass);
                }
            }
            return matrix;
        }


        // Перегрузка оператора сложения
        public static Matrix operator +(Matrix a, Matrix b)
        {
            return Add(a, b);
        }

        // Перегрузка оператора вычитания
        public static Matrix operator -(Matrix a, Matrix b)
        {
            return Sub(a, b);
        }

        // Перегрузка оператора умножения
        public static Matrix operator *(Matrix a, Matrix b)
        {
            return Mult(a, b);
        }

        // Перегрузка оператора деления
        public static Matrix operator /(Matrix a, Matrix b)
        {
            return Div(a, b);
        }

        public double MaxElement()
        {
            double max = mass[0, 0];

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    if (mass[i, j] > max)
                    {
                        max = mass[i, j];
                    }
                }
            }

            return max;
        }

        public double MinElement()
        {
            double min = mass[0, 0];

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    if (mass[i, j] < min)
                    {
                        min = mass[i, j];
                    }
                }
            }

            return min;
        }

        public double DetRec()
        {
            return detRec(mass);
        }

        public override string ToString()
        {
            string res = "";

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    //res += string.Format("{0: 0.00; -0.00; 0.00}", mass[i,j]);
                    res += string.Format("{0,7:0.00}", mass[i, j]);
                }
                res += "\n";
            }
            return res;
        }

        double detRec(double[,] matrix)
        {
            if (matrix.Length == 4)
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            double sign = 1, result = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                double[,] minor = GetMinor(matrix, i);
                result += sign * matrix[0, i] * detRec(minor);
                sign = -sign;
            }
            return result;
        }

        double[,] GetMinor(double[,] matrix, int n)
        {
            double[,] result = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < n; j++)
                    result[i - 1, j] = matrix[i, j];
                for (int j = n + 1; j < matrix.GetLength(0); j++)
                    result[i - 1, j - 1] = matrix[i, j];
            }
            return result;
        }

        Matrix Exclude(int row, int column)
        {
            if (row > RowsCount || column > ColumnsCount)
                throw new IndexOutOfRangeException("Строка или столбец не принадлежат матрице.");
            Matrix ret = new Matrix(RowsCount - 1, ColumnsCount - 1);

            int offsetX = 0;
            for (int i = 0; i < RowsCount; i++)
            {
                int offsetY = 0;
                if (i == row)
                {
                    offsetX++;
                    continue;
                }
                for (int t = 0; t < ColumnsCount; t++)
                {
                    if (t == column)
                    {
                        offsetY++;
                        continue;
                    }
                    ret[i - offsetX, t - offsetY] = mass[i, t];
                }
            }
            return ret;
        }
    }
}
