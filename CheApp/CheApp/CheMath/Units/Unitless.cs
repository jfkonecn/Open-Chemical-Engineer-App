﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    public class Unitless : AbstractUnit
    {
        /// <summary>
        /// grams
        /// </summary>
        public static readonly Unitless unitless = new Unitless("Unitless", 1);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Unitless> StringToUnit = new Dictionary<string, Unitless>
        {
            { unitless.ToString(), unitless }
        };


        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        private Unitless(string name, double conversionFactor) : base(name, conversionFactor) { }



        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnitName">String name od desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, string desiredUnitName)
        {
            return curValue;
        }

    }
}
