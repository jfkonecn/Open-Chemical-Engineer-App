﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EngineeringMath.Calculations.Components.Selectors
{
    /// <summary>
    /// Stores data where the user selects something in a picker which represents an object
    /// <para>This object allows the user to bind the results of a user's selection</para>
    /// <para>Bind with PickerList to fill your picker and bind with SelectedIndex to bind with the current index selected</para>
    /// <para>Note that the object.equals function is use</para>
    /// </summary>
    /// <typeparam name="T">The object which will represent the user's selection</typeparam>
    public class SimplePicker<T> : AbstractComponent
    {
        private static readonly string TAG = "PickerSelection:";
        /// <summary>
        /// Bind with PickerList to fill your picker and bind with SelectedIndex to bind with the current index selected
        /// </summary>
        /// <param name="objectLookup">The string key is the string which is intened to be place in the picker to represent object T</param>
        /// <param name="title">Title of the picker</param>
        internal SimplePicker(Dictionary<string, T> objectLookup, string title = null)
        {
            _ObjectLookup = objectLookup;
            this.IsEnabled = true;
            this.Title = title;
        }

        /// <summary>
        /// The string in the picker (to be binded with the picker)
        /// </summary>
        public List<string> PickerList
        {
            get
            {
                return ObjectLookup.Keys.ToList();
            }
        }

        private int _SelectedIndex = -1;
        /// <summary>
        /// The index selected in the picker (to be binded with the picker)
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                // -1 means nothing is selected
                if (value < _ObjectLookup.Count && value >= 0)
                {
                    PreviouslySelectedIndex = _SelectedIndex;
                    _SelectedIndex = value;
                }
                else
                {
                    Debug.WriteLine($"{TAG} SelectedIndex value is out of range!");
                }
                OnSelectedIndexChanged?.Invoke(this, new EventArgs());
                OnPropertyChanged("SelectedObject");
                OnPropertyChanged();
            }
        }


        private int _PreviouslySelectedIndex = -1;
        /// <summary>
        /// The index selected before the currently selected index
        /// </summary>
        public int PreviouslySelectedIndex
        {
            get
            {
                return _PreviouslySelectedIndex;
            }
            private set
            {
                _PreviouslySelectedIndex = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public delegate void SelectedIndexChangedHandler(AbstractComponent sender, EventArgs e);

        /// <summary>
        /// Called when this parameter is made to be an output
        /// </summary>
        public event SelectedIndexChangedHandler OnSelectedIndexChanged;

        /// <summary>
        /// Returns the selected objected assuming the SelectedIndex and the PickerList is binded to the correct picker
        /// </summary>
        public T SelectedObject
        {
            get
            {
                if (SelectedIndex == -1)
                {
                    Debug.WriteLine($"{TAG} Nothing is selected");
                    return default(T);
                }
                Debug.WriteLine($"{TAG} \"{ObjectLookup.Keys.ToList()[SelectedIndex]}\" was selected");
                return ObjectLookup.Values.ToList()[SelectedIndex];
            }
            set
            {
                // object must in the _ObjectLookup
                int temp = 0;
                foreach (T obj in _ObjectLookup.Values.ToList())
                {
                    if ((value == null && obj == null) || obj.Equals(value))
                    {
                        SelectedIndex = temp;
                        return;
                    }
                    temp++;
                }
                OnPropertyChanged();
                OnPropertyChanged("SelectedIndex");
                Debug.WriteLine($"{TAG} Couldn't find the object you were looking for");
            }
        }


        /// <summary>
        /// The index selected before the currently selected index
        /// </summary>
        public T PreviouslySelectedObject
        {
            get
            {
                if (PreviouslySelectedIndex == -1)
                {
                    return default(T);
                }
                return ObjectLookup.Values.ToList()[PreviouslySelectedIndex];
            }
        }


        private Dictionary<string, T> _ObjectLookup;
        private Dictionary<string, T> ObjectLookup
        {
            get
            {
                return _ObjectLookup;
            }
            set
            {
                _ObjectLookup = value;
                OnPropertyChanged("PickerList");
            }
        }

        bool _IsEnabled = true;
        /// <summary>
        /// True if the picker is enabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                OnPropertyChanged();
            }
        }

    }
}
