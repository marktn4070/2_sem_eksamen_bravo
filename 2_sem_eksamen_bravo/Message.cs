﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_sem_eksamen_bravo
{
    public class Message
    {
        public string MessageID { get; set; }
        public string Headline { get; set; }
        public string Subheadline { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public bool Email { get; set; }
        public bool Sms { get; set; }
    }
}