using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT
{
    public sealed class BT
    {
        BlackBoard blackBoard;
        public BlackBoard BlackBoard { get
            {
                return this.blackBoard;
            } }

        Node root;
        string activeTag = "";
        
        public BT(Node root)
        {
            this.root = root;
            blackBoard = new BlackBoard();
        }

        public void SetActiveTag(string tag)
        {
            activeTag = tag;
        }
        /// <summary>
        /// Returns the last run tag
        /// </summary>
        /// <returns></returns>
        public string GetActiveTag()
        {
            return activeTag;
        }

        public void Run()
        {
            activeTag = "";
            root.Run(this);
        }
    }
}
