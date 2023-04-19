namespace SnakeWF
{
    public enum Direccion
    {
        Arriba,
        Abajo,
        Derecha,
        Izquierda
    }
    public partial class Form1 : Form
    {
        Keys arriba = Keys.Up;
        Keys abajo = Keys.Down;
        Keys derecha = Keys.Right;
        Keys izquierda = Keys.Left;

        int anchoEscenario = 735;
        int alturaEscenario = 500;

        int x;
        int y;

        Marcador marcador;
        Serpiente serpiente;
        Comida comida;
        public Direccion DireccionActual { get; set; }

        List<Giro> giros;

        public Form1() // constructor del juego, inicializamos todo
        {
            InitializeComponent();
            x = anchoEscenario / 2; // la mitad de la ventana
            y = alturaEscenario / 2;
            direccionActual = Direccion.Arriba;
            this.Width = anchoEscenario;
            this.Height = alturaEscenario;
            this.BackColor = Color.LightGray;
            serpiente = new Serpiente();
            Controls.Add(Serpiente.MiPictureBox); // añadimos a una lista especial Controls todos los elementos visuales de la escena
            comida = new Comida();
            Controls.Add(comida.MiPictureBox);
            marcador = new Marcador(…);
            Controls.Add(marcador.MiLabel);
            marcador.MiLabel.SendToBack();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TeclaPulsada(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case arriba:
                    if (DireccionActual != Direccion.Arriba && DireccionActual != Direccion.Abajo) // basicamente si estamos yendo para una direccion, no podemos volver a ir para la misma o la contraria
                    {
                        DireccionActual = Direccion.Arriba;
                        giros.Add(new Giro())
                    }
                    break;
                case abajo:
                    if (DireccionActual != Direccion.Arriba && DireccionActual != Direccion.Abajo)
                    {
                        DireccionActual = Direccion.Abajo;
                    }
                    break;
                case derecha:
                    if (DireccionActual != Direccion.Derecha && DireccionActual != Direccion.Izquierda)
                    {
                        DireccionActual = Direccion.Derecha;
                    }
                    break;
                case izquierda:
                    if (DireccionActual != Direccion.Derecha && DireccionActual != Direccion.Izquierda)
                    {
                        DireccionActual = Direccion.Izquierda;
                    }
                    break;
                default:
                    return;
            }
        }
    }

    public class Serpiente // modificar
    {
        Segmento cabeza = new Segmento((0, 0));
    }

    public class Segmento // modificar
    {
        PictureBox segmentoCola;
        (int, int) posicionCola;
        Color colorSegmento;
        int tamañoSegmento;
         
        public Segmento ((int,int) posicionCola)
        {
            this.posicionCola = posicionCola;
            this.tamañoSegmento = 
        }
    }

    public class Giro // hay que modificar, pero está medio bien
    {
        (int, int) posicionGiro;
        Direccion direccionGiro;

        public Giro ((int, int) posicionGiro, Direccion direccionGiro)
        {
            this.posicionGiro = posicionGiro;
            this.direccionGiro = direccionGiro;
        }
    }

    public class Comida
    {

    }

    public class Marcador
    {
        Public int Puntos { get; set; };
        double segundosVida;

        public Marcador()
        {
            this.puntos = 0;
            this.segundosVida = 10;
        }

        public void SumarPuntos(Comida comida) // segun el tipo de comida, habrá que añadir una cantidad determinada de puntos
        {
            switch (comida)
            {
                case 
            }
        }
    }
}