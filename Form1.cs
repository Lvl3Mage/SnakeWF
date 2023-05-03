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
        // list of turns
        // list of segments

        //May need additional data such as:
            //The total snake length (in order to facilitate the snake update and point increment)


        // public move method that accepts the delta time since last frame 
            //moves the snake in its direction (updates each segment with each turn and removes the last segment and turn if the segment is of size 0)
        
        //Rotate snake method
            //rotates the snake, creating a new segment and a new turn

            

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

    public class Turn 
    {
        (int, int) position;
        Direction direction;

        public Turn((int, int) pos, Direction dir)
        {
            position = pos;
            direction = dir;
        }
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