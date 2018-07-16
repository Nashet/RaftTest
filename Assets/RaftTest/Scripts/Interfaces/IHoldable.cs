using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaftTest
{
    /// <summary>
    /// Represents objects which can be hold in players hands
    /// </summary>    
    public interface IHoldable : IHideable, IActable
    {
        /// <summary>
        /// updates block held by player 
        /// </summary>
        void UpdateBlock();

       
    }
}
