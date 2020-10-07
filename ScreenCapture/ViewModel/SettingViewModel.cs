using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenCapture.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region Command
        public RelayCommand<RoutedEventArgs> ButtonClickCommand { get; private set; }

        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<RoutedEventArgs>(OnButtonClick);
        }
        #endregion

        private void OnButtonClick(RoutedEventArgs param)
        {
            MessageBox.Show("Hi!");
        }

        public SettingViewModel()
        {
            InitRelayCommand();
        }
    }
}
