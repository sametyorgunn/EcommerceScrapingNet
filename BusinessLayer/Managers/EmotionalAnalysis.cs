using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    public class EmotionalAnalysis : IEmotinalAnalysis
    {
        public async Task<List<SentimentPredictionDto>> GetEmotionalAnalysis(List<CommentEmotionDto> comments)
        {
            var context = new MLContext();
            var dataPath = "C:\\Users\\asame\\OneDrive\\Masaüstü\\projects\\EcommerceScrapingNet\\UI\\analyse.csv";
            var data = context.Data.LoadFromTextFile<CommentAnalysisDto>(dataPath, separatorChar: '~', hasHeader: true);
			//var pipeline = context.Transforms.Text.FeaturizeText("Features", "CommentText")
			//    .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression("Label", "CommentText"));
			var pipeline = context.Transforms.Text.FeaturizeText("Features", "CommentText")
	.Append(context.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

			var trainTestData = context.Data.TrainTestSplit(data, testFraction: 0.2);
            var model = pipeline.Fit(trainTestData.TrainSet);
            var metrics = context.BinaryClassification.Evaluate(model.Transform(trainTestData.TestSet), "Label");
            //Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            //Console.WriteLine($"F1 Score: {metrics.F1Score:P2}");
            //Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:P2}");
            var testDataView = context.Data.LoadFromEnumerable(comments);
            var predictions = model.Transform(testDataView);
            var results = context.Data.CreateEnumerable<SentimentPredictionDto>(predictions, reuseRowObject: false);
            return results.ToList();
        }
    }
}
