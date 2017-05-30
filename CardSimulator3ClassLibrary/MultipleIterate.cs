using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSimulator3ClassLibrary
{
    /// <summary>
    /// This static class allows iteration in a foreach each loop with two or three IEnumerables
    /// </summary>
    public static class MultipleIterate
    {
        /// <summary>
        /// Allows two Enumerables to be enumerated as a single tuple in a foreach loop.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t1s"></param>
        /// <param name="t2s"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<T1, T2>> Over<T1, T2>(IEnumerable<T1> t1s, IEnumerable<T2> t2s)
        {
            using (var it1s = t1s.GetEnumerator())
            using (var it2s = t2s.GetEnumerator())
            {
                while (it1s.MoveNext() && it2s.MoveNext())
                {
                    yield return Tuple.Create(it1s.Current, it2s.Current);
                }
            }

        }
        /// <summary>
        /// Allows three Enumerables to be enumerated as a single tuple in a foreach loop.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t1s"></param>
        /// <param name="t2s"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<T1, T2, T3>> Over<T1, T2, T3>(IEnumerable<T1> t1s, IEnumerable<T2> t2s, IEnumerable<T3> t3s)
        {
            using (var it1s = t1s.GetEnumerator())
            using (var it2s = t2s.GetEnumerator())
            using (var it3s = t3s.GetEnumerator())
            {
                while (it1s.MoveNext() && it2s.MoveNext() && it3s.MoveNext())
                    yield return Tuple.Create(it1s.Current, it2s.Current, it3s.Current);
            }
        }
    }
}
