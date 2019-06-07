using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Decorators
{
    public interface IConditionEvaluator<T>
    {
        bool Evaluate(T obj1, T obj2);
        void Init(ConditionType type);
    }
    public abstract class BaseConditionEvaluator<T> : IConditionEvaluator<T>
    {
        public virtual bool Evaluate(T obj1, T obj2)
        {
            return operationMap[condType](obj1, obj2);
        }

        protected ConditionType condType;
        protected Dictionary<ConditionType, Func<T, T, bool>> operationMap;

        public void Init(ConditionType type)
        {
            condType = type;
            //build map
            operationMap = new Dictionary<ConditionType, Func<T, T, bool>>();
            operationMap.Add(ConditionType.Equal, IsEqual);
            operationMap.Add(ConditionType.Greater, IsGreater);
            operationMap.Add(ConditionType.GreaterEqual, IsGreaterEqual);
            operationMap.Add(ConditionType.Less, IsLess);
            operationMap.Add(ConditionType.LessEqual, IsLessEqual);
        }

        protected abstract bool IsEqual(T obj1, T obj2);
        protected abstract bool IsGreater(T obj1, T obj2);
        protected abstract bool IsLess(T obj1, T obj2);
        protected abstract bool IsGreaterEqual(T obj1, T obj2);
        protected abstract bool IsLessEqual(T obj1, T obj2);
        protected abstract bool IsNotEqual(T obj1, T obj2);
    }

    public enum ConditionType { Equal, Greater, Less, GreaterEqual, LessEqual }
    public class BlackBoardCondition<T, K> : Decorator where T : IConditionEvaluator<K>, new()
    {
        protected string obj1;
        protected string obj2;
        protected T cond;

        public BlackBoardCondition(string obj1, string obj2, ConditionType condType)
        {
            this.obj1 = obj1;
            this.obj2 = obj2;
            cond = new T();
            cond.Init(condType);
        }

        public override bool OnNodeCondition(BT bt)
        {
            return cond.Evaluate(bt.BlackBoard.ReadValue<K>(obj1), bt.BlackBoard.ReadValue<K>(obj2));
        }
    }
}
