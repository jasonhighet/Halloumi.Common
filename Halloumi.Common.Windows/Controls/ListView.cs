using System;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Controls
{
    public class ListView : System.Windows.Forms.ListView
    {
        public event EventHandler Scroll;

        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        /// Overrides WndProc
        /// </summary>
        /// <param name="m">The Windows Message to process.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_VSCROLL ||
                m.Msg == WM_HSCROLL ||
                m.Msg == WM_KEYDOWN ||
                m.Msg == WM_MOUSEWHEEL)
            {
                if (this.Scroll != null) this.Scroll(this, EventArgs.Empty);
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Gets the first displayed tile.
        /// </summary>
        /// <returns>The first displayed tile.</returns>
        public ListViewItem GetFirstDisplayedTile()
        {
            if (this.View == System.Windows.Forms.View.LargeIcon && this.LargeImageList != null)
            {
                var size = this.LargeImageList.ImageSize;
                var item = this.GetItemAt(size.Width / 16, size.Height / 16);
                if (item == null) item = this.GetItemAt(size.Width / 8, size.Height / 8);
                if (item == null) item = this.GetItemAt(size.Width / 4, size.Height / 4);
                if (item == null) item = this.GetItemAt(size.Width / 2, size.Height / 2);
                return item;
            }
            return this.GetItemAt(20, 5);
        }
    }
}