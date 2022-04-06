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
    Task<int> LoadAsync(String path);
    //fájl mentese
    //Task SaveAsync(String path, Table table, int stepCount);
    }
    
}
