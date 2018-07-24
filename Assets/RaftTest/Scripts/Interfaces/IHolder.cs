using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaftTest
{
    /// <summary>
    /// Represents objects which can hold IHoldable
    /// </summary>
    public interface IHolder : IActable
    {
        void TakeInHand(IHoldable placeable);
        IHoldable Holds { get; }

    }
}
