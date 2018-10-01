using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaftTest
{
    public interface IWeapon: IActable, IDamageSource
    {
         float DamageDistance { get; }
    }
}
