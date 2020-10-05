using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ScreenCaptureControls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScreenCapture.ViewModel
{
    public class ScreenCaptureViewModel : ViewModelBase
    {
        #region UIVariable
        private int _windowWidth = 606;
        public int WindowWidth
        {
            get { return _windowWidth; }
            set { Set(ref _windowWidth, value); }
        }

        private int _windowHeight = 508;
        public int WindowHeight
        {
            get { return _windowHeight; }
            set { Set(ref _windowHeight, value); }
        }
        
        private int _windowLeft;
        public int WindowLeft
        {
            get { return _windowLeft; }
            set { Set(ref _windowLeft, value); }
        }
        
        private int _windowTop;
        public int WindowTop
        {
            get { return _windowTop; }
            set { Set(ref _windowTop, value); }
        }

        private bool _isSettingOpen = false;
        public bool IsSettingOpen
        {
            get { return _isSettingOpen; }
            set { Set(ref _isSettingOpen, value); }
        }

        private int _captureWidth = 500;
        public int CaptureWidth
        {
            get { return _captureWidth; }
            set { Set(ref _captureWidth, value); }
        }

        private int _captureHeight = 500;
        public int CaptureHeight
        {
            get { return _captureHeight; }
            set { Set(ref _captureHeight, value); }
        }
        #endregion

        #region Command
        public RelayCommand<object> ButtonClickCommand { get; private set; }
        public RelayCommand<TextCompositionEventArgs> PreviewTextInputCommand { get; private set; }
        public RelayCommand<MouseButtonEventArgs> WindowPreviewMouseDoubleClickCommand { get; private set; }

        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
            PreviewTextInputCommand = new RelayCommand<TextCompositionEventArgs>(OnPreviewTextInput);
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

        private void OnButtonClick(object param)
        {
            switch (param.ToString())
            {
                case "OpenSetting":
                    OpenSetting();
                    break;
                case "CloseSetting":
                    CloseSetting();
                    break;
                case "SetSize":
                    SetSize();
                    break;
                case "OpenSettingWindow":
                    OpenSettingWindow();
                    break;
            }
        }

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

        private void OpenSettingWindow()
        {
            SettingWindow settingWindow = new SettingWindow() { DataContext = SettingViewModel };
            settingWindow.Owner = Application.Current.MainWindow;

            if ((bool)settingWindow.ShowDialog())
            {
                // 참일 때 실행
            }
        }

        /// <summary>
        /// 사이즈 세팅
        /// </summary>
        private void SetSize()
        {
            WindowWidth = CaptureWidth + 8;
            WindowHeight = CaptureHeight + 106;
        }

        private void OpenSetting()
        {
            IsSettingOpen = true;
        }

        private void CloseSetting()
        {
            IsSettingOpen = false;
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
        }
    }
}
