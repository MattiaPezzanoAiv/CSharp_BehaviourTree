using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Decorators
{
    public class BlackBoardBoolCondition : Decorator
    {
        protected string valueKey;
        protected bool negate;

        public BlackBoardBoolCondition(string valueKey, bool negate = false)
        {
            this.valueKey = valueKey;
            this.negate = negate;
        }
        public override bool OnNodeCondition(BT bt)
        {
            if(negate)
                return !bt.BlackBoard.ReadValue<bool>(valueKey);

            return bt.BlackBoard.ReadValue<bool>(valueKey);
        }
    }
}
