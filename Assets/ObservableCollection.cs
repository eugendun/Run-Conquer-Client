using System;
using System.Collections.Generic;
using System.Collections;

namespace AssemblyCSharp
{
    public enum ObservableCollectionActions
    {
        Add,
        Remove,
        Clear
    }

    public class ObservableCollectionEventArgs : EventArgs
    {
        public ObservableCollectionActions Action{ get; set; }
        public ICollection AddedItems{get; set;}
        public ICollection RemovedItems{get; set;}

        public ObservableCollectionEventArgs(ObservableCollectionActions action, ICollection items)
        {
            Action = action;
            if(Action == ObservableCollectionActions.Add) {
                AddedItems = items;
            } else if(Action == ObservableCollectionActions.Remove || Action == ObservableCollectionActions.Clear) {
                RemovedItems = items;
            }
        }
    }

    public class ObservableCollection<T> : ICollection<T> where T : class
    {
        private ICollection<T> _collection;

        public ObservableCollection(ICollection<T> collection)
        {
            _collection = collection;
        }

        #region CollectionChanged event implementation

        public delegate void CollectionChangedHandler(object sender,ObservableCollectionEventArgs e);

        public event CollectionChangedHandler CollectionChanged;

        private void OnCollectionChanged(ObservableCollectionEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(_collection, e);
            }
        }

        #endregion

        #region ICollection implementation

        public void Add(T item)
        {
            var addedItems = new List<T>();
            addedItems.Add(item);
            _collection.Add(item);
            OnCollectionChanged(new ObservableCollectionEventArgs(
                ObservableCollectionActions.Add, 
                addedItems
            ));
        }

        public void Clear()
        {
            var removedItems = new List<T>(_collection);
            _collection.Clear();
            OnCollectionChanged(new ObservableCollectionEventArgs(
                ObservableCollectionActions.Clear,
                removedItems
            ));
        }

        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var removedItems = new List<T>();
            removedItems.Add(item);
            bool result = _collection.Remove(item);
            OnCollectionChanged(new ObservableCollectionEventArgs(
                ObservableCollectionActions.Remove,
                removedItems
            ));
            return result;
        }

        public int Count
        {
            get
            {
                return _collection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _collection.IsReadOnly;
            }
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        #endregion


    }
}

