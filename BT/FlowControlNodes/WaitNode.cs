using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT
{
    public class WaitNode : FlowControlNode
    {
        protected DateTime endTime;
        protected float seconds;
        //protected 
        public WaitNode(float seconds)
        {
            this.seconds = seconds;
            endTime = DateTime.Now.AddSeconds(seconds);
        }

        public override NodeState Run(BT bt)
        {
            if ((endTime - DateTime.Now).Seconds <= 0)
            {
                endTime = DateTime.Now.AddSeconds(seconds);
                return NodeState.Success;
            }
            return NodeState.Running;
        }
    }
}
