using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Graph
{
    public class ParentNodeLinker : NodeLinker
    {
        public ParentNodeLinker(Rect leftRect, Rect _rightRect) : base(leftRect, _rightRect)
        {
        }

        public ParentNodeLinker(Node from, Node to) : base(from, to)
        {
        }

        protected override Color color { get => Color.blue; }
    }
}
