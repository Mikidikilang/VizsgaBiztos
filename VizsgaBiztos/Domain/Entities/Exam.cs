using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public int SheetId { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int CreatedByUserId { get; set; }

        public Sheet Sheet { get; set; }
        public User CreatedByUser { get; set; }
        public ICollection<ExamStudent> ExamStudents { get; set; }
    }
}
