using Microsoft.ML.Data;

namespace WineCommon
{
    public class WinePrediction
    {
        [ColumnName("Score")]
        public float PredictedQuality;
    }
}
