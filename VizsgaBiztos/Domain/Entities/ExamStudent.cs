using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ExamStudent //kapcsolótáblának (Junction Table)
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }

        public Exam Exam { get; set; }
        public User Student { get; set; }
    }
}
