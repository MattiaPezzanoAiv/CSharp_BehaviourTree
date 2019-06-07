using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT
{
    public enum NodeState { Success, Failure, Running}
    public abstract class Node
    {
        public Node()
        {
            children = new List<Node>();
            decorators = new List<Decorator>();
        }

        protected IList<Node> children;
        protected IList<Decorator> decorators; 

        public abstract NodeState Run(BT bt);



        public virtual void AppendDecorator(Decorator dec)
        {
            if (dec == null)
                throw new ArgumentNullException();
            if (decorators.Contains(dec))
                throw new ArgumentException("This decorator is alredy added");

            decorators.Add(dec);
        }
        public virtual void InsertDecorator(int idx, Decorator dec)
        {
            if (dec == null)
                throw new ArgumentNullException();
            if (decorators.Contains(dec))
                throw new ArgumentException("This decorator is alredy added");

            decorators.Insert(idx, dec);
        }
        public bool InvokeConditionDecorators(BT bt)
        {
            foreach (var d in decorators)
                if (!d.OnNodeCondition(bt))
                    return false;
            return true;
        }
        public void InvokeBeforeDecorators(BT bt)
        {
            foreach (var d in decorators)
                d.OnBeforeNodeExecution(bt);
        }
        public void InvokeAfterDecorators(BT bt, NodeState state)
        {
            foreach (var d in decorators)
                d.OnAfterNodeExecution(bt, state);
        }





        public int ChildrenCount
        {
            get
            {
                return children.Count;
            }
        }
        
        public Node Parent {
            get;
            private set;
        }

        public virtual Node Append(Node node)
        {
            if (node == null)
                throw new ArgumentNullException();
            if (node == this)
                throw new ArgumentException("A node can't be the child of itself");
            if (children.Contains(node))
                throw new ArgumentException("This node is alredy added as a child");

            children.Add(node);
            node.Parent = this;
            return node;
        }
        public virtual Node Insert(int idx, Node node)
        {
            if (node == null)
                throw new ArgumentNullException();
            if (node == this)
                throw new ArgumentException("A node can't be the child of itself");
            if (children.Contains(node))
                throw new ArgumentException("This node is alredy added as a child");

            children.Insert(idx, node);
            node.Parent = this;
            return node;
        }
    }
}
