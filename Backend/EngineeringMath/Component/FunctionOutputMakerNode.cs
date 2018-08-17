﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    /// <summary>
    /// Forces parameters to be outputs
    /// </summary>
    public class FunctionOutputMakerNode : FunctionTreeNode
    {
        protected FunctionOutputMakerNode() : base()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="outputParameterNames">Parameter names which will be </param>
        public FunctionOutputMakerNode(string name, params string[] outputParameterNames) : this()
        {
            Name = name;
            OutputParameterNames = new List<string>();
            if(outputParameterNames != null)
                OutputParameterNames.AddRange(outputParameterNames);
        }


        protected List<string> OutputParameterNames { get; set; }

        public override void BuildLists(List<ISetting> settings, List<Parameter> parameter)
        {
            return;
        }

        public override void Calculate()
        {
            return;
        }

        public override bool IsOutput(string parameterName)
        {
            return OutputParameterNames.Contains(parameterName);
        }
    }
}