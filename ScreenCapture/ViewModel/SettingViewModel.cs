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
using System.ComponentModel;

namespace ScreenCapture.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region UI Variable
        public ObservableCollection<EachClassSettingItem> SGClassCollection { get; set; } 
            = new ObservableCollection<EachClassSettingItem>();

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
        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }

        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
            CollectionItemButtonClickCommand = new RelayCommand<object>(OnCollectionItemButtonClick);
            TextBoxKeyDownCommand = new RelayCommand<KeyEventArgs>(OnTextBoxKeyDown);
            ClosingCommand = new RelayCommand<CancelEventArgs>(Closing);
        }
        #region CommandAction

        /// <summary>
        /// 창 종료시 변경 내역 업데이트 이벤트 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Closing(CancelEventArgs e)
        {
            if (_SettingChangeEvent != null)
            {
                _SettingChangeEvent(SGClassCollection.Select(x => x.Header).ToList());
            }
        }

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
        #endregion
        #endregion

        #region Field
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
            string header = SettingName;
            if (SGClassCollection.Any(x => x.Header == header))
            {
                MessageBox.Show("이미 존재하는 이름입니다.", "세팅명 중복");
                return;
            }
            else if (string.IsNullOrEmpty(header) || string.IsNullOrWhiteSpace(header))
            {
                header = $"New Setting";
            }

            // Setting Add Button Click Event
            if (_SettingAddEvent != null)
            {
                _SettingAddEvent();
            }
            
            SGClassCollection.Add(new EachClassSettingItem()
            {
                Header = header,
                Width = Width,
                Height = Height,
                PositionX = PositionX,
                PositionY = PositionY
            });

            SettingName = string.Empty;
        }
    }
}
