using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ScreenCaptureControls.Controls
{
    [TemplatePart(Name = PART_DialogButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_ModifierCheckBox, Type = typeof(CheckBox))]
    [TemplatePart(Name = PART_CtrlCheckBox, Type = typeof(CheckBox))]
    [TemplatePart(Name = PART_AltCheckBox, Type = typeof(CheckBox))]
    [TemplatePart(Name = PART_ShiftCheckBox, Type = typeof(CheckBox))]
    [TemplatePart(Name = PART_ComboBox, Type = typeof(ComboBox))]
    public class SettingGroupBox : Control
    {
        const string PART_DialogButton = "PART_DialogButton";
        const string PART_ModifierCheckBox = "PART_ModifierCheckBox";
        const string PART_CtrlCheckBox = "PART_CtrlCheckBox";
        const string PART_AltCheckBox = "PART_AltCheckBox";
        const string PART_ShiftCheckBox = "PART_ShiftCheckBox";
        const string PART_ComboBox = "PART_ComboBox";

        #region Dependency Property
        /// <summary>
        /// GroupBox 헤더명
        /// </summary>
        public static readonly DependencyProperty ControlNameProperty
            = DependencyProperty.Register(nameof(ControlName), 
                                          typeof(string), 
                                          typeof(SettingGroupBox),
                                          new PropertyMetadata(null, null));
        /// <summary>
        /// X 좌표
        /// </summary>
        public static readonly DependencyProperty SetPositionXProperty
            = DependencyProperty.Register(nameof(SetPositionX),
                                          typeof(int),
                                          typeof(SettingGroupBox),
                                          new PropertyMetadata(0, null)); 

        /// <summary>
        /// Y 좌표
        /// </summary>
        public static readonly DependencyProperty SetPositionYProperty
            = DependencyProperty.Register(nameof(SetPositionY),
                                          typeof(int),
                                          typeof(SettingGroupBox),
                                          new PropertyMetadata(0, null));
        
        /// <summary>
        /// Width
        /// </summary>
        public static readonly DependencyProperty SetWidthProperty
            = DependencyProperty.Register(nameof(SetWidth),
                                          typeof(int),
                                          typeof(SettingGroupBox),
                                          new PropertyMetadata(0, null));
        
        /// <summary>
        /// Height
        /// </summary>
        public static readonly DependencyProperty SetHeightProperty
            = DependencyProperty.Register(nameof(SetHeight),
                                          typeof(int),
                                          typeof(SettingGroupBox),
                                          new PropertyMetadata(0, null));
        
        /// <summary>
        /// 저장할 경로
        /// </summary>
        public static readonly DependencyProperty SetPathProperty
            = DependencyProperty.Register(nameof(SetPath),
                                          typeof(string),
                                          typeof(SettingGroupBox),
                                          new PropertyMetadata(null, null));
        
        /// <summary>
        /// Delete button Command
        /// </summary>
        public static readonly DependencyProperty CommandProperty
            = DependencyProperty.Register(nameof(Command),
                                          typeof(ICommand),
                                          typeof(SettingGroupBox),
                                          new UIPropertyMetadata(null));
        #endregion

        #region Fields
        protected Button DialogButton = null;
        protected CheckBox ModifierCheckBox = null;
        protected CheckBox CtrlCheckBox = null;
        protected CheckBox AltCheckBox = null;
        protected CheckBox ShiftCheckBox = null;
        protected ComboBox ComboBox = null;
        #endregion

        #region Properties 
        public string ControlName
        {
            get => (string)GetValue(ControlNameProperty);
            set => SetValue(ControlNameProperty, value);
        }

        public int SetPositionX
        {
            get => (int)GetValue(SetPositionXProperty);
            set => SetValue(SetPositionXProperty, value);
        }

        public int SetPositionY
        {
            get => (int)GetValue(SetPositionYProperty);
            set => SetValue(SetPositionYProperty, value);
        }

        public int SetWidth
        {
            get => (int)GetValue(SetWidthProperty);
            set => SetValue(SetWidthProperty, value);
        }

        public int SetHeight
        {
            get => (int)GetValue(SetHeightProperty);
            set => SetValue(SetHeightProperty, value);
        }

        public string SetPath
        {
            get => (string)GetValue(SetPathProperty);
            set => SetValue(SetPathProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        #endregion

        #region Public Mathod
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DialogButton = this.GetTemplateChild(PART_DialogButton) as Button;
            if (DialogButton != null)
            {
                DialogButton.Click += DialogButton_Click;
            }

            ModifierCheckBox = this.GetTemplateChild(PART_ModifierCheckBox) as CheckBox;
            if (ModifierCheckBox != null)
            {

            }

            CtrlCheckBox = this.GetTemplateChild(PART_CtrlCheckBox) as CheckBox;
            if (CtrlCheckBox != null)
            {

            }

            AltCheckBox = this.GetTemplateChild(PART_AltCheckBox) as CheckBox;
            if (AltCheckBox != null)
            {

            }

            ShiftCheckBox = GetTemplateChild(PART_ShiftCheckBox) as CheckBox;
            if (PART_ShiftCheckBox != null)
            {

            }

            ComboBox = GetTemplateChild(PART_ComboBox) as ComboBox;
            if (ComboBox != null)
            {

            }
        }
        #endregion

        #region Private Mathod
        private void DialogButton_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (!string.IsNullOrEmpty(SetPath))
                {
                    dialog.InitialDirectory = SetPath;
                }

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    SetPath = dialog.FileName;
                }
            }
        }
        #endregion
    }
}
