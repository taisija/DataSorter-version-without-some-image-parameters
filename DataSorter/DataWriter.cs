using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataSorter
{
    class DataWriter
    {
        private string fileName;

        public DataWriter(string FileName)
        {
            fileName = FileName;
        }
        public bool WriteData(double[,] Data, int DataLength)
        {
            int len = DataLength;
            double[,] newData = Data;
            try
            {
                using (TextWriter writer = File.CreateText(fileName))
                {
                    for (int i = 0; i < len; i++)
                    {
                        writer.Write((i).ToString() + " ");
                        for (int j = 0; j < len; j++)
                        {
                            writer.Write((newData[i,j]).ToString() + " ");
                        }
                        writer.WriteLine();
                    }
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }
        public bool WriteData(double[][] Data, string[][] DataNames)
        {
            int len = Data.Length;
            double[][] newData = Data;
            int newlen = newData[0].Length;
            try
            {
                using (TextWriter writer = File.CreateText(fileName))
                {
                    for (int i = 0; i < len; i++)
                    {
                        //writer.Write((i).ToString()+ " ");
                        for (int j = 0; j < newlen; j++)
                        {
                            if (newData[i] == null)
                            {
                                writer.Close();
                                writer.Dispose();
                                break;
                            }
                            else
                            {
                                writer.Write((newData[i][j]).ToString() + " ");
                            }
                        }
                        if (newData[i] != null) writer.WriteLine();
                    }
                    writer.Close();
                }
                //write names of data
                len = DataNames.Length;
                newlen = DataNames[0].Length;
                using (TextWriter writer = File.CreateText(fileName+"names.txt"))
                {
                    for (int i = 0; i < len; i++)
                    {
                        //writer.Write((i).ToString()+ " ");
                        for (int j = 0; j < newlen; j++)
                        {
                            if (DataNames[i] == null)
                            {
                                writer.Close();
                                return false;
                            }
                            else
                            {
                                writer.Write(DataNames[i][j] + " ");
                            }
                        }
                        writer.WriteLine();
                    }
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }
        public bool WriteData(double[][] Data)
        {
            int len = Data.Length;
            double[][] newData = Data;
            int newlen = newData[1].Length;
            try
            {
                using (TextWriter writer = File.CreateText(fileName))
                {
                    for (int i = 0; i < len; i++)
                    {
                        //writer.Write((i).ToString()+ " ");
                        for (int j = 0; j < newlen; j++)
                        {
                            if (newData[i] == null)
                            {
                                writer.Close();
                                return false;
                            }
                            else
                            {
                                writer.Write((newData[i][j]).ToString() + " ");
                            }
                        }
                        writer.WriteLine();
                    }
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }/*
        public bool WriteDataAsPercentage(int[][] Data, int[] DataLength, int[] ObservationNum)
        {
            int len = DataLength.Length;
            double[][] newData = ResortDataAsPercentage(Data, DataLength, ObservationNum);
            int newlen = newData[1].Length;
            try
            {
                using (TextWriter writer = File.CreateText(fileName))
                {
                    for (int i = 0; i < len; i++)
                    {
                        writer.Write(ObservationNum[i].ToString() + " ");
                        for (int j = 0; j < newlen; j++)
                        {
                            writer.Write((newData[i][j]).ToString() + " ");
                        }
                        writer.WriteLine();
                    }
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }
        public double[][] ResortDataAsPercentage(int[][] Data, int[] DataLength, int[] ObservationNum)
        {
            int len = DataLength.Length;
            double[][] newData = new double[len][];
            int filterNumber = 0;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < DataLength[i]; j++)
                {
                    if (filterNumber < Data[i][j]) filterNumber = Data[i][j];
                }
            }
            filterNumber++;
            int[] countOffiltersInObservationSet = new int[ObservationNum[len-1]+1];
            newData[1] = new double[filterNumber];
            for (int i = 0; i < len; i++)
            {
                newData[i] = new double[filterNumber];
                //countOffiltersInObservationSet[ObservationNum[i]] = 0;
                for (int j = 0; j < filterNumber; j++) newData[i][j] = 0;
                for (int j = 0; j < DataLength[i]; j++)
                {
                    newData[ObservationNum[i]][Data[i][j]]++;
                    countOffiltersInObservationSet[ObservationNum[i]]++;
                }
            }
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < filterNumber; j++)
                {
                    newData[i][j] /= (double)countOffiltersInObservationSet[ObservationNum[i]];
                }
            }
            return newData;
        }*/
        public bool WriteData(int [][] Data, int[] DataLength,int[] ObservationNum)
        {
            int len = DataLength.Length;
            try
            {
                using (TextWriter writer = File.CreateText(fileName))
                {
                    for (int i = 0; i < len; i++)
                    {
                        writer.Write((i-(i/325)*325).ToString() + " ");
                        for (int j = 0; j < DataLength[i]; j++)
                        {
                                writer.Write((Data[i][j]).ToString() + " ");
                        }
                        writer.WriteLine();
                    }
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }
        private int[][] ResortData(int[][] Data, int[] DataLength)
        {
            int len = DataLength.Length;
            int[][] newData = new int[len][];
            int k = 0;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < DataLength[i]; j++)
                {
                    if (k < Data[i][j]) k = Data[i][j];
                }
            }
            k++;
            for (int i = 0; i < len; i++)
            {
                newData[i] = new int[k];
                for (int j = 0; j < k; j++) newData[i][j] = 0;
                for (int j = 0; j < DataLength[i]; j++)
                {
                    newData[i][Data[i][j]]++;
                }
            }
            return newData;
        }
    }
}
