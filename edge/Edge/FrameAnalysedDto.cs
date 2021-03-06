namespace SaigeVAD.Edge
{
    using System.Drawing;
    public class FrameAnalysedDto
    {
        public double NormalizedScore { get; set; }

        public double Threshold { get; set; }

        public int PhaseIndex { get; set; }

        public bool IsInspected { get; set; }

        public bool IsNormal { get; set; }

        public bool IsMotion { get; set; }

        public bool IsOverwrittenByVcls { get; set; }

        public int VclsClassIndex { get; set; }

        public long TimeStamp { get; set; }

        public double BaseScore { get; set; }

        public double AdaptiveScore { get; set; }

        public Rectangle Roi { get; set; }

        public double BaseThreshold { get; set; }

        public double SlowThreshold { get; set; }

        public double SlowMean { get; set; }

        public double SlowVar { get; set; }

        public double FastThreshold { get; set; }

        public double FastMean { get; set; }

        public double FastVar { get; set; }

        public int ClusterIndex { get; set; }

        public int AdaptationStep { get; set; }

        public int FreezeThresholdCount { get; set; }

        public double HardExampleMiningScore { get; set; }

        public int ErrorCode { get; set; }
    }
}
