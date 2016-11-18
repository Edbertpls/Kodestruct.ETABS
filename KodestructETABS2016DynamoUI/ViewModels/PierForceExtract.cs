﻿ 
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Dynamo.Controls;
using Dynamo.Models;
using Dynamo.Wpf;
using ProtoCore.AST.AssociativeAST;
using System.Xml;
using Dynamo.Nodes;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using Kodestruct.ETABS.v2016.Dynamo.Common;
using Kodestruct.ETABS.v2016.ModelOutput.Frame;
using GalaSoft.MvvmLight.Command;
using Kodestruct.ETABS.v2016.Interop.Entities.Group;
using Kodestruct.ETABS.v2016.Interop;
using System.Collections.ObjectModel;
using Kodestruct.ETABS.v2016.Interop.Entities.Frame.ForceExtraction;
using Kodestruct.ETABS.v2016.Interop.Entities.Frame;
using Kodestruct.ETABS.v2016.ModelOutput.Wall;


namespace Kodestruct.ETABS.v2016.ModelOutput.Frame
{

    /// <summary>
    ///Selected element, force at end extraction 
    /// </summary>

    [NodeName("Extract pier forces for a pier definition ")]
    [NodeCategory("Kodestruct.ETABS.v2016.ModelOutput.Wall")]
    [NodeDescription("Extract pier forces for a pier definition")]
    [IsDesignScriptCompatible]
    public class PierForceExtract : UiNodeBase
    {

        public PierForceExtract()
        {


            OutPortData.Add(new PortData("V_major_max", "Major (strong axis) shear"));
            OutPortData.Add(new PortData("V_major_min", "Major (strong axis) shear"));
            OutPortData.Add(new PortData("M_major_max", "Major (strong axis) moment"));
            OutPortData.Add(new PortData("M_major_min", "Major (strong axis) moment"));
            OutPortData.Add(new PortData("P_max", "Axial force"));
            OutPortData.Add(new PortData("P_min", "Axial force "));
            OutPortData.Add(new PortData("V_minor_max", "Minor (weak axis) shear"));
            OutPortData.Add(new PortData("V_minor_min", "Minor (weak axis) shear"));
            OutPortData.Add(new PortData("M_minor_max", "Minor (weak axis) moment"));
            OutPortData.Add(new PortData("M_minor_min", "Minor (weak axis) moment"));

            RegisterAllPorts();
            SetDefaultParameters();

            RefreshCommand = new RelayCommand(RefreshEtabsData, canGetForces);
            GetReactionsCommand = new RelayCommand(GetForces, canGetForces);
            //PropertyChanged += NodePropertyChanged;
        }



        private void SetDefaultParameters()
        {
            V_major_max = 0.0;
            V_major_min = 0.0;
            M_major_max = 0.0;
            M_major_min = 0.0;
            P_max = 0.0;
            P_min = 0.0;
            V_minor_max = 0.0;
            V_minor_min = 0.0;
            M_minor_max = 0.0;
            M_minor_min = 0.0;
            ErrorMessage = "";
        }



        /// <summary>
        ///     Gets the type of this class, to be used in base class for reflection
        /// </summary>
        protected override Type GetModelType()
        {
            return GetType();
        }

        #region properties

        #region InputProperties

        #region AvaliableCombosProperty

        /// <summary>
        /// AvaliableCombos property
        /// </summary>
        /// <value>AvaliableCombos</value>
        public ObservableCollection<string> _AvaliableCombos;

        public ObservableCollection<string> AvaliableCombos
        {
            get { return _AvaliableCombos; }
            set
            {
                _AvaliableCombos = value;
                RaisePropertyChanged("AvaliableCombos");
                //OnNodeModified(true); 
            }
        }
        #endregion

        #region SelectedComboProperty

        /// <summary>
        /// SelectedCombo property
        /// </summary>
        /// <value>SelectedCombo</value>
        public string _SelectedCombo;

        public string SelectedCombo
        {
            get { return _SelectedCombo; }
            set
            {
                _SelectedCombo = value;
                RaisePropertyChanged("SelectedCombo");
                //OnNodeModified(true); 
            }
        }
        #endregion

        #region ErrorMessageProperty

        /// <summary>
        /// ErrorMessage property
        /// </summary>
        /// <value>ErrorMessage</value>
        public string _ErrorMessage;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                RaisePropertyChanged("ErrorMessage");
                //OnNodeModified(true); 
            }
        }
        #endregion

        #region AvaliablePiersProperty

        /// <summary>
        /// AvaliablePiers property
        /// </summary>
        /// <value>AvaliablePiers</value>
        public ObservableCollection<string> _AvaliablePiers;

        public ObservableCollection<string> AvaliablePiers
        {
            get { return _AvaliablePiers; }
            set
            {
                _AvaliablePiers = value;
                RaisePropertyChanged("AvaliablePiers");
                //OnNodeModified(true); 
            }
        }
        #endregion

        #region AvaliableStoriesProperty

        /// <summary>
        /// AvaliableStories property
        /// </summary>
        /// <value>AvaliableStories</value>
        public ObservableCollection<string> _AvaliableStories;

        public ObservableCollection<string> AvaliableStories
        {
            get { return _AvaliableStories; }
            set
            {
                _AvaliableStories = value;
                RaisePropertyChanged("AvaliableStories");
                //OnNodeModified(true); 
            }
        }
        #endregion

        #endregion

        #region OutputProperties


        #region V_major_maxProperty

        /// <summary>
        /// V_major_max property
        /// </summary>
        /// <value>V_major_max</value>
        public double _V_major_max;

        public double V_major_max
        {
            get { return _V_major_max; }
            set
            {
                _V_major_max = value;
                RaisePropertyChanged("V_major_max");
                OnNodeModified(true);
            }
        }
        #endregion

        #region V_major_minProperty

        /// <summary>
        /// V_major_min property
        /// </summary>
        /// <value>V_major_min</value>
        public double _V_major_min;

        public double V_major_min
        {
            get { return _V_major_min; }
            set
            {
                _V_major_min = value;
                RaisePropertyChanged("V_major_min");
                OnNodeModified(true);
            }
        }
        #endregion

        #region M_major_maxProperty

        /// <summary>
        /// M_major_max property
        /// </summary>
        /// <value>M_major_max</value>
        public double _M_major_max;

        public double M_major_max
        {
            get { return _M_major_max; }
            set
            {
                _M_major_max = value;
                RaisePropertyChanged("M_major_max");
                OnNodeModified(true);
            }
        }
        #endregion

        #region M_major_minProperty

        /// <summary>
        /// M_major_min property
        /// </summary>
        /// <value>M_major_min</value>
        public double _M_major_min;

        public double M_major_min
        {
            get { return _M_major_min; }
            set
            {
                _M_major_min = value;
                RaisePropertyChanged("M_major_min");
                OnNodeModified(true);
            }
        }
        #endregion

        #region P_maxProperty

        /// <summary>
        /// P_max property
        /// </summary>
        /// <value>P_max</value>
        public double _P_max;

        public double P_max
        {
            get { return _P_max; }
            set
            {
                _P_max = value;
                RaisePropertyChanged("P_max");
                OnNodeModified(true);
            }
        }
        #endregion

        #region P_minProperty

        /// <summary>
        /// P_min property
        /// </summary>
        /// <value>P_min</value>
        public double _P_min;

        public double P_min
        {
            get { return _P_min; }
            set
            {
                _P_min = value;
                RaisePropertyChanged("P_min");
                OnNodeModified(true);
            }
        }
        #endregion

        #region V_minor_maxProperty

        /// <summary>
        /// V_minor_max property
        /// </summary>
        /// <value>V_minor_max</value>
        public double _V_minor_max;

        public double V_minor_max
        {
            get { return _V_minor_max; }
            set
            {
                _V_minor_max = value;
                RaisePropertyChanged("V_minor_max");
                OnNodeModified(true);
            }
        }
        #endregion

        #region V_minor_minProperty

        /// <summary>
        /// V_minor_min property
        /// </summary>
        /// <value>V_minor_min</value>
        public double _V_minor_min;

        public double V_minor_min
        {
            get { return _V_minor_min; }
            set
            {
                _V_minor_min = value;
                RaisePropertyChanged("V_minor_min");
                OnNodeModified(true);
            }
        }
        #endregion

        #region M_minor_maxProperty

        /// <summary>
        /// M_minor_max property
        /// </summary>
        /// <value>M_minor_max</value>
        public double _M_minor_max;

        public double M_minor_max
        {
            get { return _M_minor_max; }
            set
            {
                _M_minor_max = value;
                RaisePropertyChanged("M_minor_max");
                OnNodeModified(true);
            }
        }
        #endregion

        #region M_minor_minProperty

        /// <summary>
        /// M_minor_min property
        /// </summary>
        /// <value>M_minor_min</value>
        public double _M_minor_min;

        public double M_minor_min
        {
            get { return _M_minor_min; }
            set
            {
                _M_minor_min = value;
                RaisePropertyChanged("M_minor_min");
                OnNodeModified(true);
            }
        }
        #endregion

        #endregion
        #endregion

        #region Serialization

        /// <summary>
        ///Saves property values to be retained when opening the node     
        /// </summary>
        protected override void SerializeCore(XmlElement nodeElement, SaveContext context)
        {
            base.SerializeCore(nodeElement, context);
            //nodeElement.SetAttribute("BeamCopeCase", BeamCopeCase);
            nodeElement.SetAttribute("V_major_max", V_major_max.ToString());
            nodeElement.SetAttribute("V_major_min", V_major_min.ToString());
            nodeElement.SetAttribute("M_major_max", M_major_max.ToString());
            nodeElement.SetAttribute("M_major_min", M_major_min.ToString());
            nodeElement.SetAttribute("P_max", P_max.ToString());
            nodeElement.SetAttribute("P_min", P_min.ToString());
            nodeElement.SetAttribute("V_minor_max", V_minor_max.ToString());
            nodeElement.SetAttribute("V_minor_min", V_minor_min.ToString());
            nodeElement.SetAttribute("M_minor_max", M_minor_max.ToString());
            nodeElement.SetAttribute("M_minor_min", M_minor_min.ToString());


            nodeElement.SetAttribute("SelectedCombo", SelectedCombo);

        }

        /// <summary>
        ///Retrieved property values when opening the node     
        /// </summary>
        protected override void DeserializeCore(XmlElement nodeElement, SaveContext context)
        {
            base.DeserializeCore(nodeElement, context);


            var attribV_major_max = nodeElement.Attributes["V_major_max"]; V_major_max = Double.Parse(attribV_major_max.Value);
            var attribV_major_min = nodeElement.Attributes["V_major_min"]; V_major_min = Double.Parse(attribV_major_min.Value);
            var attribM_major_max = nodeElement.Attributes["M_major_max"]; M_major_max = Double.Parse(attribM_major_max.Value);
            var attribM_major_min = nodeElement.Attributes["M_major_min"]; M_major_min = Double.Parse(attribM_major_min.Value);
            var attribP_max = nodeElement.Attributes["P_max"]; P_max = Double.Parse(attribP_max.Value);
            var attribP_min = nodeElement.Attributes["P_min"]; P_min = Double.Parse(attribP_min.Value);
            var attribV_minor_max = nodeElement.Attributes["V_minor_max"]; V_minor_max = Double.Parse(attribV_minor_max.Value);
            var attribV_minor_min = nodeElement.Attributes["V_minor_min"]; V_minor_min = Double.Parse(attribV_minor_min.Value);

            var attribM_minor_max = nodeElement.Attributes["M_minor_max"]; M_minor_max = Double.Parse(attribM_minor_max.Value);
            var attribM_minor_min = nodeElement.Attributes["M_minor_min"]; M_minor_min = Double.Parse(attribM_minor_min.Value);

            var attrSC = nodeElement.Attributes["SelectedCombo"]; SelectedCombo = attrSC.Value;



        }


        #endregion

        #region Commands

        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand GetReactionsCommand { get; private set; }

        private void RefreshEtabsData()
        {
            ErrorMessage = "";

            try
            {
                ETABSModelManager manager = new ETABSModelManager();

                List<string> comboNames = manager.GetModelComboNames();
                AvaliableCombos = new ObservableCollection<string>(comboNames);

                List<string> PierNames = manager.GetModelPierNames();
                AvaliablePiers = new ObservableCollection<string>(PierNames);

                List<string> StoryNames = manager.GetModelStoryNames();
                AvaliableStories = new ObservableCollection<string>(StoryNames);
            }
            catch (Exception)
            {

                ErrorMessage = "Could not connect to ETABS model.";
            }


        }


        private void GetForces()
        {
            try
            {
                ETABSModelManager manager = new ETABSModelManager();
                List<string> comboNames = manager.GetModelComboNames();
                AvaliableCombos = new ObservableCollection<string>(comboNames);

                List<WallForceResult> r = manager.GetPierForces(SelectedCombo, Entities.Enums.PierPointLocation.Bottom, ModelUnits.kip_in);

        ////public List<WallForceResult> GetPierForces(string ComboName, PierPointLocation PierPointLocation, ModelUnits ModelUnits)
        //        ErrorMessage = "";
        //        FrameDataExtractor mde = new FrameDataExtractor();
        //        FrameEnvelopeForceResult result = mde.GetSelectedFrameForces(SelectedCombo, "kip_in");
        //        V_major_max = result.ShearMajorMax;
        //        V_major_min = result.ShearMajorMin;
        //        M_major_max = result.MomentMajorMax;
        //        M_major_min = result.MomentMajorMin;
        //        P_max = result.AxialForceMax;
        //        P_min = result.AxialForceMin;
        //        V_minor_max = result.ShearMinorMax;
        //        V_minor_min = result.ShearMinorMin;
        //        M_minor_max = result.MomentMinorMax;
        //        M_minor_min = result.MomentMinorMin;
            }
            catch (Exception)
            {
                SetDefaultParameters();
                ErrorMessage = "Data extraction failed. Either ETABS is not running, or results are unavailable for selected Combo.";
            }
        }

        private bool canGetForces()
        {
            //Add check if ETABS is running
            return true;
        }
        #endregion

        /// <summary>
        ///Customization of WPF view in Dynamo UI      
        /// </summary>
        public class SelectionForceExtractViewCustomization : INodeViewCustomization<PierForceExtract>
        {
            public void CustomizeView(PierForceExtract model, NodeView nodeView)
            {

                PierForceExtractView control = new PierForceExtractView();
                control.DataContext = model;


                nodeView.inputGrid.Children.Add(control);


            }


            public void Dispose()
            {
                //throw new NotImplementedException();
            }
        }
    }


}
