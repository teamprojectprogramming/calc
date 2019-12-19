using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixOperations.Models;
using MatrixOperations.Commands;
using System.Windows.Input;

namespace MatrixOperations.ViewModels
{
    public class MatrixVM : BaseVM
    {
        #region Commands 

        DelegateCommand _randomFillCommand;
        public ICommand RandomFillCommand
        {
            get
            {
                if (_randomFillCommand == null)
                {
                    _randomFillCommand = new DelegateCommand(randomFillExecute);
                }
                return _randomFillCommand;
            }
        }

        private void randomFillExecute()
        {
            matrix.RandomFill(-10, 10);
            loadElements();
        }

        DelegateCommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new DelegateCommand(clearExecute);
                }
                return _clearCommand;
            }
        }

        private void clearExecute()
        {
            matrix.Clear();
            loadElements();
        }

        #endregion


        public string Name { get; private set; }

        public IEnumerable<int> RowSizes
        {
            get { return _rowSizes; }
        }
        public IEnumerable<int> ColumnSizes
        {
            get { return _columnSizes; }
        }

        int _m = 4; int _n = 4;
        public int M
        {
            get { return _m; }
            private set 
            {
                _m = value;
                sizeChanged(M, N);
                RaisePropertyChanged(); 
            }
        }       
        public int N
        {
            get { return _n; }
            private set 
            {
                _n = value;
                sizeChanged(M, N);
                RaisePropertyChanged(); 
            }
        }

        List<ValueVM<double>> elements;
        public List<ValueVM<double>> Elements 
        {
            get { return elements; }
            private set { elements = value; RaisePropertyChanged(); }
        }

        public MatrixVM()
        {
            Name = "X";          
            this.matrix = new Matrix(M, N);
            initializeParameters();
            loadElements();
        }

        public MatrixVM(string name)
        {
            Name = name;
            this.matrix = new Matrix(M, N);
            initializeParameters();
            loadElements();
        }

        public MatrixVM(string name, Matrix matrix)
        {
            Name = name;
            this.matrix = matrix;
            initializeParameters();
            loadElements();
        }

        void initializeParameters()
        {
            _columnSizes = Enumerable.Range(1, 5);
            _rowSizes = Enumerable.Range(1, 5);

            _m = matrix.RowsCount;
            _n = matrix.ColumnsCount;            
        }

        void sizeChanged(int row, int col)
        {
            int[,] mass = new int[row, col];
            matrix = new Matrix(row, col);
            loadElements();
        }

        void loadElements()
        {
            int rows = matrix.RowsCount; int columns = matrix.ColumnsCount;
            var elements = new ValueVM<double>[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    elements[i, j] = new ValueVM<double>(matrix[i, j]);
                }
            }

            Elements = elements.Cast<ValueVM<double>>().ToList();
        }

        public void SaveElements()
        {
            int rows = matrix.RowsCount; int columns = matrix.ColumnsCount;
            double[,] mass = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    mass[i, j] = elements[i * columns + j].Value;
                }
            }

            matrix = new Matrix(mass);
        }

        public Matrix matrix;


        IEnumerable<int> _columnSizes;
        IEnumerable<int> _rowSizes;
        
    }

   
}
