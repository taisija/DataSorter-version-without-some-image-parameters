using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace DataSorter
{
    class TxtParser
    {
        private double[][,]patterns;
        private int[] patternsSizes;
        private int[] patternsDenom;
        private int numberOfLinesInEachPattern;
        private int numberOfPatterns;

        private int[] patternIndicesInFile;
        private int patternNumInFile;

        public int[] PatternIndicesInFile
        {
            get { return patternIndicesInFile; }
        }
        public int PatternNumInFile
        {
            get { return patternNumInFile; }
        }

        public TxtParser(string FileNameOfPatterns, int NumberOfLinesInEachPattern)
        {
            numberOfLinesInEachPattern = NumberOfLinesInEachPattern;
            try
            {
                var lines = System.IO.File.ReadAllLines(FileNameOfPatterns);
                var numberOfLines = lines.Length;
                numberOfPatterns = numberOfLines / NumberOfLinesInEachPattern;
                using (TextReader reader = File.OpenText(FileNameOfPatterns))
                {
                    patterns = new double[numberOfPatterns][,];
                    patternsSizes = new int[numberOfPatterns];
                    patternsDenom = new int[numberOfPatterns];
                    for (int n = 0; n < numberOfPatterns; n++)
                    {
                        string text = reader.ReadLine();
                        string[] bits = text.Split(' ');
                        patternsSizes[n] = int.Parse(bits[0]);
                        patternsDenom[n] = int.Parse(bits[1]);
                        patterns[n] = new double[patternsSizes[n], patternsSizes[n]];
                        for (int i = 0; i < patternsSizes[n]; i++)
                        {
                            text = reader.ReadLine();
                            string[] nbits = text.Split(' ');
                            for (int j = 0; j < patternsSizes[n]; j++)
                            {
                                patterns[n][i, j] = double.Parse(nbits[j]);
                            }
                        }
                        for (int i = patternsSizes[n]; i < (NumberOfLinesInEachPattern-1); i++)
                            reader.ReadLine();
                    }
                    reader.Close();
                }
                
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.ToString());
                MessageBox.Show(".txt file not found");
            }
        }
        public bool ParseTxtFile(string FileName, string filePattern)
        {
            double[,] pattern;
            int patternSize;
            try
            {
                string[] fileName = Directory.GetFiles(FileName, filePattern);
                var lines = System.IO.File.ReadAllLines(fileName[0]);
                var numberOfLines = lines.Length;
                patternNumInFile = numberOfLines / numberOfLinesInEachPattern;
                patternIndicesInFile = new int[patternNumInFile];
                using (TextReader reader = File.OpenText(fileName[0]))
                {
                    for (int n = 0; n < patternNumInFile; n++)
                    {
                        string text = reader.ReadLine();
                        string[] bits = text.Split(' ');
                        patternSize = int.Parse(bits[0]);
                        pattern = new double[patternSize, patternSize];
                        for (int i = 0; i < patternSize; i++)
                        {
                            text = reader.ReadLine();
                            string[] nbits = text.Split(' ');
                            for (int j = 0; j < patternSize; j++)
                            {
                                pattern[i, j] = double.Parse(nbits[j]);
                            }
                        }
                        for (int i = patternSize; i < (numberOfLinesInEachPattern-1); i++)
                            reader.ReadLine();
                        patternIndicesInFile[n] = GetPatternIndex(pattern, patternSize);
                    }
                    reader.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.ToString());
                MessageBox.Show(".txt file not found");
                return false;
            }
        }
        private int GetPatternIndex(double[,] Pattern, int PatternSize)
        {
            int counter;
            for (int i = 0; i < numberOfPatterns; i++)
            {
                counter = 0;
                if (PatternSize == patternsSizes[i])
                {
                    for (int k = 0; k < PatternSize; k++)
                        for (int l = 0; l < PatternSize; l++)
                        {
                            if (patterns[i][k, l] == Pattern[k, l]) counter++;
                        }
                    if (counter == PatternSize * PatternSize) return i;
                }
            }
            return -1;
        }
    }
}
