using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Helpers;

namespace Halloumi.Common.Windows.Controls
{
    /// <summary>
    /// A data grid view
    /// </summary>
    [ToolboxBitmap(typeof(ComponentFactory.Krypton.Toolkit.KryptonDataGridView))]
    public class DataGridView : ComponentFactory.Krypton.Toolkit.KryptonDataGridView
    {
        #region Private Variables

        /// <summary>
        /// A list of rows that should have their columns merged
        /// </summary>
        private List<int> _mergeRows = new List<int>();

        /// <summary>
        /// A list of rows that are selected
        /// </summary>
        private List<SavedRow> _savedSelectedRows = new List<SavedRow>();

        private SavedRow _savedFirstRow = null;

        ///// <summary>
        ///// Indicates whether columns are created automatically when the datasource is set.
        ///// </summary>
        //private bool _autoGenerateColumns = false;

        ///// <summary>
        ///// The color of the border
        ///// </summary>
        //private Color _borderColor = SystemColors.ControlDarkDark;

        ///// <summary>
        ///// If true, a border is shown around the grid
        ///// </summary>
        //private bool _showBorder = true;

        private class SavedRow
        {
            public int Index;
            public string Text;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridView"/> class.
        /// </summary>
        public DataGridView()
            : base()
        {
            // base.AutoGenerateColumns = _autoGenerateColumns;
            this.AutoGenerateColumns = false;
            this.MergeColor = Color.Gainsboro;
            this.MergeColumn = 0;
            this.AutoCommit = false;

            this.KeyPress += new KeyPressEventHandler(DataGridView_KeyPress);
            this.MouseDown += new MouseEventHandler(DataGridView_MouseDown);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether changes to the cells will automatically committ
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(false)]
        [Description("If true, changes to the cells will automatically update the datasource")]
        public bool AutoCommit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the merge column.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(0)]
        [Description("The index of the column that provides the text for each merged row")]
        public int MergeColumn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the merged rows.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("The background color for each merged row")]
        public Color MergeColor
        {
            get;
            set;
        }

        ///// <summary>
        ///// Gets or sets a value indicating whether columns are created automatically when the datasource is set.
        ///// </summary>
        //[Browsable(true), Category("Behavior"), DefaultValue(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //[Description("Indicates whether columns are created automatically when the DataSource or DataMember properties are set.")]
        //public new bool AutoGenerateColumns
        //{
        //    get
        //    {
        //        base.AutoGenerateColumns = _autoGenerateColumns;
        //        return _autoGenerateColumns;
        //    }
        //    set
        //    {
        //        _autoGenerateColumns = value;
        //        base.AutoGenerateColumns = _autoGenerateColumns;
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether columns are created automatically when the datasource is set.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool AutoGenerateColumns
        {
            get
            {
                return base.AutoGenerateColumns;
            }
            set
            {
                base.AutoGenerateColumns = false;
            }
        }

        ///// <summary>
        ///// If true, a border is shown around the grid.
        ///// </summary>
        //[Browsable(true), Category("Appearance"), DefaultValue(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //[Description("If true, a border is shown around the grid")]
        //public bool ShowBorder
        //{
        //    get { return _showBorder; }
        //    set
        //    {
        //        _showBorder = value;
        //        this.Invalidate();
        //    }
        //}

        #endregion

        #region Public Methods

        /// <summary>
        /// Merges all columns in the specified row.
        /// </summary>
        /// <param name="row">The index of the row to merge.</param>
        public void MergeRow(int row)
        {
            _mergeRows.Add(row);
            this.Invalidate();
        }

        /// <summary>
        /// Reverts all merged rows to normal rows
        /// </summary>
        public void ClearMergedRows()
        {
            _mergeRows.Clear();
            this.Invalidate();
        }

        /// <summary>
        /// Clears the selected rows.
        /// </summary>
        public void ClearSelectedRows()
        {
            foreach (DataGridViewRow row in this.Rows)
            {
                row.Selected = false;
            }
        }

        public void InvalidateDisplayedRows()
        {
            var start = this.FirstDisplayedScrollingRowIndex;
            var end = start + this.DisplayedRowCount(true);

            for (var i = start; i < end; i++)
            {
                this.InvalidateRow(i);
            }
        }

        /// <summary>
        /// Saves the selected rows.
        /// </summary>
        public void SaveSelectedRows()
        {
            var firstRow = this.FirstDisplayedScrollingRowIndex;
            if (firstRow > 0)
            {
                _savedFirstRow = new SavedRow()
                {
                    Index = firstRow,
                    Text = this.Rows[firstRow].Cells[0].Value.ToString()
                };
            }
            else
            {
                _savedFirstRow = null;
            }

            _savedSelectedRows.Clear();
            foreach (DataGridViewRow row in this.SelectedRows)
            {
                _savedSelectedRows.Add(new SavedRow()
                    {
                        Index = row.Index,
                        Text = this.Rows[row.Index].Cells[0].Value.ToString()
                    });
            }
        }

        /// <summary>
        /// Restores the selected rows.
        /// </summary>
        public void RestoreSelectedRows()
        {
            ClearSelectedRows();
            if (_savedSelectedRows.Count == 0) return;

            foreach (var savedRow in _savedSelectedRows)
            {
                if (savedRow.Index < this.Rows.Count && savedRow.Text == this.Rows[savedRow.Index].Cells[0].Value.ToString())
                {
                    this.Rows[savedRow.Index].Selected = true;
                }
                else
                {
                    var matchingRow = this.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => this.Rows[this.Rows.IndexOf(r)].Cells[0].Value.ToString() == savedRow.Text)
                        .OrderByDescending(r => Math.Abs(savedRow.Index - r.Index))
                        .ThenByDescending(r => savedRow.Index - r.Index)
                        .FirstOrDefault();

                    if (matchingRow != null) matchingRow.Selected = true;
                }
            }

            _savedSelectedRows.Clear();

            if (_savedFirstRow != null)
            {
                if (_savedFirstRow.Index < this.Rows.Count && _savedFirstRow.Text == this.Rows[_savedFirstRow.Index].Cells[0].Value.ToString())
                {
                    this.FirstDisplayedScrollingRowIndex = _savedFirstRow.Index;
                }
                else
                {
                    var matchingFirstRow = this.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => this.Rows[this.Rows.IndexOf(r)].Cells[0].Value.ToString() == _savedFirstRow.Text)
                        .OrderByDescending(r => Math.Abs(_savedFirstRow.Index - r.Index))
                        .ThenByDescending(r => _savedFirstRow.Index - r.Index)
                        .FirstOrDefault();

                    if (matchingFirstRow != null) this.FirstDisplayedScrollingRowIndex = matchingFirstRow.Index;
                }
            }
            _savedFirstRow = null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Overides the OnCurrentCellDirtyStateChanged method - forces a commit whenever a cell is edited
        /// </summary>
        protected override void OnCurrentCellDirtyStateChanged(EventArgs e)
        {
            base.OnCurrentCellDirtyStateChanged(e);
            if (this.IsCurrentCellDirty && this.AutoCommit)
            {
                this.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Draws a row so that all columns are merged into one
        /// </summary>
        /// <param name="row">The index of the row to draw</param>
        /// <param name="graphics">The graphics object</param>
        private void DrawMergeRow(int row, Graphics graphics)
        {
            Rectangle rowRectangle = this.GetRowDisplayRectangle(row, true);

            if (rowRectangle.Y == 0) return;

            graphics.FillRectangle(new SolidBrush(this.MergeColor), rowRectangle);
            rowRectangle.X--;
            rowRectangle.Y--;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Center;

            var font = FontHelper.EmboldenFont(this.ColumnHeadersDefaultCellStyle.Font);

            graphics.DrawString(this.Rows[row].Cells[this.MergeColumn].Value.ToString(),
                font,
                new SolidBrush(Color.Black),
                rowRectangle,
                format);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Paint event.
        /// Also draws a border around the control if showborder set to true
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // draw all merged cells.
            foreach (int row in _mergeRows)
            {
                DrawMergeRow(row, e.Graphics);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the KeyPress event of the DataGridView control.
        /// </summary>
        private void DataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            var character = e.KeyChar.ToString().ToLower();

            foreach (DataGridViewRow row in this.Rows)
            {
                if (row.Cells[0].Value.ToString().ToLower().StartsWith(character))
                {
                    this.ClearSelectedRows();
                    row.Selected = true;
                    this.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the DataGridView control.
        /// </summary>
        private void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTest = this.HitTest(e.X, e.Y);
                if (hitTest.RowIndex >= 0)
                {
                    this.Rows[hitTest.RowIndex].Selected = true;
                }
            }
        }

        #endregion
    }
}