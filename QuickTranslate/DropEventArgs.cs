using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTranslate
{
    /// <summary>
    /// Event arguments representing a TranslateItem being dragged and dropped to another location.
    /// </summary>
    public class DropEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new DropEventArgs with data prefilled.
        /// </summary>
        /// <param name="target">The target UI element that the item is being dropped onto.</param>
        /// <param name="above">Determine whether to put the dropped item above (before) the target element.</param>
        /// <param name="data">Data representing the item being dropped.</param>
        public DropEventArgs(TranslateItem target, bool above, string data)
        {
            TargetItem = target;
            DropAbove = above;
            Data = data;
        }

        public TranslateItem TargetItem { get; private set; }
        public bool DropAbove { get; private set; }

        public string Data { get; private set; }

    }
}
