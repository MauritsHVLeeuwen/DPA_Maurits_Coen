using System;

namespace DPA_Musicsheets.Managers
{
    public class LilypondEventArgs : EventArgs
    {
        public string LilypondText { get; set; }
        public string Message { get; set; }
    }
}