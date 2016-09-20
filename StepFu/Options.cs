using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepFu
{
    public class Options
    {
        public int moveMax = 4; // maximum movement amount
        public int moveAvg = 2; // non-stressful movement amount

        public int moveWeightZero = 8;   // chance of not moving at all
        public int moveWeightAvg = 20;    // chance of moving up to average
        public int moveWeightMax = 10;    // chance of moving beyond average
        public int moveWeightExtreme = 4; // chance of moving beyond maximum
        
        // NOTE: the following options are currently not supported:

        //public bool useFootswitches = false; // allows movement that swaps feet on the same panel
        //public bool useCrossovers = false;   // allows movement that crosses feet horizontally
        //public bool useTurnarounds = false;  // allows movement that turns the player any amount

        //public bool forceDoubleTaps = false;
        //public bool forceFootswitches = false;
    }

    // +1 adjacent, +2 for every additional space
    // +2 extra if feet vertically aligned at the edge of the pad(s)
    // +3 for every horizontal crossover space

    // x x x       x x x
    // x x x  -->  x x x  = 0 base movement
    // o x x       o x x

    // x x x       x x x
    // x x x  -->  o x x  = 1 base movement
    // o x x       x x x

    // x x x       x x x
    // x x x  -->  x o x  = 2 base movement
    // o x x       x x x

    // x x x       o x x
    // x x x  -->  x x x  = 3 base movement
    // o x x       x x x

    // x x x       x o x
    // x x x  -->  x x x  = 4 base movement
    // o x x       x x x

    // x x x       x x o
    // x x x  -->  x x x  = 6 base movement
    // o x x       x x x

    // x x x x x x       x x x x x x
    // x x x x x x  -->  x x x x x x  = 5 base movement
    // o x x x x x       x x x o x x

    // x x x x x x       x x x x x x
    // x x x x x x  -->  x x x o x x  = 6 base movement
    // o x x x x x       x x x x x x

    // x x x x x x       x x x x x x
    // x x x x x x  -->  x x x x x x  = 7 base movement
    // o x x x x x       x x x x o x

    // x x x x x x       x x x x x x
    // x x x x x x  -->  x x x x o x  = 8 base movement
    // o x x x x x       x x x x x x

    // x x x x x x       x x x x o x
    // x x x x x x  -->  x x x x x x  = 10 base movement
    // o x x x x x       x x x x x x

    // x x x x x x       x x x x x o
    // x x x x x x  -->  x x x x x x  = 12 base movement
    // o x x x x x       x x x x x x

    
}
