namespace VisualAcademy.Models
{
    public class PrimitiveCollections
    {
        public IList<DateOnly> Dates { get; set; }
        public uint[] UnsignedInts { get; set; }
        public List<bool> Booleans { get; set; }
        public List<Uri> Urls { get; set; }
        public IEnumerable<int> Ints { get; set; } // Must be an IList<int>() at runtime.
        public ICollection<string> Strings { get; set; } // Must be an IList<int>() at runtime.
    }
}
