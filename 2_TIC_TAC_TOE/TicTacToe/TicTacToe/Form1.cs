using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {

        private int field_size;

        private int[,] xo_matrix;      // Two dimensional array storing current game state

        private Button[,] buttons;

        List<WeightedSum> weights_list;

        public Form1(int _field_size)
        {
            field_size = _field_size;

            xo_matrix = new int[field_size, field_size];
            buttons = new Button[field_size, field_size];
            weights_list = new List<WeightedSum>();

            // Two dimensional array that stores the current state of the game 
            /*
            for (int i = 0; i < field_size; i++)       //This array we initialize with -1
                for (int j = 0; j < field_size; j++)
                    xo_matrix[i, j] = -1;
             */

            InitializeComponent();

        }

        private void buildGameField()
        {
            int start_pos_x = 1;
            int start_pos_y = 1;
            int button_width = 75;
            int button_height = 75;

            int current_pos_x = start_pos_x;
            int current_pos_y = start_pos_y;

            Button button;

            for (int j = 0; j < field_size; j++)
            {
                current_pos_x = start_pos_x;
                for (int i = 0; i < field_size; i++)
                {
                    button = new Button();

                    button.Name = "button" + i.ToString();

                    button.Tag = i; //We identify each button with its tag property and initialize it with the index value

                    button.SetBounds(current_pos_x, current_pos_y, button_width, button_height);
                    button.Text = "";

                    button.Click += new EventHandler(button_Click);

                    this.Controls.Add(button);

                    buttons[i, j] = button;
                    xo_matrix[i, j] = -1;

                    current_pos_x += button_width + 1;
                }
                current_pos_y += button_height + 1;
            }

            this.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buildGameField();
            initWeights(); //initialize the weights list
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /*
         *                                       Weight function is the heart of algorithm
         *                                       Weight is calculated for every empty cell
         * */
        private int[] weightFunction(int x, int y)
        {
            int row_sum, col_sum, left_diag_sum, right_diag_sum;
            int[] weights = new int[4];
            row_sum = col_sum = left_diag_sum = right_diag_sum =  0;

            for (int i = 0; i < field_size; i++)  //calculate the row sum of the cell row
                row_sum += xo_matrix[x, i];
            weights[0] = row_sum;

            for (int j = 0; j < field_size; j++)  //calculate the col sum of the cell row
                col_sum += xo_matrix[j, y];
            weights[1] = col_sum;

            //if the cell lies on the left diagonal, calculate its sum
            if (x == y)
            {
                for (int m = 0; m < field_size; m++)
                    left_diag_sum += xo_matrix[m, m];
            }
            weights[2] = left_diag_sum;

            //if the cell lies on the right diagonal, calculate its sum
            if (x + y == field_size - 1)
            {
                for (int k = field_size - 1, n = 0; k >= 0; k--, n++)
                    right_diag_sum += xo_matrix[k, n];
            }
            weights[3] = right_diag_sum;
            
            return weights;
        }

        private void initWeights()
        {
            for (int j = 0; j < field_size; j++)
                for (int i = 0; i < field_size; i++)
                {
                    if (xo_matrix[i, j] == -1)
                    {
                        int[] weights = weightFunction(i, j);
                        WeightedSum wsum = new WeightedSum(j, i, weights[0],weights[1],weights[2],weights[3]);
                        weights_list.Add(wsum);
                    }
                }
        }

        private void adjustWeights()
        {
            foreach (WeightedSum wsum in weights_list)
            {
                int[] weights = weightFunction(wsum.x, wsum.y);
                for (int i = 0; i < 4; i++)
                    wsum.sums[i] = weights[i];
                wsum.weight = weights.Sum();
            }

        }

        private void removeWeightFromList(int i, int j)
        {
            foreach (WeightedSum wsum in weights_list)
            {
                if (wsum.x == i && wsum.y == j)
                {
                    weights_list.Remove(wsum);
                    break;
                }
            }
        }

        private WeightedSum getMax()
        {
            WeightedSum max = weights_list[0];
            for(int i=1; i<=weights_list.Count-1; i++)
            {
                if (weights_list[i].weight > weights_list[i - 1].weight)
                {
                    max = weights_list[i];
                }
            }

            return max;
        }
        
        //---------------------------------------------------------------------------------------------------
        private WeightedSum getX9() //this is the function that is looking for single empty cells
        {                              //algorithm is based on the knowledge that an object contains 19, 29 or 39 value among its sums
            WeightedSum wsum = null;

            foreach (WeightedSum ws in weights_list)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (ws.sums[i] == (field_size - 1) * 10 - 1)
                    {
                        wsum = ws;
                        break;
                    }
                }
                if (wsum != null) break;
            }


            return wsum;
        }

        //---------------------------------------------------------------------------------------------------
        //---------------------------------Check if someone won-------------------------------------------
        //-------------------------------------------------------------------------------------------------
        private int sumOfRow(int num_row)
        {
            int sum = 0;

            for (int i = 0; i < field_size; i++)
                sum += xo_matrix[num_row, i];

            return sum;

        }

        private int sumOfColumn(int num_col)
        {
            int sum = 0;

            for (int i = 0; i < field_size; i++)
                sum += xo_matrix[i, num_col];

            return sum;

        }

        private int sumOfLeftDiagonal()
        {
            int sum = 0;
            for (int i = 0; i < field_size; i++)
                sum += xo_matrix[i, i];
            return sum;
        }


        //-------------------------------------------------------------------
        private int sumOfRightDiagonal()
        {
            int sum = 0;
            for (int i = field_size - 1, j = 0; i >= 0; i--, j++)
                sum += xo_matrix[i, j];
            return sum;
        }
        //--------------------------------------------------------------------
        //--------------------------------------------------------------------

        private int checkIfSomeoneWon()
        {
            int result = -1;

            for (int i = 0; i < field_size; i++) //check columns
            {
                int sum_of_row = sumOfRow(i);
                if (sum_of_row == 0)
                {
                    result = 0; //computer won
                    break;
                }
                else if (sum_of_row == 10 * field_size)
                {
                    result = 10;
                    break; //user won
                }
            }
            if (result == 0 || result == 10) return result;

            for (int i = 0; i < field_size; i++) //check columns
            {
                int sum_of_row = sumOfColumn(i);
                if (sum_of_row == 0)
                {
                    result = 0; //computer won
                    break;
                }
                else if (sum_of_row == 10 * field_size)
                {
                    result = 10;
                    break; //user won
                }
            }
            if (result == 0 || result == 10) return result;

            int ld = sumOfLeftDiagonal();
            if (ld == 0)
            {
                return 0; //computer won
           
            }
            else if (ld == 10 * field_size)
            {
                return 10; //user won
            }

            int rd = sumOfRightDiagonal();
            if (rd == 0)
            {
                result = 0 ; //computer won

            }
            else if (rd == 10 * field_size)
            {
                result = 10; //user won
            }

            return result;

        }
        //-----------------------------------------------------------------------------------------------

        private void button_Click(object sender, EventArgs e)
        {
            if (weights_list.Count == 0)
            {
                MessageBox.Show("GAME OVER!");
                return;
            }
            Button button = (Button)sender;
            button.Text = "O";
                     
           
            int x = (int)Math.Floor((decimal)button.Location.X / 75);
            int y = (int)Math.Floor((decimal)button.Location.Y / 75);
            //MessageBox.Show(x.ToString()+","+y.ToString());
          
            xo_matrix[x, y] = 10;  // user puts 10
            removeWeightFromList(x, y);
       

             button.Refresh();
            
            removeWeightFromList(x, y);
            adjustWeights();
            int win = checkIfSomeoneWon();
            if (win == 0)
            {
                MessageBox.Show("Computer won!");
                return;

            }
            else if (win == 10)
            {
                MessageBox.Show("You won!");
                return;

            }


            if (weights_list.Count == 0)
            {
                MessageBox.Show("GAME OVER!");
                return;
            }

            WeightedSum x9 = getX9();
            if (x9 != null)
            {

                MessageBox.Show(x9.x.ToString() + "," + x9.y.ToString());
                xo_matrix[x9.x, x9.y] = 0; //computer puts 0
                adjustWeights();
                buttons[x9.x, x9.y].Text = "X";
                buttons[x9.x, x9.y].Refresh();
                removeWeightFromList(x9.x, x9.y);
            }
            else
            {
                WeightedSum max = getMax();
                //MessageBox.Show(max.x.ToString()+","+max.y.ToString());
                xo_matrix[max.x, max.y] = 0; //computer puts 0
                adjustWeights();
                buttons[max.x, max.y].Text = "X";
                buttons[max.x, max.y].Refresh();
                removeWeightFromList(max.x, max.y);
            }

            win = checkIfSomeoneWon();
            if (win == 0)
            {
                MessageBox.Show("Computer won!");
                return;

            }
            else if (win == 10)
            {
                MessageBox.Show("You won!");
                return;

            }
            
            
               
                        
        }

    }
}
