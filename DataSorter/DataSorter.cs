using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataSorter
{
    public partial class DataSorter : Form
    {
        public DataSorter()
        {
            InitializeComponent();
        }

        private void buttonLoadData_Click(object sender, EventArgs e)
        {
            DataLoader DL;
            DataWriter DW;
            ImageParameterization IP;
            int ImagesNum = (int)numericUpDownImNum.Value;
            int matricesNum1 = 2;
            int matricesNum2 = 14;
            int matricesNum2ParamNum = 3;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                DL = new DataLoader(folderBrowserDialog.SelectedPath, folderBrowserDialog.SelectedPath);
                DL.ReadSourceParametersForAllCombinetions(ImagesNum, 6);
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    int[,] m1 = new int[2, matricesNum1];
                    int[,] m2 = new int[2, matricesNum2];
                    m1[0, 0] = 0;
                    m1[1, 0] = 1;
                    m1[0, 1] = 1;
                    m1[1, 1] = -4;
                    m2[0, 0] = 0;
                    m2[1, 0] = 20;
                    m2[0, 1] = 2;
                    m2[1, 1] = -2;
                    m2[0, 2] = 0;
                    m2[1, 2] = 11;
                    m2[0, 3] = 13;
                    m2[1, 3] = -13;
                    m2[0, 4] = 0;
                    m2[1, 4] = 17;
                    m2[0, 5] = 17;
                    m2[1, 5] = 0;
                    m2[0, 6] = 13;
                    m2[1, 6] = -2;
                    m2[0, 7] = 24;
                    m2[1, 7] = 6;
                    m2[0, 8] = 7;
                    m2[1, 8] = -7;
                    m2[0, 9] = -7;
                    m2[1, 9] = 7;
                    m2[0, 10] = 7;
                    m2[1, 10] = 17;
                    m2[0, 11] = 10;
                    m2[1, 11] = 23;
                    m2[0, 12] = 24;
                    m2[1, 12] = -10;
                    m2[0, 13] = 5;
                    m2[1, 13] = 5;
                    IP = new ImageParameterization(ImagesNum, 0, 5 + 8 * matricesNum1 +
                        matricesNum2ParamNum * matricesNum2, true);
                    IP.LoadImages("");
                    IP.Calculate_mN(2);
                    IP.Calculate_mN(3);
                    IP.Calculate_mN(4);
                    IP.CalculateDivergence();
                    IP.ParamFromCOoccurenceMatrix(m1, matricesNum1);
                    IP.ParamFromCOoccurenceMatrix_Some(m2, matricesNum2);
                    //IP.AddResortDataAsPercentage(DL.LoadingData, DL.DataLen, DL.ObservationNum);
                    double[][] newData = new double[DL.DataLen.Length][];
                    string[][] newDataNames = new string[1][];
                    for (int i = 0; i < ImagesNum; i++)
                    {
                        for (int k = i*325; k < ((i+1)*325); k++)
                        {
                            newData[k] = new double[IP.ParamCounter + 3];
                            for (int j = 3; j < (IP.ParamCounter+3); j++)
                            {
                                newData[k][j] = IP.TestImageResults[i][j-3];
                            }
                            newData[k][0] = i;
                            newData[k][1] = k - i * 325;
                            newData[k][2] = DL.FitnessLevel[k];
                        }
                    }
                    /*for (int i = 0; i < ImagesNum; i++)
                    {
                        for (int k = i * 325; k < ((i + 1) * 325); k++)
                        {*/
                            newDataNames[0] = new string[IP.ParamCounter/* + 3*/];
                            for (int j = 0/*3*/; j < (IP.ParamCounter /*+ 3*/); j++)
                            {
                                newDataNames[0][j] = IP.TestImageResultsNames[0][j/* - 3*/];
                            }
                            /*newDataNames[k][0] = "imageNumber";
                            newDataNames[k][1] = "filterNumber";
                            newDataNames[k][2] = "fitnessLevel";*/
                       /* }
                    }*/
                        DW = new DataWriter(saveFileDialog.FileName);
                        DW.WriteData(newData, newDataNames);
                        //DW.WriteData(IP.CorrelationT(0.9), 5 + 128 + 13);
                  /*    
                   * part for R Graphics and another statistic analysis
                        DW = new DataWriter(saveFileDialog.FileName);
                        DW.WriteData(IP.TestImageResults);
                        DW = new DataWriter(saveFileDialog.FileName + ".txt");
                        DW.WriteData(IP.Correlation(), 5 + 8 * matricesNum1 + 
                            matricesNum2ParamNum * matricesNum2);
                        DW = new DataWriter(saveFileDialog.FileName + "Thresh.txt");
                        DW.WriteData(IP.CorrelationT(0.9), 5 + 8 * matricesNum1 + 
                            matricesNum2ParamNum * matricesNum2);*/
                }
            }
        }

        private void resortButton_Click(object sender, EventArgs e)
        {
            DataLoader DL;
            DataWriter DW;
            ImageParameterization IP;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                 DL = new DataLoader(folderBrowserDialog.SelectedPath, folderBrowserDialog.SelectedPath);
                 DL.ReadSourceParametersForAllCombinetions(50, 6);
                 if (saveFileDialog.ShowDialog() == DialogResult.OK)
                 {
                     DW = new DataWriter(saveFileDialog.FileName);
                     DW.WriteData(DL.LoadingData, DL.DataLen, DL.ObservationNum);
                 }
            }
        }
    }
}
