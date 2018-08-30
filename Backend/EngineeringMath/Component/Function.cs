﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Linq;
using EngineeringMath.Resources;

namespace EngineeringMath.Component
{
    /// <summary>
    /// Performs an engineering calculation
    /// </summary>
    public class Function : NotifyPropertyChangedExtension, IChildItem<Category<Function>>, IParameterContainerNode, ICategoryItem
    {
        protected Function()
        {
            if (!LibraryResourceFullName.Equals(string.Empty))
                FullName = (string)typeof(LibraryResources).GetProperty(LibraryResourceFullName).GetValue(null, null);
            AllParameters = new List<IParameter>();
            AllSettings = new List<ISetting>();

            Solve = new Command(
                SolveFunction,
                CanSolve
                );
        }

        private void NextNode_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e == null)
                return;
        }

        public Function(string fullName, bool isUserDefined = false) : this()
        {
            LibraryResourceFullName = isUserDefined ? string.Empty : fullName;
            FullName = LibraryResourceFullName.Equals(string.Empty) ? fullName : (string)typeof(LibraryResources).GetProperty(LibraryResourceFullName).GetValue(null, null);
        }


        #region Methods

        

        /// <summary>
        /// Function called by the solve command
        /// </summary>
        protected virtual void SolveFunction(object parameter)
        {
            OnSolving();
            this.Calculate();
            OnSolved();
        }

        /// <summary>
        /// Function called by the solve command
        /// </summary>
        protected virtual bool CanSolve(object parameter)
        {
            return true;
        }


        protected void OnSolved()
        {
            Solved?.Invoke(this, EventArgs.Empty);
        }

        protected void OnSolving()
        {
            Solving?.Invoke(this, EventArgs.Empty);
        }

        protected void OnError(Exception e)
        {
            HadError?.Invoke(this, e);
        }

        protected void OnReset()
        {
            WasReset?.Invoke(this, EventArgs.Empty);
        }

        public void Calculate()
        {
            NextNode.Calculate();
        }

        public IParameter FindParameter(string paraName)
        {
            throw new ParameterNotFoundException(paraName);
        }

        public void Reset()
        {
            AllSettings = new List<ISetting>();
            AllParameters = new List<IParameter>();
            NextNode.BuildLists(AllSettings, AllParameters);
        }

        public void SettingAdded(ISetting setting)
        {
            AllSettings.Add(setting);
        }
        public void SettingRemoved(ISetting setting)
        {
            AllSettings.Remove(setting);
        }
        public void SettingRemoved(IList<ISetting> settings)
        {
            foreach(ISetting setting in settings)
            {
                AllSettings.Remove(setting);
            }
        }

        public void ParameterAdded(IParameter parameter)
        {
            AllParameters.Add(parameter);
        }
        public void ParameterRemoved(IParameter parameter)
        {
            AllParameters.Remove(parameter);
        }
        public void ParameterRemoved(IList<IParameter> parameters)
        {
            foreach (IParameter parameter in parameters)
            {
                AllParameters.Remove(parameter);
            }
        }
        #endregion

        #region Properties

        public IEnumerable<IParameter> InputParameters
        {
            get
            {
                return from para in AllParameters
                       where para.CurrentState == ParameterState.Input
                       orderby para.ToString()
                       select para;
            }
        }

        public IEnumerable<IParameter> OutputParameters
        {
            get
            {
                return from para in AllParameters
                        where para.CurrentState == ParameterState.Output
                        orderby para.ToString()
                        select para;
            }
        }


        public IEnumerable<ISetting> ActiveSettings
        {
            get
            {
                return from setting in AllSettings
                        where setting.CurrentState == SettingState.Active
                        orderby setting.ToString()
                        select setting;
            }
        }

        private string _FullName;

        public string FullName
        {
            get { return _FullName; }
            set
            {
                _FullName = value;
                OnPropertyChanged();
            }
        }



        private Command _Solve;
        /// <summary>
        /// Solves this function 
        /// </summary>
        [XmlIgnore]
        public Command Solve
        {
            get
            {
                return _Solve;
            }
            private set
            {
                _Solve = value;
                OnPropertyChanged();
            }
        }

        private List<ISetting> _AllSettings;
        [XmlIgnore]
        public List<ISetting> AllSettings
        {
            get
            {
                return _AllSettings;
            }
            protected set
            {
                _AllSettings = value;
                OnPropertyChanged();
            }
        }


        private List<IParameter> _AllParameters;
        [XmlIgnore]
        public List<IParameter> AllParameters
        {
            get
            {
                return _AllParameters;
            }
            protected set
            {
                _AllParameters = value;
                OnPropertyChanged();
            }
        }


        private Command _Save;
        /// <summary>
        /// Saves this functions
        /// </summary>
        public Command Save
        {
            get
            {
                return _Save;
            }
            private set
            {
                _Save = value;
                OnPropertyChanged();
            }
        }

        FunctionTreeNode _NextNode;

        public FunctionTreeNode NextNode
        {
            get { return _NextNode; }
            set
            {
                if (_NextNode != null && _NextNode.Equals(value))
                    return;
                if(_NextNode != null)
                {
                    _NextNode.Parent = null;
                }                    

                _NextNode = value;
                if(_NextNode != null)
                {
                    _NextNode.CurrentState = FunctionTreeNodeState.Active;
                    _NextNode.Parent = this;
                }
                OnPropertyChanged();
            }
        }

        public bool IsUserDefined { get { return LibraryResourceFullName.Equals(string.Empty); } }
        /// <summary>
        /// If the full name is a reference to LibraryResources then this string will equal the property name
        /// </summary>
        internal string LibraryResourceFullName { get; } = string.Empty;

        [XmlIgnore]
        private Category<Function> ParentObject { get; set; }


        public Category<Function> Parent
        {
            get
            {
                return this.ParentObject;
            }
            internal set
            {
                this.ParentObject = value;
            }
        }

        Category<Function> IChildItem<Category<Function>>.Parent { get => Parent; set => Parent = value; }

        string IChildItem<Category<Function>>.Key => FullName;

        #endregion

        #region Events
        public event EventHandler Solved;

        public event EventHandler Solving;

        public event ErrorEventHandler HadError;

        public delegate void ErrorEventHandler(object sender, Exception e);

        public event EventHandler WasReset;
        #endregion

        public class ParameterNotFoundException : ArgumentException
        {
            public ParameterNotFoundException(string ParameterName) : base(string.Empty, paramName: ParameterName)
            {

            }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
