using System.Windows.Forms;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Change setting of the Panel control to prevent flickering when dragging Picturebox over the Panel
    /// </summary>
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
        }
    }
}