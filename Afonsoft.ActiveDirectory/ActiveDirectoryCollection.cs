using Afonsoft.ActiveDirectory.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Afonsoft.ActiveDirectory
{
    /// <summary>
    /// ActiveDirectoryCollection
    /// </summary>
    public class ActiveDirectoryCollection : ICollection<IActiveDirectory>, IEnumerable<IActiveDirectory>
    {
        private readonly IList<IActiveDirectory> _entries = new List<IActiveDirectory>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IActiveDirectory this[int index] => _entries[index];

        /// <summary>
        /// Count 
        /// </summary>
        public int Count => _entries.Count;

        /// <summary>
        /// IsReadOnly 
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="item">IActiveDirectory</param>
        public void Add(IActiveDirectory item)
        {
            _entries.Add(item);
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            _entries.Clear();
        }

        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="item">IActiveDirectory</param>
        /// <returns></returns>
        public bool Contains(IActiveDirectory item)
        {
            return _entries.Contains(item);
        }

        /// <summary>
        /// CopyTo
        /// </summary>
        /// <param name="array">IActiveDirectory</param>
        /// <param name="arrayIndex">int</param>
        public void CopyTo(IActiveDirectory[] array, int arrayIndex)
        {
            _entries.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IActiveDirectory> GetEnumerator()
        {
            return ((IEnumerable<IActiveDirectory>)_entries).GetEnumerator();
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="item">IActiveDirectory</param>
        /// <returns></returns>
        public bool Remove(IActiveDirectory item)
        {
            return _entries.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

 }
