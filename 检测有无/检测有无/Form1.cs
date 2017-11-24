using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace 检测有无
{
    public partial class Form1 : Form
    {
        int i = 0;
        HTuple ModelID = null;
        public HObject ho_Image, ho_ImageR, ho_ImageG, ho_ImageB;
        public HObject ho_Rectangle, ho_ImageReduced, ho_ImageMean;
        public HObject ho_RegionDynThresh, ho_RegionErosion, ho_ConnectedRegions;
        public HObject ho_SelectedRegions, ho_RegionUnion, ho_RegionDilation;
        public HObject ho_ImageReduced1, ho_GrayImage;
        public HObject ho_Image1 = null, ho_Circle = null;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ModelSearch(HTuple hv_ModelID, int i,out HTuple hv_Row,out HTuple hv_Column)
        {
            // Local iconic variables   
           
            HTuple hv_num = new HTuple(), hv_a = new HTuple();
            HTuple hv_Seconds1 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_Seconds2 = new HTuple(), hv_Time = new HTuple();
            HTuple hv_HeightWin = null, hv_WidthWin = null, hv_n = 1;

            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Image);

            hv_Row = null;
            hv_Column = null;

            // stop(); only in hdevelop
            //**********************************************
            //**********************************************
            //设置模板完成开始匹配
            try
            {

                ho_Image.Dispose();
                HOperatorSet.ReadImage(out ho_Image, ("acA2500-14gc0" + i) + ".bmp");
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                HOperatorSet.CountSeconds(out hv_Seconds1);
                HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), 0.8, 15, 0.2, "none", 0, 0.9, out hv_Row,
                    out hv_Column, out hv_Angle, out hv_Score);
                HOperatorSet.CountSeconds(out hv_Seconds2);
                hv_Time = 1000.0 * (hv_Seconds2 - hv_Seconds1);
                HOperatorSet.GetImageSize(ho_Image, out hv_HeightWin, out hv_WidthWin);// 获取输入图像的尺寸
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, hv_WidthWin, hv_HeightWin);//将获得的图像铺满整个窗口
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "yellow");
                HOperatorSet.SetLineWidth(hWindowControl1.HalconWindow, 1);
                HOperatorSet.SetLineWidth(hWindowControl1.HalconWindow, 3);
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "green");
                HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");

                hv_num = new HTuple(hv_Row.TupleLength());
                HTuple end_val35 = hv_num - 1;
                HTuple step_val35 = 1;
                for (hv_a = 0; hv_a.Continue(end_val35, step_val35); hv_a = hv_a.TupleAdd(step_val35))
                {
                    ho_Circle.Dispose();
                    HOperatorSet.GenCircle(out ho_Circle, hv_Row.TupleSelect(hv_a), hv_Column.TupleSelect(
                        hv_a), 220);

                    HOperatorSet.DispObj(ho_Circle, hWindowControl1.HalconWindow);
                    
                }
                double d = hv_Time[0];
                txtNumber.Text = hv_num.ToString();
                txtTime.Text = d.ToString("0.##");
                ho_GrayImage.Dispose();
                ho_Image.Dispose();
                ho_Circle.Dispose();
            }
            catch
            {
                if (ho_Image.IsInitialized())
                {
                    MessageBox.Show("模型未建立", "Error");
                }
                else
                {
                    MessageBox.Show("图像不存在", "Error");
                }
            }
        }

        private HTuple CreatModel()
        {


            // Local control variables 

            HTuple hv_ModelID = null,  hv_Seconds1 = new HTuple();
            HTuple hv_num = new HTuple(), hv_a = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageR);
            HOperatorSet.GenEmptyObj(out ho_ImageG);
            HOperatorSet.GenEmptyObj(out ho_ImageB);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
        
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, "acA2500-14gc01.bmp");
            ho_ImageR.Dispose(); ho_ImageG.Dispose(); ho_ImageB.Dispose();
            HOperatorSet.Decompose3(ho_Image, out ho_ImageR, out ho_ImageG, out ho_ImageB
                );
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
            }
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, 1085, 1060, 0, 100, 65);
            ho_GrayImage.Dispose();
            HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rectangle, out ho_ImageReduced);
            ho_ImageMean.Dispose();
            HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, 60, 60);
            ho_RegionDynThresh.Dispose();
            HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean, out ho_RegionDynThresh,
                5, "light");
            ho_RegionErosion.Dispose();
            HOperatorSet.ErosionCircle(ho_RegionDynThresh, out ho_RegionErosion, 3.5);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionErosion, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", 150, 99999);
            ho_RegionUnion.Dispose();
            HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
            ho_RegionDilation.Dispose();
            HOperatorSet.DilationCircle(ho_RegionUnion, out ho_RegionDilation, 5);
            ho_ImageReduced1.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionDilation, out ho_ImageReduced1);
            HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", (new HTuple(0)).TupleRad()
                , (new HTuple(360)).TupleRad(), "auto", "auto", "use_polarity", "auto", "auto",
                out hv_ModelID);
            

            ho_Image.Dispose();
            ho_ImageR.Dispose();
            ho_ImageG.Dispose();
            ho_ImageB.Dispose();
            ho_Rectangle.Dispose();
            ho_GrayImage.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageMean.Dispose();
            ho_RegionDynThresh.Dispose();
            ho_RegionErosion.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionUnion.Dispose();
            ho_RegionDilation.Dispose();
            ho_ImageReduced1.Dispose();

            return hv_ModelID;
        }
       
        private void btnstart_Click(object sender, EventArgs e)
        {
            i += 1;
            int a = 0,b=0;
            HTuple hv_Row = null;
            HTuple hv_Column = null;
            ModelSearch(ModelID,i,out hv_Row,out hv_Column);
            int [] Ax= hv_Column;
            int [] Ay = hv_Row;
            //分割区域
            int x1 = 750, y1 = 850, x2 = 1250, y2 = 1250;
            for (int k = 0; k < Ay.Length; k++)
            {
                if (Ay[k] < y1)
                {
                    a = 0;
                }
                else if (Ay[k] < y2 && Ay[k] > y1)
                {
                    a = 3;
                }
                else
                {
                    a = 6;
                }
                for (int j = 0; j < Ax.Length; j++)
                {
                    if (Ay[j] < x1)
                    {
                        b = 1;
                    }
                    else if (Ay[j] < x2 && Ay[j] > x1)
                    {
                        b = 2;
                    }
                    else
                    {
                        b = 3;
                    }
                }
            }
            

        }

        private void btnmodel_Click(object sender, EventArgs e)
        {
            ModelID=CreatModel();
        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            i = 0;
            try
            {
                HOperatorSet.ClearWindow(hWindowControl1.HalconWindow);
                HOperatorSet.ClearShapeModel(ModelID);
                ho_Image.Dispose();
                ho_ImageR.Dispose();
                ho_ImageG.Dispose();
                ho_ImageB.Dispose();
                ho_Rectangle.Dispose();
                ho_GrayImage.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionErosion.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDilation.Dispose();
                ho_ImageReduced1.Dispose();
                ho_GrayImage.Dispose();
                ho_Image1.Dispose();
                ho_Circle.Dispose();
            }
            catch
            {
            }           
        }
    }
}
