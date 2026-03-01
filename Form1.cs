using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake__5
{
    public partial class Form1 : Form
    {
        #region Adattagok
        private List<Point> snake;
        private List<Point> obstacles = new List<Point>();
        private Point food;
        private int meret = 20;
        private int points;
        private Random rnd = new Random();
        private string irany = "jobbra";
        private Timer gametimer;
        #endregion

        #region Konstruktor
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            snake = new List<Point>()
            {
                new Point(100,100),
                new Point(100,80),
                new Point(100,60)
            };

            gametimer = new Timer();
            gametimer.Interval = 100;
            gametimer.Tick += GameTimer;
            gametimer.Start();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }
        #endregion

        #region Alprogramok
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            foreach(Point test in snake)
            {
                graphics.FillRectangle(Brushes.Green, new Rectangle(test.X,test.Y,meret,meret));
            }

            foreach(Point obs in obstacles)
            {
                graphics.FillRectangle(Brushes.Black, new Rectangle(obs.X, obs.Y, meret, meret));
            }

            graphics.FillRectangle(Brushes.Red, new Rectangle(food.X, food.Y, meret, meret));

            lb_points.Text = "Pontszám: " + points.ToString();

            base.OnPaint(e);
        }

        public Point NewFood()
        {
            Point newfood = food;

            int foodX = rnd.Next(0, this.ClientSize.Width / meret) * meret;
            int foodY = rnd.Next(0, this.ClientSize.Height / meret) * meret;

            newfood = new Point(foodX, foodY);

            bool helyespozicio = false;

            for (int i = 0; i < snake.Count; i++)
            {
                helyespozicio = true;

                if(newfood == snake[i])
                {
                    helyespozicio = false;
                    break;
                }
            }

            foreach(Point obs in obstacles)
            {
                if(newfood == obs)
                {
                    helyespozicio = false;
                    break;
                }
            }

            if(newfood == food)
            {
                helyespozicio = false;
            }

            return newfood;

        }
        private Point NewObs()
        {
            Point newobs;

            int obsX = rnd.Next(0, this.ClientSize.Width / meret) * meret;
            int obsY = rnd.Next(0, this.ClientSize.Height / meret) * meret;

            newobs = new Point(obsX, obsY);

            bool helyespozicio = false;

            for (int i = 0; i < snake.Count; i++)
            {
                if(newobs == snake[i])
                {
                    helyespozicio = false;
                    break;
                }
                else
                {
                    helyespozicio = true;
                }
            }

            foreach(Point obs in obstacles)
            {
                if(newobs == obs)
                {
                    helyespozicio = false;
                    break;
                }
            }

            if(food == newobs)
            {
                helyespozicio = false;
                
            }

            return newobs;

        }
        private void GameTimer(object sender, EventArgs e)
        {
            Point head = snake[0];

            switch (irany)
            {
                case "jobbra":
                    head.X += meret;
                    break;

                case "balra":
                    head.X -= meret;
                    break;

                case "fel":
                    head.Y -= meret;
                    break;

                case "le":
                    head.Y += meret;
                    break;
            }


            snake.Insert(0, head);

            if(head == food)
            {
                food = NewFood();
                points++;

                if (points >= 5)
                {
                    obstacles.Add(NewObs());
                }
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            if(head.X < 0 || head.X >= this.ClientSize.Width || head.Y < 0 || head.Y >= this.ClientSize.Height)
            {
                GameOver();
            }

            foreach(Point obs in obstacles)
            {
                if(head == obs)
                {
                    GameOver();
                }
            }

            for(int i = 1; i < snake.Count - 1 ; i++)
            {
                if(head == snake[i])
                {
                    GameOver();
                }
            }


                this.Invalidate();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (irany != "balra")
                {
                    irany = "jobbra";
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (irany != "jobbra")
                {
                    irany = "balra";
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                if (irany != "le")
                {
                    irany = "fel";
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                if (irany != "fel")
                {
                    irany = "le";
                }
            }

        }
        private void GameOver()
        {
            gametimer.Stop();
            MessageBox.Show("Game Over! Pontszám: " + points.ToString(), "Snake");
            this.Close();
        }
        #endregion

        
    }
}
