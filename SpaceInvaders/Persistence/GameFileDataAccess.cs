using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Persistence;

namespace SpaceInvaders.Persistence
{
    /*public class GameFileDataAccess : IGameDataAccess
    {
        public struct Data
        {
            public Data(int t, int step)
            {
                T = t;
                Step = step;
            }

            public int T { get; set; }
            public int Step { get; set; }

            public override string ToString() => $"({T}, {Step})";
        }

        // fajl betoltese
        public async Task<Data> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync(); //
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    int populationSize = int.Parse(numbers[0]); // beolvassuk a tábla méretét
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(' ');
                    int weightsSize = int.Parse(numbers[0]); // beolvassuk a lepesek szamat méretét

                    for (int i = 0; i < populationSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');

                        for (int j = 0; j < weightsSize; j++)
                        {
                            if (int.Parse(numbers[j]) == 0)
                            {
                                //table.SetOwner(i, j, Table.Player.None);
                            }
                            else if (int.Parse(numbers[j]) == 1)
                            {
                                //table.SetOwner(i, j, Table.Player.Hunter);
                            }
                            else if (int.Parse(numbers[j]) == 2)
                            {
                                //table.SetOwner(i, j, Table.Player.Escaper);
                            }
                        }
                    }

                    Data data = new Data();
                    return data;
                }
            }
            catch
            {
                // throw new HunterDataException();
            }
        }
    }*/
}
