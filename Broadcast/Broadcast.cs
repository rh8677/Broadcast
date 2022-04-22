using System;
using System.Collections;
using System.Collections.Generic;

namespace Broadcast
{
    internal static class Program
    {

        // This function is used by the main function to check whether the first
        // argument is a power of 2 greater than 1.
        //
        // Param:
        // @ intCheck - the integer to check the power for
        private static bool CheckPower(int intCheck)
        {
            return intCheck != 0 && intCheck != 1 && (intCheck & (intCheck - 1)) == 0;
        }

        // This function will handle the proper command line arguments and print
        // out the proper output for this program.
        //
        // Param:
        // @ args - the array of command line arguments
        private static void Main(string[] args)
        {
            // If the amount of arguments is not 2, we know it is wrong
            if (args.Length != 2)
            {
                Console.WriteLine("Error - must specify 2 arguments, 'Number of Nodes' and 'Originating Node'.");
            }
            else
            {
                var numNodes = int.Parse(args[0]); // The number of nodes in the network
                var originNode = int.Parse(args[1]); // The node to start broadcasting from
                
                // If the first argument is not an int of power of 2 greater than 1, we
                // inform the user
                if (!CheckPower(int.Parse(args[0])))
                {
                    Console.WriteLine("Error - first argument must be a power of 2 greater than 1.");
                    return;
                }

                // If the first argument is not an int between 0 and the number of nodes
                // minus 1, we inform the user
                if (originNode < 0 || originNode >= numNodes)
                {
                    Console.WriteLine("Error - second argument must be a number between 0 and first argument minus 1.");
                    return;
                }
                
                for (var count = 0; count < numNodes; count++)
                {
                    Console.Write(count + ": ");
                    var infoNodes = new List<int>();
                    infoNodes.Add(originNode);

                    for (var bite = 0; bite < Math.Log2(numNodes); bite++)
                    {
                        for (var compare = 0; compare < numNodes; compare++)
                        {
                            var differences = 0;
                            var atDiff = 0;

                            for (var bit = 0; bit < Math.Log2(numNodes); bit++)
                            {
                                if (((count >> bit) & 1) != ((compare >> bit) & 1)) {
                                    differences++;
                                    atDiff = bit;
                                }
                            }

                            if (differences == 1 && bite == atDiff)
                            {
                                if (infoNodes.Contains(compare))
                                {
                                    Console.Write("Recv " + compare + " ");
                                }
                                else
                                {
                                    Console.Write("Send " + compare + " ");
                                    infoNodes.Add(compare);
                                }
                            }
                        }
                    }
                    
                    Console.WriteLine();
                }
            }
        }
    }
}