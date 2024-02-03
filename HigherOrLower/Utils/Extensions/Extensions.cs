using Newtonsoft.Json;

namespace HigherOrLower.Utils.Extensions
{
    public static class Extensions
    {
        private static readonly Random rnd = new();
     
        // Kind of an overkill for this small project,
        // but just to show that I know extensions and generics :)
        public static T GetRandomElement<T>(this ICollection<T> list)
            => list.ElementAt(rnd.Next(list.Count));

        public static string ToJsonWithMessage(this string message)
            => JsonConvert.SerializeObject(new { Message = message }, Formatting.Indented);
    }
}
