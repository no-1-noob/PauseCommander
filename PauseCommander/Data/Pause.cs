namespace PauseCommander.Data
{
    internal class Pause
    {
        private readonly float start;
        private readonly float length;
        private readonly int noteCount;

        public Pause(float start, float length, int noteCount)
        {
            this.start = start;
            this.length = length;
            this.noteCount = noteCount;
        }

        public float Start { get => start; }
        public float Length { get => length; }
        public int NoteCount { get => noteCount;}
    }
}
