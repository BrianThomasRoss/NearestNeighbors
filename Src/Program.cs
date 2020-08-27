using System;

/// <summary>
/// k Nearest Neighbors algorithm implementation in C#.
/// </summary>
namespace NearestNeighbors
{
    class kNN
    {
        static void Main(string[] args)
        
        {
            // Begin program
            Console.WriteLine("k Nearest Neighbors implementation using C#");
            // Select distance metric
            string distanceMetric = "Euclidean";
            Console.WriteLine("Distance metric: " + distanceMetric);
            // Get training data
            double[][] trainData = LoadData();
            // Number of unique classes
            int numClasses = 3;
            // Values to predict
            double[] unknown = new double[] {5.25, 1.75};
            // User-defined constant - number of neighbors of which to form the voting pool.
            int k = 10;
            // 
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
            /// TODO: Allow to read from filepath or buffer
            double[][] data = new double[10][];

            data[0] = new double[] {1.1, 5.0, 0};
            data[1] = new double[] {2.3, 7.0, 1};
            data[2] = new double[] {2.3, 4.0, 2};
            data[3] = new double[] {2.3, 2.0, 2};
            data[4] = new double[] {3.3, 4.0, 2};
            data[5] = new double[] {2.3, 2.0, 2};
            data[6] = new double[] {2.3, 4.0, 2};
            data[7] = new double[] {2.3, 4.0, 2};
            data[8] = new double[] {2.3, 4.0, 2};
            data[9] = new double[] {4.3, 4.0, 1};

            return data;

        }
        /// <summary>
        /// Euclidean distance between two points.
        /// </summary>
        /// <param name="unknown"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static double Distance(double[] unknown, double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < unknown.Length; i++)
                sum += (unknown[i] - data[i]) * (unknown[i] - data[i]);
            return Math.Sqrt(sum);
        }
        /// <summary>
        /// Return predicted class for each observation.
        /// </summary>
        /// <param name="info">Index and distance information.</param>
        /// <param name="trainData">X: data to train with.</param>
        /// <param name="numClasses">Number of target classes.</param>
        /// <param name="k">User defined constant.</param>
        /// <returns>Majority class for k nearest neighbors</returns>
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
        int majorityClass = 0;
        for (int j = 0; j < numClasses; ++j) {
            if (votes[j] > mostVotes) {
            mostVotes = votes[j];
            majorityClass = j;
            }
        }
        return majorityClass;
}

        /// <summary>
        /// 
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
