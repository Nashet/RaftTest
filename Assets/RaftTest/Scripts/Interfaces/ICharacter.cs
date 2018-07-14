using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaftTest
{ 
    /// <summary>
    /// Represents objects with basic humanoid behavior
    /// </summary>
    public interface ICharacter: IActable
    {
        void TakeInHand(IHoldable placeable);
        IHoldable Holds { get; }
        
    }
}
