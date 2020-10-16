using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ScreenCaptureControls.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using ScreenCaptureCore.Util;
using ScreenCapture.Model;

namespace ScreenCapture.ViewModel
{
    public class ScreenCaptureViewModel : ViewModelBase
    {
        #region UI Variable
        private int _WindowWidth = 508;
        public int WindowWidth
        {
            get => _WindowWidth; 
            set => Set(ref _WindowWidth, value);
        }

        private int _WindowHeight = 606;
        public int WindowHeight
        {
            get => _WindowHeight;
            set => Set(ref _WindowHeight, value);
        }

        private int _WindowLeft;
        public int WindowLeft
        {
            get => _WindowLeft;
            set => Set(ref _WindowLeft, value);
        }
        
        private int _WindowTop;
        public int WindowTop
        {
            get => _WindowTop;
            set => Set(ref _WindowTop, value);
        }

        private bool _IsSettingOpen = false;
        public bool IsSettingOpen
        {
            get => _IsSettingOpen;
            set => Set(ref _IsSettingOpen, value);
        }

        private int _CaptureWidth = 500;
        public int CaptureWidth
        {
            get => _CaptureWidth;
            set => Set(ref _CaptureWidth, value);
        }

        private int _CaptureHeight = 500;
        public int CaptureHeight
        {
            get => _CaptureHeight;
            set => Set(ref _CaptureHeight, value);
        }

        private List<string> _Settings = new List<string>();
        public List<string> Settings
        {
            get => _Settings;
            set => Set(ref _Settings, value);
        }

        private string _SelectedSetting;
        public string SelectedSetting
        {
            get => _SelectedSetting;
            set => Set(ref _SelectedSetting, value);
        }

        private bool _IsEnableSettingBtn;
        public bool IsEnableSettingBtn
        {
            get => _IsEnableSettingBtn;
            set => Set(ref _IsEnableSettingBtn, value);
        }
        #endregion

        #region Command
        public RelayCommand<object> ButtonClickCommand { get; private set; }
        public RelayCommand<TextCompositionEventArgs> PreviewTextInputCommand { get; private set; }
        public RelayCommand<KeyEventArgs> TextBoxKeyDownCommand { get; private set; }
        public RelayCommand<MouseButtonEventArgs> WindowPreviewMouseDoubleClickCommand { get; private set; }

        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
            PreviewTextInputCommand = new RelayCommand<TextCompositionEventArgs>(OnPreviewTextInput);
            TextBoxKeyDownCommand = new RelayCommand<KeyEventArgs>(OnTextBoxKeyDown);
            WindowPreviewMouseDoubleClickCommand = new RelayCommand<MouseButtonEventArgs>(OnWindowPreviewMouseDoubleClick);

        }

        #region CommandAction
        /// <summary>
        /// 윈도우 타이틀바 더블클릭했을 때 전체화면 안되게끔 처리함
        /// </summary>
        /// <param name="e"></param>
        private void OnWindowPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void OnTextBoxKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SetSize();
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
                case "SetSize":
                    SetSize();
                    break;
                case "SetSetting":
                    SetSetting();
                    break;
                case "OpenSettingWindow":
                    OpenSettingWindow();
                    break;
                case "Capture":
                    CaptureClick();
                    break;
            }
        }

        /// <summary>
        /// 캡처 사이즈 텍스트 박스에 숫자만 입력 받음
        /// </summary>
        /// <param name="e"></param>
        private void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 세팅창 열기
        /// </summary>
        private void OpenSettingWindow()
        {
            SettingWindow settingWindow = new SettingWindow() { DataContext = SettingViewModel };
            settingWindow.Owner = Application.Current.MainWindow;

            settingWindow.Show();
        }

        /// <summary>
        /// 윈도우 사이즈 세팅
        /// </summary>
        private void SetSize()
        {
            WindowWidth = CaptureWidth + 8;
            WindowHeight = CaptureHeight + 106;
        }

        private void SetSetting()
        {
            foreach (var SGClass in SettingViewModel.SGClassCollection)
            {
                if (SGClass.Header == SelectedSetting)
                {
                    SGClass.Width = CaptureWidth;
                    SGClass.Height = CaptureHeight;
                    SGClass.PositionX = WindowLeft;
                    SGClass.PositionY = WindowTop;
                }
            }
        }

        private void CaptureClick()
        {
            CaptureScreen();
        }
        #endregion
        #endregion

        #region Field

        #endregion
        
        public SettingViewModel SettingViewModel { get; set; }

        public ScreenCaptureViewModel()
        {
            InitRelayCommand();

            SettingViewModel = new SettingViewModel();
            SettingViewModel._SettingAddEvent += new SettingViewModel.SettingAddHandler(SendScreenInfo);
            SettingViewModel._SettingChangeEvent += new SettingViewModel.SettingChangeHandler(ApplySetting);
        }

        private void CaptureScreen()
        {
            int captureX = WindowLeft + 4;
            int captureY = WindowTop + 77;

            BitmapImage ClipImage;

            using (Bitmap bmp = new Bitmap(CaptureWidth, CaptureHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(captureX, captureY, 0, 0, bmp.Size);
                    BitmapController bitmapController = new BitmapController();
                    ClipImage = bitmapController.BitmapToImageSource(bmp);

                    Clipboard.SetImage(ClipImage);
                }
            }
        }

        /// <summary>
        /// Setting 창에서 항목 추가시 현재 스크린의 정보를 업데이트해서 전달함
        /// </summary>
        private void SendScreenInfo()
        {
            SettingViewModel.Width = CaptureWidth;
            SettingViewModel.Height = CaptureHeight;
            SettingViewModel.PositionX = WindowLeft;
            SettingViewModel.PositionY = WindowTop;
        }

        /// <summary>
        /// 세팅창 종료 후 설정한 값 적용
        /// </summary>
        private void ApplySetting(List<string> settingList)
        {
            Settings = settingList;
        }
    }
}
