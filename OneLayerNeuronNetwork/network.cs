using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace OneLayerNeuronNetwork
{
    class network
    {
        private int countNeuro; //Проверяет output и считает кол-во строк
        public List<double> input = new List<double>(4096);
        public List<neuron> net = new List<neuron>();
        public List<string> output = new List<string>();


        public double errorNet = 0;


        public string path = @"D:\proj\weight.txt";
        public string pathOut = @"D:\proj\output.txt";
        //string errorProgramm;


        double errorCompute()
        {
            double tmp;
            tmp = 0;
            for (int i = 0; i < net.Count(); i++)
            {
                tmp += net[i].error;
            }
            return tmp / 2;
        }

        public void addNeuron()
        {
            neuron tmp = new neuron();
            //tmp.rWeight();
            net.Add(tmp);
        }
        public void newNeuron(string output = null)
        {
            neuron tmp = new neuron();
            tmp.output = output;
            tmp.rWeight();
            net.Add(tmp);

        }
        public void getNeuron()
        {

            if (File.Exists(pathOut))
            {
                countNeuro = File.ReadAllLines(pathOut).Length;
                try
                {
                    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                    {
                        // Console.WriteLine(sr);
                        string strWeight;
                        int counter = 0;
                        while ((strWeight = sr.ReadLine()) != null)
                        {
                            newNeuron();
                            List<double> arrayWeight = new List<double>(4096);
                            string[] symvols = strWeight.Split(' ');
                            if (strWeight != "")
                            {
                                //                 newNeuron();
                            }
                            for (int i = 0; i < 4096; i++)
                            {
                                if (strWeight != "")
                                {
                                    if (symvols[i] != "")
                                    {
                                        arrayWeight.Add(Convert.ToDouble(symvols[i].Trim('"')));
                                    }
                                }
                            }
                            net[counter].addWeight(arrayWeight);
                            net[counter].output = output[counter];
                            counter++;
                        }
                        sr.Close();
                    }
                }
                catch (IOException e)
                {

                    MessageBox.Show(e.ToString());
                }

            }
            else
            {
                newNeuron();
            }



        }

        public void rasp()
        {
            double max = -1.0;
            int number_neuron;
            string output = "-1";
            //number_neuron = 0;
            for (int i = 0; i < net.Count(); i++)
            {
                Console.WriteLine("Номер нейрона: " + i + " Значение " + net[i].summ(input));
            }
            for (int i = 0; i < net.Count(); i++)
            {
                if (net[i].summ(input) > max)
                {
                    max = net[i].summ(input);
                    number_neuron = i;
                    output = net[i].output;
                    //    Console.WriteLine("Номер нейрона: " + i + " Значение " + max);
                    
                }

            }
            Console.WriteLine("Значение " + output);
            if (output == "0909")
            {
                Process.Start(@"C:\Users\Вадим\Desktop\numbers\training\Обучающая выборка\wtf.png");
            }else if (output != "-1")
            {
                MessageBox.Show("На картинке: " + output);
            }
            errorNet = errorCompute();
            // Console.WriteLine("number neuron: " + number_neuron + " Значение " +  max);

        }

        public bool learnNetwork(string yhat)
        {

            //    for (int i = 0; i < epoch; i++)
            //    {
            for (int j = 0; j < net.Count(); j++)
            {

            //    if (net[j].output == yhat)
            //    {
                    if (net[j].summ(input) == 1)
                    {
                        return false;
                    }
                    net[j].learn(yhat, input);
                    Console.WriteLine("Обучаем  " + j + " Нейрон");
              //  }
            }
            //  }

            errorNet = errorCompute();
            return true;
        }

        public void recogn()
        {

        }

        public void altReLearn(string yhat)
        {
            bool stat;
            do
            {
              stat = learnNetwork(yhat);
            } while (false);
        }
        public void getOutputText()
        {
            try
            {
                using (StreamReader outFile = new StreamReader(pathOut, System.Text.Encoding.Default))
                {
                    string strOut;

                    while ((strOut = outFile.ReadLine()) != null)
                    {
                        output.Add(strOut);

                    }
                    outFile.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Заглушка");
            }
        }
        public async void saveAllWeight()
        {
            System.IO.StreamWriter weightTxt = new System.IO.StreamWriter(path);
            for (int i = 0; i < net.Count(); i++)
            {   
                foreach (double w in net[i].weight)
                {
                    await weightTxt.WriteAsync(w + " ");
                }
                weightTxt.WriteLine();                              
            }
            weightTxt.Close();
        }


        public void saveOutput()
        {
            System.IO.StreamWriter outputTxt = new System.IO.StreamWriter(pathOut);
              for (int i = 0; i < net.Count(); i++)
              {
                 outputTxt.WriteLine(net[i].output.ToString());
              }
            outputTxt.Close();
        }
       
    }
}
