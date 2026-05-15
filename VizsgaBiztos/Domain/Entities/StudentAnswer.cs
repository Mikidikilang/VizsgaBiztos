using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public int ExamResultId { get; set; }
        public int QuestionId { get; set; }
        public string GivenAnswer { get; set; }
        public bool IsCorrect { get; set; }

        public ExamResult ExamResult { get; set; }
        public Question Question { get; set; }
    }
}
