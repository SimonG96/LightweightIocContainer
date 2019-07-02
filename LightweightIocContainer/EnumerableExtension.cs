// // Author: Gockner, Simon
// // Created: 2019-07-02
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace LightweightIocContainer
{
    internal static class EnumerableExtension
    {
        /// <summary>
        /// Returns the first element of a <see cref="IEnumerable{T}"/>, or a new instance of a given <see cref="Type"/> if the <see cref="IEnumerable{T}"/> contains no elements
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="Type"/> of the <see cref="IEnumerable{T}"/></typeparam>
        /// <typeparam name="TGiven">The given <see cref="Type"/> to return if the <see cref="IEnumerable{T}"/> contains no elements</typeparam>
        /// <param name="source">The given <see cref="IEnumerable{T}"/></param>
        /// <returns>The first element of the <see cref="IEnumerable{T}"/>, or a new instance of a given <see cref="Type"/> if the <see cref="IEnumerable{T}"/> contains no elements</returns>
        public static TSource FirstOrGiven<TSource, TGiven>(this IEnumerable<TSource> source) where TGiven : TSource, new() =>
            source.TryGetFirst<TSource, TGiven>(null);

        /// <summary>
        /// Returns the first element of a <see cref="IEnumerable{T}"/> that satisfies a condition, or a new instance of a given <see cref="Type"/> if no such element is found
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="Type"/> of the <see cref="IEnumerable{T}"/></typeparam>
        /// <typeparam name="TGiven">The given <see cref="Type"/> to return if the <see cref="IEnumerable{T}"/> contains no element that satisfies the given condition</typeparam>
        /// <param name="source">The given <see cref="IEnumerable{T}"/></param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>The first element of the <see cref="IEnumerable{T}"/> that satisfies a condition, or a new instance of the given <see cref="Type"/> if no such element is found</returns>
        public static TSource FirstOrGiven<TSource, TGiven>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) where TGiven : TSource, new() =>
            source.TryGetFirst<TSource, TGiven>(predicate);

        /// <summary>
        /// Tries to get the first element of the given <see cref="IEnumerable{T}"/> or creates a new element of a given <see cref="Type"/> when no element is found
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="Type"/> of the <see cref="IEnumerable{T}"/></typeparam>
        /// <typeparam name="TGiven">The given <see cref="Type"/> to create a new element when no fitting element is found</typeparam>
        /// <param name="source">The given <see cref="IEnumerable{T}"/></param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>The first element of the <see cref="IEnumerable{T}"/> or a new instance of the given <see cref="Type"/> when no element is found</returns>
        private static TSource TryGetFirst<TSource, TGiven>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) where TGiven : TSource, new()
        {
            try
            {
                TSource first;
                if (predicate == null)
                    first = source.First();
                else
                    first = source.First(predicate);

                return first;
            }
            catch (Exception)
            {
                return new TGiven();
            }
        }
    }
}