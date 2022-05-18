using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        Rectangle spaceship1 = new Rectangle(150, 460, 10, 30);
        Rectangle spaceship2 = new Rectangle(450, 460, 10, 30);
        Rectangle win = new Rectangle(0, 0, 600, 2);

        List<Rectangle> asteroidsLeft = new List<Rectangle>();
        List<Rectangle> asteroidsRight = new List<Rectangle>();
        int asteroidSize = 5;
        int asteroidSpeed = 5;


        bool upArrowDown = false;
        bool downArrowDown = false;
        bool wDown = false;
        bool sDown = false;

        int spaceshipSpeed = 8;

        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Random randGen = new Random();
        int randValue = 0;

        int spaceship1Score = 0;
        int spaceship2Score = 0;

        int counter = 0;
        string gameState = "waiting";

        SoundPlayer bang = new SoundPlayer(Properties.Resources.bang);
        SoundPlayer point = new SoundPlayer(Properties.Resources.point);

        public Form1()
        {
            InitializeComponent();
        }

        public void GameInitialize()
        {
            titleLabel.Text = "";
            titleLabel.Visible = false;
            subTitleLabel.Text = "";
            subTitleLabel.Visible = false;

            gameEngine.Enabled = true;
            gameState = "running";
            asteroidsLeft.Clear();
            asteroidsRight.Clear();

            spaceship1.Y = 460;
            spaceship1.Y = 460;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }

        private void gameEngine_Tick(object sender, EventArgs e)
        {
            //move spaceship 1
            if (wDown == true && spaceship1.Y > 0)
            {
                spaceship1.Y -= spaceshipSpeed;
            }

            if (sDown == true && spaceship1.Y < this.Height - spaceship1.Height)
            {
                spaceship1.Y += spaceshipSpeed;
            }

            //move spaceship 2
            if (upArrowDown == true && spaceship2.Y > 0)
            {
                spaceship2.Y -= spaceshipSpeed;
            }

            if (downArrowDown == true && spaceship2.Y < this.Height - spaceship2.Height)
            {
                spaceship2.Y += spaceshipSpeed;
            }


            //create asteroids

            randValue = randGen.Next(0, 101);

            if (randValue < 11)
            {
                int y = randGen.Next(10, this.Height - 60);
                asteroidsLeft.Add(new Rectangle(10, y, asteroidSize, asteroidSize));

            }

            if (randValue < 11)
            {
                int y = randGen.Next(10, this.Height - 60);
                asteroidsRight.Add(new Rectangle(this.Width - 10, y, asteroidSize, asteroidSize));

            }

            //Move Asteroids
            for (int i = 0; i < asteroidsLeft.Count(); i++)
            {
                //find the new postion of x based on speed 
                int x = asteroidsLeft[i].X + asteroidSpeed;

                //replace the rectangle in the list with updated one using new y 
                asteroidsLeft[i] = new Rectangle(x, asteroidsLeft[i].Y, asteroidSize, asteroidSize);
            }

            for (int i = 0; i < asteroidsLeft.Count(); i++)
            {
                //find the new postion of x based on speed 
                int x = asteroidsRight[i].X - asteroidSpeed;

                //replace the rectangle in the list with updated one using new y 
                asteroidsRight[i] = new Rectangle(x, asteroidsRight[i].Y, asteroidSize, asteroidSize);
            }

            //Spaceship 1 collides with asteroid
            for (int i = 0; i < asteroidsLeft.Count(); i++)
            {
                if (spaceship1.IntersectsWith(asteroidsLeft[i]))
                {
                    spaceship1.Y = 460;
                }
            }

            for (int i = 0; i < asteroidsRight.Count(); i++)
            {
                if (spaceship1.IntersectsWith(asteroidsRight[i]))
                {
                    spaceship1.Y = 460;
                    //counter++;

                    //if (counter == 50)
                    //{
                    //    wDown = true;
                    //    sDown = true;
                    //}

                }
            }

            //Spaceship 2 collides with asteroid
            for (int i = 0; i < asteroidsLeft.Count(); i++)
            {
                if (spaceship2.IntersectsWith(asteroidsLeft[i]))
                {
                    bang.Play();
                    spaceship2.Y = 460;
                }
            }

            for (int i = 0; i < asteroidsRight.Count(); i++)
            {
                if (spaceship2.IntersectsWith(asteroidsRight[i]))
                {
                    bang.Play();
                    spaceship2.Y = 460;
                }
            }

            //Remove asteroids off screen
            for (int i = 0; i < asteroidsLeft.Count(); i++)
            {
                if (asteroidsLeft[i].X == this.Width)
                {
                    asteroidsLeft.RemoveAt(i);
                }
            }

            for (int i = 0; i < asteroidsRight.Count(); i++)
            {
                if (asteroidsRight[i].X + asteroidSize == 0)
                {
                    asteroidsRight.RemoveAt(i);
                }
            }



            //Spaceship 1 intersects with win

            if (spaceship1.IntersectsWith(win))
            {
                point.Play();

                spaceship1Score++;
                if (spaceship1Score == 3)
                {
                    gameEngine.Enabled = false;
                    gameState = "win1";
                }

                spaceship1.Y = 460;
                player1ScoreLabel.Text = $"{spaceship1Score}";

            }

            //Spaceship 2 intersects with win

            if (spaceship2.IntersectsWith(win))
            {
                point.Play();

                spaceship2Score++;
                if (spaceship2Score == 3)
                {
                    gameEngine.Enabled = false;
                    gameState = "win2";
                }

                spaceship2.Y = 460;
                player2ScoreLabel.Text = $"{spaceship2Score}";

            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")

            {

                titleLabel.Text = "SPACE RACE";

                subTitleLabel.Text = "Press SPACE Bar to Start or ESCAPE to Exit";
            }

            else if (gameState == "running")
            {
                e.Graphics.FillRectangle(whiteBrush, spaceship1);
                e.Graphics.FillRectangle(whiteBrush, spaceship2);

                for (int i = 0; i < asteroidsLeft.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidsLeft[i]);

                }

                for (int i = 0; i < asteroidsRight.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidsRight[i]);

                }

            }
            else if (gameState == "win1")
            {
                titleLabel.Visible = true;
                subTitleLabel.Visible = true;

                titleLabel.Text = "Player 1 Wins!";

                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";

                gameState = "waiting";

                spaceship1Score = 0;
                spaceship2Score = 0; 
                player1ScoreLabel.Text = $"{spaceship1Score}";
                player2ScoreLabel.Text = $"{spaceship2Score}";

            }
            else if (gameState == "win2")
            {
                titleLabel.Visible = true;
                subTitleLabel.Visible = true;

                titleLabel.Text = "Player 2 Wins!";

                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";

                gameState = "waiting";

                spaceship1Score = 0;
                spaceship2Score = 0;
                player1ScoreLabel.Text = $"{spaceship1Score}";
                player2ScoreLabel.Text = $"{spaceship2Score}";

            }
        }

    }
}
