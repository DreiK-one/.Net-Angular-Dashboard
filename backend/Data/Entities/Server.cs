using System.ComponentModel.DataAnnotations;

namespace backend.Data.Entities
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }

    }
}
