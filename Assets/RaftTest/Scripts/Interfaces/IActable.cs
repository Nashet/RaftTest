using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaftTest
{
    public interface IActable
    {
        /// <summary>
        /// Reaction on players use of this item
        /// </summary>
        void Act();
    }
}
