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
        private Dictionary<String, FormAnswer> _AnswerForms;
        private RootQuestion _RootQuestion;

        public Form()
        {
            _RootQuestion = new RootQuestion(this);
            _AnswerForms = new Dictionary<String, FormAnswer> (); 
        }

        public string Title 
        {get{
            if (_RootQuestion.Title != null)
                    {
                        return _RootQuestion.Title;
                    }
                    return _Label;
        
       } set { _Label = value; } }

        public int AnswerCount { get { return _AnswerForms.Count(); } }

        public RootQuestion Questions { get { return _RootQuestion; } }

        public FormAnswer FindOrCreateFormAnswer(string name)
        {
            FormAnswer a; 
            _AnswerForms.TryGetValue(name, out a);
            if (a == null)
            {
                a = new FormAnswer(name);
                _AnswerForms.Add(name,a);
            }
            return a;
        }


    }

    public class FormAnswer
    {
        private String _UniqueName;
        private Dictionary<BaseQuestion, BaseAnswer> _Answers;

        public FormAnswer(string name)
        {
            _UniqueName = name;
            _Answers = new Dictionary<BaseQuestion, BaseAnswer>();
        }

        public string UniqueName{ get { return _UniqueName; } }

        public BaseAnswer FindAnswerFor(BaseQuestion bq)
        {
            BaseAnswer ba; 
            _Answers.TryGetValue(bq, out ba);
            return ba;
        }
        public BaseAnswer AddAnswerFor(BaseQuestion bq)
        {
             BaseAnswer ba; 
            _Answers.TryGetValue(bq, out ba);
            if (ba == null)
            {
                ba = (BaseAnswer)Activator.CreateInstance(bq.AnswerType);
                _Answers.Add(bq, ba);
            }
            return ba;
        }


    }

    public abstract class BaseQuestion
    {
        BaseQuestion _Parent;
        List<BaseQuestion> _Childs = new List<BaseQuestion>();
        String _Title;
        int _Index;
        bool _Required = true;

        public BaseQuestion()
        {
            List<BaseQuestion> _Childs = new List<BaseQuestion>();
        }

        protected void NotifiyChildsOfIndexMeddling(int oldindex, int newindex)
        {
            if (oldindex != newindex)
            {
                if (newindex >= 0)
                {
                    foreach (BaseQuestion qq in _Childs)
                    {
                        qq.IndexModif(oldindex, newindex);
                    }
                }
                else
                {
                    foreach (BaseQuestion qq in _Childs)
                    {
                        qq.IndexSuppr(oldindex);
                    }
                }

            }

        }
        protected void IndexModif(int oldindex, int newindex)
        {
            if (_Index == oldindex)
            {
                _Index = newindex;
            }else if(_Index <= oldindex & _Index >= newindex)
            {
                _Index++;
            }
            else if (_Index >= oldindex & _Index <= newindex)
            {
                _Index--;
            }
        }
        protected void IndexSuppr(int deletedIndex)
        {
            if(_Index > deletedIndex)
            {
                _Index--;
            }
        }

        public int Index { get { return _Index; } set 
        {
            if (value <= 0) { _Parent.NotifiyChildsOfIndexMeddling(_Index,0);}
            else { _Parent.NotifiyChildsOfIndexMeddling(_Index, value); }
        } }
        public string Title { get { return _Title; } set { _Title = value; } }
        public bool AllowEmptyAnswer { get { return _Required; } set { _Required = value; } }
        List<BaseQuestion> Childs { get { return _Childs; }}
        public BaseQuestion Parent { get { return _Parent; } set 
        {
            if((value != _Parent) & (_Parent != null))
            {
                _Parent.NotifiyChildsOfIndexMeddling(_Index, -1);
                _Parent.Childs.Remove(this);
            }
            if(value == null)
            {
                _Parent = null;
            }else if(value != _Parent)
            {
                _Parent = value;
                if(_Parent.Childs == null )
                {
                    _Index = 0;
                }
                else
                {
                    _Index = _Parent.Childs.Count();
                }
                _Parent.Childs.Add(this);
            }
        } }
        abstract public Type AnswerType { get; set; }

        public bool Contains(BaseQuestion q)
        {
            return q.HaveAncestor(this);
        }
        public bool HaveAncestor(BaseQuestion q)
        {
            if (this.Parent == q)
                return true;
            if (this.Parent is RootQuestion)
                return false;
            return this.Parent.HaveAncestor(q);
        }

        public BaseQuestion AddNewQuestion(Type type)
        {
            BaseQuestion a = (BaseQuestion)Activator.CreateInstance(type);
            a.Parent = this;
            return a;
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

        public RootQuestion(Form form)
        {
            _Parent = form;
        }

       override public Type AnswerType { get; set; }

    }

    public class PaneQuestion : BaseQuestion
    {
        override public Type AnswerType { get; set; }
    }

    public class OpenQuestion : BaseQuestion
    {
        Type _AnswerType;
       override public Type AnswerType { get{
           if (_AnswerType == null)
           {
               return typeof(OpenAnswer);
           }
           else return _AnswerType;
           
       } set{_AnswerType = value;} }
    }


    public abstract class BaseAnswer
    {

    }

    public class OpenAnswer : BaseAnswer
    {
        public string FreeAnswer;
    }

}
