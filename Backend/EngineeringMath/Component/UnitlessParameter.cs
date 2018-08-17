﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class UnitlessParameter : Parameter<double>
    {
        protected UnitlessParameter() : base()
        {

        }
        public UnitlessParameter(string name, double minValue = double.MinValue, double maxValue = double.MaxValue) : base(name, minValue, maxValue)
        {

        }

        protected override double BaseToBindValue(double value)
        {
            return value;
        }

        protected override double BindToBaseValue(double value)
        {
            return value;
        }
    }
}
