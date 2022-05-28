using ProjectTesting.Viewmodels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Testing;
using WPFbase;

namespace ProjectTesting.Viewmodels
{
    public class ResultsViewmodel
    {
        Test Test { get; set; }
        public ObservableCollection<Button> Buttons { get; set; } = new ObservableCollection<Button>();
        public ResultsViewmodel(Test test)
        {
            Test = test;
            int i = 1;
            foreach (var item in Test.Questions)
            {
                var newbut = new Button
                {
                    Content = i.ToString(),
                    Height = 25,
                    Width = 20,
                    Background=item.brush
                };
                newbut.Command = new RelayCommand(o =>
                {
                    Num = Buttons.IndexOf(newbut);
                    Change.Execute(Buttons[Buttons.IndexOf(newbut)]);
                });
                Buttons.Add(newbut);
                i++;
            }
            Change.Execute(Buttons[0]);
        }
        public int Num { get; set; } = 0;
        public ObservableCollection<CheckBox> CheckBoxes { get; set; } = new ObservableCollection<CheckBox>();
        public ObservableCollection<RadioButton> RadioButtons { get; set; } = new ObservableCollection<RadioButton>();
        public ICommand Change => new RelayCommand(o =>
        {
            Text.Item = current.Text;
            if (current.HaveManyAnswers)
            {
                CheckBoxes.Clear();
                RadioButtons.Clear();
                foreach (var item in current.Answers)
                {
                    var cb = new CheckBox
                    {
                        Content = item.Text,
                        Height = 20,
                        Width = item.Text.Length + 100,
                        IsChecked = item.Answered,
                        IsEnabled = false,
                        FontSize=15
                    };
                    if (item.RightAnswer)
                    {
                        cb.Foreground = Brushes.Green;
                    }
                    CheckBoxes.Add(cb);
                }
            }
            else
            {
                CheckBoxes.Clear();
                RadioButtons.Clear();
                foreach (var item in current.Answers)
                {
                    var rb = new RadioButton
                    {
                        Content = item.Text,
                        Height = 20,
                        Width = item.Text.Length + 100,
                        GroupName = "Answers",
                        IsChecked = item.Answered,
                        IsEnabled = false,
                        FontSize = 15
                    };
                    if (item.RightAnswer)
                    {
                        rb.Foreground = Brushes.Green;
                    }
                    RadioButtons.Add(rb);
                }
            }
        }, o => true);
        public ChangingItem<string> Text { get; set; } = new ChangingItem<string>();
        public Question current => Test.Questions[Num];
        public string Rate => "Ваш резуьтат: " + Math.Round(Test.MyRate, 0).ToString() + "/" + Test.MaxRate.ToString();
    }
}
