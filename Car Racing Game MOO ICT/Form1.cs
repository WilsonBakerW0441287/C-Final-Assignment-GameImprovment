using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Car_Racing_Game_MOO_ICT
{
    //Questions from assignment 
    // Write a comprehensive description for your game as comments inside the program
    // explaining for any new user how to operate the game and what are the keys or
    // mouse actions to be used.Also, explain in your comments the following concepts:
    // a. GDI+ for graphics applications
    // A. a)GDI+ is a graphics API in the windows OS. It is used to render Vector graphics on screen and
    // is a better version of the original GDI,
    // GDI+ allows programers to create graphics on screen using a UI,
    // It is used to display and manipulate graphics.

    // b. Double‐buffering and its benefits in games or graphics application
    // B. a) Double-buffering is useful becuase it allows data to be stored while its being transferred.
    // this allows us to perform 2 operations at the same time, we are read program A while writting program B,
    // and vice versa! Computers are really good at dealing with chuncks of data, not entire programs,
    // this increases the efficiency of loading significantly 

    //This Game is a simple top down view of a racing game
    //The user uses the left and right arrow keys to naviagte across the lanes
    //Your objective is to not get hit by the cars, and to get the highest score possible!

    public partial class Form1 : Form
    {

        int roadSpeed;
        int trafficSpeed;
        int playerSpeed = 17;
        int score;
        int carImage;
        //using random
        Random rand = new Random();
        Random carPosition = new Random();

        bool goleft, goright;


        public Form1()
        {
            InitializeComponent();//defult windows form method
            ResetGame(); //custom made method that allows us to reset the game
        }

        private void keyisdown(object sender, KeyEventArgs e) //KeyEventArgs are the keys that are pressed so this function is an eventhandler 
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }

        }

        private void keyisup(object sender, KeyEventArgs e)//stop sending the goLeft and goRight when a button is not pressed
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
        }

        private void gameTimerEvent(object sender, EventArgs e) //this houses some of the logic
        {

            txtScore.Text = "Score: " + score;
            score++; //score that increases as the timer increases


            if (goleft == true && player.Left > 10)
            {
                player.Left -= playerSpeed;
            }
            if (goright == true && player.Left < 415)
            {
                player.Left += playerSpeed;
            }
            //making sure the player does not go off the screen

            roadTrack1.Top += roadSpeed;
            roadTrack2.Top += roadSpeed;

            if (roadTrack2.Top > 519)
            {
                roadTrack2.Top = -519;
            }
            if (roadTrack1.Top > 519)
            {
                roadTrack1.Top = -519;
            }

            //The RoadTrack is a background image moving so this dictates how fast it moves

            AI1.Top += trafficSpeed;
            AI2.Top += trafficSpeed;

            //aswell as how fast the cars come at you

            if (AI1.Top > 530)
            {
                changeAIcars(AI1);
            }

            if (AI2.Top > 530)
            {
                changeAIcars(AI2);
            }
            //depending where the car is on the screen it will change the next car output


            if (player.Bounds.IntersectsWith(AI1.Bounds) || player.Bounds.IntersectsWith(AI2.Bounds)) //using png intersection checking
                                                                                                      //we see if the users car has "crashed" with a AI car
            {
                gameOver(); //call the game over method
            }
            //depending how high the score is the user will be "rewarded" with a different award
            if (score > 40 && score < 500)
            {
                award.Image = Properties.Resources.bronze;
            }


            if (score > 500 && score < 2000)
            {
                award.Image = Properties.Resources.silver;
                roadSpeed = 20;
                trafficSpeed = 22;
            }

            if (score > 2000)
            {
                award.Image = Properties.Resources.gold;
                trafficSpeed = 27;
                roadSpeed = 25;
            }


        }

        private void changeAIcars(PictureBox tempCar) //changes what car "spawns" next
        {

            //carImage = rand.Next(1, 9); //commenting out this line to increase the number of overall cars the game can use
            carImage = rand.Next(1, 15);
            switch (carImage)
            {
                case 1:
                    tempCar.Image = Properties.Resources.ambulance;
                    break;
                case 2:
                    tempCar.Image = Properties.Resources.carGreen;
                    break;
                case 3:
                    tempCar.Image = Properties.Resources.carGrey;
                    break;
                case 4:
                    tempCar.Image = Properties.Resources.carOrange;
                    break;
                case 5:
                    tempCar.Image = Properties.Resources.carPink;
                    break;
                case 6:
                    tempCar.Image = Properties.Resources.CarRed;
                    break;
                case 7:
                    tempCar.Image = Properties.Resources.carYellow;
                    break;
                case 8:
                    tempCar.Image = Properties.Resources.TruckBlue;
                    break;
                case 9:
                    tempCar.Image = Properties.Resources.TruckWhite;
                    break;
                //these are all custom images I added
                //Adding custom images was harder than it needed to be lol
                case 10:
                    tempCar.Image = Properties.Resources.RocketShip;
                    break;
                case 11:
                    tempCar.Image = Properties.Resources.copCar;
                    break;
                case 12:
                    tempCar.Image = Properties.Resources.ufo;
                    break;
                case 13:
                    tempCar.Image = Properties.Resources.carPurple;
                    break;
                case 14:
                    tempCar.Image = Properties.Resources.Mcqueen;
                    break;

            }

            //this is the code that changes the position of the car
            tempCar.Top = carPosition.Next(100, 400) * -1;

            //this is the random number generator that dictates /where/ the car will "spawn"
            if ((string)tempCar.Tag == "carLeft")
            {
                tempCar.Left = carPosition.Next(5, 200);
            }
            if ((string)tempCar.Tag == "carRight")
            {
                tempCar.Left = carPosition.Next(245, 422);
            }
        }

        private void gameOver()
        {
            //when the user crashes;
            playSound(); //play the crash sound
            gameTimer.Stop(); //stop the timer
            explosion.Visible = true; //show the explosion png
            player.Controls.Add(explosion);
            explosion.Location = new Point(-8, 5); //tells the png where the explosion is
            explosion.BackColor = Color.Transparent; //makes the explosion background transparent
            
            award.Visible = true;//show the user their award
            award.BringToFront();//draw the textbox ontop of everything else

            //allow the user to interact with the buttons
            btnStart.Enabled = true;
            btnHELP.Enabled = true;

        }

        private void ResetGame()
        {
            //dont allow the user to focus on the buttons as it will messup the <- and -> keys
            btnStart.Enabled = false;
            btnHELP.Enabled = false;

            explosion.Visible = false;//hide the explosion
            award.Visible = false; //hide the award
            goleft = false; 
            goright = false;
            helpbox.Visible = false;//hide the help box
            score = 0; //make sure score is 0
            award.Image = Properties.Resources.bronze; //set the award to bronze by default
            helpbox.Image = Properties.Resources.Help; //set the help to a help png I made

            //speed of the game
            roadSpeed = 12;
            trafficSpeed = 15;
            //inital spawn bias
            AI1.Top = carPosition.Next(200, 500) * -1;
            AI1.Left = carPosition.Next(5, 200);

            AI2.Top = carPosition.Next(200, 500) * -1;
            AI2.Left = carPosition.Next(245, 422);
            //start the timer
            gameTimer.Start();

        }

        private void restartGame(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void btnHELP_Click(object sender, EventArgs e)
        {
            //custom made handler to account for the "help" requirement of the assignment, this toggled a png to be visible or not visible
            if (helpbox.Visible == false)
            {
                helpbox.Visible = true;
            }
            else if (helpbox.Visible == true)
            {
                helpbox.Visible = false;
            }

        }

        private void playSound()
        {
            //finds the audio file to play when the user crashes into a car
            System.Media.SoundPlayer playCrash = new System.Media.SoundPlayer(Properties.Resources.hit);
            playCrash.Play();

        }
    }
}
