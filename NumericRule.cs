using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MatrixOperations.ViewModels
{
    public class NumericRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int num;
            try
            {
                if (((string)value).Length > 0)
                    num = Int32.Parse((string)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Допустимы только числа \n" + e.Message);
            }
           
            return ValidationResult.ValidResult;
            
        }
    }
}
