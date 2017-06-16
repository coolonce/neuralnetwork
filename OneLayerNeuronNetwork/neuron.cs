using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace OneLayerNeuronNetwork
{
   
    class neuron
    {

       // public List<double> input = new List<double>(1024);        //Матрица входов
        public List<double> weight = new List<double>(4096);       //Матрица весов  

        public double error = 0; 
        public string output;
        public double outputSumm;
        public void rWeight() {
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < weight.Capacity; i++)
                weight.Add(random.NextDouble() * 0.05 * (random.Next(2) == 1 ? -1 : 1)); //заполнение весов случайными занчениями
            
        }
        public void addWeight(List<double> w)
        {
            weight = w;        
        }
        public double summ(List<double> input)                       // Сумматорная функция
        {
            double result;
            result = 0;         
            
            for (int i = 0; i < weight.Count(); i++)
            {                
                result += input[i] * this.weight[i];
            }
            outputSumm = activation(result);           
            return activation(result);
        }
        public double activation(double sum)
        {     
                
            return 1/(1+Math.Exp(-sum));
        }
        
        /*
        * yhat -требуемое значение  y - вывод персептрона
        * alpha - Скорость обучения 
        * weight - вес входа
        * x - вход
        * */
        public void learn(string yhat, List<double> input)
        {
            double alpha = 0.0015;
            double y = summ(input);
            bool yh = output == yhat;
            int yh1 = Convert.ToInt32(yh); 
            //error = (yh1 - y) * (yh1 - y);
            error = (yh1 - y);
            Console.WriteLine("Output neuron " + output);
            Console.WriteLine("y: "+y);
            for (int i = 0; i < weight.Count ; i++)
            {
                weight[i] = weight[i] + alpha * error * input[i];             
            }
        }

       
        /*
        public async void save()
        {            
            System.IO.StreamWriter weightTxt = new System.IO.StreamWriter(@"D:\proj\weight.txt");
            foreach(double w in weight)
            {
             await   weightTxt.WriteLineAsync(w+"");
            }
            weightTxt.Close();
        }*/
        public void saveOutput()
        {
            System.IO.StreamWriter outputTxt = new System.IO.StreamWriter(@"D:\proj\output.txt");
       //     for (int i = 0; i < output.Count(); i++)
       //     {
                outputTxt.WriteLine(output);
            //    }
            outputTxt.Close();
        }
    }

}
