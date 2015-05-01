using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace DataSorter
{
    class TxtListsParser
    {
        private int numberOfLinesInEachFile;
        private double[,] linesInDouble;
        public double[,] LinesInDouble
        {
            get { return linesInDouble; }
        }
        public TxtListsParser(string FileNameOfLines)
        {
            try
            {
                var lines = System.IO.File.ReadAllLines(FileNameOfLines);
                numberOfLinesInEachFile = lines.Length;
                linesInDouble = new double[2,numberOfLinesInEachFile];
                using (TextReader reader = File.OpenText(FileNameOfLines))
                {
                    for (int n = 0; n < numberOfLinesInEachFile; n++)
                    {
                        string text = reader.ReadLine();
                        string[] nbits = text.Split(' ');
                        linesInDouble[1, n] = double.Parse(nbits[0]);
                        linesInDouble[0, n] = n;
                    }
                    reader.Close();
                }
               /* double xi = 0;
                for (int j = 0; j < numberOfLinesInEachFile; j++)
                    for (int i = 1; i < (numberOfLinesInEachFile - j); i++)
                    {
                        if (linesInDouble[1, i - 1] > linesInDouble[1, i])
                        {
                            xi = linesInDouble[0, i];
                            linesInDouble[0, i] = linesInDouble[0, i - 1];
                            linesInDouble[0, i - 1] = xi;
                            xi = linesInDouble[1, i];
                            linesInDouble[1, i] = linesInDouble[1, i - 1];
                            linesInDouble[1, i - 1] = xi;
                        }
                    }*/
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.ToString());
                MessageBox.Show(".txt file not found");
            }
        }
    }
}
