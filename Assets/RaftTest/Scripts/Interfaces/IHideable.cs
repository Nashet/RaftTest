using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaftTest
{
    /// <summary>
    /// Common interface for objects which can change visibility
    /// </summary>
    public interface IHideable
    {
        event EventHandler<EventArgs> Hidden;

        event EventHandler<EventArgs> Shown;

        void Hide();

        void Show();
    }
}
