namespace BrunoMikoski.TextJuicer
{
    public struct CharacterData
    {
        public float progress { get; private set; }

        private float startingTime;

        private float totalAnimationTime;
        public int order { get; }

        public CharacterData(float startTime, float targetAnimationTime, int targetOrder)
        {
            progress = 0.0f;
            startingTime = startTime;
            totalAnimationTime = (startingTime + targetAnimationTime) - startTime;
            order = targetOrder;
        }

        public void UpdateTime(float time)
        {
            if (time < startingTime)
                return;

            progress = (time - startingTime) / totalAnimationTime;
        }
    }
}