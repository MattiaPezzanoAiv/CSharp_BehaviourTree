using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Decorators
{
    public class BlackBoardConstantCondition<T, K> : Decorator where T : IConditionEvaluator<K>, new() 
    {
        protected string obj1;
        protected K obj2;
        protected T cond;

        public BlackBoardConstantCondition(string obj1, K obj2, ConditionType condType)
        {
            this.obj1 = obj1;
            this.obj2 = obj2;
            cond = new T();
            cond.Init(condType);
        }

        public override bool OnNodeCondition(BT bt)
        {
            return cond.Evaluate(bt.BlackBoard.ReadValue<K>(obj1), obj2);
        }
    }
}
