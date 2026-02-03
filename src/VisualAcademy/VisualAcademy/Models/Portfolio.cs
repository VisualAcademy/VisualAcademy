using System.Text.Json;
using System.Text.Json.Serialization;

namespace VisualAcademy.Models
{
    /// <summary>
    /// 모델 클래스: Model, ViewModel, Dto, Object, Entity, ...
    /// </summary>
    public class Portfolio
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }

        [JsonPropertyName("img")]
        public required string Image { get; set; }

        public int[] Ratings { get; set; } = Array.Empty<int>();

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
