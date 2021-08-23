using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace ShowWindowAPITool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<int, dynamic> _getSystemMetricsDictionary;
        public MainWindow()
        {
            InitializeComponent();
            _getSystemMetricsDictionary = new Dictionary<int, dynamic>();
            var obj = new GetSystemMetricsIndexs();
            var type = typeof(GetSystemMetricsIndexs);
            type.GetProperties();
            var memberInfos = type.GetFields();
            foreach (var info in memberInfos)
            {
                if (info.FieldType.Name is "Int32")
                {
                    var key = type.GetField(info.Name).GetValue(obj);
                    _getSystemMetricsDictionary.Add(Convert.ToInt32(key), info.Name);
                }
            }

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var key in _getSystemMetricsDictionary.Keys)
            {
                _getSystemMetricsDictionary[key] = $"{_getSystemMetricsDictionary[key]}:{GetSystemMetrics(key)}";
            }
            var list = _getSystemMetricsDictionary.Values.ToList();
            ListView.ItemsSource = list;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Loaded -= MainWindow_Loaded;
            base.OnClosed(e);
        }

        [DllImport("user32")]
        private static extern int GetSystemMetrics(int nIndex);
    }
}
