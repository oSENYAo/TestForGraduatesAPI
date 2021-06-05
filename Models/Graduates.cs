using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestForGraduates.Models
{
    public class Graduates : IdentityUser
    {
        [Required(ErrorMessage ="Введите Имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Введите Фамилию")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Введите свой город")]
        public string City { get; set; }
        [Required(ErrorMessage = "Введите название своего Учебного Заведения")]
        public string School { get; set; }

    }
}
