using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SIS.Framework.Attributes.Property
{
    public class NumberRangeAttribute: ValidationAttribute
    {
        private readonly double minimumValue;
        private readonly double maximumValue;

        public NumberRangeAttribute(double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            this.minimumValue = minValue;
            this.maximumValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return this.minimumValue <= (double)value && (double)value <= this.maximumValue;
        }
    }
}
