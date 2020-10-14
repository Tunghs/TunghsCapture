﻿using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;

namespace ScreenCapture.Model
{
    public class EachClassSettingItem : ObservableObject
    {
        private string _header;
        public string Header
        {
            get => _header;
            set => Set(ref _header, value);
        }

        private int _positionX;
        public int PositionX
        {
            get => _positionX;
            set => Set(ref _positionX, value);
        }

        private int _positionY;
        public int PositionY
        {
            get => _positionY;
            set => Set(ref _positionY, value);
        }

        private int _width;
        public int Width
        {
            get => _width;
            set => Set(ref _width, value);
        }

        private int _height;
        public int Height
        {
            get => _height;
            set => Set(ref _height, value);
        }

        private string _savePath;
        public string SavePath
        {
            get => _savePath;
            set => Set(ref _savePath, value);
        }

        private List<Key> _shortcutKeyList = new List<Key>();
        public List<Key> ShortcutKeyList
        {
            get => _shortcutKeyList;
            set => Set(ref _shortcutKeyList, value);
        }
    }
}
