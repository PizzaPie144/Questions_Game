using System.Collections;
using System.Collections.Generic;
using System;

namespace PizzaPie
{
    public class BeforeLoadEventArgs : EventArgs
    {
        public PizzaPie.Unity.Utils.SequenceLoader SequenceLoader { get; private set; }
        public GameDifficulty Difficulty;

        public BeforeLoadEventArgs(Unity.Utils.SequenceLoader sequenceLaoder, GameDifficulty difficulty)
        {
            SequenceLoader = sequenceLaoder;
            Difficulty = difficulty;
        }
    }
}