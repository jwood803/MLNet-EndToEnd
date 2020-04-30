using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ML;
using Microsoft.ML;
using WineCommon;

namespace WineAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly PredictionEnginePool<WineData, WinePrediction> _predictionEnginePool;

        public ModelController(PredictionEnginePool<WineData, WinePrediction> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        [HttpPost]
        public float Post([FromBody] WineData wineData)
        {
            var prediction = _predictionEnginePool.Predict(wineData);

            return prediction.PredictedQuality;
        }

        [HttpGet]
        public string Get()
        {
            return "Working";
        }
    }
}
