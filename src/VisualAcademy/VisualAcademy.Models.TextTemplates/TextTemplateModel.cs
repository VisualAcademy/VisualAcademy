using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VisualAcademy.Models.TextTemplates
{
    /// <summary>
    /// 테이블과 일대일로 매핑되는 모델 클래스: TextTemplate, TextTemplateModel, ...
    /// </summary>
    [Table("TextTemplates")]
    public class TextTemplateModel
    {
        /// <summary>
        /// 텍스트템플릿 고유 아이디, 자동 증가
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        ///// <summary>
        ///// 활성 상태 표시, 기본값 true (활성)
        ///// </summary>
        //public bool? Active { get; set; }

        ///// <summary>
        ///// 레코드 생성 시간
        ///// </summary>
        //public DateTimeOffset CreatedAt { get; set; }

        ///// <summary>
        ///// 레코드 생성자 이름
        ///// </summary>
        //public string? CreatedBy { get; set; }

        ///// <summary>
        ///// 텍스트템플릿명
        ///// </summary>
        //public string? Name { get; set; }
    }
}
