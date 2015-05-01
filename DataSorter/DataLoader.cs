using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace DataSorter
{
    class DataLoader
    {
        private string sourcePath;
        private string sourceParametersPath;
        private int scenario;
        private int bestPopulationsNumber;
        private int generationsNumber;
        private TxtParser txtParser;
        private TxtListsParser txtListsParser;
        private int[][] loadingData;
        private double[] fitnessLevel;
        private int[]observationNum;
        private int[] dataLen;
        private int len;
        /*
        Microsoft.Office.Interop.Excel.Application application;
        object missing;
        Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

        Microsoft.Office.Interop.Excel.Application application_stat;
        object missing_stat;
        Microsoft.Office.Interop.Excel.Workbook xlWorkBook_stat;
        Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet_stat;
        */
        public int[][] LoadingData
        {
            get { return loadingData; }
        }
        public int[] ObservationNum
        {
            get { return observationNum; }
        }
        public int[] DataLen
        {
            get { return dataLen; }
        }
        public double[] FitnessLevel
        {
            get { return fitnessLevel; }
        }
        public DataLoader(String SourcePath, String SourceParametersPath)
        {
            sourcePath = SourcePath;
            sourceParametersPath = SourceParametersPath;
            bestPopulationsNumber = 20;
            generationsNumber = 40;
            scenario = 0;
            len = 100;
            loadingData = new int[len][];
            fitnessLevel = new double[len];
            observationNum = new int[len];
            dataLen = new int[len];
        }
        public DataLoader(string SourcePath, string SourceParametersPath, int BestPopulationsNumber, int GenerationsNumber, int Scenario)
        {
            sourcePath = SourcePath;
            sourceParametersPath = SourceParametersPath;
            scenario = Scenario;
            bestPopulationsNumber = BestPopulationsNumber;
            generationsNumber = GenerationsNumber;
            len = 6000;
            loadingData = new int[len][];
            fitnessLevel = new double[len];
            observationNum = new int[len];
            dataLen = new int[len];
        }
        public bool ReadSourceParametersForAllCombinetions(int AllFoldersCounter, int NumberOfLinesInPattern)
        {
            int k = 0;
            int dataCounter = 0;
            int observationN = 0;
            txtParser = new TxtParser(sourcePath + "\\filter.txt", 6);
            int withoutDuplicates = 0;
            int startPoint = 0;
            for (int i = 0; i < AllFoldersCounter; i++)
            {
                txtListsParser = new TxtListsParser(sourceParametersPath + "\\" + (i).ToString() + "\\" + (i).ToString() + "_.txt");
                double[,] paramValues = txtListsParser.LinesInDouble;
                k = 0;
                bestPopulationsNumber = paramValues.Length / 2;
                if (withoutDuplicates == 0) withoutDuplicates = bestPopulationsNumber;
                startPoint = dataCounter;
                while (k < bestPopulationsNumber)
                {
                    txtParser.ParseTxtFile(sourcePath + "\\" + (i).ToString(), "0_" + ((int)paramValues[0, k]).ToString() + ".txt");
                    if (CheckForDuplicate(txtParser.PatternIndicesInFile, txtParser.PatternNumInFile, dataCounter, startPoint))
                    {
                        if (dataCounter >= len) rewriteData(2);
                        loadingData[dataCounter] = txtParser.PatternIndicesInFile;
                        fitnessLevel[dataCounter] = paramValues[1, k];
                        observationNum[dataCounter] = observationN;
                        dataLen[dataCounter] = txtParser.PatternNumInFile;
                        dataCounter++;
                    }
                    else
                        if (i == 0) { withoutDuplicates--; }
                    k++;
                }
                observationN++;
            }
            return false;

        }
        public bool CheckForDuplicate(int[] indices, int len, int counter, int startPoint)
        {
            int check = 0;
            for (int i = startPoint; i < counter; i++)
            {
                if (dataLen[i] == len)
                    for (int j = 0; j < len; j++)
                    {
                        if (indices[j] == loadingData[i][j]) check++;
                    }
                if (check == len) return false;
                check = 0;
            }
                return true;
        }/*
        public bool ReadSourceParameters(int AllFoldersCounter, int NumberOfLinesInPattern, int StatPeriod)
        {
            int k = 0;
            int dataCounter = 0;
            int observationN = 0;
            txtParser = new TxtParser(sourcePath + "\\filter.txt", 6);
            for (int i = 0; i < AllFoldersCounter; i += bestPopulationsNumber)
            {
                openExcelFile(sourceParametersPath + "\\test0 (" + (i / StatPeriod + 1).ToString()+ ").xlsx");
                double[,] paramValues = ReadParameterValuesFromCurrentExelFile(i);
                k = 1;
                while (paramValues[1,bestPopulationsNumber-k] > median[i / StatPeriod])
                {
                    txtParser.ParseTxtFile(sourcePath + "\\" + (paramValues[0, bestPopulationsNumber - k] + i), "39_*.txt");
                    if (dataCounter >= len) rewriteData(2);
                    loadingData[dataCounter] = txtParser.PatternIndicesInFile;
                    observationNum[dataCounter] = observationN;
                    dataLen[dataCounter] = txtParser.PatternNumInFile;
                    dataCounter++;
                    k++;
                }
                observationN++;
            }
            return false;
        }

        public bool ReadBestSourceParameters(int AllFoldersCounter, int NumberOfLinesInPattern, int StatPeriod)
        {
            int k = 0;
            int dataCounter = 0;
            int observationN = 0;
            txtParser = new TxtParser(sourcePath + "\\filter.txt", 6);
            for (int i = 0; i < AllFoldersCounter; i += bestPopulationsNumber)
            {
                openExcelFile(sourceParametersPath + "\\test0 (" + (i / StatPeriod + 1).ToString() + ").xlsx");
                double[,] paramValues = ReadParameterValuesFromCurrentExelFile(i);
                k = 1;
                txtParser.ParseTxtFile(sourcePath + "\\" + (paramValues[0, bestPopulationsNumber - k] + i), "39_*.txt");
                if (dataCounter >= len) rewriteData(2);
                loadingData[dataCounter] = txtParser.PatternIndicesInFile;
                observationNum[dataCounter] = observationN;
                dataLen[dataCounter] = txtParser.PatternNumInFile;
                dataCounter++;
                observationN++;
            }
            return false;
        }
        */
        private void rewriteData(int n)
        {
            int[][] l = new int[len][];
            for(int i = 0; i< len; i++)
            {
                l[i] = loadingData[i];
            }
            int[]o = observationNum;
            int[]d = dataLen;
            double[] f = fitnessLevel;
            len+=n;
            loadingData = new int[len][];
            observationNum = new int[len];
            dataLen = new int[len];
            fitnessLevel = new double[len];
            for (int i = 0; i < (len-n); i++)
            {
                loadingData[i] = l[i];
                observationNum[i] = o[i];
                dataLen[i] = d[i];
                fitnessLevel[i] = f[i];
            }
        }/*
        private void openExcelFile(string longName)
        {
            System.Globalization.CultureInfo cultInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.CultureInfo cultUIInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            if (application_stat != null)
            {
                application_stat.Quit();
                application_stat = null;
            }
            application_stat = new Microsoft.Office.Interop.Excel.Application();
            missing_stat = System.Reflection.Missing.Value;
            application_stat.Caption = "Test";
            application_stat.Visible = false;
            xlWorkBook_stat = application_stat.Workbooks.Open
            (
                longName,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat,
                missing_stat
            );

            xlWorkSheet_stat = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook_stat.Worksheets.get_Item(1);
            System.Threading.Thread.CurrentThread.CurrentCulture = cultInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultUIInfo;
        }
        private string getColumnByNum(int num)
        {
            int n = num - 1;
            string s = Char.ConvertFromUtf32(n % 26 + (int)'A');
            while (n / 26 > 0)
            {
                n = n / 26 - 1;
                s = Char.ConvertFromUtf32((int)'A' + n % 26) + s;
            }
            return s;
        }
        private double[,] ReadParameterValuesFromCurrentExelFile(int count)
        {
            //double[] bestFitnessValues = new double[bestPopulationsNumber];
            double[,] bestFitnessValues = new double[2, bestPopulationsNumber];
            System.Globalization.CultureInfo cultInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.CultureInfo cultUIInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            for (int i = 1; i <= bestPopulationsNumber; i++)
            {
                bestFitnessValues[1, i-1] = (double)xlWorkSheet_stat.get_Range(getColumnByNum((count + i - 1) % 50 + 1) + (generationsNumber+1).ToString(), missing_stat).Value2;
                //bestFitnessValues[2, i] = (double)xlWorkSheet_stat.Cells.get_Item(generationsNumber + 1, (count + i - 1) % 50 + 1);
                bestFitnessValues[0, i-1] = i-1;
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = cultInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultUIInfo;
            application_stat.Quit();
            application_stat = null;
            double xi = 0;
            for (int j = 0; j < bestPopulationsNumber; j++)
                for (int i = 1; i < (bestPopulationsNumber - j); i++)
                {
                    if (bestFitnessValues[1,i-1] > bestFitnessValues[1,i])
                    {
                        xi = bestFitnessValues[0, i];
                        bestFitnessValues[0, i] = bestFitnessValues[0, i-1];
                        bestFitnessValues[0, i - 1] = xi;
                        xi = bestFitnessValues[1, i];
                        bestFitnessValues[1, i] = bestFitnessValues[1, i - 1];
                        bestFitnessValues[1, i - 1] = xi;
                    }
                }
            return bestFitnessValues;
        }
        private void closeExcelFile()
        {
            System.Globalization.CultureInfo cultInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.CultureInfo cultUIInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            xlWorkBook_stat.Save();
            application_stat.Quit();
            application_stat = null;
            System.Threading.Thread.CurrentThread.CurrentCulture = cultInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultUIInfo;
        }
        */
        private double[] median = 
        {
            0.9,
            0.974052645,
            0.971382364,
            0.926183234,
            0.983014276,
            0.975748566,
            0.985161719,
            0.943805971,
            0.928728744,
            0.9,
            0.943585318,
            0.950823484,
            0.964505556,
            0.972248811,
            0.9,
            0.945584184,
            0.9,
            0.943462174,
            0.943478179,
            0.955659905,
            0.944135932,
            0.905716642,
            0.974066306,
            0.962092323,
            0.916532255,
            0.987622757,
            0.977486672,
            0.982173823,
            0.938873829,
            0.97738589,
            0.976976522,
            0.94097218,
            0.942926649,
            0.921038018,
            0.939636752,
            0.933948917,
            0.938296854,
            0.983069498,
            0.953666935,
            0.9,
            0.958843216,
            0.9,
            0.968284441,
            0.968755365,
            0.915845539,
            0.9633552,
            0.949238867,
            0.950850144,
            0.984221649,
            0.96358646
        };
    }
}