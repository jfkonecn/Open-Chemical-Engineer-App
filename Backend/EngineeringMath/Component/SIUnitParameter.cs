﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class SIUnitParameter : Parameter
    {
        protected SIUnitParameter() : base()
        {
            
        }
        public SIUnitParameter(string displayName, string varName, string unitCategoryName, 
            double minSIValue = double.MinValue, double maxSIValue = double.MaxValue) 
            : base(displayName, varName, minSIValue, maxSIValue)
        {
            UnitCategoryName = unitCategoryName;
        }

        protected override double BaseToBindValue(double value)
        {
            return UnitCategory.ConverterFromSIUnit(ParameterUnits.ItemAtSelectedIndex.FullName, value);
        }

        protected override double BindToBaseValue(double value)
        {
            return UnitCategory.ConverterToSIUnit(ParameterUnits.ItemAtSelectedIndex.FullName, value);
        }


        [XmlIgnore]
        private UnitCategory UnitCategory { get; set; }


        private SelectableList<Unit, Category<Unit>> _ParameterUnits;
        [XmlIgnore]
        public SelectableList<Unit, Category<Unit>> ParameterUnits
        {
            get { return _ParameterUnits; }
            set
            {
                _ParameterUnits = value;
                OnPropertyChanged(nameof(DisplayDetail));
                OnPropertyChanged();
            }
        }



        private string _UnitCategoryName;
        public string UnitCategoryName
        {
            get
            {
                return _UnitCategoryName;
            }
            set
            {
                _UnitCategoryName = value;
                UnitCategory = MathManager.AllUnits.GetCategoryByName(_UnitCategoryName);
                ParameterUnits = new SelectableList<Unit, Category<Unit>>(VarName, UnitCategory.Children);
                OnPropertyChanged();
            }
        }

        public override string DisplayDetail
        {
            get
            {
                return $"{BindValue} {ParameterUnits.ItemAtSelectedIndex.Symbol}";
            }
        }
    }
}
