using System;
using System.ComponentModel.DataAnnotations;

namespace VisualAcademy.Models
{
    public class TextMessage
    {
        public long Id { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTimeOffset DateSent { get; set; } = DateTimeOffset.Now;

        public int? TextMessageType { get; set; }
    }
}
