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
        public Data(int evolutionType, int round ,int populationSize, int weightsSize, double [,] weights, double [] individualFittnes, double learningTime, int[] individualScore, double[] evolutionParameters)
        {
            _round = round;
            _populationSize = populationSize;
            _weightsSize = weightsSize;
            _weights = weights;
            _individualFittnes = individualFittnes;
            _learningTime = learningTime;
            _individualScore = individualScore;
            _evolutionType = evolutionType;
            _evolutionParameters=evolutionParameters;
        }

        public int _round { get; set; }
        public int _populationSize { get; set; }
        public int _weightsSize { get; set; }
        public double[,] _weights { get; set; }
        public double[] _individualFittnes { get; set; }
        public double _learningTime { get; set; }
        public int[] _individualScore { get; set; }
        public int _evolutionType { get; set; }
        public double[] _evolutionParameters { get; set; }

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
                    int evolutionType;
                    if(numbers[0]== "Simple")
                    {
                        evolutionType = 0;
                    }
                    else
                    {
                        evolutionType = 1;
                    }
                    double[] evolutionParameters = new double[6];
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(' ');
                    for (int i = 0; i < 6; i++)
                    {
                        evolutionParameters[i] = double.Parse(numbers[i]);
                    }
                    line = await reader.ReadLineAsync(); //
                    numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    int round = int.Parse(numbers[0]); // beolvassuk a korok szamat
                    line = await reader.ReadLineAsync(); //
                    numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    int populationSize = int.Parse(numbers[0]); // beolvassuk az egyedek szamat
                    line = await reader.ReadLineAsync(); //
                    numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    double learningTime = double.Parse(numbers[0]); // beolvassuk a taulasi idot
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
                    //fittnesek beolvasasa
                    double[] individualFittnes = new double [populationSize];
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(' ');
                    for (int i = 0; i < populationSize; i++)
                    { 
                        individualFittnes[i] = double.Parse(numbers[i]);
                    }
                    //pontszamok beolvasasa
                    int[] individualScore = new int[populationSize];
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(' ');
                    for (int i = 0; i < populationSize; i++)
                    {
                        individualScore[i] = int.Parse(numbers[i]);
                    }
                    Data data=new Data(evolutionType,round, populationSize,weightsSize,weights, individualFittnes, learningTime, individualScore, evolutionParameters);
                    return data;
                }
            //}
            //catch
            //{
                // throw new GameDataException();
            //}
        }

        // Mentes
        public async Task SaveAsync(String path, int evolutionType, int round, int populationSize, int weightsSize, double[,] weights, double[] individualFittnes, double learningTime, int[] individualScore, double[] evolutionParameters)
        {
            //try
            //{
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    if (evolutionType == 0)
                    {
                        writer.Write("Simple evolution"); // kiírjuk a korok szamat
                        await writer.WriteLineAsync(); //uj sor
                    }
                    else
                    {
                        writer.Write("Red Queen evolution"); // kiírjuk a korok szamat
                        await writer.WriteLineAsync(); //uj sor
                    }
                    //Kiirjuk az evolucio parametereit
                    for (Int32 i = 0; i < 6; i++)
                    {
                        await writer.WriteAsync(evolutionParameters[i] + " ");
                    }
                    await writer.WriteLineAsync();
                    writer.Write(round); // kiírjuk a korok szamat
                    await writer.WriteLineAsync(); //uj sor
                    writer.Write(populationSize); // kiírjuk az egyedek szamat
                    await writer.WriteLineAsync(); //uj sor
                    writer.Write(learningTime); // kiírjuk a tanulasi idot
                    await writer.WriteLineAsync(); //uj sor
                    writer.Write(weightsSize); // kiírjuk a sulyok szamat
                    await writer.WriteLineAsync(); //uj sor
                    //Kiirjuk a sulyokat
                    for (Int32 i = 0; i < populationSize; i++)
                    {
                        for (Int32 j = 0; j < weightsSize; j++)
                        {
                            await writer.WriteAsync(weights[i,j] + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                    //kiirjuk a fittnest
                    for (int i=0; i < populationSize; i++)
                    {
                        await writer.WriteAsync(individualFittnes[i] + " ");
                    }
                    await writer.WriteLineAsync();
                //kiirjuk a potszamokat 
                    for (int i=0;  i < populationSize; i++)
                    {
                        await writer.WriteAsync(individualScore[i] + " ");
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
