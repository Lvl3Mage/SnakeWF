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
        List<Segment> segments = new List<Segment> ();
        List<Turn> turns = new List<Turn> ();
        int pixelsPerSecond = 100;

        //May need additional data such as:
            //The total snake length (in order to facilitate the snake update and point increment)

        public void Move(float deltaTime)
        {
            int delta = (int)(pixelsPerSecond * deltaTime);
            //moves the head segment in its direction

            //updates the last segment to move to head with new length

            //updates the first segment and removes it if length 0
        }
        // public move method that accepts the delta time since last frame 
            //moves the snake in its direction (updates each segment with each turn and removes the last segment and turn if the segment is of size 0)
        
        //Rotate snake method
            //rotates the snake, creating a new segment and a new turn

            

        //checkInCollision
            //Checks if snake head collides with any of the segments

            // potential pitfall - head colliding with one of the segments behind it
    }

    public class Segment // think about whether the head should actually inherit from the segment class (it doesn't actually have that many similarities with it)
    {
        //Look into whether the intersections will be handled here or in the snake class
        PictureBox visual;
        (int, int) startPoint;
        int length;
        Direction direction;
        Color color;
        Form attachedForm;
        public Segment ((int,int) _startPoint, Form form)
        {
            //create the visual and add it to form controls
            attachedForm = form;
            startPoint = _startPoint;
            //call the update method
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

    public class Food
    {
        // has position, color and point count
        // has an attached PictureBox
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