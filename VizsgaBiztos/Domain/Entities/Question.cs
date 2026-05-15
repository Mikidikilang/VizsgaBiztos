using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int SheetId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }//baszta az ocd-met
        public Sheet Sheet { get; set; }
    }
}
