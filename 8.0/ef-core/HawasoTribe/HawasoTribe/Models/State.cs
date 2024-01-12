namespace HawasoTribe.Models;

public partial class State
{
    public int StateId { get; set; }
    public string Name { get; set; } = null!;
}

public partial class State
{
    public ICollection<Tribe> Tribes { get; set; } 
        = new List<Tribe>();
}
