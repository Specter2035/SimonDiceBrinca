using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SimonDiceBrinca_1_
{
    public partial class Form1 : Form
    {
        int blocksX = 80;
        int blocksY = 80;
        int puntaje = 0;

        int level = 3;
        List<PictureBox> pictureBoxes = new List<PictureBox>();
        List<PictureBox> BoxesSeleccionados = new List<PictureBox>();
        List<Color> colores = new List<Color> { Color.Red, Color.Yellow, Color.Blue, Color.Green };
        Random rnd = new Random();

        Color temp;

        int index = 0;
        int intentos = 0;

        int LimiteTiempo = 0;
        bool SeleccionandoColores = false;

        string OrdenCorrecto = String.Empty;
        string OrdenSeleccionado = String.Empty;

        public Form1()
        {
            InitializeComponent();
            AcomodarBlocks();
        }

        private void EventoTimer(object sender, EventArgs e)
        {
            if (SeleccionandoColores)
            {
                LimiteTiempo++;
                switch (LimiteTiempo)
                {
                    case 10:
                        temp = BoxesSeleccionados[index].BackColor;
                        BoxesSeleccionados[index].BackColor = Color.White;
                        break;
                    case 20:
                        BoxesSeleccionados[index].BackColor = temp;
                        break;
                    case 30:
                        if (index < BoxesSeleccionados.Count-1)
                        {
                            index++;
                            LimiteTiempo = 0;
                        }
                        else
                        {
                            SeleccionandoColores = false;
                            CambiarColores();
                        }
                        break;
                }
            }
            
            if (intentos >= level)
            {
                
                if (OrdenCorrecto == OrdenSeleccionado)
                {
                    intentos = 0;
                    TimerJuego.Stop();
                    MessageBox.Show("Seleccionaste el orden de colores correcto.");
                    puntaje++;
                }
                else
                {
                    intentos = 0;
                    TimerJuego.Stop();
                    MessageBox.Show("No seleccionaste el orden de colores correcto.");
                }
            }

            label2.Text = "Haz click en la misma secuencia de "+level+" colores";
        }

        private void btnEventoClick(object sender, EventArgs e)
        {
            if (puntaje == 1 && level < 7)
            {
                level++;
                puntaje = 0;
            }
            OrdenCorrecto = string.Empty;
            OrdenSeleccionado = string.Empty;

            //Crear una nueva lista para guardar pictureboxes unicos para boxes seleccionados
            List<PictureBox> PictureBoxesunicos = pictureBoxes.OrderBy(x => rnd.Next()).ToList();

            BoxesSeleccionados.Clear();
            //Asegurarse que boxesseleccionados contenga solo cuatro unicos pictureboxes
            for (int i = 0; i < level; i++)
            {
                BoxesSeleccionados.Add(PictureBoxesunicos[i % pictureBoxes.Count]);
                OrdenCorrecto += BoxesSeleccionados[i].BackColor.ToString() + " ";
            }

            Debug.WriteLine(OrdenCorrecto);
            index = 0;
            LimiteTiempo = 0;
            SeleccionandoColores = true;
            TimerJuego.Start();
        }

        
        private void AcomodarBlocks()
        {            
            for (int i = 1; i < 5; i++)
            {
                PictureBox NuevoPB = new PictureBox();
                NuevoPB.Name = "pic_" + i;
                NuevoPB.Height = 63;
                NuevoPB.Width = 63;
                //NuevoPB.BackColor = Color.Black;
                NuevoPB.Left = blocksX;
                NuevoPB.Top = blocksY;
                NuevoPB.Click += ClickEnPictureBox;

                if (i == 2 || i == 4) 
                {
                    blocksY += 65;
                    blocksX = 80;
                }
                else
                {
                    blocksX += 65;
                }

                this.Controls.Add(NuevoPB);
                pictureBoxes.Add(NuevoPB);
            }


            pictureBoxes[0].BackColor = Color.Red;
            pictureBoxes[1].BackColor = Color.Yellow;
            pictureBoxes[2].BackColor = Color.Blue;
            pictureBoxes[3].BackColor = Color.Green;


        }

        private void ClickEnPictureBox(object sender, EventArgs e)
        {
            if (!SeleccionandoColores && BoxesSeleccionados.Count > 1)
            {
                
                PictureBox temp = sender as PictureBox;
                /*temp.BackColor = Color.Black;*/
                OrdenSeleccionado += temp.BackColor + " ";
                
                Debug.WriteLine(OrdenSeleccionado);
                intentos++;
            }
            else
            {
                return;
            }
        }

        private void CambiarColores()
        {
            //Reordenar los colores de manera aleatoria
            colores = colores.OrderBy(c => rnd.Next()).ToList();

            //Asegurarse que los colores no se repitan en la secuencia
            HashSet<Color> Coloresunicos = new HashSet<Color>();

            //Aplicar colores unicos a los pictureboxes
            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                //Asegurarse que el color es unico
                Color Colorunico;
                do
                {
                    Colorunico = colores[rnd.Next(colores.Count)];
                } while (!Coloresunicos.Add(Colorunico));

                pictureBoxes[i].BackColor = Colorunico;
            }
        }
    }
}
