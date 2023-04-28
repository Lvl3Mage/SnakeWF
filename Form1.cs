namespace SnakeWF
{
    public delegate void TickHandler();
    public delegate void LifetimeHandler(int lifetime);
    public partial class Form1 : Form
    {
        enum SnakeDir
        {
            Up,
            Down,
            Left,
            Right
        }
        Dictionary<(int, int), Cell> snakeCells = new Dictionary<(int, int), Cell>();
        Dictionary<(int, int), Cell> foodCells = new Dictionary<(int, int), Cell>();
        event TickHandler tickEvent;
        event LifetimeHandler lifetimeEvent;
        int snakeX = 5, snakeY = 5;
        SnakeDir snakeDir = SnakeDir.Up;
        Keys lastKey = Keys.None;
        (int, int) gridSize = (30,20);
        int gridScale = 30;
        int snakeLength = 1;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(gridSize.Item1*gridScale +gridScale, gridSize.Item2 * gridScale + gridScale*2);
            this.KeyDown += OnKeyDown;
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            GameLoop();
        }

        void OnKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.S:
                case Keys.A:
                case Keys.D:
                    lastKey = e.KeyCode;
                    break;
                default:
                    return;
            }
        }
        async void GameLoop()
        {
            while (true)
            {
                RotateSnake();
                MoveSnake();

                if (snakeCells.ContainsKey((snakeX, snakeY)))
                {
                    break;
                }

                if (tickEvent != null)
                {
                    tickEvent.Invoke();

                }
                

                if (foodCells.ContainsKey((snakeX, snakeY)))
                {
                    foodCells[(snakeX, snakeY)].RemoveCell();
                    snakeLength+=1;
                    if (lifetimeEvent != null)
                    {
                        lifetimeEvent.Invoke(snakeLength);
                    }
                }
                CreateSnakeCell(snakeX, snakeY);
                //DrawSnake();


                if (foodCells.Count < 1)
                {
                    (int, int) pos = GetEmptyPos();
                    if (pos.Item1 != -1)
                    {
                        CreateFoodCell(pos.Item1, pos.Item2, 50);
                    }
                }


                await Task.Delay(200);

            }
        }
        List<Label> snake = new List<Label>();
        void DrawSnake()
        {
            foreach(Label label in snake)
            {
                Controls.Remove(label);

            }
            foreach (Cell cell in snakeCells.Values)
            {
                Label label = new Label();
                label.Location = new Point(cell.position.Item1 * gridScale, cell.position.Item2 * gridScale);
                label.Size = new Size(gridScale, gridScale);
                label.BackColor = Color.DarkGreen;
                Controls.Add(label);
                snake.Add(label);

            }
        }
        (int, int) GetEmptyPos()
        {
            List<(int, int)> emptyPositions = new List<(int, int)>();
            for(int i = 0; i < gridSize.Item1; i++)
            {
                for (int j = 0; j < gridSize.Item2-1; j++)
                {
                    if(!snakeCells.ContainsKey((i,j)) && !foodCells.ContainsKey((i, j)))
                    {
                        emptyPositions.Add((i, j));
                    }
                }
            }
            if(emptyPositions.Count == 0) { return (-1, -1); }
            Random random = new Random();
            return emptyPositions[random.Next(0, emptyPositions.Count)];
        }
        void RotateSnake()
        {
            switch((lastKey, snakeDir))// gate condition
            {
                case (Keys.W, SnakeDir.Down):
                case (Keys.S, SnakeDir.Up):
                case (Keys.A, SnakeDir.Right):
                case (Keys.D, SnakeDir.Left):
                    return;
                default:
                    break;
            }
            switch (lastKey)
            {
                case (Keys.W):
                    snakeDir = SnakeDir.Up; 
                    break;
                case (Keys.S):
                    snakeDir = SnakeDir.Down;
                    break;
                case (Keys.A):
                    snakeDir = SnakeDir.Left;
                    break;
                case (Keys.D):
                    snakeDir = SnakeDir.Right;
                    break;
                default:
                    break;
            }
        }

        void MoveSnake()
        {
            switch (snakeDir)
            {
                case SnakeDir.Up:
                    snakeY--;
                    break;
                case SnakeDir.Down:
                    snakeY++;
                    break;
                case SnakeDir.Left:
                    snakeX--;
                    break;
                case SnakeDir.Right:
                    snakeX++;
                    break;
                default:
                    break;
            }
            if (snakeY < 0) { snakeY = gridSize.Item2-1; }
            if (snakeY >= gridSize.Item2) { snakeY = 0; }
            if (snakeX < 0) { snakeX = gridSize.Item1-1; }
            if (snakeX >= gridSize.Item1) { snakeX = 0; }
        }
        void CreateSnakeCell(int x, int y)
        {
            Label label = new Label();
            label.Location = new Point(x * gridScale, y * gridScale);
            label.Size = new Size(gridScale, gridScale);
            Controls.Add(label);
            label.BackColor = Color.Green;

            Cell cell = new Cell((x, y), snakeLength,  (Cell cell) => { snakeCells.Remove((cell.position.Item1, cell.position.Item2)); Controls.Remove(cell.drawnLabel); }, label, Color.Green, Color.FromArgb(255,60,90,30));
            tickEvent += cell.OnTick;
            lifetimeEvent += cell.UpdateLifetime;
            snakeCells[(x, y)] = cell;
        }
        void CreateFoodCell(int x, int y, int lifespan)
        {
            Label label = new Label();
            label.Location = new Point(x * gridScale, y * gridScale);
            label.Size = new Size(gridScale, gridScale);
            label.BackColor = Color.Purple;
            Controls.Add(label);
            Cell cell = new Cell((x, y), lifespan, (Cell cell) => { foodCells.Remove((cell.position.Item1, cell.position.Item2)); Controls.Remove(cell.drawnLabel); }, label, Color.Purple, Color.Red);
            tickEvent += cell.OnTick;
            foodCells[(x, y)] = cell;
        }
    }
    public class Cell
    {

        public delegate void CellClearHandler(Cell pos);

        public (int, int) position { get; private set; }
        int lifetime;
        int age = 0;
        CellClearHandler clearCallback;
        public Label drawnLabel { get; private set; }
        Color startColor, endColor;
        public Cell((int, int) _position, int _lifetime, CellClearHandler _clearCallback, Label _drawnLabel, Color _startColor, Color _endColor)//
        {
            lifetime = _lifetime;
            position = _position;
            clearCallback = _clearCallback;
            drawnLabel = _drawnLabel;
            startColor = _startColor;
            endColor = _endColor;
        }
        public void UpdateLifetime(int _lifetime)
        {
            lifetime = _lifetime;
        }
        public void OnTick()
        {
            age++;
            if (age>=lifetime)
            {
                RemoveCell();
                return;
            }

            drawnLabel.BackColor = MathUtils.colorLerp(startColor, endColor, ((float)age / (float)lifetime));
        }
        public void RemoveCell()
        {
            drawnLabel.BackColor = Color.Red;
            clearCallback.Invoke(this);
        }
    }
    public static class MathUtils
    {
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
        public static Color colorLerp(Color a, Color b, float t)
        {
            return Color.FromArgb(255,(int)Lerp(a.R, b.R, t), (int)Lerp(a.G, b.G, t), (int)Lerp(a.B, b.B, t));
        }
    }

}