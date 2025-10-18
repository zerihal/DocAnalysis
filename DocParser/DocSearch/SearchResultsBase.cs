using DocParser.Interfaces;
using System.Collections;

namespace DocParser.DocSearch
{
    public abstract class SearchResultsBase<T> : ISearchResultCollection<T>
    {
        protected List<T> _results = new List<T>();

        public int DiscoveredCount => _results.Count();

        /// <summary>
        /// Gets the enumerator instance for this collection.
        /// </summary>
        /// <returns>Enumerator of search results.</returns>
        public IEnumerator<T> GetEnumerator() => _results.GetEnumerator();

        /// <summary>
        /// Gets the enumerator instance for this collection.
        /// </summary>
        /// <returns>Enumerator of search results.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public bool Add(T item)
        {
            if (!_results.Contains(item))
            {
                _results.Add(item);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool AddRange(IEnumerable<T> items)
        {
            bool anyAdded = false;
            foreach (var item in items)
            {
                if (Add(item))
                    anyAdded = true;
            }

            return anyAdded;
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            if (_results.Remove(item))
            {
                _results.Remove(item);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void Clear() => _results.Clear();
    }
}
