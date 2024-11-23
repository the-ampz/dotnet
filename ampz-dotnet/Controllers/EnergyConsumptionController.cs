using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using System.IO;
using ampz_dotnet.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.ML.Data;
using ampz_dotnet.Model;

public class EnergyConsumptionData
{
    [LoadColumn(0)]
    public float HoursUsed { get; set; }

    [LoadColumn(1)]
    public float ConsumptionValue { get; set; }

    [LoadColumn(2)]
    public float EnergySaved { get; set; }
}

public class EnergyConsumptionInput
{
    public int DeviceId { get; set; }
    public float HoursUsed { get; set; }
}

public class EnergySavedPrediction
{
    [ColumnName("Score")]
    public float PredictedEnergySaved { get; set; }
}

[Route("api/[controller]")]
[SwaggerTag("Controller de Consumo de Energia")]
[ApiController]
public class EnergyConsumptionController : ControllerBase
{
    public string ModelPath { get; set; }
    public string TrainingDataPath { get; set; }

    private readonly MLContext _mlContext;
    private readonly AppDbContext _context;

    public EnergyConsumptionController(AppDbContext context)
    {
        _mlContext = new MLContext();
        _context = context;

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        ModelPath = Path.Combine(baseDirectory, "MLModels", "EnergyConsumptionModel.zip");
        TrainingDataPath = Path.Combine(baseDirectory, "Datasets", "energy_consumption_training.csv");

        EnsureModelExists();
    }

    private void EnsureModelExists()
    {
        var modelDirectory = Path.GetDirectoryName(ModelPath);

        try
        {
            if (!string.IsNullOrWhiteSpace(modelDirectory) && !Directory.Exists(modelDirectory))
            {
                Directory.CreateDirectory(modelDirectory);
            }

            if (!System.IO.File.Exists(ModelPath))
            {
                if (!System.IO.File.Exists(TrainingDataPath))
                {
                    throw new FileNotFoundException($"O arquivo de treinamento não foi encontrado: {TrainingDataPath}");
                }

                TrainModel();
            }
        }
        catch (Exception ex)
        {
            throw new IOException($"Erro ao garantir a existência do modelo: {ex.Message}", ex);
        }
    }

    private void TrainModel()
    {
        try
        {
            var trainingData = _mlContext.Data.LoadFromTextFile<EnergyConsumptionData>(
                TrainingDataPath, hasHeader: true, separatorChar: ',');

            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(EnergyConsumptionData.HoursUsed), nameof(EnergyConsumptionData.ConsumptionValue))
                .Append(_mlContext.Regression.Trainers.FastTree(labelColumnName: nameof(EnergyConsumptionData.EnergySaved)));

            var model = pipeline.Fit(trainingData);

            var modelDirectory = Path.GetDirectoryName(ModelPath);
            if (!string.IsNullOrWhiteSpace(modelDirectory) && !Directory.Exists(modelDirectory))
            {
                Directory.CreateDirectory(modelDirectory);
            }

            _mlContext.Model.Save(model, trainingData.Schema, ModelPath);

            if (!System.IO.File.Exists(ModelPath))
            {
                throw new IOException($"Falha ao salvar o modelo em {ModelPath}");
            }
        }
        catch (Exception ex)
        {
            throw new IOException($"Erro ao treinar e salvar o modelo: {ex.Message}", ex);
        }
    }

    [HttpPost("predict")]
    [SwaggerOperation(Summary = "Estime sua economia de energia com base no dispositivo e horas de uso")]
    public async Task<ActionResult<string>> PredictEnergySaved([FromBody] EnergyConsumptionInput input)
    {
        var device = await _context.Devices.Include(d => d.Kid).FirstOrDefaultAsync(d => d.Id == input.DeviceId);
        if (device == null) return NotFound("Dispositivo não encontrado.");

        var kid = device.Kid;
        if (kid == null) return NotFound("Nenhuma criança associada ao dispositivo encontrado.");

        var prediction = PredictEnergy(new EnergyConsumptionData
        {
            HoursUsed = input.HoursUsed,
            ConsumptionValue = input.HoursUsed
        });

        var formattedEnergySaved = (float)Math.Round(prediction.PredictedEnergySaved, 2);

        kid.TotalEnergySaved += (decimal)formattedEnergySaved;
        kid.TotalScore += CalculateScore(formattedEnergySaved);

        _context.Kids.Update(kid);
        await _context.SaveChangesAsync();

        return Ok(GenerateResponseMessage(input.HoursUsed, formattedEnergySaved, kid));
    }

    private int CalculateScore(float energySaved) =>
        energySaved >= 1 ? 10 : energySaved >= 0.5 ? 5 : 1;

    private EnergySavedPrediction PredictEnergy(EnergyConsumptionData input)
    {
        if (!System.IO.File.Exists(ModelPath))
        {
            throw new FileNotFoundException($"Modelo não encontrado em {ModelPath}");
        }

        using var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var model = _mlContext.Model.Load(stream, out _);
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<EnergyConsumptionData, EnergySavedPrediction>(model);

        return predictionEngine.Predict(input);
    }

    private string GenerateResponseMessage(float hoursUsed, float energySaved, Kid kid)
    {
        if (hoursUsed > 8 && energySaved <= 0.2)
        {
            return $"Atenção! O dispositivo ficou ligado por muito tempo e economizou pouca energia ({energySaved} kWh). Até agora, você economizou um total de {kid.TotalEnergySaved} kWh e sua pontuação atual é {kid.TotalScore}.";
        }
        else if (hoursUsed <= 4 && energySaved > 0.5)
        {
            return $"Incrível! Você conseguiu economizar bastante energia: {energySaved} kWh. No total, você já economizou {kid.TotalEnergySaved} kWh e sua pontuação é {kid.TotalScore}. Continue assim!";
        }
        return $"Ótimo trabalho! Você economizou {energySaved} kWh desta vez. Acumulou um total de {kid.TotalEnergySaved} kWh economizados e sua pontuação subiu para {kid.TotalScore}. Continue se desafiando!";
    }
}
