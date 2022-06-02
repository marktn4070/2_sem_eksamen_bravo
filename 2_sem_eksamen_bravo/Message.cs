using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_sem_eksamen_bravo
{
    #region Coded by Mark
    public class Message
    {
        public int MessageID { get; set; }
        public string Headline { get; set; }
        public string Subheadline { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public bool Email { get; set; }
        public bool Sms { get; set; }
    }
    #endregion
}
