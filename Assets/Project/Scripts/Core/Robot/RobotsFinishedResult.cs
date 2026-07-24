namespace Project.Core.Robot
{
    public struct RobotsFinishedResult
    {
        public readonly int successAmount;
        public readonly int fullAmount;

        public RobotsFinishedResult(int successAmount, int fullAmount)
        {
            this.successAmount = successAmount;
            this.fullAmount = fullAmount;
        }
    }
}