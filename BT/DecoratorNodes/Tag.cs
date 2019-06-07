using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Decorators
{
    public class Tag : Decorator
    {
        string tag;

        public Tag(string tag)
        {
            this.tag = tag;
        }

        public override void OnBeforeNodeExecution(BT bt)
        {
            bt.SetActiveTag(tag);
        }
    }
}
