using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class WeightDl : IWeightDl
    {
        ProjectDBContext dbContext;
        public WeightDl(ProjectDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddWeight(Weight w)
        {
            await dbContext.Weights.AddAsync(w);
            return await dbContext.SaveChangesAsync();

        }
        public async Task<Dictionary<int, double?>> GetWeeklyWinnerGroup()
        {
            DateTime today = DateTime.Today;
            return await dbContext.Weights/*.Where(w => w.Date.AddDays(7) >= today)*/
                       .GroupBy(w => w.GroupId).Select(g => new { id = g.Key, s = g.Sum(u => u.Kg) }).ToDictionaryAsync(g => g.id, g => g.s);

        }
        public async Task<Dictionary<int, double?>> GetWeeklyWinner()
        {
            DateTime today = DateTime.Today;
            return await dbContext.Weights/*.Where(w => w.Date.AddDays(7) >= today)*/
                 .Select(u => new { id = u.UserId, w = u.Kg }).ToDictionaryAsync(u => u.id, u => u.w);

        }
        public async Task<Dictionary<int, double?>> GetWeeklyGroupWinner(int id)
        {
            DateTime today = DateTime.Today;
            return await dbContext.Weights.Where(w => /*w.Date.AddDays(7) >= today &&*/ w.GroupId == id)
                .Select(u => new { id = u.UserId, w = u.Kg }).ToDictionaryAsync(u => u.id, u => u.w);

        }
        public async Task<Dictionary<int, double?>> GetGroupWinner(int groupId)
        {
            Dictionary<int, double?> weights = new Dictionary<int, double?>();
            List<int> users = await this.dbContext.UserInGroups.Where(uig => uig.GroupId == groupId).Select(uig => uig.UserId).ToListAsync();
            users.ForEach(user =>
            {
                double? sum = dbContext.Weights.Where(weight => weight.UserId == user).Sum(weight => weight.Kg);
                weights.Add(user, sum);
            });
            return weights;
        }
        public async Task<List<Weight>> GetProgress(int userId, int groupId)
        {

            return await dbContext.Weights.Where(w => w.UserId == userId && w.GroupId == groupId)
                .OrderBy(w => w.Date).ToListAsync();

        }


    }
}
