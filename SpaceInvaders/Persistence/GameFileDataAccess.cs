using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Persistence;

namespace SpaceInvaders.Persistence
{
    public struct Data
    {
        public Data(int round ,int populationSize, int weightsSize, double [,] weights)
        {
            _round = round;
            _populationSize = populationSize;
            _weightsSize = weightsSize;
            _weights = weights;
        }

        public int _round { get; set; }
        public int _populationSize { get; set; }
        public int _weightsSize { get; set; }
        public double[,] _weights { get; set; }

        public override string ToString() => $"({_round},{_populationSize}, {_weightsSize}, {_weights})";
    }
    public class GameFileDataAccess : IGameDataAccess
    {

        // fajl betoltese
        public async Task<Data> LoadAsync(String path)
        {
            //try
            //{
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync(); //
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    int round = int.Parse(numbers[0]); // beolvassuk a korok szamat
                    line = await reader.ReadLineAsync(); //
                    numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    int populationSize = int.Parse(numbers[0]); // beolvassuk az egyedek szamat
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(' ');
                    int weightsSize = int.Parse(numbers[0]); // beolvassuk a sulyok szamat
                    double[,] weights = new double[populationSize, weightsSize];    

                    for (int i = 0; i < populationSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');

                        for (int j = 0; j < weightsSize; j++)
                        {
                            weights[i, j] = double.Parse(numbers[j]);
                        }
                    }
                    Data data=new Data(round, populationSize,weightsSize,weights);
                    return data;
                }
            //}
            //catch
            //{
                // throw new GameDataException();
            //}
        }

        // Mentes
        public async Task SaveAsync(String path, int round, int populationSize, int weightsSize, double[,] weights)
        {
            //try
            //{
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    
                    writer.Write(round); // kiírjuk a korok szamat
                    await writer.WriteLineAsync(); //uj sor
                    writer.Write(populationSize); // kiírjuk az egyedek szamat
                    await writer.WriteLineAsync(); //uj sor
                    writer.Write(weightsSize); // kiírjuk a sulyok szamat
                    await writer.WriteLineAsync(); //uj sor

                    for (Int32 i = 0; i < populationSize; i++)
                    {
                        for (Int32 j = 0; j < weightsSize; j++)
                        {
                            await writer.WriteAsync(weights[i,j] + " ");
                        }
                        await writer.WriteLineAsync();
                    }

                }
            //}
            /*catch
            {
                throw new GameDataException();
            }*/
        }
    }
}
