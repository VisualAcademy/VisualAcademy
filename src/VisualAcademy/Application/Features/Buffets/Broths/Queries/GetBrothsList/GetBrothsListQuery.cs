using Application.Common.Interfaces.Buffets;

namespace Application.Features.Buffets.Broths.Queries.GetBrothsList;

public class GetBrothsListQuery : IGetBrothsListQuery
{
    private readonly IBuffetDatabaseService _db;

    public GetBrothsListQuery(IBuffetDatabaseService db)
    {
        _db = db;
    }

    public List<BrothListItemModel> Execute()
    {
        var broths = _db.Broths.Select(b => new BrothListItemModel 
        { 
            Id = b.Id, 
            Name = b.Name, 
            IsVegan = b.IsVegan 
        });

        return broths.ToList();
    }
}
