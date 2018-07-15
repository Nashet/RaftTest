using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RaftTest
{
    public interface ICanSelect
    {
        void Select(GameObject someObject);
        void Deselect(GameObject someObject);
    }
}
