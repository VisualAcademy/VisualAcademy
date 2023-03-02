using System.Text.Json;
using System.Text.Json.Serialization;

namespace RedPlus.Models
{
    /// <summary>
    /// 모델 클래스: Model, ViewModel, Dto, Object, Entity, ...
    /// </summary>
    public class Portfolio
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        [JsonPropertyName("img")]
        public string Image { get; set; }

        public int[] Ratings { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Portfolio>(this);
        }
    }
}
