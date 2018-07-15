using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaftTest
{    
    public interface IBreakable 
    {
        //event EventHandler<EventArgs> Damaged;
        void HitBy(IDamageSource damageSource);
    }
}
