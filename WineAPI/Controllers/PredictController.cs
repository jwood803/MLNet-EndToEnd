using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using WineCommon;

namespace WineAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PredictController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PredictController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<float> Post([FromBody] WineData wineData)
        {
            var context = new MLContext();
            ITransformer model;
            var modelPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "wine.zip");

            if (!System.IO.File.Exists(modelPath))
            {
                var blob = BlobConnection.GetBlobReference(_configuration["blobConnectionString"], "models", "wine.zip");

                await blob.DownloadToFileAsync(modelPath, System.IO.FileMode.CreateNew);
            }

            using (var stream = System.IO.File.OpenRead(modelPath))
            {
                model = context.Model.Load(stream);
            }

            var predictionEngine = model.CreatePredictionEngine<WineData, WinePrediction>(context);

            var prediction = predictionEngine.Predict(wineData);

            return prediction.PredictedQuality;
        }
    }
}