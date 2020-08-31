using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace kNN
{
    class kNN
    {
        private int lines;
        // Initialize a collection to store training data
        private List<double[]>   trainingSetValues = new List<double[]>();
        // Initialize a collection to store the testing data.
        private List<string>     trainingSetClasses = new List<string>();
        // same for the test input
        private List<double[]>   testSetValues = new List<double[]>();
        private List<string>     testSetClasses = new List<string>();
        private int K;
        // Enumerables
        public enum DataType
        {
            TRAININGDATA, TESTDATA
        };
        /// <summary>
        /// Loads data from a CSV file.
        /// </summary>
        /// <param name="path">The filepath to the CSV file.</param>
        /// <param name="dataType">One of testing or training sets.</param>
        public void LoadData(string path, DataType dataType)
        {
            // Instantiate a new stream reader.
            StreamReader file = new StreamReader(path);
            // Initialize local variables.
            string row;
            this.lines = 0;
            // Print output to user to ensure proper filepath.
            Console.WriteLine("[i] reading data from {0} ...", path);
            // While there are still lines in the file.
            while((row = file.ReadLine()) != null)
            {
                // Split the row and store in an array of strings
                // this is most similar to list(some_string.split(','))
                // in Python.
                string[] splitRow = row.Split(',').ToArray();
                // Add the values to a list
                List<string> lineItems = new List<string>(splitRow.Length);           
                lineItems.AddRange(splitRow);
                // Initialize an array of doubles to store the casted values
                double[] lineDoubles = new double[lineItems.Count - 1];
                // A string object which holds the class label.
                string lineClass = lineItems.ElementAt(lineItems.Count - 1);
                // For every value except the class label.
                for(int i = 0; i < lineItems.Count - 1; i++)
                {
                    // Cast the value to double
                    double val = Double.Parse(lineItems.ElementAt(i));
                    lineDoubles[i] = val;
                }

                // Store the data to the appropriate arrays
                if (dataType == DataType.TRAININGDATA)
                {
                    this.trainingSetValues.Add(lineDoubles);
                    this.trainingSetClasses.Add(lineClass);
                }
                else if(dataType == DataType.TESTDATA)
                {
                    this.testSetValues.Add(lineDoubles);
                    this.testSetClasses.Add(lineClass);
                }
                // Increment.
                this.lines++;
            }
            // Inform user file has finished loading. Return summary statistics.
            Console.WriteLine("[+] done. read {0} lines.", this.lines);
            // Explicitly close the connection
            file.Close();
        }
        /// <summary>
        /// Predict the class label.
        /// </summary>
        /// <param name="k">Voting pool.</param>
        public void Predict(int k)
        {
            // Initialize local variables.
            this.K = k;
            double[][] distances = new double[trainingSetValues.Count][];
            double accuracy = 0;
            double correct = 0, testNumber = 0;

            for (int i = 0; i < trainingSetValues.Count; i++)
                distances[i] = new double[2];

            Console.WriteLine("[i] classifying...");

            // For the values in the test set.
            for(var test = 0; test < this.testSetValues.Count; test++)
            {
                Parallel.For(0, trainingSetValues.Count, index =>
                    {
                        var dist = EuclideanDistance(this.testSetValues[test], this.trainingSetValues[index]);
                        distances[index][0] = dist;
                        distances[index][1] = index;
                    }
                );

                Console.WriteLine("closest K={0} neighbors: ", this.K);

                
                var sortedDistances = distances.AsParallel().OrderBy(t => t[0]).Take(this.K);

                string realClass = testSetClasses[test];

                // Validation.
                foreach (var d in sortedDistances)
                {
                    string predictedClass = trainingSetClasses[(int) d[1]];
                    if (string.Equals(realClass, predictedClass) == true)
                        correct++;
                    testNumber++;
                    Console.WriteLine("test {0}: real class: {1}\n predicted class: {2}", test, realClass, predictedClass);
                }
            }

            Console.WriteLine();

            // compute and print the accuracy
            accuracy = (correct / testNumber) * 100;
            Console.WriteLine("[i] accuracy: {0}%", accuracy);

        }
        /// <summary>
        /// Calculate the Euclidean distance between two points in
        /// n-dimensional space.
        /// </summary>
        /// <param name="left">The left vector.</param>
        /// <param name="right">The right vector.</param>
        /// <returns></returns>
        private static double EuclideanDistance(double[] left, double[] right)
        {
            double sum = 0.0;
            // Check that vectors are of equal magnitude.
            if (left.Length != right.Length)
            {
                // If false throw Exception.
                throw new Exception("Vectors must have equal magnitudes.");
            }
            // For each value in the array
            for(int i = 0; i < left.Length; i++)
            {
                sum += (left[i] - right[i]) * (left[i] - right[i]);
            }
            return Math.Sqrt(sum);
        }
    }
} // ns
