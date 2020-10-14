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
using System.Windows.Input;

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
        public RelayCommand<KeyEventArgs> TextBoxKeyDownCommand { get; private set; }

        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
            CollectionItemButtonClickCommand = new RelayCommand<object>(OnCollectionItemButtonClick);
            TextBoxKeyDownCommand = new RelayCommand<KeyEventArgs>(OnTextBoxKeyDown);
        }
        #region CommandAction

        /// <summary>
        /// 버튼 클릭 이벤트
        /// </summary>
        /// <param name="param"></param>
        private void OnButtonClick(object param)
        {
            switch (param.ToString())
            {
                case "AddSetting":
                    AddSGSettingItem();
                    break;
            }
        }

        private void OnTextBoxKeyDown(KeyEventArgs e)
        {
           if (e.Key == Key.Return)
            {
                AddSGSettingItem();
            }
        }

        /// <summary>
        /// SGItem 삭제 이벤트
        /// </summary>
        /// <param name="param"></param>
        private void OnCollectionItemButtonClick(object param)
        {
            string header = param.ToString();
            var eachClassSettingItem = SGClassCollection.Where(x => x.Header == header).ToList()[0];
            SGClassCollection.Remove(eachClassSettingItem);
        }

        /// <summary>
        /// 추가하려는 세팅 항목의 이름 확인
        /// </summary>
        /// <param name="settingName"></param>
        private bool CheckSettingName(string settingName)
        {
            // null, 공백 필터링
            if (string.IsNullOrEmpty(settingName) || string.IsNullOrWhiteSpace(settingName))
            {
                return false;
            }
            // 공백이면 추가 인덱스로 변경하기

            // 중복 세팅명 필터링
            if (SGClassCollection.Any(x => x.Header == settingName))
            {
                MessageBox.Show(Window, "이미 존재하는 이름입니다.", "hi");
                return false;
            }
            return true;
        }
        #endregion
        #endregion

        #region Field
        public Window Window;
        public int Width, Height, PositionX, PositionY;
        #endregion

        #region Event
        public delegate void SettingChangeHandler(List<string> settingNames);
        public event SettingChangeHandler _SettingChangeEvent;

        public delegate void SettingAddHandler();
        public event SettingAddHandler _SettingAddEvent;
        #endregion

        public SettingViewModel()
        {
            InitRelayCommand();
        }

        /// <summary>
        /// 세팅 추가
        /// </summary>
        private void AddSGSettingItem()
        {
            if (!CheckSettingName(SettingName))
            {
                return;
            }

            // Setting Add Button Click Event
            if (_SettingAddEvent != null)
            {
                _SettingAddEvent();
            }

            SGClassCollection.Add(new EachClassSettingItem()
            {
                Header = SettingName,
                Width = Width,
                Height = Height,
                PositionX = PositionX,
                PositionY = PositionY
            });
        }
    }
}
