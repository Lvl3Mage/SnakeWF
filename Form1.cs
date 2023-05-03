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


        public Form1() 
        {
            // do later once the snake and segments have been written
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            //Handle key presses and change the direction of the snake
        }
    }

    public class Snake 
    {
        Segment head;
        List<Segment> segments = new List<Segment>();
        int pixelsPerSecond = 100;
        int snakeWidth = 50;

        //May need additional data such as:
            //The total snake length (in order to facilitate the snake update and point increment)
        
        public Snake((int,int) position)
        {
            //create the head

            //create the first segment
        }
        public void Move(float deltaTime)
        {

            //calculates the movement distance and movement vector
            int delta = (int)(pixelsPerSecond * deltaTime);
            (int, int) movementVector = (0,0);
            switch (head.direction)
            {
                case Direction.Up:
                    movementVector = (0, delta);
                    break;
                case Direction.Down:
                    movementVector = (0, -delta);
                    break;
                case Direction.Left:
                    movementVector = (-delta, 0);
                    break;
                case Direction.Right:
                    movementVector = (delta, 0);
                    break;
            }
            
            //moves the head segment in its direction
            (int, int) newHeadPos = head.position;
            newHeadPos = (newHeadPos.Item1 + movementVector.Item1, newHeadPos.Item2 + movementVector.Item2);
           
            head.UpdateSegment(newHeadPos, snakeWidth, snakeWidth, head.direction);


            //updates the last segment to move to head with new length
            Segment closestSegment = segments[segments.Count-1];

            (int, int) newClosestSegPos = closestSegment.position;
            newClosestSegPos = (newClosestSegPos.Item1 + movementVector.Item1, newClosestSegPos.Item2 + movementVector.Item2);

            closestSegment.UpdateSegment(newClosestSegPos, closestSegment.length + delta, snakeWidth, closestSegment.direction);


            //updates the first segment and removes it if length 0 and not last segment

            Segment lastSegment = segments[0];
            lastSegment.UpdateSegment(lastSegment.position, lastSegment.length - delta, snakeWidth, lastSegment.direction);
            if(segments.Count > 1 && lastSegment.length <= 0) {
                segments.RemoveAt(0);
            }
        }
        public void RotateSnake(Direction direction)//rotates the snake, creating a new segment
        {
            //move head to new position and change direction
            //update previous segment to the previous head position
            //decrease the length of the last segment by the head movement
            // add new segment next to head with length 0
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
        PictureBox visual;
        Color color;
        Form attachedForm;
        public Segment (Form form)
        {
            //create the visual and add it to form controls
            attachedForm = form;
            //call the update method
        }

        public void UpdateSegment ((int, int) startPoint, int length, int width, Direction direction)
        {
            this.direction = direction;
            this.length = length;
            this.position = startPoint;
            int xAux;
            int yAux;
            int segmentX = startPoint.Item1;
            int segmentY = startPoint.Item2;

            switch (direction)
            {
                case Direction.Up:
                    xAux = segmentX - width / 2;
                    yAux = segmentY;
                    visual.Location = new Point(xAux, yAux);
                    break;
                case Direction.Down:
                    xAux = segmentX - width / 2;
                    yAux = segmentY - length;
                    visual.Location = new Point(xAux, yAux);
                    break;
                case Direction.Left:
                    xAux = segmentX;
                    yAux = segmentY - length / 2;
                    visual.Location = new Point(xAux, yAux);
                    break;
                case Direction.Right:
                    xAux = segmentX - width;
                    yAux = segmentY - length / 2;
                    visual.Location = new Point(xAux, yAux);
                    break;
                default:
                    break;
            }
        }
        // update segment method that redraws the visual with new dimentions (from start point in the direction and with specified length)
        
        // remove segment method that removes the segment from the form controls
    }   












    public class FoodManager
    {
        
    }
    public class Food
    {

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