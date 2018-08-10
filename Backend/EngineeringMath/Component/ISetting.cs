﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public interface ISetting
    {
        int SelectedIndex { get; set; }

        IList<string> AllOptions { get; }
    }
}