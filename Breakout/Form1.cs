using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Breakout
{

    public partial class Form1 : Form
    {

        private Rectangle Paddle;
        private List<Rectangle> Blocks;
        private Rectangle Ball;
        private List<Color> Colors;
        private readonly int TILESIZE = 16;
        private int paddleVelocity;
        private int xVelocity = 4;
        private int yVelocity = 4;
        //this.ClientSize.Width/height

        Random random = new Random();

        public Form1()
        {
            
            //Prevents flickering on the screen
            this.DoubleBuffered = true;
                                    
            InitializeComponent();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            //Create the brush
            SolidBrush blockBrush = new SolidBrush(Colors.ElementAt(0));
            SolidBrush ballBrush = new SolidBrush(Color.DarkGray);
            SolidBrush paddleBrush = new SolidBrush(Color.Black);

            int colorNum = 0;
            
            //Only draw the blocks if we need to
            

                //Draw the blocks
                for (int i = 0; i < Blocks.Count; i++)
                {
                    e.Graphics.FillRectangle(blockBrush, Blocks.ElementAt(i));

                    //Change the colors for each row
                    colorNum++;
                    if (colorNum >= Colors.Count)
                    {
                        colorNum = 0;
                    }

                    blockBrush.Color = Colors.ElementAt(colorNum);
                }
            

            //Draw the ball
            e.Graphics.FillRectangle(ballBrush, Ball);
            //Draw the paddle
            e.Graphics.FillRectangle(paddleBrush, Paddle);

            //Clean up
            blockBrush.Dispose();
            ballBrush.Dispose();
            paddleBrush.Dispose();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Create paddle and ball
            Paddle = new Rectangle(this.ClientSize.Width / 2, this.ClientSize.Height - 50, 100, 10);
            Ball = new Rectangle(300, 200, 10, 10);

            //Create list of blocks and colors used for drawing
            Blocks = new List<Rectangle>();
            Colors = new List<Color>();

            //Randomly assign the ball's initial direction
            GenerateDirection();

            paddleVelocity = 0;

            //Add all the colors
            Colors.Add(Color.Violet);
            Colors.Add(Color.Red);
            Colors.Add(Color.Orange);
            Colors.Add(Color.Yellow);
            Colors.Add(Color.Green);
            Colors.Add(Color.Blue);

            //Create the blocks
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < Colors.Count; j++)
                {
                    Blocks.Add(new Rectangle(i * TILESIZE + 65, j * TILESIZE + 50, TILESIZE, TILESIZE));
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            //Move the paddle
            Paddle.X += paddleVelocity;

            if (Paddle.Left <= 0)
            {
                Paddle.X = 0;
            }
            else if (Paddle.Right >= this.ClientSize.Width)
            {
                Paddle.X = this.ClientSize.Width - Paddle.Width;
            }

            //Move the ball
            Ball.X += xVelocity;
            Ball.Y += yVelocity;

            //Ball bounces off the walls
            if (Ball.Right >= this.ClientSize.Width || Ball.X <= 0)
            {
                Math.Abs(xVelocity);
                xVelocity *= -1;
            }
            if (Ball.Top <= 0)
            {
                Math.Abs(yVelocity);
                yVelocity *= -1;
            }

            //Ball goes below the paddle which places it near the middle of the screen
            else if (Ball.Top > this.ClientSize.Height)
            {
                Ball.X = this.ClientSize.Width / 2;
                Ball.Y = this.ClientSize.Height - 100;
                GenerateDirection();
                //I want the ball to go up (otherwise the game will be pretty hard)
                Math.Abs(yVelocity);
                yVelocity *= -1;
            }

            //Bounce the ball off the paddle
            if (Ball.IntersectsWith(Paddle))
            {
                //Change the xVelocity based off what side the paddle the ball hits
                yVelocity *= -1;
                if (paddleVelocity > 0)
                {
                    Math.Abs(xVelocity);
                }
                else if (paddleVelocity < 0)
                {
                    Math.Abs(xVelocity);
                    xVelocity *= -1;
                }
            }

            //Check for block collision
            foreach (Rectangle block in Blocks)
            {
                //Fix
                if (Ball.IntersectsWith(block))
                {
                    //Reverse xVelocity if collision from left or right
                    //This sees if a point on the left or right ball center is colliding with the block
                    if (block.Contains(Ball.Left, Ball.Y + Ball.Height / 2) || block.Contains(Ball.Right, Ball.Y + Ball.Height / 2))
                    {
                        xVelocity *= -1;
                        
                    }
                    //If it's not a left/right collision then it's a top/down collision
                    else
                    {
                        yVelocity *= -1;
                    }
                    
                    Blocks.Remove(block);
                    break;
                }
            }

            Invalidate();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Left ^ e.KeyCode == Keys.Right)
            {
                paddleVelocity = 0;
            }
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                paddleVelocity = -5;
            }
            if (e.KeyCode == Keys.Right)
            {
                paddleVelocity = 5;
            }
        }

        /// <summary>
        /// Generates a random direction (negative or positive) for xVelocity and yVelocity.
        /// </summary>
        private void GenerateDirection()
        {
            if (random.Next(2) == 0)
            {
                xVelocity *= -1;
            }
            if (random.Next(2) == 0)
            {
                //yVelocity *= -1;
            }
        }
    }
}
