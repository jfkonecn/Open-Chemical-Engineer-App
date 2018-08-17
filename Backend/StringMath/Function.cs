﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StringMath
{
    internal class Function : IOperator
    {

        private Function(string methodName)
        {
            MethodInfo = typeof(Math).GetMethod(methodName);
        }

        public ushort TotalParameters { get; internal set; } = 1;


        private readonly MethodInfo MethodInfo;

        public ushort Precedence { get { return ushort.MaxValue; } }

        public OperatorAssociativity Associativity { get { return OperatorAssociativity.LeftAssociative; } }

        internal static bool TryGetFunction(ref string equationString,
            IEquationToken previousToken,
            out Function fun)
        {
            if (previousToken == null || previousToken as IOperator != null)
            {
                if (HelperFunctions.RegularExpressionParser(@"^\s*[\w\_]+[\w\d]*\(", ref equationString, out string matchStr))
                {
                    // '(' should be the last character
                    fun = new Function(matchStr.Remove(matchStr.Length - 1));
                    equationString = $"({ equationString }";                    
                    return true;
                }
            }
            fun = null;
            return false;
        }
        internal static bool IsFunctionArgumentSeparator(ref string equationString)
        {
            return HelperFunctions.RegularExpressionParser(@"^\s*,", ref equationString);
        }

        public double Evaluate(ref Stack<double> vs)
        {
            if (TotalParameters > vs.Count)
                throw new SyntaxException();

            Stack<object> nums = new Stack<object>();
            for(int i = 0; i < TotalParameters; i++)
            {
                nums.Push(vs.Pop());
            }

            try
            {
                return (double)MethodInfo.Invoke(null, nums.ToArray());
            }
            catch
            {
                throw new SyntaxException();
            }            
        }
    }
}
