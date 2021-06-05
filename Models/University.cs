using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestForGraduates.Models
{
    public class University
    {
        public int Id { get; set; }
        [DataType(DataType.Url)]
        public string UrlAdress { get; set; }
        
    }
}
