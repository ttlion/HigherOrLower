namespace HigherOrLower.Utils.Extensions
{
    public static class RandomValuesExtensions
    {
        private static readonly Random rnd = new();
     
        // Kind of an overkill for this small project,
        // but just to show that I know extensions and generics :)
        public static T GetRandomElement<T>(this ICollection<T> collection)
        {
            return collection.ElementAt(rnd.Next(collection.Count));
        }
    }
}
