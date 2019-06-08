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
        private readonly string _modelPath;
        ITransformer _model;
        MLContext _context;

        public ModelController(IConfiguration configuration)
        {
            _configuration = configuration;

            _context = new MLContext();
            _modelPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "wine.zip");
        }

        [HttpPost]
        public async Task<float> Post([FromBody] WineData wineData)
        {
            if (!System.IO.File.Exists(_modelPath))
            {
                var blob = BlobConnection.GetBlobReference(_configuration["blobConnectionString"], "model", "wine.zip");

                await blob.DownloadToFileAsync(_modelPath, System.IO.FileMode.CreateNew);
            }

            using (var stream = System.IO.File.OpenRead(_modelPath))
            {
                _model = _context.Model.Load(stream);
            }

            var predictionEngine = _model.CreatePredictionEngine<WineData, WinePrediction>(_context);

            var prediction = predictionEngine.Predict(wineData);

            return prediction.PredictedQuality;
        }
    }
}