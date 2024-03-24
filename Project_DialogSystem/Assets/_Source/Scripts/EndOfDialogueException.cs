using System;

namespace Scripts
{
    public class EndOfDialogueException : Exception
    {
        public EndOfDialogueException(string error) : base(error) { }
    }
}