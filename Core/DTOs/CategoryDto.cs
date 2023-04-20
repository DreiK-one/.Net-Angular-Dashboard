using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string? ImageSource { get; set; }
        public int UserId { get; set; }
        public string UserAccessToken { get; set; }
    }
}
