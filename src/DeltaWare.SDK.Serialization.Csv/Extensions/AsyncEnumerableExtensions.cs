using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv.Extensions
{
    internal static class AsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken = default)
        {
            List<T> values = new List<T>();

            await foreach (var value in enumerable.WithCancellation(cancellationToken))
            {
                values.Add(value);
            }

            return values;
        }
    }
}
