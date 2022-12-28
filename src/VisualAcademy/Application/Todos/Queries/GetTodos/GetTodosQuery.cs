using Application.Common.Interfaces;

namespace Application.Todos.Queries.GetTodos
{
    public class GetTodosQuery : IGetTodosQuery
    {
        private readonly IDatabaseService _db;

        public GetTodosQuery(IDatabaseService db) => _db = db;

        public List<TodoModel> Execute()
        {
            var todos = _db.Todos.Select(x => new TodoModel
            {
                Id = x.Id,
                Name = x.Name,
                IsComplete = x.IsComplete
            });

            return todos.ToList();
        }
    }
}
