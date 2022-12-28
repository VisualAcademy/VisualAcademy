namespace Application.Todos.Queries.GetTodos
{
    public interface IGetTodosQuery
    {
        List<TodoModel> Execute();
    }
}