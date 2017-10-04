using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Managers
{
    public class WPFStaffsEventArgs : EventArgs
    {
        public IEnumerable<MusicalSymbol> Symbols { get; set; }
        public string Message { get; set; }
    }
}