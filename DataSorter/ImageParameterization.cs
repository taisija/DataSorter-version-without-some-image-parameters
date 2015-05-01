using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace DataSorter
{
    class ImageParameterization
    {
        private bool loadingFlag;
        private bool normalizationFlag;
        private bool histogramCalcFlag;
        private double[][] histograms;
        private int testImageNum;
        private byte[][,] testImageList;
        private int[,] testImageSize;
        private double[][] testImageResults;
        private string[][] testImageResultsNames;
        private double[][,] sortTestImageResults;
        private double[] expectance;
        private int paramNum;
        private int subImagesNum;
        private int paramCounter;

        public int ParamCounter
        {
            get { return paramCounter; }
        }
        public double[][] TestImageResults
        {
            get { return testImageResults; }
        }
        public string[][] TestImageResultsNames
        {
            get { return testImageResultsNames; }
        }
        public ImageParameterization(int imagesNum, int numOfParameters, bool normalization)
        {
            testImageNum = imagesNum;
            subImagesNum = 0;
            paramNum = numOfParameters;
            testImageList = new byte[testImageNum][,];
            testImageSize = new int[2, testImageNum];
            testImageResults = new double[testImageNum][];
            testImageResultsNames = new string[testImageNum][];
            expectance = new double[testImageNum];
            histograms = new double[testImageNum][];
            for (int i = 0; i < testImageNum; i++)
            {
                histograms[i] = new double[256];
                testImageResults[i] = new double[numOfParameters];
                testImageResultsNames[i] = new string[numOfParameters];
                expectance[i] = 0;
                for (int j = 0; j < 256; j++)
                {
                    histograms[i][j] = 0;
                }
            }
            paramCounter = 0;
            loadingFlag = false;
            histogramCalcFlag = false;
            normalizationFlag = normalization;
        }
        public ImageParameterization(int imagesNum, int SubImegesNum, int numOfParameters, bool normalization)
        {
            testImageNum = imagesNum* (SubImegesNum+1);
            subImagesNum = SubImegesNum;
            paramNum = numOfParameters;
            testImageList = new byte[testImageNum][,];
            testImageSize = new int[2, testImageNum];
            testImageResults = new double[testImageNum][];
            testImageResultsNames = new string[testImageNum][];
            expectance = new double[testImageNum];
            histograms = new double[testImageNum][];
            for (int i = 0; i < testImageNum; i++)
            {
                histograms[i] = new double[256];
                testImageResults[i] = new double[numOfParameters];
                testImageResultsNames[i] = new string[numOfParameters];
                expectance[i] = 0;
                for (int j = 0; j < 256; j++)
                {
                    histograms[i][j] = 0;
                }
            }
            loadingFlag = false;
            histogramCalcFlag = false;
            normalizationFlag = normalization;
            paramCounter = 0;
        }
        public void Refresh(int count)
        {
            if (count < paramNum)
            {
                paramCounter = count;
            }
            else
                paramCounter = 0;

        }
        public bool Calculate_m()
        {
            if (histogramCalcFlag && (paramCounter < paramNum))
            {
                for (int i = 0; i < testImageNum; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        expectance[i] += j * histograms[i][j];
                    }
                    
                    //normalization
                    if (normalizationFlag)expectance[i] /= 255;
                    testImageResults[i][paramCounter] = expectance[i];
                    testImageResultsNames[i][paramCounter] = "m1";
                }
                paramCounter++;
            }
            return histogramCalcFlag;
        }
        public bool Calculate_mN(int N)
        {
            double power;
            if (histogramCalcFlag && (N > 1) && ((paramCounter) < paramNum))
            {
                for (int i = 0; i < testImageNum; i++)
                {
                    testImageResults[i][paramCounter] = 0;
                    for (int j = 0; j < 256; j++)
                    {
                        power = 1;
                        if (normalizationFlag)
                        {
                            for (int k = 0; k < N; k++)
                                power *= (j/255.0 - expectance[i]);
                            testImageResults[i][paramCounter] += power * histograms[i][j];
                            testImageResultsNames[i][paramCounter] = "m"+N.ToString();
                        }
                        else
                        {
                            for (int k = 0; k < N; k++)
                                power *= (j - expectance[i]);
                            testImageResults[i][paramCounter] += power * histograms[i][j];
                            testImageResultsNames[i][paramCounter] = "m" + N.ToString() + "notNormalized";
                        }
                    }
                    if (N == 2)
                    {
                        testImageResults[i][paramCounter] = Math.Sqrt(testImageResults[i][paramCounter]);
                        testImageResultsNames[i][paramCounter] = "m2";
                    }
                }
                paramCounter++;
            }
            return histogramCalcFlag;
        }
        public bool CalculateDivergence()
        {
            int k,j;
            if (histogramCalcFlag && ((paramCounter) < paramNum))
            {
                for (int i = 0; i < testImageNum; i++)
                {
                    for (j = 0; j < 256; j++)
                    {
                        if (histograms[i][j] > 0) break;
                    }
                    for (k = 255; k >=0; k--)
                    {
                        if (histograms[i][k] > 0) break;
                    }
                    if (normalizationFlag)
                    {
                        testImageResults[i][paramCounter] = (expectance[i] - (k - j) / 500.0);
                        testImageResultsNames[i][paramCounter] = "Divergence";
                    }
                    else
                    {
                        testImageResults[i][paramCounter] = expectance[i] - (k - j) / 2.0;
                        testImageResultsNames[i][paramCounter] = "Divergence notNormalized";
                    }
                }
                paramCounter++;
            }
            return histogramCalcFlag;
        }
        public bool SaveResults()
        {
            
           /* for (int i = 1; i <= testImageNum; i++)
                for (int j = 1; j <= paramNum; j++)
                xlWorkSheet.Cells.set_Item(i + 1, j + 1, (object)testImageResults[i-1][j-1]);*/
            return true;
        }
        public bool SaveHistograms(string longFileName)
        {
            if (histogramCalcFlag)
            {
               /* string fName = @"d:\image_param\image_histograms.xlsx";
                if (longFileName != "") fName = longFileName;
                System.Globalization.CultureInfo cultInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Globalization.CultureInfo cultUIInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                if (application != null)
                {
                    xlWorkBook.Save();
                    application.Quit();
                    application = null;
                }
                application = new Microsoft.Office.Interop.Excel.Application();
                missing = System.Reflection.Missing.Value;
                application.Caption = "image_histogram";
                application.Visible = false;
                xlWorkBook = application.Workbooks.Open
                (
                    fName,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing,
                    missing
                );

                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                for (int i = 1; i <= testImageNum; i++)
                    for (int j = 1; j <= 256; j++)
                        xlWorkSheet.Cells.set_Item(j + 1, i + 1, (object)histograms[i - 1][j - 1]);
                Microsoft.Office.Interop.Excel.Range chartRange;

                Microsoft.Office.Interop.Excel.ChartObjects xlCharts = (Microsoft.Office.Interop.Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
                Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);
                Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
                string s;
                for (int i = 1; i <= testImageNum; i++)
                {
                    s = getColumnByNum(i+1);
                    myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(10 + testImageNum, 80 + (i-1)*250, 300, 250);
                    chartPage = myChart.Chart;
                    chartRange = xlWorkSheet.get_Range(s + "2:" + s + "257", missing);
                    chartPage.SetSourceData(chartRange, missing);
                    chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlColumnClustered;
                }

                xlWorkBook.Save();
                application.Quit();
                application = null;
                System.Threading.Thread.CurrentThread.CurrentCulture = cultInfo;
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultUIInfo;*/
                return true;
            }
            return false;
        }
        public bool LoadImages(string folderName)
        {
            Bitmap image;
            string fName;
            try
            {
                {
                    if (folderName == "")
                        fName = @"d:\image_param\images";
                    else
                        fName = folderName;
                    string[] st = new string[testImageNum];
                    for (int i = 0; i < testImageNum; i++)
                    {
                        image = new Bitmap(fName + "\\" + ((i) / (subImagesNum + 1)).ToString() + ".jpg");
                        testImageSize[0, i] = image.Height;
                        testImageSize[1, i] = image.Width;
                        testImageList[i] = new byte[testImageSize[0, i], testImageSize[1, i]];
                        for (int k = 0; k < testImageSize[0, i]; k++)
                            for (int j = 0; j < testImageSize[1, i]; j++)
                            {
                                testImageList[i][k, j] = image.GetPixel(j, k).G;
                            }
                        st[i] = fName + "\\" + ((i) / (subImagesNum + 1)).ToString() + ".jpg";
                        image.Dispose();
                        for (int t = 0; t < subImagesNum; t++)
                        {
                            i++;
                            image = new Bitmap(fName + "\\" + ((i) / (subImagesNum+1)).ToString() + "_" + t.ToString() + ".jpg");
                            testImageSize[0, i] = image.Height;
                            testImageSize[1, i] = image.Width;
                            testImageList[i] = new byte[testImageSize[0, i], testImageSize[1, i]];
                            for (int k = 0; k < testImageSize[0, i]; k++)
                                for (int j = 0; j < testImageSize[1, i]; j++)
                                {
                                    testImageList[i][k, j] = image.GetPixel(j, k).G;
                                }
                            image.Dispose();
                            st[i] = fName + "\\" + ((i ) / (subImagesNum + 1)).ToString() + "_" + t.ToString() + ".jpg";
                        }
                    }
                    loadingFlag = true;
                    calculateHistograms();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "image not found" + ex.ToString());
                return false;
            }
        }
        public bool ParamFromCOoccurenceMatrix(int[,] MatricesParam, int MatricesNumber)
        {
            double[][] param;
            if (((MatricesParam.Length / 2) == MatricesNumber) && ((paramCounter + MatricesNumber*8-1) < paramNum))
            {
                param = new double[8][];
                for (int i = 0; i < testImageNum; i++)
                {
                    CooccurenceMatricesStatistics CMS = new CooccurenceMatricesStatistics(MatricesParam, MatricesNumber, testImageList[i], testImageSize[0, i], testImageSize[1, i]);
                    param[0] = CMS.GetContrast();
                    param[1] = CMS.GetAngularSecondMoment();
                    param[2] = CMS.GetEntrophy();
                    param[3] = CMS.GetCorreletion();
                    param[4] = CMS.GetContrastFromContrastHist();
                    param[5] = CMS.GetAngularSecondMomentFromContrastHist();
                    param[6] = CMS.GetEntrophyFromContrastHist();
                    param[7] = CMS.GetMeanFromContrastHist();
                    for (int j = 0; j < MatricesNumber; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            testImageResults[i][paramCounter+j*8+k] = param[k][j];
                        }
                        testImageResultsNames[i][paramCounter + j * 8] = "GetContrast(0," + MatricesParam[0, j].ToString() +"),(1,"+ MatricesParam[1, j].ToString()+")";
                        testImageResultsNames[i][paramCounter + j * 8 + 1] = "GetAngularSecondMoment(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                        testImageResultsNames[i][paramCounter + j * 8 + 2] = "GetEntrophy(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                        testImageResultsNames[i][paramCounter + j * 8 + 3] = "GetCorreletion(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                        testImageResultsNames[i][paramCounter + j * 8 + 4] = "GetContrastFromContrastHist(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                        testImageResultsNames[i][paramCounter + j * 8 + 5] = "GetAngularSecondMomentFromContrastHist(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                        testImageResultsNames[i][paramCounter + j * 8 + 6] = "GetEntrophyFromContrastHist(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                        testImageResultsNames[i][paramCounter + j * 8 + 7] = "GetMeanFromContrastHist(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                    }
                }
                paramCounter += MatricesNumber * 8;
                return true;
            }
            return false;
        }
        public bool ParamFromCOoccurenceMatrix_Some(int[,] MatricesParam, int MatricesNumber)
        {
            double[][] param;
            if (((MatricesParam.Length / 2) == MatricesNumber) && ((paramCounter + MatricesNumber * 3 - 1) < paramNum))
            {
                param = new double[3][];
                for (int i = 0; i < testImageNum; i++)
                {
                    CooccurenceMatricesStatistics CMS = new CooccurenceMatricesStatistics(MatricesParam, MatricesNumber, testImageList[i], testImageSize[0, i], testImageSize[1, i]);
                    param[0] = CMS.GetContrast();
                    param[1] = CMS.GetAngularSecondMoment();
                    CMS.GetEntrophy();
                    CMS.GetCorreletion();
                    CMS.GetContrastFromContrastHist();
                    param[2] = CMS.GetAngularSecondMomentFromContrastHist();
                    for (int j = 0; j < MatricesNumber; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            testImageResults[i][paramCounter + j * 3 + k] = param[k][j];
                        }
                        testImageResultsNames[i][paramCounter + j * 3] = "GetContrast(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                        testImageResultsNames[i][paramCounter + j * 3 + 1] = "GetAngularSecondMoment(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                        testImageResultsNames[i][paramCounter + j * 3 + 2] = "GetAngularSecondMomentFromContrastHist(0," + MatricesParam[0, j].ToString() + "),(1," + MatricesParam[1, j].ToString() + ")";
                    }
                }
                paramCounter += MatricesNumber * 3;
                return true;
            }
            return false;
        }
        public double[,] Correletion()
        {
            double[,] cor;
            double[] med;
            double[] std;

            cor = new double[paramCounter,paramCounter];
            med = new double[paramCounter];
            std = new double[paramCounter];
            for (int i = 0; i < paramCounter; i++)
                for (int j = 0; j < paramCounter; j++)
                {
                    cor[i, j] = 0;
                    med[i] = 0;
                    std[i] = 0;
                }
            for (int i = 0; i < paramCounter; i++)
            {
                for (int k = 0; k < testImageNum; k++)
                {
                    med[i] += TestImageResults[k][i];
                    std[i] += (TestImageResults[k][i] * TestImageResults[k][i]);
                }                    
                med[i] /= (double)testImageNum;
                std[i] /= (double)testImageNum;
            }
            double den = 0;
           for (int i = 0; i < paramCounter; i++)
                for (int j = i; j < paramCounter; j++)
                {
                    for (int k = 0; k < testImageNum; k++)
                            cor[i, j] += (TestImageResults[k][i]-med[i]) * (TestImageResults[k][j]- med[j]);
                    den = (testImageNum * Math.Sqrt((std[i] - med[i] * med[i]) * (std[j] - med[j] * med[j])));
                    if (den != 0)
                        cor[i, j] /= den;// (testImageNum * Math.Sqrt((std[i] - med[i] * med[i]) * (std[j] - med[j] * med[j]))); 
                }
            return cor;
        }
        public double[,] CorreletionT(double th)
        {
            double[,] cor = Correletion();
            for (int i = 0; i < paramCounter; i++)
                for (int j = i; j < paramCounter; j++)
                {
                    if (cor[i, j] > th) cor[i, j] = 1;
                    else cor[i, j] = 0;
                }
            return cor;
        }
        private bool calculateHistograms()
        {
            if (loadingFlag)
            {
                for (int i = 0; i < testImageNum; i++)
                {
                    for (int y = 0; y < testImageSize[0, i]; y++)
                        for (int x = 0; x < testImageSize[1, i]; x++)
                        {
                            histograms[i][testImageList[i][y, x]]++;
                        }
                    for (int j = 0; j < 256; j++)
                    {
                        histograms[i][j] /= (testImageSize[0, i] * testImageSize[1, i]);
                    }
                }
                histogramCalcFlag = true;
                Calculate_m();
            }
            return histogramCalcFlag;
        }

        public bool AddResortDataAsPercentage(int[][] Data, int[] DataLength, int[] ObservationNum)
        {
            int len = DataLength.Length;
            double[][] newData = new double[testImageNum][];
            int filterNumber = 0;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < DataLength[i]; j++)
                {
                    if (filterNumber < Data[i][j]) filterNumber = Data[i][j];
                }
            }
            filterNumber++;
            if (paramNum >= (paramCounter + filterNumber))
            {
                int[] countOffiltersInObservationSet = new int[testImageNum];
                for (int i = 0; i < testImageNum; i++)
                {
                    newData[i] = new double[filterNumber];
                    for (int j = 0; j < filterNumber; j++) newData[i][j] = 0;
                }
                for (int i = 0; i < len; i++)
                {
                    newData[ObservationNum[i]] = new double[filterNumber]; 
                    for (int j = 0; j < DataLength[i]; j++)
                    {
                        newData[ObservationNum[i]][Data[i][j]]++;
                        countOffiltersInObservationSet[ObservationNum[i]]++;
                    }
                }
                for (int i = 0; i < testImageNum; i++)
                {
                    for (int j = 0; j < filterNumber; j++)
                    {
                        if (countOffiltersInObservationSet[i]>0)
                        newData[i][j] /= (double)countOffiltersInObservationSet[i];
                    }
                }
                for (int k = 0; k < testImageNum; k++)
                {
                    for (int l = 0; l < filterNumber; l++)
                        testImageResults[k][paramCounter + l] = newData[k][l];
                }
                paramCounter += filterNumber;
                return true;
            }
            return false;
        }

        public double[][] ResortDataAsPercentage(int[][] Data, int[] DataLength, int[] ObservationNum)
        {
            int len = DataLength.Length;
            double[][] newData = new double[ObservationNum[len - 1] + 1][];
            int filterNumber = 0;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < DataLength[i]; j++)
                {
                    if (filterNumber < Data[i][j]) filterNumber = Data[i][j];
                }
            }
            filterNumber++;
            int[] countOffiltersInObservationSet = new int[ObservationNum[len - 1] + 1];
            for (int i = 0; i < (ObservationNum[len - 1] + 1); i++)
            {
                newData[i] = new double[filterNumber];
            }
            for (int i = 0; i < len; i++)
            {
                newData[ObservationNum[i]] = new double[filterNumber];
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
        }
    }
}
