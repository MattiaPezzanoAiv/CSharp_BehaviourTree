using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Decorators
{
    public class SetValueOnFail : Decorator
    {
        protected string key;
        protected Func<object> val;
        public SetValueOnFail(string key, Func<object> value)
        {
            this.key = key;
            this.val = value;
        }

        public override void OnAfterNodeExecution(BT bt, NodeState state)
        {
            if (state != NodeState.Failure)
                return;

            bt.BlackBoard.SetValue(key, val());
        }
    }
}
