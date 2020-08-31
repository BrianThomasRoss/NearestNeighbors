using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kNN
{
    class Program
    {
        static void Main(string[] args)
        {
            kNN knn = new kNN();
            // Load training data
            knn.LoadData("Data/Med-Cab.train", kNN.DataType.TRAININGDATA);
            // Load test data
            knn.LoadData("Data/Med-Cab.test", kNN.DataType.TESTDATA);
            knn.Predict(1);
            Console.ReadKey();
        }
    }
}