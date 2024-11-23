using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Hosting;
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
        public async Task<List<SentimentPredictionDto>> GetEmotionalAnalysis(List<CommentDto> comments)
        {
            try
            {
                var context = new MLContext();
                var dataPath = "wwwroot\\analyse.csv";
                var data = context.Data.LoadFromTextFile<CommentAnalysisDto>(dataPath, separatorChar: '~', hasHeader: true);
				var pipeline = context.Transforms.Text.FeaturizeText("Features", "CommentText")
	            .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
				var trainTestData = context.Data.TrainTestSplit(data, testFraction: 0.5);
                var model = pipeline.Fit(trainTestData.TrainSet);
                var testDataView = context.Data.LoadFromEnumerable(comments);
                var predictions = model.Transform(testDataView);
				//var results = context.Data.CreateEnumerable<SentimentPredictionDto>(predictions, reuseRowObject: false);
				var results = context.Data.CreateEnumerable<SentimentPredictionDto>(predictions, reuseRowObject: false)
	            .Select(result => {
		            result.Prediction = result.Score >= -1; 
		            return result;
	            }).ToList();

                return results.ToList();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<SentimentPredictionDto>() { };
            }
        }
    }
}
