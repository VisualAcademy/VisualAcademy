namespace Application.Features.Buffets.Broths.Queries.GetBrothsList;

public interface IGetBrothsListQuery
{
    List<BrothListItemModel> Execute();
}
