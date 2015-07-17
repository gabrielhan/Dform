using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Form
{
    public class Form
    {
        private String _Label;
        private Dictionary<String, FormAnswer> _Answers;
        private RootQuestion _RootQuestion;

        public Form()
        {
            _RootQuestion = new RootQuestion(this);
            _Answers = new Dictionary<String, FormAnswer> (); 
        }

        public string Title 
        {get{
            if (_RootQuestion.Title != null)
                    {
                        return _RootQuestion.Title;
                    }
                    return _Label;
        
       } set { _Label = value; } }

        public int AnswerCount { get { return _Answers.Count(); } }

        public RootQuestion Questions { get { return _RootQuestion; } }

        public FormAnswer FindOrCreateAnswer(string name)
        {
            FormAnswer a; 
            _Answers.TryGetValue(name, out a);
            if (a == null)
            {
                a = new FormAnswer(_Answers, name);
            }
            return a;
        }


    }

    public class FormAnswer
    {
        private String _UniqueName;

        public FormAnswer(Dictionary<String, FormAnswer> d, string name)
        {
            _UniqueName = name;
            d.Add(name,this);
        }

        public string UniqueName{ get { return _UniqueName; } }


    }

    public abstract class BaseQuestion
    {
        BaseQuestion _Parent;
        List<BaseQuestion> _Childs;
        String _Title;
        int _Index;

        public int Index { get { return _Index; } set { _Index = value; } }
        public string Title { get { return _Title; } set { _Title = value; } }
        protected List<BaseQuestion> Childs { get { return _Childs; }}
        public BaseQuestion Parent { get { return _Parent; } set 
        {
            if((value != _Parent) & (_Parent != null))
            {
                _Parent.Childs.Remove(this);
            }
            if(value == null)
            {
                _Parent = null;
            }else if(value != _Parent)
            {
                _Parent = value;
                _Index = _Parent._Childs.Count();
                _Parent.Childs.Add(this);
            }
        } }

        public bool Contains(BaseQuestion q)
        {
            return true;
        }

        public BaseQuestion AddNewQuestion(String s)
        {
            BaseQuestion a = (BaseQuestion)Activator.CreateInstance(Type.GetType(s));
            a.Parent = this; 
            return a;
        }

    }

    public class RootQuestion : BaseQuestion
    {
        private Form _Parent;
        private String _Title;
        private List<BaseQuestion> _Childs;

        public RootQuestion(Form form)
        {
            _Parent = form;
        }


        public BaseQuestion AddNewQuestion(Type type)
        {
            return (BaseQuestion)Activator.CreateInstance(type);
        }


    }

    public class PaneQuestion : BaseQuestion
    {
        private Form _Parent;
        private String _Title;
        private List<BaseQuestion> _Childs;
    }
}
