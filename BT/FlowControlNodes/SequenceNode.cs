using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT
{
    public class SequenceNode : FlowControlNode
    {
        public override NodeState Run(BT bt)
        {
            var toExecute = (runningNode == null) ? this.children :   //if there is not node in running state execute all children
                this.children.SkipWhile((n) => n != runningNode);   //else skip nodes 

            foreach (var child in toExecute)
            {
                if (!child.InvokeConditionDecorators(bt))    //conditions not satisfied, skip this node
                    continue;

                child.InvokeBeforeDecorators(bt);            //invoke pre operations

                var state = child.Run(bt);                              //run task

                child.InvokeAfterDecorators(bt, state);      //invoke after operations

                if (state == NodeState.Failure)
                    return NodeState.Failure;
                if(state == NodeState.Running)
                {
                    runningNode = child;
                    return NodeState.Running;
                }
            }
            runningNode = null;
            return NodeState.Success;
        }
    }
}
