﻿namespace WaZuF.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;

        public char CorrectAnswer { get; set; } // 'A' أو 'B' أو 'C' أو 'D'

        public int JobRequestId { get; set; }
        public JobRequest JobRequest { get; set; } = null!;
    }

}
