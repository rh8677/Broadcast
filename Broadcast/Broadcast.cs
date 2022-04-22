using System;
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
                
                // For each node in the network, we will print out information for it
                for (var count = 0; count < numNodes; count++)
                {
                    Console.Write(count + ": ");
                    var infoNodes = new List<int>(); // A list of nodes that will be added to for each dimension
                    infoNodes.Add(originNode); // We start off with the starting node

                    // Simulates each dimension (or the bit value from least to most significant)
                    for (var bit = 0; bit < Math.Log2(numNodes); bit++)
                    {
                        var nodesToAdd = new List<int>(); // A list of nodes to add from this dimension

                        // For each node in the dimension, we want to determine which nodes it will pass on info to
                        foreach (var node in infoNodes)
                        {
                            
                            // The potential node to pass on information to
                            for (var compare = 0; compare < numNodes; compare++)
                            {
                                var difference = 0; // The difference in bits between the two
                                var atDiff = 0; // The bit that contains the difference

                                // For each bit in the two values, we determine the total amount of differences
                                for (var bite = 0; bite < Math.Log2(numNodes); bite++)
                                {
                                    if (((node >> bite) & 1) != ((compare >> bite) & 1))
                                    {
                                        difference++;
                                        atDiff = bite;
                                    }
                                }

                                // If the difference is one bit and that difference is at the dimension we
                                // are working with, we know that we can pass on info to this node
                                if (difference == 1 && atDiff == bit)
                                {
                                    if (!infoNodes.Contains(compare) && !nodesToAdd.Contains(compare))
                                    {
                                        nodesToAdd.Add(compare);
                                    }
                                }
                            }
                        }

                        // We add each node that we passed on info for to infoNodes
                        foreach (var adding in nodesToAdd)
                        {
                            infoNodes.Add(adding);
                        }

                        // For the current node in the current dimension, we want to know whether we are
                        // receiving or passing on information, and to which node
                        foreach (var node in infoNodes)
                        {
                            if (node == count)
                            {
                                
                                // The potential node that we might have connected with in this dimension
                                for (var compare = 0; compare < numNodes; compare++)
                                {
                                    var difference = 0; // The difference in bits between the two
                                    var atDiff = 0; // The bit that contains the difference

                                    // For each bit in the two values, we determine the total amount of differences
                                    for (var bite = 0; bite < Math.Log2(numNodes); bite++)
                                    {
                                        if (((node >> bite) & 1) != ((compare >> bite) & 1))
                                        {
                                            difference++;
                                            atDiff = bite;
                                        }
                                    }

                                    // If the difference is one bit and that difference is at the dimension we
                                    // are working with, we know that we had connected with this node in this
                                    // dimension
                                    if (difference == 1 && atDiff == bit)
                                    {
                                        
                                        // If this node was just added, we know it must have received info
                                        // from the other node
                                        if (nodesToAdd.Contains(node))
                                        {
                                            Console.Write("Recv " + compare + " ");
                                        }
                                        
                                        // Otherwise, we know it must have sent info out to the other node
                                        else
                                        {
                                            Console.Write("Send " + compare + " ");
                                        }
                                    }
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