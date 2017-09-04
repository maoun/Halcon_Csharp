using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HalconDotNet;

namespace develop_calibrate_camera.calibrate_camera
{
    public partial class Calibrate : Form
    {
        public Calibrate()
        {
            InitializeComponent();
        }

        //命名变量
        private string ImagePath;
        //

        private void Calibrate_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }

        private void chosefile_Click(object sender, EventArgs e)
        {
            txtFiles.Clear();
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.txtFiles.Text = folder.SelectedPath;
            }
            ImagePath = txtFiles.Text;
            txtLog.Text = "选择路径为：" + ImagePath +"\r\n"+DateTime.Now+"\r\n";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {          
        }

        public void list_image_files(HTuple hv_ImageDirectory, HTuple hv_Extensions, HTuple hv_Options,
        out HTuple hv_ImageFiles)
        {
            HTuple hv_HalconImages = null, hv_OS = null;
            HTuple hv_Directories = null, hv_Index = null, hv_Length = null;
            HTuple hv_network_drive = null, hv_Substring = new HTuple();
            HTuple hv_FileExists = new HTuple(), hv_AllFiles = new HTuple();
            HTuple hv_i = new HTuple(), hv_Selection = new HTuple();
            HTuple hv_Extensions_COPY_INP_TMP = hv_Extensions.Clone();
            HTuple hv_ImageDirectory_COPY_INP_TMP = hv_ImageDirectory.Clone();

            // Initialize local and output iconic variables 
            //This procedure returns all files in a given directory
            //with one of the suffixes specified in Extensions.
            //
            //input parameters:
            //ImageDirectory: as the name says
            //   If a tuple of directories is given, only the images in the first
            //   existing directory are returned.
            //   If a local directory is not found, the directory is searched
            //   under %HALCONIMAGES%/ImageDirectory. If %HALCONIMAGES% is not set,
            //   %HALCONROOT%/images is used instead.
            //Extensions: A string tuple containing the extensions to be found
            //   e.g. ['png','tif',jpg'] or others
            //If Extensions is set to 'default' or the empty string '',
            //   all image suffixes supported by HALCON are used.
            //Options: as in the operator list_files, except that the 'files'
            //   option is always used. Note that the 'directories' option
            //   has no effect but increases runtime, because only files are
            //   returned.
            //
            //output parameter:
            //ImageFiles: A tuple of all found image file names
            //
            if ((int)((new HTuple((new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(""))))).TupleOr(new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(
                "default")))) != 0)
            {
                hv_Extensions_COPY_INP_TMP = new HTuple();
                hv_Extensions_COPY_INP_TMP[0] = "ima";
                hv_Extensions_COPY_INP_TMP[1] = "tif";
                hv_Extensions_COPY_INP_TMP[2] = "tiff";
                hv_Extensions_COPY_INP_TMP[3] = "gif";
                hv_Extensions_COPY_INP_TMP[4] = "bmp";
                hv_Extensions_COPY_INP_TMP[5] = "jpg";
                hv_Extensions_COPY_INP_TMP[6] = "jpeg";
                hv_Extensions_COPY_INP_TMP[7] = "jp2";
                hv_Extensions_COPY_INP_TMP[8] = "jxr";
                hv_Extensions_COPY_INP_TMP[9] = "png";
                hv_Extensions_COPY_INP_TMP[10] = "pcx";
                hv_Extensions_COPY_INP_TMP[11] = "ras";
                hv_Extensions_COPY_INP_TMP[12] = "xwd";
                hv_Extensions_COPY_INP_TMP[13] = "pbm";
                hv_Extensions_COPY_INP_TMP[14] = "pnm";
                hv_Extensions_COPY_INP_TMP[15] = "pgm";
                hv_Extensions_COPY_INP_TMP[16] = "ppm";
                //
            }
            if ((int)(new HTuple(hv_ImageDirectory_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                hv_ImageDirectory_COPY_INP_TMP = ".";
            }
            HOperatorSet.GetSystem("image_dir", out hv_HalconImages);
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                hv_HalconImages = hv_HalconImages.TupleSplit(";");
            }
            else
            {
                hv_HalconImages = hv_HalconImages.TupleSplit(":");
            }
            hv_Directories = hv_ImageDirectory_COPY_INP_TMP.Clone();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_HalconImages.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_Directories = hv_Directories.TupleConcat(((hv_HalconImages.TupleSelect(hv_Index)) + "/") + hv_ImageDirectory_COPY_INP_TMP);
            }
            HOperatorSet.TupleStrlen(hv_Directories, out hv_Length);
            HOperatorSet.TupleGenConst(new HTuple(hv_Length.TupleLength()), 0, out hv_network_drive);
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((((hv_Directories.TupleSelect(hv_Index))).TupleStrlen()
                        )).TupleGreater(1))) != 0)
                    {
                        HOperatorSet.TupleStrFirstN(hv_Directories.TupleSelect(hv_Index), 1, out hv_Substring);
                        if ((int)(new HTuple(hv_Substring.TupleEqual("//"))) != 0)
                        {
                            if (hv_network_drive == null)
                                hv_network_drive = new HTuple();
                            hv_network_drive[hv_Index] = 1;
                        }
                    }
                }
            }
            hv_ImageFiles = new HTuple();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Directories.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.FileExists(hv_Directories.TupleSelect(hv_Index), out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    HOperatorSet.ListFiles(hv_Directories.TupleSelect(hv_Index), (new HTuple("files")).TupleConcat(
                        hv_Options), out hv_AllFiles);
                    hv_ImageFiles = new HTuple();
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Extensions_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_i = (int)hv_i + 1)
                    {
                        HOperatorSet.TupleRegexpSelect(hv_AllFiles, (((".*" + (hv_Extensions_COPY_INP_TMP.TupleSelect(
                            hv_i))) + "$")).TupleConcat("ignore_case"), out hv_Selection);
                        hv_ImageFiles = hv_ImageFiles.TupleConcat(hv_Selection);
                    }
                    HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("\\\\")).TupleConcat(
                        "replace_all"), "/", out hv_ImageFiles);
                    if ((int)(hv_network_drive.TupleSelect(hv_Index)) != 0)
                    {
                        HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("//")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFiles);
                        hv_ImageFiles = "/" + hv_ImageFiles;
                    }
                    else
                    {
                        HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("//")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFiles);
                    }

                    return;
                }
            }

            return;
        }

        private void action()
        {


            // Local iconic variables 

            HObject ho_Image, ho_MarkContours = null;

            // Local control variables 

            HTuple hv_AllCalibImageFiles = null, hv_ImageFiles = null;
            HTuple hv_Width = null, hv_Height = null, hv_CalibDataID = null;
            HTuple hv_CalPlateDescr = null, hv_StartParam = null, hv_Index = null;
            HTuple hv_StartPose = new HTuple(), hv_Error = null, hv_CameraParametersCalibrated = null;
            HTuple hv_ParLabels = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_MarkContours);
            //This example shows how to perform the basic calibration of a single
            //camera with multiple images of a calibration object.
            //Initialize visualization
            list_image_files(ImagePath, "default", new HTuple(), out hv_AllCalibImageFiles);
            HOperatorSet.TupleRegexpSelect(hv_AllCalibImageFiles, "calib_single_camera",
                out hv_ImageFiles);
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, hv_ImageFiles);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            //
            //Create the 3d object models that visualize the current configuration
            //of camera and calibration plate. This may take a moment.
            //
            //Part I
            //
            //Create and setup the calibration model
            //
            //First, create the calibration data structure
            HOperatorSet.CreateCalibData("calibration_object", 1, 1, out hv_CalibDataID);
            //Specify the used calibration plane using the name of the
            //description file

            hv_CalPlateDescr = "calplate_" + txtCalibtate.Text + "mm.cpd";
            HOperatorSet.SetCalibDataCalibObject(hv_CalibDataID, 0, hv_CalPlateDescr);

            double sx = System.Convert.ToDouble(txtSx.Text);
            double sy = System.Convert.ToDouble(txtSy.Text);

            hv_StartParam = new HTuple();
            hv_StartParam[0] = System.Convert.ToDouble(txtFocus.Text);
            hv_StartParam[1] = System.Convert.ToDouble(txtKappa.Text);
            hv_StartParam[2] = sx * 1e-006;
            hv_StartParam[3] = sy * 1e-006;
            hv_StartParam[4] = System.Convert.ToDouble(txtCx.Text);
            hv_StartParam[5] = System.Convert.ToDouble(txtCy.Text);
            hv_StartParam[6] = System.Convert.ToDouble(txtIx.Text);
            hv_StartParam[7] = System.Convert.ToDouble(txtIy.Text);
            HOperatorSet.SetCalibDataCamParam(hv_CalibDataID, 0, "area_scan_division", hv_StartParam);
            try
            {
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ImageFiles.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    try
                    {
                        ho_Image.Dispose();
                        HOperatorSet.ReadImage(out ho_Image, hv_ImageFiles.TupleSelect(hv_Index));

                        HOperatorSet.FindCalibObject(ho_Image, hv_CalibDataID, 0, 0, hv_Index, new HTuple(),
                            new HTuple());

                        HOperatorSet.GetCalibDataObservPose(hv_CalibDataID, 0, 0, hv_Index, out hv_StartPose);

                        ho_MarkContours.Dispose();
                        HOperatorSet.GetCalibDataObservContours(out ho_MarkContours, hv_CalibDataID,
                            "marks", 0, 0, hv_Index);
                        //图像标定成功
                        txtLog.Text = txtLog.Text + "图像" + hv_Index.ToString() + "处理成功" + "\r\n" + DateTime.Now + "\r\n";
                    }
                    catch
                    {
                        //图像标定失败
                        txtLog.Text = txtLog.Text + "图像" + hv_Index.ToString() + "处理失败" + "\r\n" + DateTime.Now + "\r\n";
                    }


                }

                HOperatorSet.CalibrateCameras(hv_CalibDataID, out hv_Error);
                HOperatorSet.GetCalibData(hv_CalibDataID, "camera", 0, "params", out hv_CameraParametersCalibrated);
                HOperatorSet.GetCalibData(hv_CalibDataID, "camera", 0, "params_labels", out hv_ParLabels);

                HOperatorSet.ClearCalibData(hv_CalibDataID);
                ho_Image.Dispose();
                ho_MarkContours.Dispose();
                //txtLog.Clear();
                txtLog.Text = txtLog.Text + "相机内参" + hv_CameraParametersCalibrated.ToString() + "\r\n" + DateTime.Now + "\r\n";

            }
            catch
            {
                txtLog.Text = txtLog.Text + "图像质量出现问题，标定失败" + "\r\n" + DateTime.Now + "\r\n";
            }
            
        }

        private void startcalibrate_Click(object sender, EventArgs e)
        {
            if (txtFiles.Text=="")
            {
                txtLog.Text = txtLog.Text + "未选定图像路径" + "\r\n" + DateTime.Now + "\r\n";
            }
            else
            {
                action();
            }           
        }

    }
}
