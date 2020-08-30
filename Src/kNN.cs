using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            // Write out selection
            Console.WriteLine("Distance metric: " + distanceMetric);
            // Get training data
            string dataPath = "Data/Med-Cab.csv";
            double[][] trainData = LoadData(dataPath);
            // Each item in the training set is a unique class
            int numClasses = trainData.Length;
            Console.WriteLine(numClasses);
            // Observation to predict. For validation purposes currently using an
            // exact copy of the strain "24K Gold" from the training dataset.
            double[] unknown = new double[] {0.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,
                 0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,0.0,0.0,
                 0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,1.0,0.0,
                 0.0,0.0,1.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0};
            // User-defined constant - number of neighbors which form the voting pool.
            int k = 10;
            // Call the predict method, store return to variable.
            int prediction = Predict(unknown, trainData, numClasses, k, distanceMetric);
            // Write out predicted class
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
            double[][] trainData, int numClasses, int k, string metric)
        {
            // Get size of matrix
            int n = trainData.Length;
            // Initialize IndexAndDistance Comparable
            IndexAndDistance[] info = new IndexAndDistance[n];
            // For vectors in training matrix
            for (int i = 0; i < n; ++i)
            {
                IndexAndDistance current = new IndexAndDistance();
                // Check for distance metric
                double distance = EuclideanDistance(unknown, trainData[i]);
                current.idx = i;
                current.distance = distance;
                info[i] = current;
            }
            int result = Vote(info, trainData, numClasses, k);
            return result;
        }
        /// <summary>
        /// Loads Data from CSV file.
        /// </summary>
        /// <param name="path">Path to data file.</param>
        /// <returns>A doubly nested array representation of the matrix.</returns>
        static double[][] LoadData(string path)
        {
            // Initialize the StreamReader
            StreamReader file = new StreamReader($"{path}");
            // Initialize local variables.
            string line;
            int lines = 0;
            // Hard coding the length of the array.
            double[][] data = new double[329][];
            // Print out feedback for user.
            Console.WriteLine("Reading data from '{0}'", path);
            // While there are still lines to read.
            while ((line = file.ReadLine()) != null)
            {
                // Split row on delimiter
                // This is the equivalent of list(some_string.split(',')) in Python
                string[] splitLine = line.Split(',').ToArray();
                // Initialize a new list
                List<string> rowItems = new List<String>(splitLine.Length);
                rowItems.AddRange(splitLine);
                // Initialize an array of doubles.
                // Last value is the class, which is string.
                double[] rowDoubles = new double[rowItems.Count - 1];
                // Initialize variable to store row class.
                string rowClass = rowItems.ElementAt(rowItems.Count - 1);
                // For each value in the row
                for (int i = 0; i < rowItems.Count - 1; i++)
                {
                    // Cast each value to double
                    double value = Double.Parse(rowItems.ElementAt(i));
                    Console.WriteLine(value);
                    // Set array index to double
                    rowDoubles[i] = value;
                }
                Console.WriteLine(rowDoubles);
                // Store the values in the main array
                data[lines - 1] = rowDoubles;
                // Increment

                lines++;
            }
            // Inform user file read has completed, return summary statistics.
            Console.WriteLine("[+] done. {0} observations loaded.", lines);
            // Explicitly close out file.
            file.Close();
            // Return the array.
            Console.WriteLine(data);
            return data;
        }
        /// <summary>
        /// Euclidean distance between two points.
        /// </summary>
        /// <param name="left">Left vector</param>
        /// <param name="right">Right vector</param>
        /// <returns>Math.sqrt(x1-y1^2 ...)</returns>
        static double EuclideanDistance(double[] left, double[] right)
        {
            // Initialize distance as 0
            double sum = 0.0;
            // Validate vectors are of equal length
            if (left.Length != right.Length - 1)
            {
                // If not true throw exception.
                throw new System.ArgumentException("Vectors must be of equal length.");
            }
            // For each point 
            for (int i = 0; i < left.Length; i++)
                // Add to sum square of difference
                sum += (left[i] - right[i]) * (left[i] - right[i]);
            // Take square root of sum
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
            // One cell per class   
            int[] votes = new int[numClasses];  // One cell per class
            // First neighbor
            for (int i = 0; i < k; ++i) {
                // Which training item
                int idx = info[i].idx;
                // Class is in the last cell
                int c = (int)trainData[idx][2];
                ++votes[c];
        }
        // Initialize voting variable
        int mostVotes = 0;
        // Initialize majority class variable
        int majorityClass = 0;
        // For class in number of classes
        for (int j = 0; j < numClasses; ++j) 
        {
            // If this class has more votes than the mostVotes
            if (votes[j] > mostVotes)
            {
            // Reassign variable    
            mostVotes = votes[j];
            // Assign majority class
            majorityClass = j;
            }
        }
        // Return the majority class when loop is finished
        return majorityClass;
}

        /// <summary>
        /// Compare distances and index.
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
