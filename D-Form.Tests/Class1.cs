using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using D_Form;

namespace D_Form.Tests
{
    [TestFixture]
    public class Dform
    {
        [Test]
        public void CreateAnswers()
        {
            Form f = new Form();
            Assert.IsNull(f.Title);
            f.Title = "jj";
            Assert.AreEqual("jj", f.Title);

            FormAnswer a = f.FindOrCreateAnswer("Emilie");
            Assert.IsNotNull(a);
            FormAnswer b = f.FindOrCreateAnswer("Emilie");
            Assert.AreSame(a,b);

            Assert.AreEqual(1, f.AnswerCount);
            FormAnswer c = f.FindOrCreateAnswer("John Doe");
            Assert.AreNotSame(a, c);

            Assert.AreEqual("Emilie", a.UniqueName);
            Assert.AreEqual("John Doe", c.UniqueName);
        }

        [Test]
        public void CreateQuestionFolders()
        {
            Form f = new Form();
            f.Questions.Title = "HG68-Bis";
            Assert.AreEqual("HG68-Bis", f.Title);
            BaseQuestion q1 = f.Questions.AddNewQuestion(typeof(PaneQuestion));
            BaseQuestion q2 = f.Questions.AddNewQuestion("D_Form.PaneQuestion");
            Assert.AreEqual(0, q1.Index);
            Assert.AreEqual(1, q2.Index);
            q2.Index = 0;
            Assert.AreEqual(0, q2.Index);
            Assert.AreEqual(1, q1.Index);
            q2.Parent = null;
            Assert.AreEqual(0, q1.Index);
            q2.Parent = q1;
            Assert.IsTrue(f.Questions.Contains(q1));
            Assert.IsTrue(f.Questions.Contains(q2));
        }
    }
}
