using System;

namespace CVA_Rep_Logging
{
    public sealed class SequencerExceptionEventArgs : EventArgs
    {
        public SequencerExceptionEventArgs(SequencerException e)
        {
            Exception = e;
        }

        public SequencerException Exception { get; }
    }
}