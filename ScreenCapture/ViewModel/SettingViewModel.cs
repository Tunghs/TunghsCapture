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
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Media.Imaging;
using System.Drawing;
using ScreenCaptureCore.Util;

namespace ScreenCapture.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region UI Variable
        public ObservableCollection<EachClassSettingItem> SGClassCollection { get; set; } 
            = new ObservableCollection<EachClassSettingItem>();

        private string _SettingName;
        public string SettingName
        {
            get => _SettingName;
            set => Set(ref _SettingName, value);
        }

        private string _SettingPath;
        public string SettingPath
        {
            get => _SettingPath;
            set => Set(ref _SettingPath, value);
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

        /// <summary>
        /// 텍스트 박스 키다운 이벤트
        /// </summary>
        /// <param name="e"></param>
        private void OnTextBoxKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AddSGSettingItem();
            }
        }

        /// <summary>
        /// 세팅 아이템 삭제 이벤트
        /// </summary>
        /// <param name="param"></param>
        private void OnCollectionItemButtonClick(object param)
        {
            string header = param.ToString();
            var eachClassSettingItem = SGClassCollection.Where(x => x.Header == header).ToList()[0];
            SGClassCollection.Remove(eachClassSettingItem);

            if (_SettingChangeEvent != null)
            {
                _SettingChangeEvent(SGClassCollection.Select(x => x.Header).ToList());
            }
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
            if (SGClassCollection.Count == 5)
            {
                MessageBox.Show("설정할 수 있는 세팅의 최대 개수는 5개 입니다. 추가를 하시려면 세팅을 삭제하시거나 변경하세요.", "세팅명 중복");
                return;
            }

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

            if (_SettingChangeEvent != null)
            {
                _SettingChangeEvent(SGClassCollection.Select(x => x.Header).ToList());
            }
        }

        public void GetSavePathFromSetting()
        {
            foreach (var classItem in SGClassCollection)
            {
                List<Key> keyList = classItem.ShortcutKeyList;

                if (keyList.All(x => Keyboard.IsKeyDown(x)))
                {
                    BitmapImage ClipImage;
                    using (Bitmap bmp = new Bitmap(classItem.Width, classItem.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.CopyFromScreen(classItem.PositionX, classItem.PositionY, 0, 0, bmp.Size);
                            BitmapController bitmapController = new BitmapController();
                            ClipImage = bitmapController.BitmapToImageSource(bmp);

                            Clipboard.SetImage(ClipImage);
                        }
                    }
                    break;
                }
            }
        }
    }
}
