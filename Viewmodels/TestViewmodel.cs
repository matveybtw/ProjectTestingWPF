using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Testing;
using System.Windows.Input;
using WPFbase;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.IO;

namespace ProjectTesting.Viewmodels
{
    public class TestViewmodel
    {
        public Test Test { get; set; }
        int sec;
        int m => sec / 60;
        int s => sec - m * 60;
        string Path = new FileInfo(new DirectoryInfo(".").Parent.Parent.FullName + "\\" + "history.txt").FullName;
        public ChangingItem<string> mins { get; set; } = new ChangingItem<string>();
        public ChangingItem<string> secs { get; set; } = new ChangingItem<string>();

        TestWindow window;
        public ObservableCollection<Button> Buttons { get; set; } = new ObservableCollection<Button>();

        public Action ac { get; set; }
        public TestViewmodel(Test test, TestWindow tw)
        {
            window = tw;
            Test = test;
            sec = Test.TimeToPass;
            using (FileStream fs = new FileStream(Path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} {Test.NameOfUser} тестрирование начато.");
                }
            }

            var task = Task.Run(() =>
            {
                do
                {

                    sec--;
                    mins.Item = (m / 10).ToString() + (m % 10).ToString();
                    secs.Item = (s / 10).ToString() + (s % 10).ToString();
                    if (sec == 0)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                } while (sec != 0);
                tw.Dispatcher.Invoke(finish);
            });
            int i = 1;
            foreach (var item in Test.Questions)
            {
                var newbut = new Button
                {
                    Content = i.ToString(),
                    Height = 35,
                    Width = 40
                };

                if (Buttons.Count == 0)
                {
                    newbut.Background = Brushes.Yellow;
                }
                newbut.Command = new RelayCommand(o =>
                {
                    Save();
                    Num = Buttons.IndexOf(newbut);
                    Buttons[Num].Background = Brushes.Yellow;
                    Change.Execute(Buttons[Buttons.IndexOf(newbut)]);
                });
                Buttons.Add(newbut);
                i++;
            }
            Change.Execute(Buttons[0]);
        }
        string Chosen()
        {
            string str = "";
            foreach (var item in current.Answers)
            {
                if (item.Answered == true)
                {
                    str += " " + (current.Answers.IndexOf(item) + 1).ToString() + ",";
                }
            }
            if (str != "")
            {
                str = str.Remove(str.Length - 1);
            }
            return str;
        }
        void Save()
        {
            Buttons[Num].Background = Brushes.White;
            if (current.HaveManyAnswers)
            {
                int ind = 0;
                foreach (var ans in CheckBoxes)
                {
                    Test.Questions[Num].Answers[ind].Answered = ans.IsChecked;
                    ind++;
                }
            }
            else
            {
                int ind = 0;
                foreach (var ans in RadioButtons)
                {
                    Test.Questions[Num].Answers[ind].Answered = ans.IsChecked;
                    ind++;
                }
            }
            using (FileStream fs = new FileStream(Path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} {Test.NameOfUser} вопрос {Num + 1} выбраны варианты:{Chosen()}.");
                }
            }

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
                    CheckBoxes.Add(new CheckBox
                    {
                        Content = item.Text,
                        Height = 20,
                        Width = item.Text.Length + 100,
                        IsChecked = item.Answered,
                        FontSize = 15
                    });

                }
            }
            else
            {
                CheckBoxes.Clear();
                RadioButtons.Clear();
                foreach (var item in current.Answers)
                {
                    RadioButtons.Add(new RadioButton
                    {
                        Content = item.Text,
                        Height = 20,
                        Width = item.Text.Length + 100,
                        GroupName = "Answers",
                        IsChecked = item.Answered,
                        FontSize = 15
                    });
                }
            }
        }, o => true);
        public ChangingItem<string> Text { get; set; } = new ChangingItem<string>();
        public Question current => Test.Questions[Num];
        public ICommand Finish => new RelayCommand(o =>
        {
            window.Close();
        });
        public void finish()
        {
            Save();
            foreach (var q in Test.Questions)
            {
                if (q.HaveManyAnswers)
                {
                    if (!(q.Answers.FindAll(a => a.Answered == true).Count > q.Answers.FindAll(a => a.RightAnswer).Count))
                    {
                        double right = q.Answers.FindAll(a => a.RightAnswer).Count;
                        double rightandanswered = q.Answers.FindAll(a => a.RightAnswer && a.Answered == true).Count;
                        double rate = Test.RatePerQ * (rightandanswered / right);
                        Test.MyRate += rate;
                        if (rightandanswered == 0)
                        {
                            Test.Questions[Test.Questions.IndexOf(q)].brush = Brushes.Red;
                        }
                        else if (rightandanswered == right)
                        {
                            Test.Questions[Test.Questions.IndexOf(q)].brush = Brushes.Green;
                        }
                        else
                        {
                            Test.Questions[Test.Questions.IndexOf(q)].brush = Brushes.Yellow;
                        }
                    }
                }
                else
                {
                    if (q.Answers.Find(a => a.RightAnswer == true).Answered == true)
                    {
                        Test.MyRate += Test.RatePerQ;
                        Test.Questions[Test.Questions.IndexOf(q)].brush = Brushes.Green;
                    }
                    else
                    {
                        Test.Questions[Test.Questions.IndexOf(q)].brush = Brushes.Red;
                    }
                }
            }
            using (FileStream fs = new FileStream(Path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} {Test.NameOfUser} тестрирование закончено. Оценка {Test.MyRate}/{Test.MaxRate}");
                }
            }
        }
        public ICommand Previous => new RelayCommand(o =>
        {
            Buttons[Num - 1].Command.Execute(1);
        }, o=> Num > 0);
        public ICommand Next => new RelayCommand(o =>
        {
            Buttons[Num + 1].Command.Execute(1);
        }, o => Num < Buttons.Count - 1);

    }
}