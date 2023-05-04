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
    }
    public class IntVec
    {
        public int x, y;
        public IntVec(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public IntVec((int,int) tup)
        {
            x = tup.Item1;
            y = tup.Item2;
        }
        public (int,int) ToTuple()
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
            return new IntVec(self.x+other.x, self.y+other.y);
        }
        public static IntVec operator -(IntVec self, IntVec other)
        {
            return new IntVec(self.x - other.x, self.y - other.y);
        }

        //Integer scaling
        public static IntVec operator *(IntVec self, int val)
        {
            return new IntVec(self.x*val, self.y*val);
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

        //May need additional data such as:
            //The total snake length (in order to facilitate the snake update and point increment)
        
        public Snake((int,int) position, Form form)
        {
            attachedForm = form;
            //create the head
            head = new Segment(attachedForm, position, snakeWidth, snakeWidth, Direction.Up, Color.Red);

            IntVec newSegmentPos = new IntVec(head.position) - GetNormalVectorFromDir(head.direction) * snakeWidth;


            //add new segment next to head with length 0
            segments.Add(new Segment(attachedForm, newSegmentPos.ToTuple(), 1000, snakeWidth, head.direction, Color.Green));
            //create the first segment
            //AddSegmentAtHead();
        }
        public void Move(float deltaTime)
        {
            //calculates the movement distance and movement vector
            int delta = (int)(pixelsPerSecond * deltaTime);
            
            IntVec movementVector = GetNormalVectorFromDir(head.direction)*delta;

            //moves the head segment in its direction

            IntVec newHeadPos = new IntVec(head.position) + movementVector;
            //Debug.WriteLine($"pos: {head.position} new vec {movementVector.ToTuple()}, sum {(new IntVec(head.position) + movementVector).y} ");
            head.UpdateSegment(newHeadPos.ToTuple(), snakeWidth, snakeWidth, head.direction);
            //Debug.WriteLine(head.position);

            //updates the last segment to move to head with new length
            Segment closestSegment = segments[segments.Count-1];

            IntVec newClosestSegPos = new IntVec(closestSegment.position) + movementVector;

            closestSegment.UpdateSegment(newClosestSegPos.ToTuple(), closestSegment.length + delta, snakeWidth, closestSegment.direction);


            //updates the first segment and removes it if length 0 and not last segment
            int leftoverLength = delta;

            

            while (leftoverLength > 0) {
                if (segments[0].length > leftoverLength)
                {
                    segments[0].UpdateSegment(segments[0].position, segments[0].length - leftoverLength, snakeWidth, segments[0].direction);
                    break;
                }
                else
                {
                    int removedLength = segments[0].length;
                    if(segments.Count > 1)
                    {
                        segments[0].RemoveSegment();
                        segments.RemoveAt(0);
                    }
                    else
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
            IntVec centerPos = new IntVec(head.position) - (GetNormalVectorFromDir(head.direction)*snakeWidth)/2;
            //rotate head
            head.UpdateSegment((centerPos + (GetNormalVectorFromDir(direction) * snakeWidth)/2).ToTuple(), head.length, snakeWidth, direction);
            AddSegmentAtHead();
        }

        void AddSegmentAtHead()
        {
            IntVec newSegmentPos = new IntVec(head.position) - GetNormalVectorFromDir(head.direction) * snakeWidth;


            //add new segment next to head with length 0
            segments.Add(new Segment(attachedForm, newSegmentPos.ToTuple(), 0, snakeWidth, head.direction, Color.Green));
        }
        /*Direction GetOppositeDir(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default: return Direction.Up;
            }
        }*/
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
            

            

        //checkInCollision
            //Checks if snake head collides with any of the segments

            // potential pitfall - head colliding with one of the segments behind it
    }

    public class Segment// think about whether the head should actually inherit from the segment class (it doesn't actually have that many similarities with it)
    {
        public Direction direction { get; private set; }
        public int length { get; private set; }
        public (int, int) position { get; private set; }
        //Look into whether the intersections will be handled here or in the snake class
        public PictureBox visual { get; private set;}
        Color color;
        Form attachedForm;
        public Segment (Form form, (int, int) startPoint, int length, int width, Direction direction, Color color)
        {
            //create the visual and add it to form controls
            visual = new PictureBox ();
            visual.BackColor = color;
            attachedForm = form;
            attachedForm.Controls.Add (visual);
            UpdateSegment(startPoint, length, width, direction);
        }

        // update segment method that redraws the visual with new dimentions (from start point in the direction and with specified length)
        public void UpdateSegment ((int, int) startPoint, int length, int width, Direction direction)
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
        public void RemoveSegment ()
        {
            attachedForm.Controls.Remove(visual);
        }
        
        public bool isSegmentCollisioningWith (Rectangle bounds)
        {
            return visual.Bounds.IntersectsWith(bounds);
        }
    }   












    public class FoodManager
    {
        //get visuals and look for collitions, random timing, send bool to snake
    }
    public class Food
    {
        //create the visuals
    }

    /*public class Marker
    {
        Label label;

        public Marker(Form1 window, (int,int) pos)
        {
            label = new Label();
            window.Controls.Add(label);
            label.Location = new Point(pos.Item1, pos.Item2);
        }

        public void UpdateMarker(int points, int lifetime) // segun el tipo de comida, habrá que añadir una cantidad determinada de puntos
        {
            label.Text = $"{points}, {lifetime}";
        }
    }*/
}