using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Model;

namespace SpaceInvaders.Persistence
{
    public interface IGameDataAccess
    {
        //fájl betoltese
    Task<Data> LoadAsync(String path);
    //fájl mentese
    Task SaveAsync(String path,int evolutionType, int round, int populationSize, int weightsSize, double [,] weights, double[] individualFittnes, double learningTime, int[] individualScore, double [] evolutionParameters);
    }
    
}
