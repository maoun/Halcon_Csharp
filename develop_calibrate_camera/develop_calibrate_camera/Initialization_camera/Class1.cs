/*************************************************************************************
		* CLR版本 ：       4.0.30319.42000
		* 类 名 称：       Class1
		* 机器名称：       AFOHC-608062000
		* 命名空间：       develop_calibrate_camera.Initialization_camera
		* 文 件 名：       Class1
		* 创建时间：       2017/8/11 17:10:46
		* 作    者：       mao
		* 说    明：。。。。。
		* 修改时间：
		* 修 改 人：
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;                          //引用halcon
using System.IO;                            //调用系统文件目录
using IniFile;

namespace Machine_Vision
{
    public partial class FormFindCircle : Form
    {

        public int Tnum;
        public FormFindCircle()
        {
            InitializeComponent();
        }

        private HalconWindow wch = new HalconWindow();

        public HTuple ht_hWindowHandle_FindCircle; //显示图形窗口的句柄
        //声明图像变量
        public HObject ho_source_Image;         //原图像
        public HObject ho_gray_Image;           //灰度图
        public HObject ho_invert_Image;
        public HObject ho_filter_Image;         //形态处理后图
        public HObject ho_bin_Image;            //二值图
        public HObject ho_morph_Image;          //形态学处理

        //定义寻找区域
        HObject ho_RectangleROI;
        //测量参数
        public int m_minCircle = 1;      //最小外界圆
        public int m_maxCircle = 50;     //最大外界圆

        //声明ROI的坐标
        public HTuple ht_r1, ht_c1, ht_r2, ht_c2;
        public string rootpath;

        //主窗体加载事件
        private void FormFindCircle_Load(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_invert_Image);
            HOperatorSet.GenEmptyObj(out ho_RectangleROI);
            HOperatorSet.GenEmptyObj(out ho_source_Image);
            HOperatorSet.GenEmptyObj(out ho_gray_Image);           //灰度图
            HOperatorSet.GenEmptyObj(out ho_bin_Image);            //二值图
            HOperatorSet.GenEmptyObj(out ho_filter_Image);         //滤波处理
            HOperatorSet.GenEmptyObj(out ho_morph_Image);          //形态处理后图

            HOperatorSet.DumpWindowImage(out ho_source_Image, hw_Ctrl_FindCircle.HalconWindow);
            ht_hWindowHandle_FindCircle = hw_Ctrl_FindCircle.HalconWindow;

            this.cmbFilter.Items.Clear();
            cmbFilter.Items.Add("None");
            cmbFilter.Items.Add("SmoothImage");
            cmbFilter.Items.Add("SigmaImage");
            cmbFilter.Items.Add("MeanImage");
            cmbFilter.Items.Add("GaussImage");
            cmbFilter.Items.Add("MedianImage");
            cmbFilter.Items.Add("MeanImage");
            cmbFilter.SelectedIndex = 1;

            LoadResultView();
        }

        /*****************树形列表**************************************/
        private void LoadResultView()
        {

            this.ResultView.GridLines = true;
            this.ResultView.View = View.Details;
            this.ResultView.FullRowSelect = false;      //选中模式
            this.ResultView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.ResultView.Clear();
            //添加列表头
            this.ResultView.Columns.Add("Num", 40, HorizontalAlignment.Center); //一步添加 
            this.ResultView.Columns.Add("X", 80, HorizontalAlignment.Center);
            this.ResultView.Columns.Add("Y", 80, HorizontalAlignment.Center);
            this.ResultView.Columns.Add("R", 80, HorizontalAlignment.Center);
            //this.ResultView.BeginUpdate();              //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            //for (int i = 0; i < CountTemplateID; i++)   //添加行数据 
            //{
            //    ListViewItem lvi = new ListViewItem();
            //    lvi.ImageIndex = i;                     //通过与imageList绑定，显示imageList中第i项图标 
            //    lvi.Text = "Item" + i;
            //    lvi.SubItems.Add("*");
            //    lvi.SubItems.Add("*");
            //    lvi.SubItems.Add("*");
            //    lvi.SubItems.Add("*");
            //    lvi.SubItems.Add("*");
            //    lvi.SubItems.Add("*");
            //    this.ResultView.Items.Add(lvi);
            //}
            //this.ResultView.EndUpdate();  //结束数据处理，UI界面一次性绘制。
        }


        //加载窗体控件设定值
        private void loadInitSetValue(int row)
        {
            //Global.TemplateStruct[row].SourceImage    //预留
            //Global.TemplateStruct[row].Filter 
            //Global.TemplateStruct[row].R1
            //Global.TemplateStruct[row].R2
            //Global.TemplateStruct[row].R3


            //this.Text = Global.TemplateDT.Rows[row][0].ToString();
            //this.tBarBin_Threshold.Value = Convert.ToInt32(Global.TemplateDT.Rows[row][1]);

            //this.txtNumLevels.Text = Global.TemplateDT.Rows[row][4].ToString();
            //this.txtAngleExtent.Text = Global.TemplateDT.Rows[row][5].ToString();

            //this.txtMinContrast.Text = Global.TemplateDT.Rows[row][6].ToString();
            //this.txtLowLimit.Text = Global.TemplateDT.Rows[row][9].ToString();

            //if (Convert.ToInt32(Global.TemplateDT.Rows[row][10]) == 0)
            //    this.chkSubpixel.Checked = false;
            //else
            //    this.chkSubpixel.Checked = true;


        }

        //public void HobjectToHimage(HObject hobject, ref HImage image)      //类型转换
        //{
        //    HTuple pointer, type, width, height;

        //    HOperatorSet.GetImagePointer1(hobject, out pointer, out type, out width, out height);
        //    image.GenImage1(type, width, height, pointer);
        //}

        ///******************************************************************************************/
        ////显示图像
        ///******************************************************************************************/
        //public void ShowImage(string Path, out HObject Image, HWindowControl hWindowControl, HTuple Angle)
        //{
        //    HTuple hWindowHandle = hWindowControl.HalconWindow;
        //    HTuple width, height;
        //    HOperatorSet.ReadImage(out Image, Path);
        //    HOperatorSet.RotateImage(Image, out Image, Angle, "constant");
        //    HOperatorSet.GetImageSize(Image, out width, out height);
        //    if (1.0 * width[0].I / hWindowControl.Width > 1.0 * height[0].I / hWindowControl.Height)
        //    {
        //        double real = 1.0 * width[0].I / hWindowControl.Width;
        //        HOperatorSet.SetPart(hWindowHandle, 0, 0, real * hWindowControl.Height, real * hWindowControl.Width);
        //        HOperatorSet.DispObj(Image, hWindowHandle);
        //    }
        //    else
        //    {
        //        double real = 1.0 * height[0].I / hWindowControl.Height;
        //        HOperatorSet.SetPart(hWindowHandle, 0, 0, real * hWindowControl.Height, real * hWindowControl.Width);
        //        HOperatorSet.DispObj(Image, hWindowHandle);
        //    }
        //}

        private void btnTurngray_Click(object sender, EventArgs e)      //图像转灰度
        {

            if (ho_source_Image != null)
            {
                //                HObject ho_equimage;
                HOperatorSet.Rgb1ToGray(ho_source_Image, out ho_gray_Image);
                //HOperatorSet.EquHistoImage(ho_equimage,out ho_gray_Image);  //灰度直方图均衡化
                //ho_equimage.Dispose();
                HOperatorSet.DispObj(ho_gray_Image, ht_hWindowHandle_FindCircle);
            }
            else
            {
                labStateMessage.Text = "NO INPUT IMAGE";
            }
        }

        private void chkInversion_CheckedChanged(object sender, EventArgs e)
        {
            if (ho_gray_Image != null)
            {
                if (chkInversion.Checked == true)
                {
                    HOperatorSet.InvertImage(ho_gray_Image, out ho_invert_Image);
                    HOperatorSet.DispObj(ho_invert_Image, ht_hWindowHandle_FindCircle);
                }
                else
                {
                    HOperatorSet.DispObj(ho_gray_Image, ht_hWindowHandle_FindCircle);
                }
            }

        }


        private void tBarBin_Threshold_ValueChanged(object sender, EventArgs e)
        {
            labbinThreshold.Text = tBarBin_Threshold.Value.ToString();

        }

        private void btnFilter_Click(object sender, EventArgs e)        //灰度滤波
        {
            if (ho_gray_Image != null)
            {

                if (chkInversion.Checked == true)
                {
                    HOperatorSet.InvertImage(ho_gray_Image, out ho_invert_Image);
                    //       HOperatorSet.DispObj(ho_invert_Image, ht_hWindowHandle_FindCircle);
                    HOperatorSet.CopyObj(ho_invert_Image, out ho_gray_Image, 1, 1);
                }

                int mode = cmbFilter.SelectedIndex;
                switch (mode)
                {
                    case 0:
                        ho_filter_Image = ho_gray_Image;//ho_filter_Image
                        break;
                    case 1:
                        HOperatorSet.SmoothImage(ho_gray_Image, out ho_filter_Image, "deriche2", 0.5);//'deriche1', 'deriche2', 'shen' and 'gauss'.// 递归滤波                       
                        break;
                    case 2:
                        HOperatorSet.SigmaImage(ho_gray_Image, out ho_filter_Image, 5, 5, 3);   // 标准方差滤波
                        break;
                    case 3:
                        HOperatorSet.MeanImage(ho_gray_Image, out ho_filter_Image, 9, 9);      //均值滤波
                        break;
                    case 4:
                        HOperatorSet.GaussImage(ho_gray_Image, out ho_filter_Image, 5);      //离散高斯滤波
                        break;
                    case 5:
                        HOperatorSet.MedianImage(ho_gray_Image, out ho_filter_Image, "circle", 1, "mirrored");
                        //  HOperatorSet.MedianImage(ho_bin_Image, out ho_filter_Image,"square",1,"mirrored");      //中值滤波
                        break;
                    case 6:
                        HOperatorSet.Emphasize(ho_gray_Image, out ho_filter_Image, 7, 7, 1.0);      //图像锐化
                        break;
                    default: break;
                }
                HOperatorSet.DispObj(ho_filter_Image, ht_hWindowHandle_FindCircle);   //显示
            }
            else
            {
                labStateMessage.Text = "NO INPUT IMAGE";
            }
        }
        private void btnToBin_Click(object sender, EventArgs e) //二值化
        {
            if (ho_filter_Image != null)
            {
                int Threshold = tBarBin_Threshold.Value;
                //   HObject th_image;
                ////   HOperatorSet.Threshold(ho_filter_Image, out th_image, 0, Threshold);
                //   if (chkInversion.Checked == false)
                //       HOperatorSet.Threshold(ho_filter_Image, out th_image, tBarBin_Threshold.Value, 255);   //二值化
                //   else
                //       HOperatorSet.Threshold(ho_filter_Image, out th_image, 0, tBarBin_Threshold.Value);   //二值化
                //   HTuple width, height;
                //   HOperatorSet.GetImageSize(ho_source_Image, out width, out height);
                //   if (chkInversion.Checked == true)
                //       HOperatorSet.RegionToBin(th_image, out ho_bin_Image, 255, 0, width, height);
                //   else
                //       HOperatorSet.RegionToBin(th_image, out ho_bin_Image, 0, 255, width, height);
                HOperatorSet.Threshold(ho_filter_Image, out ho_bin_Image, tBarBin_Threshold.Value, 255);

                HOperatorSet.DispObj(ho_bin_Image, ht_hWindowHandle_FindCircle);
                //     th_image.Dispose();
            }
            else
            {
                labStateMessage.Text = "NO INPUT IMAGE";
            }
        }
        private void btnMorphology_Click(object sender, EventArgs e)//形态处理
        {
            if (ho_bin_Image != null)
            {
                //HObject RegionErosion;
                //HOperatorSet.GenEmptyObj(out RegionErosion);

                //HObject se;
                //HOperatorSet.GenDiscSe(out se, "byte", 4, 4, 0);
                //HOperatorSet.GrayErosion(ho_bin_Image, se, out ho_filter_Image);  //腐蚀原图
                //HOperatorSet.GrayDilation(ho_bin_Image, se, out ho_filter_Image);  //膨胀原图

                //HOperatorSet.ErosionCircle(ho_bin_Image, out RegionErosion, 5.5);//圆形结构 先腐蚀
                //HOperatorSet.DilationCircle(RegionErosion, out ho_filter_Image, 5.5);//圆形结构 在膨胀

                //HOperatorSet.GrayErosionRect(ho_bin_Image, out RegionErosion, 5, 5);//圆形结构 在腐蚀
                //HOperatorSet.GrayDilationRect(RegionErosion, out ho_morph_Image, 5, 5);//圆形结构 先膨胀

                //HOperatorSet.GrayDilationRect(ho_bin_Image, out RegionErosion, 10, 10);//圆形结构 在腐蚀
                //HOperatorSet.GrayErosionRect(RegionErosion, out ho_morph_Image, 10, 10);//圆形结构 先膨胀

                //HOperatorSet.DispObj(ho_morph_Image, ht_hWindowHandle_FindCircle);   //显示
                //RegionErosion.Dispose();

                HObject ho_regionOpening;// ho_connectedRegions;
                HOperatorSet.GenEmptyObj(out ho_regionOpening);
                //     HOperatorSet.GenEmptyObj(out ho_connectedRegions);
                if (radioBtnOpen.Checked == true)
                    HOperatorSet.OpeningCircle(ho_bin_Image, out ho_regionOpening, 10);
                else
                    HOperatorSet.ClosingCircle(ho_bin_Image, out ho_regionOpening, 10);

                HOperatorSet.Connection(ho_regionOpening, out ho_morph_Image);
                HOperatorSet.DispObj(ho_morph_Image, ht_hWindowHandle_FindCircle);   //显示
            }
            else
            {
                labStateMessage.Text = "NO INPUT IMAGE";
            }
        }

        private void btnROI_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(ho_source_Image, ht_hWindowHandle_FindCircle);

            HOperatorSet.SetDraw(ht_hWindowHandle_FindCircle, "margin");      //只显示边框
            HOperatorSet.SetColor(ht_hWindowHandle_FindCircle, "blue");       //设置画笔的颜色及线宽
            HOperatorSet.SetLineWidth(ht_hWindowHandle_FindCircle, 2);
            //设置ROI区域
            hw_Ctrl_FindCircle.Focus();
            btnFindCirle.Enabled = false;
            HOperatorSet.DrawRectangle1(ht_hWindowHandle_FindCircle, out ht_r1, out ht_c1, out ht_r2, out ht_c2);
            btnFindCirle.Enabled = true;

            HOperatorSet.DispRectangle1(ht_hWindowHandle_FindCircle, ht_r1, ht_c1, ht_r2, ht_c2);
            wch.disp_message(ht_hWindowHandle_FindCircle, " ROI ", "image", ht_r1, ht_c1, "blue", "true");


            HOperatorSet.GenRectangle1(out ho_RectangleROI, ht_r1, ht_c1, ht_r2, ht_c2);    //获得矩形区域

            //string tifpath = rootpath + @"\" + this.Text + "_ROI.tif";
            //HOperatorSet.WriteRegion(ho_RectangleROI, tifpath);
        }



        public HTuple hv_Row, hv_Column, hv_Angle, hv_Score;
        //      /******************************************************************************************/
        //      //模板匹配事件
        //      /******************************************************************************************/
        //      private void btnMatchingTest_Click(object sender, EventArgs e)
        //      {                   

        //          HObject ho_RectangleROI;        //ROI区域
        //          HOperatorSet.GenEmptyObj(out ho_RectangleROI);

        //          try
        //          {
        //              string shmpath = rootpath + @"\" + this.Text + "_Model.shm";
        //              HOperatorSet.ReadShapeModel(shmpath, out ht_ModelID);        //读取模板ID


        //              string tifpath = rootpath + @"\" + this.Text + "_ROI.tif";
        //              HOperatorSet.ReadRegion(out ho_RectangleROI, tifpath);       //读取矩形ROI
        //          }
        //          catch (HalconException ex)
        //          {
        //              MessageBox.Show(ex.Message);
        //          }

        //          if (ht_ModelID == null)
        //          {
        //              wch.disp_message(ht_hWindowHandle_Measure, "NO SET MODEL", "image", 1, 1, "red", "false");
        //              return;
        //          }

        //          HOperatorSet.GenEmptyObj(out ho_gray_Image);
        //          HOperatorSet.Rgb1ToGray(ho_source_Image, out ho_gray_Image);    //灰度化

        //          HOperatorSet.GenEmptyObj(out ho_filter_Image);
        //          int mode = cmbFilter.SelectedIndex;
        //          switch (mode)
        //          {
        //              case 0:
        //                  ho_filter_Image = ho_gray_Image;//ho_filter_Image
        //                  break;
        //              case 1:
        //                  HOperatorSet.SmoothImage(ho_gray_Image, out ho_filter_Image, "deriche2", 0.5);//'deriche1', 'deriche2', 'shen' and 'gauss'.// 递归滤波                       
        //                  break;
        //              case 2:
        //                  HOperatorSet.SigmaImage(ho_gray_Image, out ho_filter_Image, 5, 5, 3);   // 标准方差滤波
        //                  break;
        //              case 3:
        //                  HOperatorSet.MeanImage(ho_gray_Image, out ho_filter_Image, 9, 9);      //均值滤波
        //                  break;
        //              case 4:
        //                  HOperatorSet.GaussImage(ho_gray_Image, out ho_filter_Image, 5);      //离散高斯滤波
        //                  break;
        //              case 5:
        //                  HOperatorSet.MedianImage(ho_gray_Image, out ho_filter_Image, "circle", 1, "mirrored");
        //                  //  HOperatorSet.MedianImage(ho_bin_Image, out ho_filter_Image,"square",1,"mirrored");      //中值滤波
        //                  break;
        //              case 6:
        //                  HOperatorSet.Emphasize(ho_gray_Image, out ho_filter_Image, 7, 7, 1.0);      //图像锐化
        //                  break;
        //              default: break;
        //          }

        //          HObject th_image;
        //          HOperatorSet.GenEmptyObj(out th_image);
        //          HOperatorSet.Threshold(ho_filter_Image, out th_image, 0, tBarBin_Threshold.Value);   //二值化
        //          HObject ho_bin_Image;
        //          HOperatorSet.GenEmptyObj(out ho_bin_Image);
        //          HTuple width, height;
        //          HOperatorSet.GetImageSize(ho_source_Image, out width, out height);
        //          HOperatorSet.RegionToBin(th_image, out ho_bin_Image, 0, 255, width, height);//获取二值区域


        //          HObject RegionErosion;
        //          HOperatorSet.GenEmptyObj(out RegionErosion);

        //          HOperatorSet.GrayErosionRect(ho_bin_Image, out RegionErosion, 5, 5);//圆形结构 在腐蚀
        //          HOperatorSet.GrayDilationRect(RegionErosion, out ho_morph_Image, 5, 5);//圆形结构 先膨胀 

        //          //*********************
        //          HOperatorSet.SetDraw(ht_hWindowHandle_Measure, "margin");      //只显示边框
        //          HOperatorSet.SetColor(ht_hWindowHandle_Measure, "blue");       //设置画笔的颜色及线宽
        //          HOperatorSet.SetLineWidth(ht_hWindowHandle_Measure, 2);

        //          HObject ho_ModelContours;       //定义模板轮廓
        //          HOperatorSet.GenEmptyObj(out ho_ModelContours);
        //          HOperatorSet.GetShapeModelContours(out ho_ModelContours, ht_ModelID, 1);       //获取形状模型的轮廓

        //          //HOperatorSet.DispRectangle1(ht_hWindowHandle_Tempalte, ht_r1, ht_c1, ht_r2, ht_c2);//仅显示
        //          //wch.disp_message(ht_hWindowHandle_Tempalte, " ROI ", "image", ht_r1, ht_c1, "blue", "true");

        //          HOperatorSet.DispRegion(ho_RectangleROI, ht_hWindowHandle_Measure);
        //          //HOperatorSet.GenEmptyObj(out ho_RectangleROI);
        //          //HOperatorSet.GenRectangle1(out ho_RectangleROI, ht_r1, ht_c1, ht_r2, ht_c2);    //获得矩形区域
        //          HObject ho_ImageROI;
        //          HOperatorSet.GenEmptyObj(out ho_ImageROI);
        //          HOperatorSet.ReduceDomain(ho_morph_Image, ho_RectangleROI, out ho_ImageROI);   //获取图像区域 形态学处理后

        //          hw_Ctrl_Measure.Focus();
        //          HOperatorSet.FindShapeModel(ho_ImageROI, ht_ModelID, -0.39, 0.79, 0.5, 0.5, 0.5, "least_squares", 0, 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);

        //          if ((int)(new HTuple(hv_Score.TupleNotEqual(new HTuple()))) != 0)
        //          {

        //              HTuple hv_MovementOfObject;
        //              HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out hv_MovementOfObject);
        //              HObject ho_ModelAtNewPosition;            
        //              HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ModelAtNewPosition, hv_MovementOfObject);
        //              //HOperatorSet.DispObj(ho_Image, ht_hWindowHandle_Tempalte);
        //              HOperatorSet.SetColor(ht_hWindowHandle_Measure, "green");
        //              HOperatorSet.SetLineWidth(ht_hWindowHandle_Measure, 1);        //设置线宽及颜色
        //              HOperatorSet.DispObj(ho_ModelAtNewPosition, ht_hWindowHandle_Measure);

        //              ho_ModelAtNewPosition.Dispose();

        //              HOperatorSet.DispRectangle2(ht_hWindowHandle_Measure, hv_Row, hv_Column, hv_Angle,
        //                  50, 50);    //绘制图形框
        //              HOperatorSet.DispCross(ht_hWindowHandle_Measure, hv_Row, hv_Column, 7, hv_Angle);  //绘制十字线
        //              //设置显示文字的样式
        //              wch.set_display_font(ht_hWindowHandle_Measure, 14, "mono", "true", "false");
        //              //显示的对象只有边缘线，
        //              HOperatorSet.SetDraw(ht_hWindowHandle_Measure, "margin");
        //              //线宽用Line Width 指定 s
        //              HOperatorSet.SetLineWidth(ht_hWindowHandle_Measure, 2);
        //              HOperatorSet.SetColor(ht_hWindowHandle_Measure, "blue");

        //              //显示ROI及文字结果
        //              wch.disp_message(ht_hWindowHandle_Measure, (((((("Result: " + hv_Row) + new HTuple(",")) + hv_Column) + new HTuple(",")) + hv_Angle) + "\r\n Score:") + (hv_Score * 100).TupleString(".2f") + "%", "window", 1, 1, "green", "false");
        //              string bmppath = rootpath + @"\" + this.Text + "_TestResult.bmp";
        //              HOperatorSet.DumpWindow(ht_hWindowHandle_Measure, "bmp", bmppath);
        //          }
        //          else
        //          {
        //              wch.disp_message(ht_hWindowHandle_Measure, "NO MATCHING", "image", 1, 1, "red", "false");
        //          }

        //          ho_RectangleROI.Dispose();      //内存释放
        //          ho_ImageROI.Dispose();
        //          ho_ModelContours.Dispose();
        ////          HOperatorSet.ClearTemplate(ht_ModelID);

        //          labStateMessage.Text = "匹配完成！";
        //      }


        /******************************************************************************************/
        //鼠标滚轮事件
        /******************************************************************************************/
        private void hWindowControl_FindCircle_HMouseWheel(object sender, HMouseEventArgs e)
        {
            HTuple mode = e.Delta;  //获取滚轮的方向
            int button_state;
            double mouse_post_row, mouse_post_col;
            //获取当前鼠标的位置
            hw_Ctrl_FindCircle.HalconWindow.GetMpositionSubPix(out mouse_post_row, out mouse_post_col, out button_state);
            wch.DispImageZoom(ho_source_Image, hw_Ctrl_FindCircle, mode, mouse_post_row, mouse_post_col);

        }


        /******************************************************************************************/
        //图像尺寸改变按钮事件
        /******************************************************************************************/
        private void btnChangebig_Click(object sender, EventArgs e)
        {
            HTuple mode = 1;  //放大
            double mouse_post_row, mouse_post_col;
            mouse_post_row = hw_Ctrl_FindCircle.WindowSize.Width / 2;      //窗体的中心
            mouse_post_col = hw_Ctrl_FindCircle.WindowSize.Height / 2;

            wch.DispImageZoom(ho_source_Image, hw_Ctrl_FindCircle, mode, mouse_post_row, mouse_post_col);
        }

        private void btnChangesmall_Click(object sender, EventArgs e)
        {
            HTuple mode = -1;  //缩小
            double mouse_post_row, mouse_post_col;
            mouse_post_row = hw_Ctrl_FindCircle.WindowSize.Width / 2;      //窗体的中心
            mouse_post_col = hw_Ctrl_FindCircle.WindowSize.Height / 2;

            wch.DispImageZoom(ho_source_Image, hw_Ctrl_FindCircle, mode, mouse_post_row, mouse_post_col);
        }

        private void btnFitwindow_Click(object sender, EventArgs e)
        {
            wch.DispImageFit(ho_source_Image, hw_Ctrl_FindCircle);        //最大化适应窗体尺寸显示
        }

        private void btnOpenimage_Click(object sender, EventArgs e)
        {
            this.hw_Ctrl_FindCircle.HalconWindow.ClearWindow();        //清空显示

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "BMP文件(*.bmp)|*.bmp|所有文件(*.*)|*.*||";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string Path = openFile.FileName;
                HOperatorSet.ReadImage(out ho_source_Image, Path);
                // HOperatorSet.DispObj(ho_source_Image, ht_hWindowHandle_Tempalte);
                wch.DispImageFit(ho_source_Image, hw_Ctrl_FindCircle);
                HTuple hv_HeightWin, hv_WidthWin;
                HOperatorSet.GetImageSize(ho_source_Image, out hv_HeightWin, out hv_WidthWin);          // 获取输入图像的尺寸
                String str_imgSize = String.Format("Size:{0}x{1}", hv_HeightWin, hv_WidthWin);
                wch.disp_message(ht_hWindowHandle_FindCircle, str_imgSize, "window", hw_Ctrl_FindCircle.Height - 20, 1, "blue", "false");

            }
        }

        private void btnSaveas_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "图像|*.bmp";
            //得到选择的路径
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.DumpWindow(ht_hWindowHandle_FindCircle, "bmp", saveFileDialog.FileName.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            //Global.TemplateDT.Rows[Tnum][1] = this.tBarBin_Threshold.Value;
            //Global.TemplateDT.Rows[Tnum][2] = this.cmbFilter.SelectedIndex;
            //Global.TemplateDT.Rows[Tnum][4] = Convert.ToInt32(this.txtNumLevels.Text);
            //Global.TemplateDT.Rows[Tnum][5] = Convert.ToInt32(this.txtAngleExtent.Text);
            //Global.TemplateDT.Rows[Tnum][6] = Convert.ToInt32(this.txtMinContrast.Text);
            //Global.TemplateDT.Rows[Tnum][9] = Convert.ToInt32(this.txtLowLimit.Text);

            //if (this.chkSubpixel.Checked == true)
            //    Global.TemplateDT.Rows[Tnum][10] = 1;
            //else
            //    Global.TemplateDT.Rows[Tnum][10] = 0;
            //IniFiles ini = new IniFiles(Global.Rootdirectory + @"\Measure" + @"\Config.ini");        //读取配置文件

            //m_Metro_Sig1 = double.Parse(txtMin.Text);
            //m_Metro_Thre1 = int.Parse(txtMax.Text);

            //ini.WriteString("Measure", "sigma", txtMin.Text);
            //ini.WriteString("Measure", "Threshold", txtMax.Text);

            //this.Close();
        }
        private void CreatTransRegion(HObject m_ModelRegion_1, HTuple Row, HTuple Column, HTuple Angle, ref HObject AffineRectangle)
        {
            ////计算模板测量区域中心
            //HTuple Area_Ref_1, Row_Ref_1, Col_Ref_1;
            //HOperatorSet.AreaCenter(m_ModelRegion_1, out Area_Ref_1, out Row_Ref_1, out Col_Ref_1);

            //HTuple Alignment;

            //HOperatorSet.HomMat2dIdentity(out Alignment);
            //HOperatorSet.HomMat2dTranslate(Alignment, -Row_Ref_1, -Col_Ref_1, out Alignment);
            //HOperatorSet.HomMat2dRotate(Alignment, Angle, 0, 0, out Alignment);
            //HOperatorSet.HomMat2dTranslate(Alignment, Row, Column, out Alignment);

            ////对左边矩形进行仿射变换
            //HOperatorSet.AffineTransRegion(m_MeasureRectanle, out AffineRectangle, Alignment, "nearest_neighbor");

        }

        private void FormFindCircle_FormClosing(object sender, FormClosingEventArgs e)
        {
            //           ho_invert_Image.Dispose();
            ho_source_Image.Dispose();         //原图像 释放资源
            ho_gray_Image.Dispose();           //灰度图
            ho_bin_Image.Dispose();            //二值图
            ho_filter_Image.Dispose();         //形态处理后图
            ho_morph_Image.Dispose();
            ho_RectangleROI.Dispose();
            //if(ht_ModelID != null)
            //{
            //    HOperatorSet.ClearShapeModel(ht_ModelID);   //释放模板内存
            //}
        }

        private void btnMeasureCirle_Click(object sender, EventArgs e)
        {

            if (ho_source_Image == null)
            {
                return;
            }
            m_minCircle = Convert.ToInt32(txtMinCircle.Text);      //最小外界圆
            m_maxCircle = Convert.ToInt32(txtMaxCircle.Text);      //最小外界圆

            HOperatorSet.GenEmptyObj(out ho_gray_Image);
            HOperatorSet.Rgb1ToGray(ho_source_Image, out ho_gray_Image);    //灰度化


            if (chkInversion.Checked == true)
            {
                HOperatorSet.InvertImage(ho_gray_Image, out ho_invert_Image);
                HOperatorSet.DispObj(ho_invert_Image, ht_hWindowHandle_FindCircle);
                HOperatorSet.CopyObj(ho_invert_Image, out ho_gray_Image, 1, 1);
            }

            HOperatorSet.GenEmptyObj(out ho_filter_Image);
            int mode = cmbFilter.SelectedIndex;
            switch (mode)
            {
                case 0:
                    //    ho_filter_Image = ho_gray_Image;//ho_filter_Image
                    HOperatorSet.CopyObj(ho_gray_Image, out ho_filter_Image, 1, 1);
                    break;
                case 1:
                    HOperatorSet.SmoothImage(ho_gray_Image, out ho_filter_Image, "deriche2", 0.5);//'deriche1', 'deriche2', 'shen' and 'gauss'.// 递归滤波                       
                    break;
                case 2:
                    HOperatorSet.SigmaImage(ho_gray_Image, out ho_filter_Image, 5, 5, 3);   // 标准方差滤波
                    break;
                case 3:
                    HOperatorSet.MeanImage(ho_gray_Image, out ho_filter_Image, 9, 9);      //均值滤波
                    break;
                case 4:
                    HOperatorSet.GaussImage(ho_gray_Image, out ho_filter_Image, 5);      //离散高斯滤波
                    break;
                case 5:
                    HOperatorSet.MedianImage(ho_gray_Image, out ho_filter_Image, "circle", 1, "mirrored");
                    //  HOperatorSet.MedianImage(ho_bin_Image, out ho_filter_Image,"square",1,"mirrored");      //中值滤波
                    break;
                case 6:
                    HOperatorSet.Emphasize(ho_gray_Image, out ho_filter_Image, 7, 7, 1.0);      //图像锐化
                    break;
                default: break;
            }

            //HObject th_image;
            //HOperatorSet.GenEmptyObj(out th_image);

            //if (chkInversion.Checked == false)
            //    HOperatorSet.Threshold(ho_filter_Image, out th_image,  tBarBin_Threshold.Value,255);   //二值化
            //else
            //    HOperatorSet.Threshold(ho_filter_Image, out th_image, 0, tBarBin_Threshold.Value);   //二值化
            //HObject ho_bin_Image;
            //HOperatorSet.GenEmptyObj(out ho_bin_Image);
            //HTuple width, height;
            //HOperatorSet.GetImageSize(ho_source_Image, out width, out height);

            //      if (chkInversion.Checked == true)
            //          HOperatorSet.RegionToBin(th_image, out ho_bin_Image, 255, 0, width, height);
            //      else
            //          HOperatorSet.RegionToBin(th_image, out ho_bin_Image, 0, 255, width, height);

            HOperatorSet.Threshold(ho_filter_Image, out ho_bin_Image, tBarBin_Threshold.Value, 255);

            HOperatorSet.DispObj(ho_bin_Image, ht_hWindowHandle_FindCircle);
            HObject ho_regionOpening;// ho_connectedRegions;
            HOperatorSet.GenEmptyObj(out ho_regionOpening);

            if (radioBtnOpen.Checked == true)
                HOperatorSet.OpeningCircle(ho_bin_Image, out ho_regionOpening, 10);
            else
                HOperatorSet.ClosingCircle(ho_bin_Image, out ho_regionOpening, 10);

            HOperatorSet.Connection(ho_regionOpening, out ho_morph_Image);

            HOperatorSet.DispObj(ho_morph_Image, ht_hWindowHandle_FindCircle);

            HObject ho_ImageROI;
            HOperatorSet.GenEmptyObj(out ho_ImageROI);
            HOperatorSet.ReduceDomain(ho_morph_Image, ho_RectangleROI, out ho_ImageROI);   //获取图像区域 形态学处理后

            HOperatorSet.DispObj(ho_ImageROI, ht_hWindowHandle_FindCircle);
            //选择圆形 条件 圆度和最小外界圆
            HObject ho_selectedRegions, ho_regionErosion;
            HOperatorSet.SelectShape(ho_ImageROI, out ho_selectedRegions, (new HTuple("circularity")).TupleConcat(
                "outer_radius"), "and", (new HTuple(0.2)).TupleConcat(20), (new HTuple(m_minCircle)).TupleConcat(m_maxCircle));

            HOperatorSet.ErosionCircle(ho_selectedRegions, out ho_regionErosion, 0.5);
            //拟合圆
            HTuple hv_row, hv_column, hv_radius;
            HOperatorSet.SmallestCircle(ho_regionErosion, out hv_row, out hv_column, out hv_radius);

            HObject ho_Contcircle, ho_cross;
            HOperatorSet.GenEmptyObj(out ho_Contcircle);
            HOperatorSet.GenEmptyObj(out ho_cross);
            int hv_numbers;
            hv_numbers = new HTuple(hv_row.TupleLength());//(int)(new HTuple(hv_numbers.TupleGreater(0)))

            if (hv_numbers != 0)
            {
                HOperatorSet.SetDraw(ht_hWindowHandle_FindCircle, "margin");
                HOperatorSet.SetColor(ht_hWindowHandle_FindCircle, "green");
                HOperatorSet.DispObj(ho_source_Image, ht_hWindowHandle_FindCircle);

                HOperatorSet.GenCircleContourXld(out ho_Contcircle, hv_row, hv_column, hv_radius,
                    0, 6.28, "positive", 1);

                HOperatorSet.GenCrossContourXld(out ho_cross, hv_row, hv_column, 10, 0);
                //HTuple hv_string1 = "C" + hv_radius;
                ////HTuple hv_string2 = (("location =" + hv_row) + new HTuple(",")) + hv_column;
                //wch.disp_message(ht_hWindowHandle_FindCircle, hv_string1, "Image", hv_row - hv_radius,
                //    (hv_column - hv_radius) - 120, "blue", "false");
                //wch.disp_message(ht_hWindowHandle_FindCircle, hv_string2, "Image", (hv_row - hv_radius) - 20,
                //    (hv_column - hv_radius) - 120, "blue", "false");

                HOperatorSet.DispObj(ho_Contcircle, ht_hWindowHandle_FindCircle);
                HOperatorSet.DispObj(ho_cross, ht_hWindowHandle_FindCircle);

                this.ResultView.BeginUpdate();              //数据更新，UI暂时挂起
                this.ResultView.Items.Clear();
                for (HTuple hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_row.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    ListViewItem lvi = new ListViewItem();

                    lvi.Text = "C" + (hv_I + 1);
                    lvi.SubItems.Add(hv_row.TupleSelect(hv_I).TupleString(".3f"));
                    lvi.SubItems.Add(hv_column.TupleSelect(hv_I).TupleString(".3f"));
                    lvi.SubItems.Add(hv_radius.TupleSelect(hv_I).TupleString(".3f"));
                    this.ResultView.Items.Add(lvi);

                    HTuple hv_string1 = lvi.Text;
                    wch.disp_message(ht_hWindowHandle_FindCircle, hv_string1, "Image", hv_row.TupleSelect(hv_I) - hv_radius.TupleSelect(hv_I),
                        (hv_column.TupleSelect(hv_I) - hv_radius.TupleSelect(hv_I)), "blue", "false");
                }
                this.ResultView.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            }
            //       ho_invert_Image.Dispose();
            ho_morph_Image.Dispose();
            ho_regionOpening.Dispose();
            ho_selectedRegions.Dispose();
            ho_regionErosion.Dispose();
            ho_Contcircle.Dispose();
            ho_cross.Dispose();
        }









        /************************************END****************************************/
    }
}
