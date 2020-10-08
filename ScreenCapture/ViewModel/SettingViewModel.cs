using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ScreenCapture.Model;
using System.Collections.ObjectModel;

namespace ScreenCapture.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region UI Variable
        public ObservableCollection<EachClassSettingItem> SGClassCollection { get; set; } = new ObservableCollection<EachClassSettingItem>();

        private string _settingName;
        public string SettingName
        {
            get => _settingName;
            set => Set(ref _settingName, value);
        }

        private string _settingPath;
        public string SettingPath
        {
            get => _settingPath;
            set => Set(ref _settingPath, value);
        }
        #endregion

        #region Command
        public RelayCommand<object> ButtonClickCommand { get; private set; }
        public RelayCommand<object> CollectionItemButtonClickCommand { get; private set; }

        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
            CollectionItemButtonClickCommand = new RelayCommand<object>(OnCollectionItemButtonClick);
        }
        #endregion

        private void OnButtonClick(object param)
        {
            switch (param.ToString())
            {
                case "Add":
                    AddSGSettingItem();
                    break;
            }
        }

        private void OnCollectionItemButtonClick(object param)
        {
            string header = param.ToString();
            var eachClassSettingItem = SGClassCollection.Where(x => x.Header == header).ToList()[0];
            SGClassCollection.Remove(eachClassSettingItem);
        }

        private void AddSGSettingItem()
        {
            SGClassCollection.Add(new EachClassSettingItem() { Header = "Index" + SGClassCollection.Count });
        }

        public SettingViewModel()
        {
            InitRelayCommand();
        }
    }
}
