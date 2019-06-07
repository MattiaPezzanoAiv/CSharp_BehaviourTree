using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Decorators
{
    public class FloatCondition : BaseConditionEvaluator<float>
    {
        protected override bool IsEqual(float obj1, float obj2)
        {
            return obj1 == obj2;
        }
        protected override bool IsGreater(float obj1, float obj2)
        {
            return obj1 > obj2;
        }
        protected override bool IsLess(float obj1, float obj2)
        {
            return obj1 < obj2;
        }
        protected override bool IsGreaterEqual(float obj1, float obj2)
        {
            return obj1 >= obj2;
        }
        protected override bool IsLessEqual(float obj1, float obj2)
        {
            return obj1 <= obj2;
        }
        protected override bool IsNotEqual(float obj1, float obj2)
        {
            return obj1 != obj2;
        }
    }
}
