using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Sheet
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User CreatedByUser { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
