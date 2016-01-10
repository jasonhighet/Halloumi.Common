using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Halloumi.Common.Windows.Controls
{
    public class RichTextBox : System.Windows.Forms.RichTextBox
    {
        #region Private Variables

        /// <summary>
        /// This is used to temporarily store the start position of the selected text
        /// </summary>
        private Stack<int> _selectionStartStack = new Stack<int>();

        /// <summary>
        /// This is used to temporarily store the length of the selected text
        /// </summary>
        private Stack<int> _selectionLengthStack = new Stack<int>();

        /// <summary>
        /// This is used to temporarily store the current type of mouse cursor
        /// </summary>
        private Stack<Cursor> _cursorStack = new Stack<Cursor>();

        /// <summary>
        /// Set to true when painting is disabled
        /// </summary>
        private bool _paintDisabled = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether painting of the control is disabled.
        /// </summary>
        [Browsable(false)]
        public bool PaintDisabled
        {
            get { return _paintDisabled; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves the current cursor to a stack, and then changes 
        /// the cursor for the control to the specified cursor.
        /// </summary>
        /// <param name="cursor">The cursor to change to.</param>
        public void ChangeCursor(Cursor cursor)
        {
            Cursor currentCursor = this.Cursor;
            _cursorStack.Push(currentCursor);
            this.Cursor = cursor;
            Application.DoEvents();
        }

        /// <summary>
        /// Restores the current cursor for the control to first cursor on the stack
        /// </summary>
        public void RestoreCursor()
        {
            if (_cursorStack.Count > 0)
            {
                this.Cursor = _cursorStack.Pop();
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Scrolls to the start of the text in the control.
        /// </summary>
        public void ScrollToStart()
        {
            this.Select(0, 0);
            this.ScrollToCaret();
        }

        /// <summary>
        /// Scrolls to the end of the text in the control.
        /// </summary>
        public void ScrollToEnd()
        {
            this.Select(this.Text.Length, 0);
            this.ScrollToCaret();
        }


        /// <summary>
        /// Saves the current cursor and/or selection position
        /// </summary>
        public void SaveSelectionState()
        {
            _selectionLengthStack.Push(this.SelectionLength);
            _selectionStartStack.Push(this.SelectionStart);
        }

        /// <summary>
        /// Restores the current cursor and/or selection position
        /// </summary>
        public void RestoreSelectionState()
        {
            if (_selectionLengthStack.Count > 0 && _selectionStartStack.Count > 0)
            {
                int selectionStart = _selectionStartStack.Pop();
                int selectionLength = _selectionLengthStack.Pop();
                this.Select(selectionStart, selectionLength);
            }
        }

        /// <summary>
        /// Disables painting of the cotrol
        /// </summary>
        public void DisablePaint()
        {
            if (DesignMode) return;

           // Stop redrawing and sending of events
           SendMessage(this.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
           _eventMask = SendMessage(this.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
            
            _paintDisabled = true;
        }

        /// <summary>
        /// Enables painting of the control
        /// </summary>
        public void EnablePaint()
        {
            if (DesignMode) return;
            
            // turn on events and redrawing
            SendMessage(this.Handle, EM_SETEVENTMASK, 0, _eventMask);
            SendMessage(this.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            
            // force repaint
            this.Invalidate();
            
            _paintDisabled = false;
        }

        /// <summary>
        /// Expands the current selection to the start of the first line 
        /// and the end of the last line in the selection.
        /// </summary>
        public void ExpandSelection()
        {
            if (this.Lines.GetUpperBound(0) < 1)
            {
                this.SelectAll();
            }
            else
            {
                // find the line index of the last line of the selected text
                int endLine = this.GetLineFromCharIndex(this.SelectionStart + this.SelectionLength);

                // find the index of the first char of the first selected line
                int start = this.GetFirstCharIndexOfCurrentLine();

                // find the index of the last char of the last selected line
                int end = this.GetFirstCharIndexFromLine(endLine) + this.Lines[endLine].Length;

                // set the length to be the difference between them
                int length = end - start;

                // select new text
                this.Select(start, length);
            }
        }

        #endregion

        #region DLL Calls

        /// <summary>
        /// Sends a windows message.
        /// </summary>
        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        private IntPtr _eventMask = IntPtr.Zero;
        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x400;
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);

        #endregion
    }
}
