using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface IEmotinalAnalysis
    {
        public Task<List<SentimentPredictionDto>> GetEmotionalAnalysis(List<Comment>comments);
    }
}
