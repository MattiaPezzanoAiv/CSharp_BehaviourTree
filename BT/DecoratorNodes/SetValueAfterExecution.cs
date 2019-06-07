using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Decorators
{
    public class SetValueAfterExecution : Decorator
    {
        string key;
        Func<object> value;
        public SetValueAfterExecution(string key, Func<object> value)
        {
            this.key = key;
            this.value = value;
        }

        public override void OnAfterNodeExecution(BT bt, NodeState state)
        {
            bt.BlackBoard.SetValue(key, value());
        }
    }
}
