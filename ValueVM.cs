using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixOperations.ViewModels
{
    public class ValueVM<T>
    {
        public T Value { get; set; }
        public ValueVM(T value)
        {
            Value = value;
        }
    }
}
