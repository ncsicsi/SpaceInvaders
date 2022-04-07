using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Persistence
{
    public interface IGameDataAccess
    {
        //fájl betoltese
    Task<Data> LoadAsync(String path);
    //fájl mentese
    Task SaveAsync(String path, int round, int populationSize, int weightsSize, double [,] weights);
    }
    
}
