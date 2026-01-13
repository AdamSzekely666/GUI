using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace MatroxLDS
{
    /// <summary>
    /// Basic implementation of a circular list. It's a wrapper on the list that enforce starting writing on itself after
    /// max capacity is reached. It's not complete, but it does what we need for now.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircularList<T>
    {
        private static readonly Object o = new Object();
        int _size_limit;
        int _start = 0;
        List<T> _list;

        public CircularList(int sizeLimit)
        {
            _size_limit = sizeLimit;
            _list = new List<T>();
        }

        /// <summary>
        /// Same as List<T> if size limit is not reach. If it is, write at the start then move the start 1 position.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (o)
            {
                if (_list.Count < _size_limit)
                {
                    _list.Add(item);
                }
                else
                {
                    _list[_start] = item;
                    // The % is import so the _start never reaches more than the list max size. At the last element, it circles back to the first one.
                    _start = (_start + 1) % _size_limit;
                }
            }
        }

        /// <summary>
        /// Returns the list properly ordered. Order is important, since we use it to display results from oldest to newest.
        /// </summary>
        /// <returns>A list<T> in the correct order.</returns>
        public List<T> GetList()
        {
            // If the size limit is not reached, behaves like a regular list. 
            if (_list.Count < _size_limit)
            {
                return _list.ToList();
            }

            // If we start cycling, the list needs to be re-assembled so that what is "left" of the _start pointer comes last, to respect the order
            // it was written.
            lock (o)
            {
                var latestPart = _list.GetRange(0, (_size_limit) - (_size_limit - _start));
                var recentPart = _list.GetRange(_start, _size_limit - latestPart.Count);

                return recentPart.Concat(latestPart).ToList();
            }
        }
    }
}
