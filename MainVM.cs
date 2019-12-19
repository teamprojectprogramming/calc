using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MatrixOperations.Commands;
using MatrixOperations.Models;


namespace MatrixOperations.ViewModels
{
    public class MainVM : BaseVM
    {
        #region Command

        DelegateCommand _swapMatrixCommand;
        public ICommand SwapMatrixCommand
        {
            get
            {
                if (_swapMatrixCommand == null)
                {
                    _swapMatrixCommand = new DelegateCommand(swapMatrixExecute);                   
                }
                return _swapMatrixCommand;
            }
        }
        private void swapMatrixExecute()
        {
            var temp = MatrixA;
            MatrixA = MatrixB;
            MatrixB = temp;
        }

        DelegateCommand _sumMatrixCommand;
        public ICommand SumMatrixCommand
        {
            get
            {
                if (_sumMatrixCommand == null)
                {
                    _sumMatrixCommand = new DelegateCommand(sumMatrixExecute);
                }
                return _sumMatrixCommand;
            }
        }
        private void sumMatrixExecute()
        {
            try
            {
                MatrixA.SaveElements();
                MatrixB.SaveElements();
                Matrix result = MatrixA.matrix + MatrixB.matrix;
                MatrixC = new MatrixVM("C", result);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
           
        }

        DelegateCommand _subMatrixCommand;
        public ICommand SubMatrixCommand
        {
            get
            {
                if (_subMatrixCommand == null)
                {
                    _subMatrixCommand = new DelegateCommand(subMatrixExecute);
                }
                return _subMatrixCommand;
            }
        }
        private void subMatrixExecute()
        {
            try
            {
                MatrixA.SaveElements();
                MatrixB.SaveElements();
                Matrix result = MatrixA.matrix - MatrixB.matrix;
                MatrixC = new MatrixVM("C", result);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        DelegateCommand _multMatrixCommand;
        public ICommand MultMatrixCommand
        {
            get
            {
                if (_multMatrixCommand == null)
                {
                    _multMatrixCommand = new DelegateCommand(multMatrixExecute);
                }
                return _multMatrixCommand;
            }
        }

        private void multMatrixExecute()
        {
            try
            {
                MatrixA.SaveElements();
                MatrixB.SaveElements();
                Matrix result = MatrixA.matrix * MatrixB.matrix;
                MatrixC = new MatrixVM("C", result);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion

        MatrixVM matrixA;
        public MatrixVM MatrixA 
        {
            get { return matrixA; }
            private set { matrixA = value; RaisePropertyChanged(); }
        }

        MatrixVM matrixB;
        public MatrixVM MatrixB
        {
            get { return matrixB;}
            private set { matrixB = value; RaisePropertyChanged(); }
        }

        MatrixVM matrixC;
        public MatrixVM MatrixC
        {
            get { return matrixC; }
            private set { matrixC = value; RaisePropertyChanged(); }
        }
      

        public MainVM()
        {
            initializeProperties();
        }

        void initializeProperties()
        {
            MatrixA = new MatrixVM("A");
            MatrixB = new MatrixVM("B");
            MatrixC = new MatrixVM("C");
        }
    }
}
