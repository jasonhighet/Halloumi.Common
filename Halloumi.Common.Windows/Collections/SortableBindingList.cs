using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Halloumi.Common.Windows.Collections
{
    /// <summary>
    /// A sortable binding list.  Used for binding lists to UI objects such as grids that can autosort.
    /// </summary>
    public class SortableBindingList<T> : BindingList<T>
    {
        #region Private Variables

        /// <summary>
        /// A dcitionary of comparers
        /// </summary>
        private readonly Dictionary<Type, PropertyComparer<T>> _comparers;
        
        /// <summary>
        /// Set to true of sorted
        /// </summary>
        private bool _isSorted;
        
        /// <summary>
        /// The sort direction
        /// </summary>
        private ListSortDirection _listSortDirection;
        
        /// <summary>
        /// The descriptpr for the property to sort on
        /// </summary>
        private PropertyDescriptor _propertyDescriptor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public SortableBindingList()
            : base(new List<T>())
        {
            _comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        public SortableBindingList(IEnumerable<T> enumeration)
            : base(new List<T>(enumeration))
        {
            _comparers = new Dictionary<Type, PropertyComparer<T>>();
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Applies the sort core.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="direction">The direction.</param>
        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)this.Items;

            Type propertyType = property.PropertyType;
            PropertyComparer<T> comparer;
            if (!_comparers.TryGetValue(propertyType, out comparer))
            {
                comparer = new PropertyComparer<T>(property, direction);
                _comparers.Add(propertyType, comparer);
            }

            comparer.SetPropertyAndDirection(property, direction);
            itemsList.Sort(comparer);

            _propertyDescriptor = property;
            _listSortDirection = direction;
            _isSorted = true;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Removes any sort applied if sorting is implemented in a derived class; otherwise, raises an exception.
        /// </summary>
        protected override void RemoveSortCore()
        {
            _isSorted = false;
            _propertyDescriptor = base.SortPropertyCore;
            _listSortDirection = base.SortDirectionCore;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Finds the core.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="key">The key.</param>
        /// <returns>The core index</returns>
        protected override int FindCore(PropertyDescriptor property, object key)
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                T element = this[i];
                if (property.GetValue(element).Equals(key))
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        /// <returns>true if the list supports sorting; otherwise, false. The default is false.</returns>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the list is sorted.
        /// </summary>
        /// <returns>true if the list is sorted; otherwise, false. The default is false.</returns>
        protected override bool IsSortedCore
        {
            get { return _isSorted; }
        }

        /// <summary>
        /// Gets the property descriptor that is used for sorting the list if sorting is implemented 
        /// in a derived class; otherwise, returns null.
        /// </summary>
        /// <returns>The property descriptor used for sorting the list.</returns>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _propertyDescriptor; }
        }

        /// <summary>
        /// Gets the direction the list is sorted.
        /// </summary>
        /// <returns>One of thesort direction values. The default is ascending. </returns>
        protected override ListSortDirection SortDirectionCore
        {
            get { return _listSortDirection; }
        }

        /// <summary>
        /// Gets a value indicating whether the list supports searching.
        /// </summary>
        /// <returns>true if the list supports searching; otherwise, false. The default is false.</returns>
        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        #endregion

        #region Internal Classes

        /// <summary>
        /// Property comparer
        /// </summary>
        public class PropertyComparer<U> : IComparer<U>
        {
            #region Private Variables

            /// <summary>
            /// The comparer object
            /// </summary>
            private readonly IComparer _comparer;

            /// <summary>
            /// The propert descriptor
            /// </summary>
            private PropertyDescriptor _propertyDescriptor;

            /// <summary>
            /// Set to -1 if reversing
            /// </summary>
            private int _reverse;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the PropertyComparer class.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <param name="direction">The direction.</param>
            public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
            {
                _propertyDescriptor = property;
                Type comparerForPropertyType = typeof(Comparer<>).MakeGenericType(property.PropertyType);
                _comparer = (IComparer)comparerForPropertyType.InvokeMember("Default", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public, null, null, null);
                this.SetListSortDirection(direction);
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Less than zero - x is less than y. Zero - x equals y. Greater than zero - x is greater than y.
            /// </returns>
            public int Compare(U x, U y)
            {
                return _reverse * _comparer.Compare(_propertyDescriptor.GetValue(x), _propertyDescriptor.GetValue(y));
            }

            /// <summary>
            /// Sets the property and direction.
            /// </summary>
            /// <param name="descriptor">The descriptor.</param>
            /// <param name="direction">The direction.</param>
            public void SetPropertyAndDirection(PropertyDescriptor descriptor, ListSortDirection direction)
            {
                this.SetPropertyDescriptor(descriptor);
                this.SetListSortDirection(direction);
            }

            #endregion

            #region Private Method

            /// <summary>
            /// Sets the property descriptor.
            /// </summary>
            /// <param name="descriptor">The descriptor.</param>
            private void SetPropertyDescriptor(PropertyDescriptor descriptor)
            {
                _propertyDescriptor = descriptor;
            }

            /// <summary>
            /// Sets the list sort direction.
            /// </summary>
            /// <param name="direction">The direction.</param>
            private void SetListSortDirection(ListSortDirection direction)
            {
                _reverse = direction == ListSortDirection.Ascending ? 1 : -1;
            }

            #endregion
        }

        #endregion
    }
}
