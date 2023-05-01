namespace SnakeWF
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public partial class Form1 : Form
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
            //Handle key presses and change the direction
        }
    }

    public class Snake 
    {
        Segment head;
        // list of turns
        // list of segments

        // public move method that accepts the delta time since last frame 
            //moves the snake in its direction (updates each segment with each turn and removes the last segment and turn if the segment is of size 0)
        
        //Rotate snake method
            //rotates the snake, creating a new segment and a new turn

            // potential pitfall - head colliding with one of the segments behind it

        //isAlive
            //Checks if snake head collides with any of the walls/segments
    }

    public class Segment
    {
        PictureBox visual;
        (int, int) startPoint;
        (int, int) endPoint;
        Color color;
        Form attachedForm;
        public Segment ((int,int) _startPoint, (int,int) _endPoint, Form form)
        {
            //create the visual and add it to form controls
            attachedForm = form;
            startPoint = _startPoint;
            endPoint = _endPoint;
            //call the update method
        }
        // update segment method that redraws the visual with new dimentions (from start point to end point)
        
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