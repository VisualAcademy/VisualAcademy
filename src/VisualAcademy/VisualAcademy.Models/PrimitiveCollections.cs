namespace VisualAcademy.Models
{
    public class PrimitiveCollections
    {
        public IList<DateOnly> Dates { get; set; } = new List<DateOnly>();
        public uint[] UnsignedInts { get; set; } = Array.Empty<uint>();
        public List<bool> Booleans { get; set; } = new();
        public List<Uri> Urls { get; set; } = new();
        public IEnumerable<int> Ints { get; set; } = new List<int>();
        public ICollection<string> Strings { get; set; } = new List<string>();
    }
}
