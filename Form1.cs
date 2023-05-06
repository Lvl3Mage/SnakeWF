using System.Diagnostics;
using System.Security.Cryptography;

namespace SnakeWF
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public partial class Form1 : Form // handle this class after the base ones have been written
    {

        int width = 735;
        int height = 500;
        Snake snake;

        public Form1()
        {
            InitializeComponent();
            snake = new Snake((100, 100), this);
            GameLoop();
            // do later once the snake and segments have been written
        }

        async void GameLoop()
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
        }
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

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            snake.Move(0.1f);
            snake.Draw();
            Invalidate();
        }
    }
    public class IntVec
    {
        public int x, y;
        public IntVec(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public IntVec((int, int) tup)
        {
            x = tup.Item1;
            y = tup.Item2;
        }
        public (int, int) ToTuple()
        {
            return (x, y);
        }


        //operators

        //basic addition
        public static IntVec operator -(IntVec other)
        {
            return new IntVec(-other.x, -other.y);
        }
        public static IntVec operator +(IntVec self, IntVec other)
        {
            return new IntVec(self.x + other.x, self.y + other.y);
        }
        public static IntVec operator -(IntVec self, IntVec other)
        {
            return new IntVec(self.x - other.x, self.y - other.y);
        }

        //Integer scaling
        public static IntVec operator *(IntVec self, int val)
        {
            return new IntVec(self.x * val, self.y * val);
        }
        public static IntVec operator *(int val, IntVec self)
        {
            return new IntVec(self.x * val, self.y * val);
        }

        public static IntVec operator /(IntVec self, int val)
        {
            return new IntVec(self.x / val, self.y / val);
        }
    }
    public class Snake
    {
        Segment head;
        List<Segment> segments = new List<Segment>();
        int pixelsPerSecond = 100;
        int snakeWidth = 30;
        Form attachedForm;
        int accumulatedLength = 1000;

        //May need additional data such as:
        //The total snake length (in order to facilitate the snake update and point increment)

        public Snake((int, int) position, Form form)
        {
            attachedForm = form;
            //create the head
            head = new Segment(attachedForm, position, snakeWidth, snakeWidth, Direction.Up, Color.Red);

            IntVec newSegmentPos = new IntVec(head.position) - GetNormalVectorFromDir(head.direction) * snakeWidth;

            //add new segment next to head with length 0
            segments.Add(new Segment(attachedForm, newSegmentPos.ToTuple(), 0, snakeWidth, head.direction, Color.Green));
        }
        public void AddToLength(int val)
        {
            accumulatedLength += val;
        }
        public void Move(float deltaTime)
        {
            //calculates the movement distance and movement vector
            int delta = (int)(pixelsPerSecond * deltaTime);
            IntVec movementVector = GetNormalVectorFromDir(head.direction) * delta;

            //moves the head segment in its direction
            IntVec newHeadPos = new IntVec(head.position) + movementVector;
            head.UpdateSegment(newHeadPos.ToTuple(), snakeWidth, snakeWidth, head.direction);


            //updates the last segment to move to head with new length
            Segment closestSegment = segments[segments.Count - 1];

            IntVec newClosestSegPos = new IntVec(closestSegment.position) + movementVector;

            closestSegment.UpdateSegment(newClosestSegPos.ToTuple(), closestSegment.length + delta, snakeWidth, closestSegment.direction);


            //sets the leftoverLength accounting for the accumulated length
            int leftoverLength = Math.Max(delta - accumulatedLength, 0);

            accumulatedLength = Math.Max(accumulatedLength - delta, 0);




            //subtracts from the last segment using leftover length (removing the segment if it hits 0 length) until the leftover length runs out
            while (leftoverLength > 0)
            {
                if (segments[0].length > leftoverLength)
                {
                    segments[0].UpdateSegment(segments[0].position, segments[0].length - leftoverLength, snakeWidth, segments[0].direction);
                    break;
                }
                else // segment is shorter than leftover length
                {
                    int removedLength = segments[0].length;
                    if (segments.Count > 1)
                    {
                        segments[0].RemoveSegment();
                        segments.RemoveAt(0);
                    }
                    else// in the case of there beign only one segment left the segment is not removed
                    {
                        segments[0].UpdateSegment(segments[0].position, segments[0].length - leftoverLength, snakeWidth, segments[0].direction);
                        break;
                    }

                    leftoverLength -= removedLength;
                }

            }
        }
        public void RotateSnake(Direction direction)//rotates the snake, creating a new segment
        {
            //check for 180 degree rotation
            IntVec combinedDir = GetNormalVectorFromDir(direction) + GetNormalVectorFromDir(head.direction);
            if (combinedDir.x == 0 && combinedDir.y == 0)
            {
                return;
            }

            //rotate head
            //find the center position of the head
            IntVec centerPos = new IntVec(head.position) - (GetNormalVectorFromDir(head.direction) * snakeWidth) / 2;
            //update the head with the new offset in the new direction
            head.UpdateSegment((centerPos + (GetNormalVectorFromDir(direction) * snakeWidth) / 2).ToTuple(), head.length, snakeWidth, direction);
            AddSegmentAtHead();
        }

        void AddSegmentAtHead()
        {


            IntVec newSegmentPos = new IntVec(head.position) - GetNormalVectorFromDir(head.direction) * snakeWidth;

            //add new segment next to head with length 0
            segments.Add(new Segment(attachedForm, newSegmentPos.ToTuple(), 0, snakeWidth, head.direction, Color.Green));
        }
        IntVec GetNormalVectorFromDir(Direction dir)//returns the direction represented as an IntVec
        {
            switch (dir)
            {
                case Direction.Up:
                    return new IntVec(0, -1);
                case Direction.Down:
                    return new IntVec(0, 1);
                case Direction.Left:
                    return new IntVec(-1, 0);
                case Direction.Right:
                    return new IntVec(1, 0);
                default: return new IntVec(0, 0);
            }
        }
        public void Draw()
        {
            foreach(Segment segment in segments) {
                segment.Draw();
            }
        }


        public bool checkInCollision()
        {
            foreach (Segment segment in segments)
            {
                if (segment.isSegmentCollisioningWith(head.visual.Bounds)) { return true; }
            }
            return false;
        }
        public bool checkHeadCollision(Rectangle Bounds)
        {
            return head.isSegmentCollisioningWith(Bounds);
        }
    }

    public class Segment// think about whether the head should actually inherit from the segment class (it doesn't actually have that many similarities with it)
    {
        public Direction direction { get; private set; }
        public int length { get; private set; }
        public (int, int) position { get; private set; }
        //Look into whether the intersections will be handled here or in the snake class
        public PictureBox visual { get; private set; }
        Color color;
        Form attachedForm;
        public Segment(Form form, (int, int) startPoint, int length, int width, Direction direction, Color color)
        {
            //create the visual and add it to form controls
            visual = new PictureBox();
            visual.BackColor = color;
            attachedForm = form;
            attachedForm.Controls.Add(visual);
            UpdateSegment(startPoint, length, width, direction);
        }

        // update segment method that redraws the visual with new dimentions (from start point in the direction and with specified length)
        public void UpdateSegment((int, int) startPoint, int length, int width, Direction direction)
        {
            this.direction = direction;
            this.length = length;
            this.position = startPoint;
            int xAux;
            int yAux;
            int segmentX = startPoint.Item1;
            int segmentY = startPoint.Item2;


            //adjusting picture box position depending on the direction and the width/length of the segment
            switch (direction)
            {
                case Direction.Up:
                    xAux = segmentX - width / 2;
                    yAux = segmentY;
                    visual.Location = new Point(xAux, yAux);
                    visual.Size = new Size(width, length);
                    break;
                case Direction.Down:
                    xAux = segmentX - width / 2;
                    yAux = segmentY - length;
                    visual.Location = new Point(xAux, yAux);
                    visual.Size = new Size(width, length);
                    break;
                case Direction.Left:
                    xAux = segmentX;
                    yAux = segmentY - width / 2;
                    visual.Location = new Point(xAux, yAux);
                    visual.Size = new Size(length, width);
                    break;
                case Direction.Right:
                    xAux = segmentX - length;
                    yAux = segmentY - width / 2;
                    visual.Location = new Point(xAux, yAux);
                    visual.Size = new Size(length, width);
                    break;
                default:
                    break;
            }
        }
        public void RemoveSegment()
        {
            attachedForm.Controls.Remove(visual);
        }

        public bool isSegmentCollisioningWith(Rectangle bounds)
        {
            return visual.Bounds.IntersectsWith(bounds);
        }
        public void Draw()
        {
            visual.Refresh();
        }
    }




    public class FoodManager
    {
        List<Food> foodObjs = new List<Food>();
        Snake snake;
        Form attachedForm;
        int desiredFoodAmount = 4;
        (int, int) screenBounds;
        Random r;
        Marker marker;
        public FoodManager(Snake _snake, Form _attachedForm, (int, int) _screenBounds, Marker _marker)
        {
            marker = _marker;
            r = new Random();
            snake = _snake;
            attachedForm = _attachedForm;
            screenBounds = _screenBounds;
        }
        public void Update()
        {
            if (foodObjs.Count == 0)
            {
                int loop = r.Next(1, 4);
                for(int i = 0; i < loop; i++)
                {
                    (int, int) randomFoodPosition = (r.Next(736), r.Next(501));
                    int randomPoints = r.Next(2, 7);
                    Color randomColor;
                    switch (randomPoints)
                    {
                        case 2:
                            randomColor = Color.Red;
                            break;
                        case 3:
                            randomColor = Color.Blue;
                            break;
                        case 4:
                            randomColor = Color.Green;
                            break;
                        case 5:
                            randomColor = Color.Yellow;
                            break;
                        case 6:
                            randomColor = Color.Purple;
                            break;
                        default:
                            break;
                    }
                    int randomLifeTime = r.Next(10, 17);
                    Food food = new Food(randomFoodPosition, randomPoints, randomLifeTime, randomColor, attachedForm);
                    foodObjs.Add(food);
                }
            }
            
            for(int i = 0; i < foodObjs.Count; i++)
            {
                if (snake.checkHeadCollision(foodObjs[i].visual.Bounds))
                {
                    marker.points += foodObjs[i].points;
                    marker.lifetime += foodObjs[i].lifetime;
                    snake.AddToLength(marker.points);
                    foodObjs[i].Remove();
                    foodObjs.RemoveAt(i);
                }
            }
        }

        public void DrawFood()
        {
            for(int i = 0; i < foodObjs.Count; i++)
            {
                foodObjs[i].Draw();
            }
        }
    }
    public class Food 
    {

        public int points { get; private set; }
        public int lifetime { get; private set; }
        public PictureBox visual { get; private set; }
        Form attachedForm;
        public Food((int, int) _position, int _points, int _lifetime, Color color, Form _attachedForm)
        {
            attachedForm = _attachedForm;
            points = _points;
            lifetime = _lifetime;

            visual = new PictureBox();
            visual.Location = new Point(_position.Item1, _position.Item2);
            visual.BackColor = color;
            visual.Size = new Size(15, 15);
            attachedForm.Controls.Add(visual);
        }
        public void Remove()
        {
            attachedForm.Controls.Remove(visual);
        }
        public void Draw()
        {
            visual.Refresh();
        }
    }

    public class Marker
    {
        Label label;
        public int points, lifetime = 10;
        public Marker(Form1 window, (int, int) pos)
        {
            label = new Label();
            window.Controls.Add(label);
            label.Location = new Point(pos.Item1, pos.Item2);
            Update();
        }
        public void Draw()
        {
            label.Refresh();
        }
        public void Update()
        {
            label.Text = $"Points: {points}, Time left: {lifetime}";
        }
    }
}