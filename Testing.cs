using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFbase;
namespace Testing
{
    public class Test
    {
        public string NameOfUser { get; set; } = "";
        public double MaxRate { get; set; }
        public double RatePerQ => MaxRate / Questions.Count;
        public double MyRate { get; set; } = 0.0;
        public string name { get; set; }
        public int TimeToPass { get; set; } = 100;
        public List<Question> Questions { get; set; } = new List<Question>();
    }
    public class Question
    {
        public string Text { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public bool HaveManyAnswers => Answers.FindAll(o => o.RightAnswer).Count > 1;
        public Brush brush { get; set; }
    }
    public class Answer
    {
        public Answer()
        {

        }
        public Answer(string txt, bool right = false, bool ans = false)
        {
            Text = txt;
            RightAnswer = right;
            Answered = ans;
        }
        public string Text { get; set; }
        public bool? Answered { get; set; }
        public bool RightAnswer { get; set; }
    }
}
