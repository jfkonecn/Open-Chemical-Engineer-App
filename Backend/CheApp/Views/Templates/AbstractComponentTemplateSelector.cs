﻿using EngineeringMath.Calculations.Components.Commands;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Parameter;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using CheApp.Behaviors;
using EngineeringMath.Calculations.Components.Selectors;
using EngineeringMath.Calculations.Components.Group;
using System.Diagnostics;
using EngineeringMath.Units;
using CheApp.Converter.Component;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Resources;
using System.Collections.ObjectModel;
using CheApp.CustomUI;
using CheApp.Converter;

namespace CheApp.Views.Templates
{
    public class AbstractComponentTemplateSelector : DataTemplateSelector
    {
        public AbstractComponentTemplateSelector()
        {
            BuildGroupOfComponentsDataTemplate();
            BuildButtonDataTemplate();
            BuildParameterDataTemplates();
            BuildSimplePickerDataTemplate();
            BuildFunctionSubberDataTemplate();
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if(item as AbstractComponent != null)
            {
                if (!((AbstractComponent)item).IsErrorEventHandlerRegistered(AbstractComponentTemplateSelector_OnErrorEvent))
                {
                    ((AbstractComponent)item).OnErrorEvent += AbstractComponentTemplateSelector_OnErrorEvent;
                }                
            }
            else
            {
                throw new ArgumentException("Item must be a typeof AbstractComponent");
            }

            if (item as AbstractGroupOfComponents != null)
            {
                return GroupOfComponentsDataTemplate;
            }
            else if(item as ButtonComponent != null)
            {
                return ButtonDataTemplate;
            }
            else if (item as SimpleParameter != null)
            {
                int unitCount = ((SimpleParameter)item).UnitSelection.Count;
                if (item as SubFunctionParameter != null)
                {
                    return ParameterDataTemplates[ParameterType.SubFunctionParameter][unitCount];
                }
                return ParameterDataTemplates[ParameterType.SimpleParameter][unitCount];
            }
            else if (item as SimplePicker<SimpleParameter> != null
                || item as SimplePicker<int> != null
                || item as FunctionPicker != null
                || item as SimplePicker<AbstractUnit> != null)
            {
                return SimplePickerDataTemplate;
            }
            else if (item as FunctionSubber != null)
            {
                return FunctionSubberDataTemplate;
            }
            else
            {
                throw new NotImplementedException("That type of abstract component is not handled in AbstractComponentTemplateSelector!");
            }
        }

        private void AbstractComponentTemplateSelector_OnErrorEvent(AbstractComponent sender, Exception e)
        {
            App.Current.MainPage.DisplayAlert(LibraryResources.ErrorMessageTitle,
                $"{ sender.Title }: { e.Message }",
                LibraryResources.Okay);
        }

        /// <summary>
        /// Creates a path to 
        /// </summary>
        /// <param name="pathToComponent">Leave null or empty if the binding context contains the target Property name</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private string CreateBindingPathToProperty(string pathToComponent, string propertyName)
        {
            string path;
            if (pathToComponent == null || pathToComponent == string.Empty)
            {
                path = string.Empty;
            }
            else
            {
                path = $"{ pathToComponent }.";
            }

            return $"{ path }.{ propertyName }";
        }

        /// <summary>
        /// Builds a view to represent a group of components, but will have no data context 
        /// </summary>
        /// <param name="pathToComponent">
        /// If the data context will not be an AbstractGroupOfComponents then add a path from the context to the component else leave this blank.
        /// <para>
        /// If MyObject will be the data context and MyObject.MyOtherObject.MyAbstractGroupOfComponents is the location of the
        /// AbstractGroupOfComponents then pass MyOtherObject.MyAbstractGroupOfComponents
        /// </para>
        /// </param>
        /// <returns></returns>
        private ListView AbstractGroupOfComponentsViewWithBindings(string pathToComponent = null)
        {
            ListView view = new ListView()
            {
                SeparatorVisibility = SeparatorVisibility.Default,
                HasUnevenRows = true,
                ItemTemplate = this
            };
            view.SetBinding(ListView.ItemsSourceProperty, CreateBindingPathToProperty(pathToComponent, "ComponentCollection"));
            


            return view; 
                
                
        }

        /// <summary>
        /// Creates a picker which is binding created for a type of simple picker, but will have no data context
        /// </summary>
        /// <param name="pathToComponent">
        /// If the data context will not be an SimplePicker then add a path from the context to the component else leave this blank.
        /// <para>
        /// If MyObject will be the data context and MyObject.MyOtherObject.MySimplePicker is the location of the
        /// SimplePicker then pass MyOtherObject.MySimplePicker
        /// </para>
        /// </param>
        /// <returns></returns>
        private Picker CreatePickerWithBindings(string pathToComponent = null)
        {
            Picker picker = new Picker()
            {

            };
            picker.SetBinding(Picker.ItemsSourceProperty, CreateBindingPathToProperty(pathToComponent, "PickerList"));
            picker.SetBinding(Picker.SelectedIndexProperty, CreateBindingPathToProperty(pathToComponent, "SelectedIndex"));
            picker.SetBinding(Picker.IsEnabledProperty, CreateBindingPathToProperty(pathToComponent, "IsEnabled"));
            return picker;
        }

        /// <summary>
        /// Creates a button which is binding created for a type of button component, but will have no data context
        /// </summary>
        /// <param name="pathToComponent">
        /// If the data context will not be an ButtonComponent then add a path from the context to the component else leave this blank.
        /// <para>
        /// If MyObject will be the data context and MyObject.MyOtherObject.MyButtonComponent is the location of the
        /// ButtonComponent then pass MyOtherObject.MyButtonComponent
        /// </para>
        /// </param>
        /// <returns></returns>
        private Button CreateButtonWithBindings(string pathToComponent = null)
        {
            Button btn = new Button()
            {
                
            };
            
            btn.SetBinding(Button.CommandProperty, CreateBindingPathToProperty(pathToComponent, "Command"));
            btn.SetBinding(Button.TextProperty, CreateBindingPathToProperty(pathToComponent, "Title"));
            return btn;
        }

        /// <summary>
        /// Creates a stacklayout which is binding with a SimpleParameter for a type of button component, but will have no data context
        /// </summary>
        /// <param name="pathToComponent">
        /// If the data context will not be a SimpleParameter then add a path from the context to the component else leave this blank.
        /// <para>
        /// If MyObject will be the data context and MyObject.MyOtherObject.MySimpleParameter is the location of the
        /// SimpleParameter then pass MyOtherObject.MySimpleParameter
        /// </para>
        /// </param>
        /// <returns></returns>
        private StackLayout CreateUnitPickersWithBindings(int unitCount, string pathToComponent = null)
        {



            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal
            };
            
            if(unitCount == 1)
            {
                Picker unitPicker = CreatePickerWithBindings();
                unitPicker.SetBinding(Picker.BindingContextProperty, new Binding("UnitSelection[0]"));
                stackLayout.Children.Add(unitPicker);
            }
            else if(unitCount == 2)
            {
                Picker[] unitPicker = new Picker[] { CreatePickerWithBindings(), CreatePickerWithBindings() };
                unitPicker[0].SetBinding(Picker.BindingContextProperty, new Binding("UnitSelection[0]"));
                Label perLbl = new Label
                {
                    Text = LibraryResources.Per,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                unitPicker[1].SetBinding(Picker.BindingContextProperty, new Binding("UnitSelection[1]"));

                stackLayout.Children.Add(unitPicker[0]);
                stackLayout.Children.Add(perLbl);
                stackLayout.Children.Add(unitPicker[1]);
            }

            return stackLayout;
        }



        /// <summary>
        /// Sets the GroupOfComponentsDataTemplate property
        /// </summary>
        private void BuildGroupOfComponentsDataTemplate()
        {
            GroupOfComponentsDataTemplate = new DataTemplate(() => 
            {
                return AbstractGroupOfComponentsViewWithBindings();
            });
        }


        public DataTemplate GroupOfComponentsDataTemplate { get; private set; }


        /// <summary>
        /// Sets the ButtonDataTemplate property
        /// </summary>
        private void BuildButtonDataTemplate()
        {
            ButtonDataTemplate = new DataTemplate(() => 
            {
                return new ViewCell { View = CreateButtonWithBindings() };
            });
        }

        public DataTemplate ButtonDataTemplate { get; private set; }



        public enum ParameterType
        {
            SimpleParameter,
            SubFunctionParameter
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paraType"></param>
        /// <returns></returns>
        private ViewCell CreateParameterStack(ParameterType paraType, int unitCount)
        {
            // title
            Label titleLb = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
            titleLb.SetBinding(Label.TextProperty, "Title");

            // subfunction picker
            Picker subFunPicker = CreatePickerWithBindings();
            subFunPicker.SetBinding(Picker.BindingContextProperty, "SubFunctionSelection");

            // subfunction button
            Button subFunBtn = new Button() { };
            subFunBtn.SetBinding(Button.TextProperty, "SubFunctionButton.Title");
            subFunBtn.SetBinding(Button.CommandProperty, "SubFunctionButton.Command");
            subFunBtn.SetBinding(Button.IsEnabledProperty, "AllowSubFunctionClick");
            AbstractComponentNavigationButtonBehavior subFunBtnBehavior = new AbstractComponentNavigationButtonBehavior();
            subFunBtnBehavior.SetBinding(AbstractComponentNavigationButtonBehavior.ComponentProperty, "SubFunctionSelection.SubFunction");
            subFunBtn.Behaviors.Add(subFunBtnBehavior);

            // Number Label
            Entry inputEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };
            inputEntry.SetBinding(Entry.TextProperty, new Binding("ValueStr"));
            inputEntry.SetBinding(Entry.IsEnabledProperty, new Binding("AllowUserInput"));
            inputEntry.SetBinding(Entry.PlaceholderProperty, new Binding("Placeholder"));

            // Unit Pickers
            ListView unitView = new ListView()
            {
                ItemTemplate = this,
            };
            unitView.SetBinding(ListView.ItemsSourceProperty, "UnitSelection");

            // Error Label
            Label errorLb = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };
            errorLb.SetBinding(Label.TextProperty, "ErrorMessage");


            CollapsibleViewCell cell = new CollapsibleViewCell();
            cell.SetBinding(CollapsibleViewCell.HeaderProperty, "Title");
            cell.SetBinding(CollapsibleViewCell.IsCollapsedProperty, "AllowUserInput");
            cell.ExpandedView.Children.Add(titleLb);
            if (paraType == ParameterType.SubFunctionParameter)
            {
                cell.ExpandedView.Children.Add(subFunPicker);
                cell.ExpandedView.Children.Add(subFunBtn);
            }
            cell.ExpandedView.Children.Add(inputEntry);
            cell.ExpandedView.Children.Add(CreateUnitPickersWithBindings(unitCount));
            cell.ExpandedView.Children.Add(errorLb);
            cell.ExpandedView.SetBinding(StackLayout.BindingContextProperty, "BindingContext");
            return cell;
        }


        /// <summary>
        /// Sets the ParameterDataTemplates property
        /// </summary>
        /// <param name="paraType"></param>
        private void BuildParameterDataTemplates()
        {

            ParameterDataTemplates = new Dictionary<ParameterType, Dictionary<int, DataTemplate>>
            {
                { ParameterType.SimpleParameter, new Dictionary<int, DataTemplate>() },
                { ParameterType.SubFunctionParameter, new Dictionary<int, DataTemplate>() }
            };

            for(int i = 1; i < 3; i++)
            {
                // avoid pointer sharing
                int temp = i;
                ParameterDataTemplates[ParameterType.SimpleParameter][temp] = new DataTemplate(() =>
                {
                    return CreateParameterStack(ParameterType.SimpleParameter, temp);
                });
                ParameterDataTemplates[ParameterType.SubFunctionParameter][temp] = new DataTemplate(() =>
                {
                    return CreateParameterStack(ParameterType.SubFunctionParameter, temp);
                });

            }

        }


        public Dictionary<ParameterType, Dictionary<int, DataTemplate>> ParameterDataTemplates { get; private set; }

        private void BuildSimplePickerDataTemplate()
        {
            SimplePickerDataTemplate = new DataTemplate(() => 
            {
                return new ViewCell { View = CreatePickerWithBindings() };
            });
        }
        public DataTemplate SimplePickerDataTemplate { get; private set; }

        private void BuildFunctionSubberDataTemplate()
        {
            FunctionSubberDataTemplate = new DataTemplate(() =>
            {
                Picker picker = CreatePickerWithBindings();
                picker.SetBinding(Picker.BindingContextProperty, "AllFunctions");
                ListView listView = new ListView()
                {
                    ItemTemplate = this
                };
                listView.SetBinding(ListView.ItemsSourceProperty, "AllFunctions.SubFunction");
                return new StackLayout
                {
                    Children =
                        {
                            picker,
                            listView
                        }
                };
            });
        }
        public DataTemplate FunctionSubberDataTemplate { get; private set; }
    }
}
