using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entities.DTOs
{
    public class CategoryAddDto
    {
        [DisplayName("Kategory Adı")]
        [Required(ErrorMessage ="{0} boş geçilemez.")]
        [MaxLength(70,ErrorMessage ="{0} {1} karakterden büyük olamaz.")]
        [MinLength(3,ErrorMessage ="{0} {1} karakterden az olmamalıdır.")]
        public string  Name { get; set; }

        [DisplayName("Kategory Açıklaması")]
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden büyük olamaz.")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string Description { get; set; }

        [DisplayName("Kategory Not Alanı")]
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden büyük olamaz.")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string Note { get; set; }

        [DisplayName("Aktif mi?")]
        [Required(ErrorMessage = "{0} boş geçilemez.")]
        public bool IsActive { get; set; }
    }
}
