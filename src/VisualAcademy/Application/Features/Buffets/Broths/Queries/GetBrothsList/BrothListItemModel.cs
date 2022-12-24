namespace Application.Features.Buffets.Broths.Queries.GetBrothsList
{
    public class BrothListItemModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsVegan { get; set; }
    }
}
