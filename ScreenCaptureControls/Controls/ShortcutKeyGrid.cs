using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenCaptureControls.Controls
{
    [TemplatePart(Name = PART_ComboBox, Type = typeof(ComboBox))]
    [TemplatePart(Name = PART_ModifierCheckBox, Type = typeof(CheckBox))]
    [TemplatePart(Name = PART_CtrlCheckBox, Type = typeof(CheckBox))]
    [TemplatePart(Name = PART_AltCheckBox, Type = typeof(CheckBox))]
    [TemplatePart(Name = PART_ShiftCheckBox, Type = typeof(CheckBox))]
    public class ShortcutKeyGrid : Control
    {
        const string PART_ComboBox = "PART_ComboBox";
        const string PART_ModifierCheckBox = "PART_ModifierCheckBox";
        const string PART_CtrlCheckBox = "PART_CtrlCheckBox";
        const string PART_AltCheckBox = "PART_AltCheckBox";
        const string PART_ShiftCheckBox = "PART_ShiftCheckBox";

        #region Dependency Property
        public static readonly DependencyProperty ShortcutKeyProperty
            = DependencyProperty.Register(nameof(ShortcutKeyList),
                                          typeof(List<Key>),
                                          typeof(ShortcutKeyGrid),
                                          new PropertyMetadata(null, null));
        #endregion

        #region Fields
        protected ComboBox comboBox = null;
        protected CheckBox modifierCheckBox = null;
        protected CheckBox ctrlCheckBox = null;
        protected CheckBox altCheckBox = null;
        protected CheckBox shiftCheckBox = null;
        #endregion

        #region Properties 
        public List<Key> ShortcutKeyList
        {
            get => (List<Key>)GetValue(ShortcutKeyProperty);
            set => SetValue(ShortcutKeyProperty, value);
        }
        #endregion

        #region Public Mathod
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            comboBox = Template.FindName(PART_ComboBox, this) as ComboBox;
            if (comboBox != null)
            {
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
            }
            modifierCheckBox = Template.FindName(PART_ModifierCheckBox, this) as CheckBox;
            if (modifierCheckBox != null)
            {
                modifierCheckBox.Checked += ModifierCheckBox_Checked;
                modifierCheckBox.Unchecked += ModifierCheckBox_UnChecked;
            }
            ctrlCheckBox = Template.FindName(PART_CtrlCheckBox, this) as CheckBox;
            if (ctrlCheckBox != null)
            {
                ctrlCheckBox.Checked += CheckBox_Checked;
                ctrlCheckBox.Unchecked += CheckBox_Checked;
            }
            altCheckBox = Template.FindName(PART_AltCheckBox, this) as CheckBox;
            if (altCheckBox != null)
            {
                altCheckBox.Checked += CheckBox_Checked;
                altCheckBox.Unchecked += CheckBox_Checked;
            }
            shiftCheckBox = Template.FindName(PART_ShiftCheckBox, this) as CheckBox;
            if (shiftCheckBox != null)
            {
                shiftCheckBox.Checked += CheckBox_Checked;
                shiftCheckBox.Unchecked += CheckBox_Checked;
            }

            SetComboBoxItem();
            SetShortcutKeyUI();
        }
        #endregion

        private void SetShortcutKeyUI()
        {
            if (ShortcutKeyList.Count == 0)
            {
                comboBox.SelectedItem = comboBox.Items[0];
                return;
            }

            List<Key> tempKeyList = new List<Key>(ShortcutKeyList);
            string mainKey = tempKeyList[0].ToString();

            // D0 ~ D9 예외처리
            int index = comboBox.Items.IndexOf(mainKey + " key");
            if (index == -1)
            {
                index = comboBox.Items.IndexOf(mainKey.Replace("D","") + " key");
            }

            comboBox.SelectedItem = comboBox.Items[index];
            if (tempKeyList.Count > 1)
            {
                modifierCheckBox.IsChecked = true;
                if (tempKeyList.Contains(Key.LeftCtrl))
                {
                    ctrlCheckBox.IsChecked = true;
                }

                if (tempKeyList.Contains(Key.LeftAlt))
                {
                    altCheckBox.IsChecked = true;
                }

                if (tempKeyList.Contains(Key.LeftShift))
                {
                    shiftCheckBox.IsChecked = true;
                }
            }
        }

        private void UpdateKeyList()
        {
            ShortcutKeyList.Clear();

            // 콤보박스 키 삽입
            KeyConverter keyConverter = new KeyConverter();
            ShortcutKeyList.Add((Key)keyConverter.ConvertFromString(comboBox.SelectedItem.ToString().Replace(" key", "")));

            // 조합키 삽입
            if (!modifierCheckBox.IsChecked.Value)
            {
                return;
            }

            if (ctrlCheckBox.IsChecked.Value)
            {
                ShortcutKeyList.Add(Key.LeftCtrl);
            }

            if (altCheckBox.IsChecked.Value)
            {
                ShortcutKeyList.Add(Key.LeftAlt);
            }

            if (shiftCheckBox.IsChecked.Value)
            {
                ShortcutKeyList.Add(Key.LeftShift);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateKeyList();
        }

        private void ModifierCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ctrlCheckBox.IsEnabled = true;
            altCheckBox.IsEnabled = true;
            shiftCheckBox.IsEnabled = true;
        }

        private void ModifierCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            ctrlCheckBox.IsEnabled = false;
            altCheckBox.IsEnabled = false;
            shiftCheckBox.IsEnabled = false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateKeyList();
        }

        private void SetComboBoxItem()
        {
            foreach (var key in GetShorcutKey())
            {
                comboBox.Items.Add(key + " key");
            }
        }

        private List<string> GetShorcutKey()
        {
            List<string> keyList = new List<string>();

            List<string> AtoZList = Enumerable.Range('A', 26)
                                                .Select(x => (char)x + "")
                                                .ToList();

            // F1~F12
            for (var i = 1; i < 13; i++)
            {
                keyList.Add("F" + i.ToString());
            }

            // 0~9
            for (var i = 0; i < 10; i++)
            {
                keyList.Add(i.ToString());
            }

            keyList.AddRange(AtoZList);

            return keyList;
        }
    }
}
