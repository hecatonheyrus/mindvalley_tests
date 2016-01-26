using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    class WeightedSum
    {

        public int x, y;
        public int weight;
        public int[] sums;       
        public WeightedSum(int _x, int _y, int rows, int cols, int ldiag, int rdiag)
        {
            sums = new int[4];
            x = _x; y = _y;
            sums[0] = rows; sums[1] = cols; sums[2] = ldiag; sums[3] = rdiag;
            weight = rows + cols + ldiag + rdiag;
        }

           
        

    }
}
