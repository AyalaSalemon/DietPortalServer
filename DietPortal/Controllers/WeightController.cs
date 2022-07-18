using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightController : Controller
    {
       IWeightBl wbl;
        public WeightController(IWeightBl wbl)
        {
            this.wbl = wbl;
        }
  
        [HttpPost]      
        public async Task<int> AddWeight([FromBody] Weight w)
        {
          
                return await wbl.AddWeight(w);

          

        }
        [HttpGet("WeeklyWinnerGroup")]
     
        public async Task<KeyValuePair<List<int>, double?>> GetWeeklyWinnerGroup()
        {
            KeyValuePair<List<int>, double?> l=await wbl.GetWeeklyWinnerGroup();
            return l;
        }
        [HttpGet("WeeklyWinner")]
       
        public async Task<KeyValuePair<List<int>, double?>> GetWeeklyWinner()
        {
            List<int> l;
            double? d;
            return await wbl.GetWeeklyWinner();
            
        }
        [HttpGet("User/{userId}")]

        public async Task<List<Weight>> GetProgress(int userId)
        {
            List<Weight> w = await wbl.GetProgress(userId);
            return w;

    }
}
    }
