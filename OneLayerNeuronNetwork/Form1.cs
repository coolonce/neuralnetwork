using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OneLayerNeuronNetwork
{
    public partial class Form1 : Form
    {
        Bitmap image;
        //mvp
        network network = new network();
        neuron neuro = new neuron();

     //   List<double> input = neuro.input; // Лист входов 

        public Form1()
        {
            InitializeComponent();
        }
        public Bitmap toWb()
        {
            System.IO.StreamWriter sourceMtrx = new System.IO.StreamWriter(@"D:\proj\sourceMtrx.txt");
            Int32 x, y;
            int c, summRGB;
            int P = 127;
            
            Bitmap result = new Bitmap(image.Width, image.Height);
            network.input.Clear();
            for (x = 0; x < image.Width; x++)
            {
                for (y = 0; y < image.Height; y++)
                {
                    Color color = image.GetPixel(x, y);
                    int alpha = color.A;
                    summRGB = color.R + color.G + color.B;
                    c = (summRGB) / 3;
                    // cAwg = Color.FromArgb(c);
                    //image.SetPixel(x, y, cAwg);
                    
                    result.SetPixel(x, y, (c <= P ? Color.FromArgb(255, 0, 0, 0) : Color.FromArgb(255, 255, 255, 255)));
//                  input.Add(result.GetPixel(x, y));
                    sourceMtrx.Write(summRGB.ToString() + " ");
                 //   network.input.Add(summRGB);
                    
                    if(c == 255)
                        network.input.Add(0);
                    else
                        network.input.Add(1);

                }
                sourceMtrx.WriteLine();
            }
            //pictureBox1.Image = result;
            sourceMtrx.Close();
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";

            if (open_dialog.ShowDialog() == DialogResult.OK) //если в окне была нажата кнопка "ОК"
            {
                try
                {
                    //MessageBox.Show(open_dialog.FileName);
                    image = new Bitmap(Image.FromFile(open_dialog.FileName /*@"C:\Users\Вадим\Desktop\img.jpg"*/), 64, 64); // Изменение размера картинки
                    
                    pictureBox1.Image = toWb();

                    network.rasp();
                    pictureBox1.Invalidate();

                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string yhat;
            try
            {
                yhat = textBox2.Text;
                 network.learnNetwork(yhat);
               // network.altReLearn(yhat);
                /*
                for (int i= 0; i < network.net.Count(); i++)
                {         
                    textBox1.Text += "number neuron " + i + " " + network.net[i].output.ToString() +"\r\n";
                }*/

                label7.Text = network.errorNet.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Введите значение");
            }
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            network.getOutputText();

            if (File.Exists(network.path)){

                //  neuro.
                network.getNeuron();
                label5.Text = "Данные загружены";
                //    label5.Text += tmpNetwork.net[0].weight.Count;  
                labelCountNeuron.Text = network.net.Count().ToString();             
             }
            else
            {
                
          //      network.newNeuron();                
                label5.Text = "Random weight add.";             
               
            }
            
               
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void buttonNewChar_Click(object sender, EventArgs e)
        {
            network.newNeuron();
            labelCountNeuron.Text = network.net.Count().ToString();
        }

        private void labelCountNeuron_Click(object sender, EventArgs e)
        {

        }

        private void buttonSaveWeight_Click(object sender, EventArgs e)
        {
            network.saveAllWeight();
            network.saveOutput();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string output = textBox2.Text;
                network.newNeuron(output);
                labelCountNeuron.Text = network.net.Count().ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Введите значение");
                
            }
            

            


        }

        private void button5_Click(object sender, EventArgs e)
        {
            network.rasp();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Вадим\Desktop\numbers\training\Обучающая выборка";
            int countFiles = 0;
            //Bitmap image;
            string[] pathFiles;
            string[] yhatStr;
            string yhat;
            foreach (var item in Directory.GetDirectories(path))
            {
                Console.WriteLine(item);
                countFiles = Directory.GetFiles(item).Length;
                pathFiles = Directory.GetFiles(item);
                yhatStr = item.Split('\\');
                yhat = yhatStr[yhatStr.Length - 1];
                network.newNeuron(yhat);
                labelCountNeuron.Text = network.net.Count().ToString();
                foreach (var file in pathFiles)
                {
                    string[] end = file.Split('\\');

                    if (end[end.Length-1] != "Thumbs.db")
                    {
                        using (image = new Bitmap(Image.FromFile(file), 64, 64))
                        {
                            toWb();
                            network.learnNetwork(yhat);
                           // label7.Text = network.errorNet.ToString();
                        }
                    }
                }
                Console.WriteLine(countFiles.ToString());
            }
            network.saveAllWeight();
            network.saveOutput();
            //MessageBox.Show(countFolder.ToString());



        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }
    }
}
