using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Logic
{
    class Program
    {
        static void Main(string[] args)
        {
            var matriz = new int[5, 4] {
                { 1, 1, 1, 1 },
                { 0, 1, 1, 0 },
                { 0, 1, 0, 1 },
                { 0, 1, 9, 1 },
                { 1, 1, 1, 1 }
            };

            Console.WriteLine("Posição 0x0 com valor: {0}", matriz[0, 0]);

            var posicaoAtual = Tuple.Create(0, 0);
            var posicaoNove = matriz.CoordinatesOf(9);
            var posicaoXMaxima = matriz.GetLength(0);
            var posicaoYMaxima = matriz.GetLength(1);

            var continuar = true;
            var caminharPosicao = "Y";

            while (continuar)
            {
                if (matriz[posicaoAtual.Item1, posicaoAtual.Item2] == 9)
                {
                    Console.WriteLine("Posição: {0}x{1} Valor: {2}", posicaoAtual.Item1, posicaoAtual.Item2, matriz[posicaoAtual.Item1, posicaoAtual.Item2]);
                    Console.WriteLine("Chegou.");
                    continuar = false;
                    break;
                }

                if (matriz[posicaoAtual.Item1 + 1, posicaoAtual.Item2] == 9)
                {
                    Console.WriteLine("Posição: {0}x{1} Valor: {2}", posicaoAtual.Item1 + 1, posicaoAtual.Item2, matriz[posicaoAtual.Item1, posicaoAtual.Item2]);
                    Console.WriteLine("Chegou.");
                    continuar = false;
                    break;
                }

                if (matriz[posicaoAtual.Item1, posicaoAtual.Item2 + 1] == 9)
                {
                    Console.WriteLine("Posição: {0}x{1} Valor: {2}", posicaoAtual.Item1, posicaoAtual.Item2 + 1, matriz[posicaoAtual.Item1, posicaoAtual.Item2 + 1]);
                    Console.WriteLine("Chegou.");
                    continuar = false;
                    break;
                }

                if (caminharPosicao == "X")
                {
                    if ((posicaoAtual.Item1 < posicaoNove.Item1 && posicaoAtual.Item1 + 1 < posicaoXMaxima) && matriz[posicaoAtual.Item1 + 1, posicaoAtual.Item2] != 0
                        && (posicaoAtual.Item1 + 2 <= posicaoXMaxima && matriz[posicaoAtual.Item1 + 2, posicaoAtual.Item2] != 0))
                    {
                        posicaoAtual = Tuple.Create(posicaoAtual.Item1 + 1, posicaoAtual.Item2);

                        Console.WriteLine("Posição: {0}x{1} Valor: {2}", posicaoAtual.Item1, posicaoAtual.Item2, matriz[posicaoAtual.Item1, posicaoAtual.Item2]);
                        caminharPosicao = "Y";
                    }
                    else if ((posicaoAtual.Item2 < posicaoNove.Item2 && posicaoAtual.Item2 + 1 < posicaoYMaxima) && matriz[posicaoAtual.Item1, posicaoAtual.Item2 + 1] != 0
                        && (posicaoAtual.Item2 + 2 <= posicaoYMaxima && matriz[posicaoAtual.Item1, posicaoAtual.Item2 + 2] != 0))
                    {
                        posicaoAtual = Tuple.Create(posicaoAtual.Item1, posicaoAtual.Item2 + 1);

                        Console.WriteLine("Posição: {0}x{1} Valor: {2}", posicaoAtual.Item1, posicaoAtual.Item2, matriz[posicaoAtual.Item1, posicaoAtual.Item2]);
                        caminharPosicao = "X";
                    }
                }
                else
                {
                    if ((posicaoAtual.Item2 < posicaoNove.Item2 && posicaoAtual.Item2 + 1 < posicaoYMaxima) && matriz[posicaoAtual.Item1, posicaoAtual.Item2 + 1] != 0
                        && (posicaoAtual.Item2 + 2 <= posicaoYMaxima && matriz[posicaoAtual.Item1, posicaoAtual.Item2 + 2] != 0))
                    {
                        posicaoAtual = Tuple.Create(posicaoAtual.Item1, posicaoAtual.Item2 + 1);

                        Console.WriteLine("Posição: {0}x{1} Valor: {2}", posicaoAtual.Item1, posicaoAtual.Item2, matriz[posicaoAtual.Item1, posicaoAtual.Item2]);
                        caminharPosicao = "X";
                    }
                    else if ((posicaoAtual.Item1 < posicaoNove.Item1 && posicaoAtual.Item1 + 1 < posicaoXMaxima) && matriz[posicaoAtual.Item1 + 1, posicaoAtual.Item2] != 0
                        && (posicaoAtual.Item1 + 2 <= posicaoXMaxima && matriz[posicaoAtual.Item1 + 2, posicaoAtual.Item2] != 0))
                    {
                        posicaoAtual = Tuple.Create(posicaoAtual.Item1 + 1, posicaoAtual.Item2);

                        Console.WriteLine("Posição: {0}x{1} Valor: {2}", posicaoAtual.Item1, posicaoAtual.Item2, matriz[posicaoAtual.Item1, posicaoAtual.Item2]);
                        caminharPosicao = "Y";
                    }
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Precione enter para fechar o programa...");
            Console.ReadKey();
        }
    }
}
