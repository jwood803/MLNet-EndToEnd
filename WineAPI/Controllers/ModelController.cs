using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using WineCommon;

namespace WineAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        ITransformer _model;
        MLContext _context;

        public ModelController(IConfiguration configuration)
        {
            _configuration = configuration;

            _context = new MLContext();
            var modelPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "wine.zip");

            if (!System.IO.File.Exists(modelPath))
            {
                var blob = BlobConnection.GetBlobReference(_configuration["blobConnectionString"], "models", "wine.zip");

                blob.DownloadToFileAsync(modelPath, System.IO.FileMode.CreateNew).RunSynchronously();
            }

            using (var stream = System.IO.File.OpenRead(modelPath))
            {
                _model = _context.Model.Load(stream);
            }
        }

        [HttpPost]
        public float Post([FromBody] WineData wineData)
        {
            var predictionEngine = _model.CreatePredictionEngine<WineData, WinePrediction>(_context);

            var prediction = predictionEngine.Predict(wineData);

            return prediction.PredictedQuality;
        }
    }
}
