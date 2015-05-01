using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSorter
{
    class CooccurenceMatricesStatistics
    {
        private double[][,] cooccurenceMatrices;
        private int [,] matricesParam;
        private int matricesNumber;
        private byte[,] image;
        private int imageHeight;
        private int imageWidth;
        private double[] matricesSum;
        private int matriceSize;
        private double[,] functionOfDeltaHistogram;

        public CooccurenceMatricesStatistics(int [,] MatricesParam, int MatricesNumber, byte [,] Image, int ImageHeight, int ImageWidth)
        {
            if ((MatricesParam.Length / 2) == MatricesNumber)
            {
                matricesParam = MatricesParam;
                matricesNumber = MatricesNumber;
                cooccurenceMatrices = new double[matricesNumber][,];
                matriceSize = 256;
                functionOfDeltaHistogram = new double[matriceSize,matricesNumber];
                image = Image;
                imageHeight = ImageHeight;
                imageWidth = ImageWidth;
                matricesSum = new double[matricesNumber];
                GenerateCoocurenceMatrices();
                GenerateFunctionOfDeltaHistogram();
            }
        }
        public double[] GetContrast()
        {
            double[] contrast = new double[matricesNumber];
            contrast = new double[matricesNumber];
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                contrast[currentMatrixNumber] = 0;
                for (int i = 0; i < matriceSize; i++)
                    for (int j = 0; j < matriceSize; j++)
                    {
                        contrast[currentMatrixNumber]+=(i-j)*cooccurenceMatrices[currentMatrixNumber][i,j]/255.0;
                    }
                //probability value must be from 0 to 1 
                //contrast[currentMatrixNumber] /= (imageHeight*imageWidth);
            }
            return contrast;
        }
        public double[] GetAngularSecondMoment()
        {
            double[] angularSecondMoment;
            angularSecondMoment = new double[matricesNumber];
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                angularSecondMoment[currentMatrixNumber] = 0;
                for (int i = 0; i < matriceSize; i++)
                    for (int j = 0; j < matriceSize; j++)
                    {
                        angularSecondMoment[currentMatrixNumber] += cooccurenceMatrices[currentMatrixNumber][i, j] * cooccurenceMatrices[currentMatrixNumber][i, j];
                    }
                //probability value must be from 0 to 1 
                //angularSecondMoment[currentMatrixNumber] /= (imageHeight * imageWidth);
                //in sum square
                //angularSecondMoment[currentMatrixNumber] /= (imageHeight * imageWidth);
            }
            return angularSecondMoment;
        }
        public double[] GetEntrophy()
        {
            double[] entropy;
            entropy = new double[matricesNumber];
            double na = 0;
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                entropy[currentMatrixNumber] = 0;
                for (int i = 0; i < matriceSize; i++)
                    for (int j = 0; j < matriceSize; j++)
                    {
                        na = cooccurenceMatrices[currentMatrixNumber][i, j];// / matricesSum[currentMatrixNumber];
                        if (na > 0)
                            na = Math.Log(na,65536);
                        entropy[currentMatrixNumber] -= cooccurenceMatrices[currentMatrixNumber][i, j] * na;
                    }
                //probability value must be from 0 to 1 
               // entropy[currentMatrixNumber] /= matricesSum[currentMatrixNumber];
            }
            return entropy;
        }
        public double[] GetCorreletion()
        {
            double[] correletion = new double[matricesNumber];
            double[] rowSum = new double[matriceSize];
            double[] columnSum = new double[matriceSize];
            double meanX = 0, meanY = 0, sdX = 0, sdY = 0;
            correletion = new double[matricesNumber];
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                rowSum = new double[matriceSize];
                columnSum = new double[matriceSize];
                meanX = 0; meanY = 0; sdX = 0; sdY = 0;
                correletion[currentMatrixNumber] = 0;
                for (int i = 0; i < matriceSize; i++)
                    for (int j = 0; j < matriceSize; j++)
                    {
                        rowSum[i] += cooccurenceMatrices[currentMatrixNumber][i, j];
                        columnSum[j] += cooccurenceMatrices[currentMatrixNumber][i, j];
                        correletion[currentMatrixNumber] += i * j * cooccurenceMatrices[currentMatrixNumber][i, j]/65025.0;
                    }
                //probability value must be from 0 to 1 
                correletion[currentMatrixNumber] /= matricesSum[currentMatrixNumber];
                for (int i = 0; i < matriceSize; i++)
                {
                    meanX += i * rowSum[i]/255.0;
                    meanY += i * columnSum[i] / 255.0;
                    sdX += i * i * rowSum[i] / 65025.0;
                    sdY += i * i * columnSum[i] / 65025.0;
                }
                meanX /= matricesSum[currentMatrixNumber];
                meanY /= matricesSum[currentMatrixNumber];
                sdX /= matricesSum[currentMatrixNumber];
                sdY /= matricesSum[currentMatrixNumber];
                sdX -= meanX * meanX;
                sdY -= meanY * meanY;
                sdX = Math.Sqrt(sdX); sdY = Math.Sqrt(sdY);
                correletion[currentMatrixNumber] -= meanY * meanX;
                correletion[currentMatrixNumber] /= sdX * sdY;
            }
            return correletion;
        }
        public double[] GetContrastFromContrastHist()
        {
            double[] contrastH = new double[matricesNumber];
            contrastH = new double[matricesNumber];
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                contrastH[currentMatrixNumber] = 0;
                for (int i = 0; i < matriceSize; i++)
                {
                    contrastH[currentMatrixNumber] += i * i * functionOfDeltaHistogram[i, currentMatrixNumber] / 65025.0;
                }
                //probability value must be from 0 to 1 
                //contrastH[currentMatrixNumber] /= matricesSum[currentMatrixNumber];
            }
            return contrastH;
        }
        public double[] GetAngularSecondMomentFromContrastHist()
        {
            double[] angularSecondMomentH = new double[matricesNumber];
            angularSecondMomentH = new double[matricesNumber];
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                angularSecondMomentH[currentMatrixNumber] = 0;
                for (int i = 0; i < matriceSize; i++)
                {
                    angularSecondMomentH[currentMatrixNumber] += functionOfDeltaHistogram[i, currentMatrixNumber] * functionOfDeltaHistogram[i, currentMatrixNumber];
                }
                //probability value must be from 0 to 1 
                //angularSecondMomentH[currentMatrixNumber] /= matricesSum[currentMatrixNumber];
                //angularSecondMomentH[currentMatrixNumber] /= matricesSum[currentMatrixNumber];
            }
            return angularSecondMomentH;
        }
        public double[] GetEntrophyFromContrastHist()
        {
            double[] entrophyH = new double[matricesNumber];
            entrophyH = new double[matricesNumber];
            double na = 0;
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                entrophyH[currentMatrixNumber] = 0;
                for (int i = 0; i < matriceSize; i++)
                {
                    na = functionOfDeltaHistogram[i, currentMatrixNumber] ;/// matricesSum[currentMatrixNumber];
                    if (na > 0)
                    na = Math.Log(na,256);
                    entrophyH[currentMatrixNumber] += functionOfDeltaHistogram[i, currentMatrixNumber] * na;
                }
                //probability value must be from 0 to 1 
                //entrophyH[currentMatrixNumber] /= matricesSum[currentMatrixNumber];
            }
            return entrophyH;
        }
        public double[] GetMeanFromContrastHist()
        {
            double[] meanH = new double[matricesNumber];
            meanH = new double[matricesNumber];
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                meanH[currentMatrixNumber] = 0;
                for (int i = 0; i < matriceSize; i++)
                {
                    meanH[currentMatrixNumber] += i * functionOfDeltaHistogram[i, currentMatrixNumber];
                }
                //probability value must be from 0 to 1 
                //meanH[currentMatrixNumber] /= matricesSum[currentMatrixNumber];
                meanH[currentMatrixNumber] /= 255.0;
            }
            return meanH;
        }
        private bool GenerateCoocurenceMatrices()
        {
            int deltaX = 0;
            int deltaY = 0;
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                matricesSum[currentMatrixNumber] = 0;
                deltaX = matricesParam[0, currentMatrixNumber];
                deltaY = matricesParam[1, currentMatrixNumber];
                cooccurenceMatrices [currentMatrixNumber] = new double[matriceSize,matriceSize];
                for (int ii = 0; ii < matriceSize; ii++)
                    for (int jj = 0; jj < matriceSize; jj++)
                    {
                        cooccurenceMatrices [currentMatrixNumber][ii,jj] = 0;
                    }
                int i = 0, j_ = 0,iH = imageHeight,iW = imageWidth;
                if (deltaX < 0) i = -deltaX;
                else iH = imageHeight - deltaX;
                if (deltaY < 0) j_ = -deltaY;
                else iW = imageWidth - deltaY;
                for(; i < iH; i++)
                    for (int j = j_; j < iW; j++)
                    {
                        cooccurenceMatrices [currentMatrixNumber][image[i,j],image[i+deltaX,j+deltaY]] ++;
                        matricesSum[currentMatrixNumber]++;
                    }
                for (int ii = 0; ii < 256; ii++)
                    for (int jj = 0; jj < 256; jj++)
                    {
                        cooccurenceMatrices[currentMatrixNumber][ii, jj] /= (double)(imageWidth*imageHeight);
                    }
                matricesSum[currentMatrixNumber] /= (double)(imageWidth * imageHeight);
            }
            return true;
        }
        private bool GenerateFunctionOfDeltaHistogram()
        {
            for (int currentMatrixNumber = 0; currentMatrixNumber < matricesNumber; currentMatrixNumber++)
            {
                for (int i = 0; i < matriceSize; i++)
                    for (int j = i; j < matriceSize; j++)
                    {
                        if (i != j)
                        {
                            functionOfDeltaHistogram[Math.Abs(i - j), currentMatrixNumber] += cooccurenceMatrices[currentMatrixNumber][i, j] + cooccurenceMatrices[currentMatrixNumber][j, i];
                        }
                        else
                        {
                            functionOfDeltaHistogram[Math.Abs(i - j), currentMatrixNumber] += cooccurenceMatrices[currentMatrixNumber][i, i];
                        }
                    }
            }
            return true;
        }
    }
}
