using Dul.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleApp.Models
{
    /// <summary>
    /// [2] Model Class: Article 모델 클래스 == Articles 테이블과 일대일로 매핑 
    /// Article, ArticleModel, ArticleViewModel, ArticleBase, ArticleDto, ArticleEntity, ArticleObject, ...
    /// </summary>
    [Table("Articles")]
    public class Article : AuditableBase
    {
        /// <summary>
        /// 일련 번호(Serial Number)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        //[Required]
        [MaxLength(255)]
        [Required(ErrorMessage = "제목을 입력하세요.")]
        public string Title { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        [Required(ErrorMessage = "내용을 입력하세요.")]
        public string Content { get; set; }

        /// <summary>
        /// 공지글로 올리기
        /// </summary>
        public bool IsPinned { get; set; } = false; 
    }
}
