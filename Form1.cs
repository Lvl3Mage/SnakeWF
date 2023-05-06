using System.Diagnostics;
using System.Security.Cryptography;

namespace SnakeWF
{
    public partial class Form1 : Form // handle this class after the base ones have been written
    {

        int width = 735;
        int height = 500;
        Snake snake;
        FoodManager foodManager;
        Marker marker;

        Stopwatch sw = new Stopwatch();
        double lastGameTime = 0;
        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(255, 251, 248, 239);
            this.ClientSize = new Size(width, height);
            snake = new Snake((width/2, height/2), this, (width, height));
            marker = new Marker(this, width, height);
            foodManager = new FoodManager(snake, this, (width, height), marker);
            sw.Start();

            //GameLoop();
            // do later once the snake and segments have been written
        }

        /*async void GameLoop()
        {
            while (true)
            {
                snake.Move(0.05f);
                if (snake.checkInCollision())
                {
                    break;
                }
                await Task.Delay(1);

            }
        }*/
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Direction newDir;
            switch (e.KeyCode)
            {
                case Keys.W:
                    newDir = Direction.Up;
                    break;
                case Keys.S:
                    newDir = Direction.Down;
                    break;
                case Keys.A:
                    newDir = Direction.Left;
                    break;
                case Keys.D:
                    newDir = Direction.Right;
                    break;
                default:
                    return;
            }
            snake.RotateSnake(newDir);
            //Handle key presses and change the direction of the snake
        }

        bool gameRunning = true;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!gameRunning)
            {
                return;
            }

            //double deltaTime = sw.ElapsedMilliseconds - lastGameTime;
            //For some reason delta time does not accurately represent the time since last frame, we suspect that this might be due to floating point precision operations


            snake.Move(0.001f);
            foodManager.Update();
            marker.elapsedTime = sw.Elapsed.Seconds;



            marker.Update();

            if (snake.checkInCollision() || snake.touchesScreenBounds() || marker.lifetime <= marker.elapsedTime)
            {
                gameRunning = false;
                marker.ShowGameOverMessage();
            }

            snake.Draw();
            foodManager.DrawFood();
            marker.Draw();
            Invalidate();

            //lastGameTime = sw.ElapsedMilliseconds;
        }
    }
}