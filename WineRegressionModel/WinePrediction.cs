using Microsoft.ML.Data;

namespace WineRegressionModel
{
    public class WinePrediction
    {
        [ColumnName("Score")]
        public float PredictedQuality;
    }
}
