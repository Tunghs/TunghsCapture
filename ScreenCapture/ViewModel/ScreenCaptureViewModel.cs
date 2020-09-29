using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenCapture.ViewModel
{
    public class ScreenCaptureViewModel : ViewModelBase
    {
        #region UIVariable
        private int _windowWidth = 600;
        public int WindowWidth
        {
            get { return _windowWidth; }
            set { Set(ref _windowWidth, value); }
        }

        private int _windowHeight = 600;
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

        private int _captureWidth = 600;
        public int CaptureWidth
        {
            get { return _captureWidth; }
            set { Set(ref _captureWidth, value); }
        }

        private int _captureHeight = 600;
        public int CaptureHeight
        {
            get { return _captureHeight; }
            set { Set(ref _captureHeight, value); }
        }
        #endregion

        #region Command
        public RelayCommand<object> ButtonClickCommand { get; private set; }
        public RelayCommand<TextCompositionEventArgs> PreviewTextInputCommand { get; private set; }
        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
            PreviewTextInputCommand = new RelayCommand<TextCompositionEventArgs>(OnPreviewTextInput);
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

        /// <summary>
        /// 사이즈 세팅
        /// </summary>
        private void SetSize()
        {
            WindowWidth = 600;
            WindowHeight = 600;
        }

        private void OpenSetting()
        {
            IsSettingOpen = true;
        }

        private void CloseSetting()
        {
            IsSettingOpen = false;
        }
        #region CommandAction

        #endregion
        #endregion
        public ScreenCaptureViewModel()
        {
            InitRelayCommand();
        }
    }
}
