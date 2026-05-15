using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ExamResult
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal Score { get; set; }
        public DateTime SubmittedAt { get; set; }

        public Exam Exam { get; set; }
        public User Student { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
