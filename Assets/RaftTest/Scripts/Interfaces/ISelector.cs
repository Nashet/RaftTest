using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// Describes ability to select & deselect some GameObject.
    /// Supposed to be a component
    /// </summary>
    public interface ISelector
    {
        void Select(GameObject someObject);
        void Deselect(GameObject someObject);
    }
}
