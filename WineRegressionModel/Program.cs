using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WineCommon;

namespace WineRegressionModel
{
    class Program
    {
        private static string _sqlConnectionString;
        private static readonly string fileName = "wine.zip";

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json");

            var configuration = builder.Build();
            _sqlConnectionString = configuration["connectionString"];

            var fileData = ReadFromFile("./winequality.csv");

            AddDataToDatabase(fileData);

            var dbData = ReadFromDatabase();

            var context = new MLContext();

            var mlData = context.Data.LoadFromEnumerable(dbData);

            var trainTestData = context.Regression.TrainTestSplit(mlData, testFraction: 0.2);

            var dataPreview = trainTestData.TrainSet.Preview();
            
            var pipeline = context.Transforms.Categorical.OneHotEncoding("TypeOneHot", "Type")
                .Append(context.Transforms.Concatenate("Features", "FixedAcidity", "VolatileAcidity", "CitricAcid",
                    "ResidualSugar", "Chlorides", "FreeSulfurDioxide", "TotalSulfurDioxide", "Density", "Ph", "Sulphates",
                    "Alcohol"))
                .Append(context.Transforms.CopyColumns(("Label", "Quality")))
                .Append(context.Regression.Trainers.FastTree());

            var model = pipeline.Fit(trainTestData.TrainSet);

            var blob = BlobConnection.GetBlobReference(configuration["blobConnectionString"], "models", fileName);

            using (var stream = File.Create(fileName))
            {
                context.Model.Save(model, stream);
            }

            await blob.UploadFromFileAsync(fileName);
        }

        private static IEnumerable<WineData> ReadFromDatabase()
        {
            var data = new List<WineData>();

            using (var conn = new SqlConnection(_sqlConnectionString))
            {
                conn.Open();

                var selectCommand = "SELECT * FROM mlnetExample.dbo.WineData";

                var sqlCommand = new SqlCommand(selectCommand, conn);

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    data.Add(new WineData
                    {
                        Type = reader.GetValue(0).ToString(),
                        FixedAcidity = Parse(reader.GetValue(1).ToString()),
                        VolatileAcidity = Parse(reader.GetValue(2).ToString()),
                        CitricAcid = Parse(reader.GetValue(3).ToString()),
                        ResidualSugar = Parse(reader.GetValue(4).ToString()),
                        Chlorides = Parse(reader.GetValue(5).ToString()),
                        FreeSulfurDioxide = Parse(reader.GetValue(6).ToString()),
                        TotalSulfurDioxide = Parse(reader.GetValue(7).ToString()),
                        Density = Parse(reader.GetValue(8).ToString()),
                        Ph = Parse(reader.GetValue(9).ToString()),
                        Sulphates = Parse(reader.GetValue(10).ToString()),
                        Alcohol = Parse(reader.GetValue(11).ToString()),
                        Quality = Parse(reader.GetValue(12).ToString())
                    });
                }
            }

            return data;
        }

        private static void AddDataToDatabase(IEnumerable<WineData> fileData)
        {
            using (var conn = new SqlConnection(_sqlConnectionString))
            {
                conn.Open();

                var insertCommand = @"INSERT INTO winedata.dbo.WineData VALUES 
                    (@type, @fixedAcidity, @volatileAcidity, @citricAcid, @residualSugar, @chlorides,
                     @freeSulfureDioxide, @totalSulfurDioxide, @density, @ph, @sulphates, @alcohol, @quality);";

                var selectCommand = "SELECT COUNT(*) From mlnetExample.dbo.WineData";

                var selectSqlCommand = new SqlCommand(selectCommand, conn);

                var results = (int)selectSqlCommand.ExecuteScalar();

                if (results > 0)
                {
                    var deleteCommand = "DELETE FROM mlnetExample.dbo.WineData";

                    var deleteSqlCommand = new SqlCommand(deleteCommand, conn);

                    deleteSqlCommand.ExecuteNonQuery();
                }

                foreach (var item in fileData)
                {
                    var command = new SqlCommand(insertCommand, conn);

                    command.Parameters.AddWithValue("@type", item.Type);
                    command.Parameters.AddWithValue("@fixedAcidity", item.FixedAcidity);
                    command.Parameters.AddWithValue("@volatileAcidity", item.VolatileAcidity);
                    command.Parameters.AddWithValue("@citricAcid", item.CitricAcid);
                    command.Parameters.AddWithValue("@residualSugar", item.ResidualSugar);
                    command.Parameters.AddWithValue("@chlorides", item.Chlorides);
                    command.Parameters.AddWithValue("@freeSulfureDioxide", item.FreeSulfurDioxide);
                    command.Parameters.AddWithValue("@totalSulfurDioxide", item.TotalSulfurDioxide);
                    command.Parameters.AddWithValue("@density", item.Density);
                    command.Parameters.AddWithValue("@ph", item.Ph);
                    command.Parameters.AddWithValue("@sulphates", item.Sulphates);
                    command.Parameters.AddWithValue("@alcohol", item.Alcohol);
                    command.Parameters.AddWithValue("@quality", item.Quality);

                    command.ExecuteNonQuery();
                }
            }
        }

        private static IEnumerable<WineData> ReadFromFile(string filePath)
        {
            var items = File.ReadAllLines(filePath)
                .Skip(1)
                .Select(line => line.Split(","))
                .Select(i => new WineData
                {
                    Type = i[0],
                    FixedAcidity = Parse(i[1]),
                    VolatileAcidity = Parse(i[2]),
                    CitricAcid = Parse(i[3]),
                    ResidualSugar = Parse(i[4]),
                    Chlorides = Parse(i[5]),
                    FreeSulfurDioxide = Parse(i[6]),
                    TotalSulfurDioxide = Parse(i[7]),
                    Density = Parse(i[8]),
                    Ph = Parse(i[9]),
                    Sulphates = Parse(i[10]),
                    Alcohol = Parse(i[11]),
                    Quality = Parse(i[12])
                });

            return items;
        }

        private static float Parse(string value)
        {
            return float.TryParse(value, out float prasedValue) ? prasedValue : default;
        }
    }
}
