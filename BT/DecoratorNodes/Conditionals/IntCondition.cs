using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Decorators
{
    public class IntCondition : BaseConditionEvaluator<int>
    {
        protected override bool IsEqual(int obj1, int obj2)
        {
            return obj1 == obj2;
        }
        protected override bool IsGreater(int obj1, int obj2)
        {
            return obj1 > obj2;
        }
        protected override bool IsLess(int obj1, int obj2)
        {
            return obj1 < obj2;
        }
        protected override bool IsGreaterEqual(int obj1, int obj2)
        {
            return obj1 >= obj2;
        }
        protected override bool IsLessEqual(int obj1, int obj2)
        {
            return obj1 <= obj2;
        }
        protected override bool IsNotEqual(int obj1, int obj2)
        {
            return obj1 != obj2;
        }
    }
}
