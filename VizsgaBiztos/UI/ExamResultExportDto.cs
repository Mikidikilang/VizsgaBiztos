using System;
using System.Xml.Serialization;

namespace Application.DTOs
{
    /// <summary>
    /// DTO osztály az XML exportáláshoz
    /// </summary>
    [XmlType("ExamResult")]
    public class ExamResultExportDto
    {
        [XmlElement("StudentName")]
        public string StudentName { get; set; } = null!;

        [XmlElement("CorrectAnswers")]
        public int CorrectAnswers { get; set; }

        [XmlElement("TotalQuestions")]
        public int TotalQuestions { get; set; }

        [XmlElement("PercentageScore")]
        public decimal PercentageScore { get; set; }

        [XmlElement("SubmittedAt")]
        public DateTime SubmittedAt { get; set; }
    }
}
