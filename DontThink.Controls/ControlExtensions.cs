using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DontThink.Controls
{
    /// <summary>
    /// Contains various <see cref="Control"></see> extension methods.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Fills specified <see cref="Panel"/> completely with the specified <see cref="Control"/>
        /// </summary>
        /// <param name="panel">Panel to fill completely.</param>
        /// <param name="control">Control to fill the panel completely with,</param>
        /// <example>
        /// Invoke as either static or extension method.
        /// <code>
        /// Panel panel = new Panel();
        /// TextArea area = new TextArea();
        /// panel.FillWithControl(area);
        /// ControlExtensions.FillWithControl(panel, area);
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">null panel or control provided</exception>
        public static void FillWithControl(this Panel panel, Control control)
        {
            if (panel == null)
                throw new ArgumentNullException("panel");

            if (control == null)
                throw new ArgumentNullException("control");

            // no need to resize ourself :)
            if (ReferenceEquals(panel, control))
                return;

            control.SetBounds(
                x: 0,
                y: 0,
                width: panel.Width,
                height: panel.Height);
        }
    }
}
