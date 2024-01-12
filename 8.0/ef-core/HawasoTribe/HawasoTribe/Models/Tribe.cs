namespace HawasoTribe.Models;

public partial class Tribe
{
    public int TribeId { get; set; }
    public string Name { get; set; } = null!;
    public int StateId { get; set; }
}

public partial class Tribe
{
    public State State { get; set; } = null!;
}
