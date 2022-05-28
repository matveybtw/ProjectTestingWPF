using System;
using Testing;
using System.Windows.Input;
using WPFbase;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
namespace ProjectTesting.Viewmodels
{
    public class MainWindowViewmodel
    {
        public ChangingItem<string> Name { get; set; } = new ChangingItem<string>("");
        public ChangingItem<string> Description { get; set; } = new ChangingItem<string>();
        public Test test { get; set; }
        private XmlSerializer serializer = new XmlSerializer(typeof(Test));
        string Path;
        public MainWindowViewmodel()
        {
            ChangeTest.Execute(test);
        }
        public ICommand ChangeTest => new RelayCommand(o =>
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "XML(*.xml)|*.xml";
            if (fd.ShowDialog() == true)
            {
                Path = fd.FileName;
                UpdateTest();
            }
        });
        public void UpdateTest()
        {
            using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
            {
                test = (Test)serializer.Deserialize(fs);
            }
        }
        public ICommand Open => new RelayCommand(o =>
        {
            UpdateTest();
            test.NameOfUser = Name.Item;
            TestWindow window = new TestWindow(test);
            window.ShowDialog();
            window.tw.finish();
            Results res = new Results(test);
            res.ShowDialog();
        }, o => test != null && Name.Item != "");

    }
}