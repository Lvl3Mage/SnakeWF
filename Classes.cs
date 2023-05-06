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
        int pixelsPerSecond = 200;
        int snakeWidth = 20;
        Form attachedForm;
        int accumulatedLength;
        (int, int) screenBounds;
        //May need additional data such as:
        //The total snake length (in order to facilitate the snake update and point increment)

        public Snake((int, int) position, Form form, (int,int) _screenBounds)
        {
            screenBounds = _screenBounds;
            attachedForm = form;

            accumulatedLength = snakeWidth * 3;
            //create the head
            head = new Segment(attachedForm, position, snakeWidth, snakeWidth, Direction.Up, Color.FromArgb(255, 189, 175, 161));

            IntVec newSegmentPos = new IntVec(head.position) - GetNormalVectorFromDir(head.direction) * snakeWidth;

            //add new segment next to head with length 0
            AddSegmentAtHead();
        }
        public void AddToLength(int val)
        {
            accumulatedLength += val*10;
        }
        float accumDelta = 0;
        public void Move(float deltaTime)
        {
            //calculates the movement distance and movement vector
            float theorDelta = (pixelsPerSecond * deltaTime) + accumDelta;
            accumDelta = 0;
            int delta = (int)theorDelta;

            accumDelta += theorDelta - delta;
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
            segments.Add(new Segment(attachedForm, newSegmentPos.ToTuple(), 0, snakeWidth, head.direction, Color.FromArgb(255, 205, 193, 179)));
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
            head.Draw();
            foreach (Segment segment in segments)
            {
                segment.Draw();
            }
        }

        public bool touchesScreenBounds()
        {
            return head.position.Item1 < 0 || head.position.Item1 > screenBounds.Item1 || head.position.Item2 < 0 || head.position.Item2 > screenBounds.Item2;
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

    public class Segment
    {
        public Direction direction { get; private set; }
        public int length { get; private set; }
        public (int, int) position { get; private set; }
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
            visual.BringToFront();
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
        const int spawnPadding = 20;
        List<Food> foodObjs = new List<Food>();
        Snake snake;
        Form attachedForm;
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
                for (int i = 0; i < loop; i++)
                {
                    (int, int) randomFoodPosition = (r.Next(spawnPadding, screenBounds.Item1 - spawnPadding), r.Next(spawnPadding, screenBounds.Item2 - spawnPadding));
                    int randomPoints = r.Next(2, 7);
                    Color randomColor = Color.Black;
                    switch (randomPoints)
                    {
                        case 2:
                            randomColor = Color.FromArgb(255, 242, 177, 121);
                            break;
                        case 3:
                            randomColor = Color.FromArgb(255, 245, 149, 99);
                            break;
                        case 4:
                            randomColor = Color.FromArgb(255, 246, 124, 96);
                            break;
                        case 5:
                            randomColor = Color.FromArgb(255, 246, 94, 59);
                            break;
                        case 6:
                            randomColor = Color.FromArgb(255, 237, 207, 115);
                            break;
                        default:
                            break;
                    }
                    int randomLifeTime = r.Next(10, 17);
                    Food food = new Food(randomFoodPosition, randomPoints, randomLifeTime, randomColor, attachedForm);
                    foodObjs.Add(food);
                }
            }

            for (int i = 0; i < foodObjs.Count; i++)
            {
                if (snake.checkHeadCollision(foodObjs[i].visual.Bounds))
                {
                    marker.points += foodObjs[i].points;
                    marker.lifetime += foodObjs[i].lifetime;
                    marker.Update();
                    snake.AddToLength(marker.points);
                    foodObjs[i].Remove();
                    foodObjs.RemoveAt(i);
                }
            }
        }

        public void DrawFood()
        {
            for (int i = 0; i < foodObjs.Count; i++)
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
            visual.Location = new Point(_position.Item1 - 8, _position.Item2-8);
            visual.BackColor = color;
            
            visual.Size = new Size(16, 16);
            attachedForm.Controls.Add(visual);
            visual.BringToFront();
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
        Label topLabel, bottomLabel;
        public int points;
        public double lifetime = 10;
        public double elapsedTime = 0;
        public Marker(Form1 window, int width, int height)
        {
            topLabel = new Label();
            window.Controls.Add(topLabel);

            topLabel.Location = new Point(0, 0);

            topLabel.ForeColor = Color.FromArgb(255, 237, 224, 200);
            topLabel.Font = new Font("Tahoma", 30, FontStyle.Bold);

            topLabel.Size = new Size(width, height/2);

            topLabel.TextAlign = ContentAlignment.MiddleCenter;
            topLabel.SendToBack();


            bottomLabel = new Label();
            window.Controls.Add(bottomLabel);

            bottomLabel.Location = new Point(0, height/2);

            bottomLabel.ForeColor = Color.FromArgb(255, 237, 224, 200);
            bottomLabel.Font = new Font("Tahoma", 30, FontStyle.Bold);

            bottomLabel.Size = new Size(width, height / 2);

            bottomLabel.TextAlign = ContentAlignment.MiddleCenter;
            bottomLabel.SendToBack();


            Update();
        }
        public void Draw()
        {
            bottomLabel.Refresh();
            topLabel.Refresh();
        }
        public void Update()
        {
            topLabel.Text = $"Score: {points}";
            bottomLabel.Text = $"Time left: {(lifetime-elapsedTime).ToString("0")} seconds";
        }
        public void ShowGameOverMessage()
        {
            topLabel.Text = "Game Over";

            bottomLabel.Text = $"Finished with {points} points";
        }
    }
}