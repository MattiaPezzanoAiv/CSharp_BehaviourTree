using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT
{
    public abstract class Decorator
    {
        public virtual bool OnNodeCondition(BT bt)
        {
            return true;
        }
        public virtual void OnBeforeNodeExecution(BT bt)
        {
        }
        public virtual void OnAfterNodeExecution(BT bt, NodeState state)
        {
        }
    }
}
