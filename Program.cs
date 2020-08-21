﻿using System;

namespace NearestNeighbors
{
    class kNN
    {
        static void Main(string[] args)
        {
            // Begin program
            Console.WriteLine("k Nearest Neighbors implementation using C#");
            double[][] trainData = LoadData();
            int numClasses = 3;
            double[] unknown = new double[] {5.25, 1.75}; // Values to predict
            int k = 10;
            int prediction = Predict(unknown, trainData, numClasses, k);
            Console.WriteLine("Predicted Class: " + prediction);
        }
        /// <summary>
        /// Return the predicted class.
        /// </summary>
        /// <param name="unknown"></param>
        /// <param name="trainData">Training dataset: X</param>
        /// <param name="numClasses">Number of classes: y</param>
        /// <param name="k">User defined constant. </param>
        /// <returns></returns>
        static int Predict(double[] unknown,
        double[][] trainData, int numClasses, int k)
        {
            int n = trainData.Length;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; ++i) {
                IndexAndDistance current = new IndexAndDistance();
                double distance = Distance(unknown, trainData[i]);
                current.idx = i;
                current.distance = distance;
                info[i] = current;
            }
            int result = Vote(info, trainData, numClasses, k);
            return result;
        }

        /// <summary>
        /// Loads Data from CSV
        /// </summary>
        /// <returns>A doubly nested array representation of the matrix.</returns>
        static double[][] LoadData()
        {
            double[][] data = new double[10][];

            data[0] = new double[] {1.1, 5.0, 0};
            data[1] = new double[] {2.3, 7.0, 1};
            data[2] = new double[] {2.3, 4.0, 2};
            data[3] = new double[] {2.3, 2.0, 2};
            data[4] = new double[] {3.3, 4.0, 2};
            data[5] = new double[] {2.3, 1.0, 2};
            data[6] = new double[] {2.3, 4.0, 2};
            data[7] = new double[] {2.3, 4.0, 2};
            data[8] = new double[] {2.3, 4.0, 2};
            data[9] = new double[] {4.3, 4.0, 1};

            return data;

        }
        static double Distance(double[] unknown, double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < unknown.Length; i++)
                sum += (unknown[i] - data[i]) * (unknown[i] - data[i]);
            return Math.Sqrt(sum);
        }
        /// <summary>
        /// Return predicted class for each observation
        /// </summary>
        /// <param name="info"></param>
        /// <param name="trainData"></param>
        /// <param name="numClasses"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        static int Vote(IndexAndDistance[] info, double[][] trainData,
        int numClasses, int k)
        {
        int[] votes = new int[numClasses];  // One cell per class
        for (int i = 0; i < k; ++i) {       // Just first k
            int idx = info[i].idx;            // Which train item
            int c = (int)trainData[idx][2];   // Class in last cell
            ++votes[c];
        }
        int mostVotes = 0;
        int classWithMostVotes = 0;
        for (int j = 0; j < numClasses; ++j) {
            if (votes[j] > mostVotes) {
            mostVotes = votes[j];
            classWithMostVotes = j;
            }
        }
        return classWithMostVotes;
}

        /// <summary>
        /// Compare the distance of two points.
        /// </summary>
        public class IndexAndDistance : IComparable<IndexAndDistance>
        {
            public int idx;
            public double distance;

            public int CompareTo(IndexAndDistance other)
            {
                if (this.distance < other.distance) return -1;
                else if (this.distance > other.distance) return + 1;
                else return 0;
            }
        }

    }
} // ns