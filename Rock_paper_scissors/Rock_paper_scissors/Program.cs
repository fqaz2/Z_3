using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rock_paper_scissors
{
    class Program
    {
        static void Main(string[] args)
        {
            bool err = false;
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = i+1; j < args.Length; j++)
                {
                    if (args[i] == args[j])
                    {
                        err = true;
                        break;
                    }
                }
            }
            if (args.Length > 2 && args.Length % 2 != 0 && err == false)
            {
                int move = 0;
                do
                {
                menu:
                    byte[] new_byte = new Byte[16];
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    rng.GetBytes(new_byte);
                    int result = BitConverter.ToInt32(new_byte, 0);
                    int r = new Random(result).Next(0, args.Length);
                    Console.WriteLine("HMAC:");
                    using (HMACSHA256 HashTool = new HMACSHA256(new_byte))
                    {
                        Byte[] EncryptedBytes = HashTool.ComputeHash(new_byte);
                        StringBuilder key = new StringBuilder();
                        foreach (var item in EncryptedBytes)
                        {
                            key.Append(item.ToString("x2"));
                        }
                        Console.WriteLine(key);
                    }

                    Console.WriteLine("Available moves:");
                    for (int i = 0; i < args.Length; i++)
                    {
                        Console.WriteLine($"{i + 1} - {args[i]}");
                    }
                    Console.Write("0 - exit\nEnter your move: ");
                    try
                    {
                        move = Convert.ToInt32(Console.ReadLine());
                        if ( move < 0 || move > args.Length)
                        {
                            throw new Exception("Not found numb");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        goto menu;
                    }
                    if (move > 0)
                    {
                        int x = move - 1;
                        int y = r;//ход компьютера

                        int shift = (args.Length - 1) / 2 - x;
                        int sh_x = x + shift;
                        int sh_y = y + shift;

                        if (sh_y >= args.Length)
                        {
                            sh_y -= args.Length;
                        }
                        else if (sh_y < 0)
                        {
                            sh_y += args.Length;
                        }

                        string res = "Draw";
                        if (sh_x > sh_y)
                        {
                            res = "You win!";
                        }
                        else if (sh_x < sh_y)
                        {
                            res = "Sory you lose";
                        }

                        Console.WriteLine("Your move: " + args[x]);
                        Console.WriteLine("Computer move: " + args[y]);
                        Console.WriteLine(res);

                        Console.WriteLine("HMAC Key: ");
                        StringBuilder key = new StringBuilder();
                        foreach (var item in new_byte)
                        {
                            key.Append(item.ToString("x2"));
                        }
                        Console.WriteLine(key+"\n");
                    }
                } while (move != 0);
            }else
            {
                Console.WriteLine("Input parameter error. Rock_paper_scissors.exe a b c d e f");
            }
        }
    }
}
