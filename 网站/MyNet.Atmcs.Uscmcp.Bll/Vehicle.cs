using System;
using System.Collections;
using System.Xml;
using MyNet.Common.Log;
using XmlReadWrite;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    /// <summary>
    /// 车驾管读取
    /// </summary>
    public class Vehicle
    {
        private OtherQueryService.OtherQueryInfo service = new OtherQueryService.OtherQueryInfo();
        private XmlRead xmlRead = new XmlRead();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url"></param>
        public Vehicle(string url)
        {
            service.Url = url;
        }

        /// <summary>
        /// 读取车驾管信息
        /// </summary>
        /// <param name="hpzl"></param>
        /// <param name="hphm"></param>
        /// <returns></returns>
        public VehicleInfo GetVehicleInfo(string hpzl, string hphm)
        {
            VehicleInfo vehinfo = new VehicleInfo();
            XmlDocument doc = new XmlDocument();
            string xmlbody = "<?xml version='1.0' encoding='GBK'?>";
            xmlbody = xmlbody + "<root>";
            xmlbody = xmlbody + "<QueryCondition>";
            xmlbody = xmlbody + "<hpzl>" + hpzl + "</hpzl>";
            xmlbody = xmlbody + "<hphm>" + hphm + "</hphm>";
            xmlbody = xmlbody + "</QueryCondition>";
            xmlbody = xmlbody + "</root>";
            string xmlReturn = "";
            try
            {
                 xmlReturn = service.QueryObjectOut("01C21", xmlbody);
                //xmlReturn = GetTestXml();
            }
            catch (Exception)
            {
                ILog.WriteErrorLog(xmlReturn);
                return null;
            }
            doc = new XmlDocument();
            doc.LoadXml(xmlReturn);
            if (string.IsNullOrEmpty(GetSingleValueByXPath(doc, "root/body")))
            {
                ILog.WriteErrorLog(xmlReturn);
                return null;
            }
            if (!string.IsNullOrEmpty(xmlReturn))
            {
                Hashtable ht = xmlRead.XmltoHashtable(xmlReturn, "veh");
                if (ht.Count > 2)
                {
                    try
                    {
                        vehinfo.Hphm = hphm;
                        vehinfo.Hpzl = hpzl;
                        vehinfo.Clpp1 = ht["clpp1"].ToString();
                        vehinfo.Clxh = ht["clxh"].ToString();
                        vehinfo.Clsbdh = ht["clsbdh"].ToString();
                        vehinfo.Fdjh = ht["fdjh"].ToString();
                        try
                        {
                            vehinfo.Csys = GetCsys(ht["csys"].ToString());
                        }
                        catch
                        {
                            vehinfo.Csys = ht["csys"].ToString();
                        }
                        vehinfo.Csysbh = ht["csys"].ToString();
                        vehinfo.Syr = ht["syr"].ToString();
                        vehinfo.Ccdjrq = ht["ccdjrq"].ToString();
                        string qzbfqz = ht["qzbfqz"].ToString();
                        if (!string.IsNullOrEmpty(qzbfqz))
                        {
                            vehinfo.Qzbfqz = qzbfqz.Substring(0, qzbfqz.Length - 2);
                        }
                        string yxqz = ht["yxqz"].ToString();
                        if (!string.IsNullOrEmpty(yxqz))
                        {
                            vehinfo.Yxqz = yxqz.Substring(0, yxqz.Length - 2);
                            vehinfo.Jyyxqz = yxqz.Substring(0, yxqz.Length - 2);
                        }
                        vehinfo.Fzjg = ht["fzjg"].ToString();
                        vehinfo.Yzbm1 = ht["yzbm1"].ToString();
                        vehinfo.Lxdh = ht["sjhm"].ToString();
                        vehinfo.Fzjg = ht["fzjg"].ToString();
                        vehinfo.Zsxxdz = ht["zzxxdz"].ToString();
                        vehinfo.Xzqh = ht["xzqh"].ToString();
                        try
                        {
                            vehinfo.Cllx = GetCllx(ht["cllx"].ToString());
                        }
                        catch
                        {
                            vehinfo.Cllx = ht["cllx"].ToString();
                        }

                        vehinfo.Cllxbh = ht["cllx"].ToString();

                        // 方正修改结束
                        try
                        {
                            vehinfo.Zt = GetZt(ht["zt"].ToString());
                        }
                        catch
                        {
                            vehinfo.Zt = ht["zt"].ToString();
                        }
                        vehinfo.Ztbh = ht["zt"].ToString();
                        vehinfo.Sfzmhm = ht["sfzmhm"].ToString();

                        vehinfo.Dabh = ht["dabh"].ToString();
                        if (!string.IsNullOrEmpty(ht["syxz"].ToString()))
                        {
                            vehinfo.Syxz = ht["syxz"].ToString();
                            vehinfo.Syxzms = GetSyxz(ht["syxz"].ToString());
                        }
                        return vehinfo;
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex.Message);
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 转换车身颜色
        /// </summary>
        /// <param name="cpys"></param>
        /// <returns></returns>
        public string GetCsys(string cpys)
        {
            switch (cpys)
            {
                case "A":
                    return "白色";

                case "B":
                    return "灰色";

                case "C":
                    return "黄色";

                case "D":
                    return "粉色";

                case "E":
                    return "红色";

                case "F":
                    return "紫色";

                case "G":
                    return "绿色";

                case "H":
                    return "蓝色";

                case "I":
                    return "棕色";

                case "J":
                    return "黑色";

                default:
                    return "其它";
            }
        }

        /// <summary>
        ///转换车辆类型
        /// </summary>
        /// <param name="cllx"></param>
        /// <returns></returns>
        public string GetCllx(string cllx)
        {
            switch (cllx)
            {
                case "K11": return "大型普通客车";
                case "K12": return "大型双层客车";
                case "K13": return "大型卧铺客车";
                case "K14": return "大型铰接客车";
                case "K15": return "大型越野客车";
                case "K21": return "中型普通客车";
                case "K22": return "中型双层客车";
                case "K23": return "中型卧铺客车";
                case "K24": return "中型铰接客车";
                case "K25": return "中型越野客车";
                case "K31": return "小型普通客车";
                case "K32": return "小型越野客车";
                case "K33": return "轿车";
                case "K41": return "微型普通客车";
                case "K42": return "微型越野客车";
                case "K43": return "微型轿车";
                case "H11": return "重型普通货车";
                case "H12": return "重型厢式货车";
                case "H13": return "重型封闭货车";
                case "H14": return "重型罐式货车";
                case "H15": return "重型平板货车";
                case "H16": return "重型集装厢车";
                case "H17": return "重型自卸货车";
                case "H18": return "重型特殊结构货车";
                case "H21": return "中型普通货车";
                case "H22": return "中型厢式货车";
                case "H23": return "中型封闭货车";
                case "H24": return "中型罐式货车";
                case "H25": return "中型平板货车";
                case "H26": return "中型集装厢车";
                case "H27": return "中型自卸货车";
                case "H28": return "中型特殊结构货车";
                case "H31": return "轻型普通货车";
                case "H32": return "轻型厢式货车";
                case "H33": return "轻型封闭货车";
                case "H34": return "轻型罐式货车";
                case "H35": return "轻型平板货车";
                case "H37": return "轻型自卸货车";
                case "H38": return "轻型特殊结构货车";
                case "H41": return "微型普通货车";
                case "H42": return "微型厢式货车";
                case "H43": return "微型封闭货车";
                case "H44": return "微型罐式货车";
                case "H45": return "微型自卸货车";
                case "H46": return "微型特殊结构货车";
                case "H51": return "低速普通货车";
                case "H52": return "低速厢式货车";
                case "H53": return "低速罐式货车";
                case "H54": return "低速自卸货车";
                case "Q11": return "重型半挂牵引车";
                case "Q21": return "中型半挂牵引车";
                case "Q31": return "轻型半挂牵引车";
                case "Z": return "专项作业车";
                case "Z11": return "大型专项作业车";
                case "Z21": return "中型专项作业车";
                case "Z31": return "小型专项作业车";
                case "Z41": return "微型专项作业车";
                case "Z51": return "重型专项作业车";
                case "Z71": return "轻型专项作业车";
                case "D11": return "无轨电车";
                case "D12": return "有轨电车";
                case "M11": return "普通正三轮摩托车";
                case "M12": return "轻便正三轮摩托车";
                case "M13": return "正三轮载客摩托车";
                case "M14": return "正三轮载货摩托车";
                case "M15": return "侧三轮摩托车";
                case "M21": return "普通二轮摩托车";
                case "M22": return "轻便二轮摩托车";
                case "N11": return "三轮汽车";
                case "T11": return "大型轮式拖拉机";
                case "T21": return "小型轮式拖拉机";
                case "T22": return "手扶拖拉机";
                case "T23": return "手扶变形运输机";
                case "J11": return "轮式装载机械";
                case "J12": return "轮式挖掘机械";
                case "J13": return "轮式平地机械";
                case "G11": return "重型普通全挂车";
                case "G12": return "重型厢式全挂车";
                case "G13": return "重型罐式全挂车";
                case "G14": return "重型平板全挂车";
                case "G15": return "重型集装箱全挂车";
                case "G16": return "重型自卸全挂车";
                case "G21": return "中型普通全挂车";
                case "G22": return "中型厢式全挂车";
                case "G23": return "中型罐式全挂车";
                case "G24": return "中型平板全挂车";
                case "G25": return "中型集装箱全挂车";
                case "G26": return "中型自卸全挂车";
                case "G31": return "轻型普通全挂车";
                case "G32": return "轻型厢式全挂车";
                case "G33": return "轻型罐式全挂车";
                case "G34": return "轻型平板全挂车";
                case "G35": return "轻型自卸全挂车";
                case "B11": return "重型普通半挂车";
                case "B12": return "重型厢式半挂车";
                case "B13": return "重型罐式半挂车";
                case "B14": return "重型平板半挂车";
                case "B15": return "重型集装箱半挂车";
                case "B16": return "重型自卸半挂车";
                case "B17": return "重型特殊结构半挂车";
                case "B21": return "中型普通半挂车";
                case "B22": return "中型厢式半挂车";
                case "B23": return "中型罐式半挂车";
                case "B24": return "中型平板半挂车";
                case "B25": return "中型集装箱半挂车";
                case "B26": return "中型自卸半挂车";
                case "B27": return "中型特殊结构半挂车";
                case "B31": return "轻型普通半挂车";
                case "B32": return "轻型厢式半挂车";
                case "B33": return "轻型罐式半挂车";
                case "B34": return "轻型平板半挂车";
                case "B35": return "轻型自卸半挂车";
                case "X99": return "其它";
                default: return "其它";
            }
        }

        /// <summary>
        ///转换车辆状态
        /// </summary>
        /// <param name="zt"></param>
        /// <returns></returns>
        public string GetZt(string zt)
        {
            switch (zt)
            {
                case "A": return "正常";
                case "B": return "转出";
                case "C": return "被盗抢";
                case "D": return "停驶";
                case "E": return "注销";
                case "G": return "违法未处理";
                case "H": return "海关监管";
                case "I": return "事故未处理";
                case "J": return "嫌疑车";
                case "K": return "查封";
                case "L": return "暂扣";
                case "M": return "强制注销";
                case "N": return "事故逃逸";
                case "O": return "锁定";
                case "Z": return "其他";
                default: return "其它";
            }
        }

        //周亮添加 使用性质
        /// <summary>
        /// 转换使用性质
        /// </summary>
        /// <param name="syxz"></param>
        /// <returns></returns>
        public string GetSyxz(string syxz)
        {
            switch (syxz)
            {
                case "A":
                    return "非营运";

                case "B":
                    return "公路客运";

                case "C":
                    return "公交客运";

                case "D":
                    return "出租客运";

                case "E":
                    return "旅游客运";

                case "F":
                    return "货运";

                case "G":
                    return "租赁";

                case "H":
                    return "警用";

                case "I":
                    return "消防";

                case "J":
                    return "救护";

                case "K":
                    return "工程救险";

                case "L":
                    return "营转非";

                case "M":
                    return "出租转非";

                case "N":
                    return "教练";

                case "O":
                    return "幼儿校车";

                case "P":
                    return "小学生校车";

                case "Q":
                    return "其他校车";

                case "R":
                    return "危化品运输";

                case "Z":
                    return "其他";

                default:
                    return "其它";
            }
        }

        /// <summary>
        /// 读取节点值
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public string GetSingleValueByXPath(XmlDocument xmlDocument, string XPath)
        {
            XmlNode node;
            try
            {
                node = xmlDocument.SelectSingleNode(XPath);
                if (node != null)
                {
                    return node.InnerText;
                }
            }
            catch
            {
            }
            return "";
        }

        private string GetTestXml()
        {
            return @"<?xml version='1.0' encoding='GBK'?>
<root>
<head>
<code>1</code>
<message>数据下载成功！</message>
<rownum>1</rownum>
</head>
<body>
<veh id='0'>
  <clsbdh>LVSHDFMC57F008832</clsbdh>
  <jyw>3279640D0E04F2DCE5E080D7F6F37579760604080B7E2624133C2934246D5659351E000D0F07020104060408027E050B7176040809045A4670405F42581F535A3330413035</jyw>
  <zsxzqh>510106</zsxzqh>
  <zxxs>1</zxxs>
  <xsrq></xsrq>
  <zzxzqh>510106</zzxzqh>
  <bzcs>0</bzcs>
  <zqyzl>0</zqyzl>
  <clpp2></clpp2>
  <clpp1>马自达牌</clpp1>
  <hlj>1515</hlj>
  <sfzmmc>A</sfzmmc>
  <zj>2640</zj>
  <ytsx>9</ytsx>
  <zt>G</zt>
  <zs>2</zs>
  <csys>B</csys>
  <fhgzrq>2009-04-27 16:01:28.0</fhgzrq>
  <yxh></yxh>
  <qpzk>0</qpzk>
  <hdfs>A</hdfs>
  <xsjg></xsjg>
  <clxh>CAF7202M</clxh>
  <gcjk>A</gcjk>
  <dybj>0</dybj>
  <gbthps>0</gbthps>
  <lxdh></lxdh>
  <jbr>蒋邦梅</jbr>
  <ccdjrq>2007-04-04 11:54:46.0</ccdjrq>
  <djrq>2009-04-27 00:00:00.0</djrq>
  <gxrq>2010-12-15 21:10:54.0</gxrq>
  <xzqh>510106</xzqh>
  <lsh>1A70404050960</lsh>
  <fdjxh>LF</fdjxh>
  <clyt>P1</clyt>
  <dzyx></dzyx>
  <fprq>2007-04-04 11:54:46.0</fprq>
  <jkpzhm></jkpzhm>
  <lts>4</lts>
  <nszmbh>6510753723</nszmbh>
  <bz></bz>
  <qlj>1530</qlj>
  <hxnbgd>0</hxnbgd>
  <yxqz>2011-04-30 00:00:00.0</yxqz>
  <fdjrq>2007-04-04 12:22:11.0</fdjrq>
  <ccrq>2007-03-19 00:00:00.0</ccrq>
  <zzcmc>长安福特马自达汽车有限公司(公告C4)</zzcmc>
  <xsdw></xsdw>
  <hgzbh>WDV100007035722</hgzbh>
  <zsxxdz>成都市金牛区西安南路６５号２栋１单元５楼１０号</zsxxdz>
  <bpcs>0</bpcs>
  <cwkc>4539</cwkc>
  <hdzk>5</hdzk>
  <hpzl>02</hpzl>
  <hpzk>0</hpzk>
  <pzbh1>00874003</pzbh1>
  <jkpz></jkpz>
  <djzsbh>510003867598</djzsbh>
  <pzbh2></pzbh2>
  <fdjh>062001</fdjh>
  <sqdm>510106000000</sqdm>
  <dabh></dabh>
  <hphm>AWT890</hphm>
  <syr>胡咏梅</syr>
  <cwkk>1755</cwkk>
  <dwbh>51010000006118</dwbh>
  <ltgg>205/55 R16</ltgg>
  <zbzl>1287</zbzl>
  <xszbh></xszbh>
  <zzxxdz>成都市金牛区西安南路６５号２栋１单元５楼１０号</zzxxdz>
  <cwkg>1465</cwkg>
  <hbdbqk></hbdbqk>
  <sfzmhm>511102197902257720</sfzmhm>
  <syq>2</syq>
  <dphgzbh></dphgzbh>
  <pl>1999</pl>
  <syxz>A</syxz>
  <gl>110</gl>
  <qmbh></qmbh>
  <nszm>1</nszm>
  <hdzzl>0</hdzzl>
  <sjhm>13348846398</sjhm>
  <fzrq>2007-04-04 12:21:57.0</fzrq>
  <hmbh></hmbh>
  <hxnbkd>0</hxnbkd>
  <bxzzrq>2010-04-04 00:00:00.0</bxzzrq>
  <zdyzt></zdyzt>
  <llpz1>A</llpz1>
  <fzjg>鲁M</fzjg>
  <zdjzshs></zdjzshs>
  <zzg>156</zzg>
  <glbm>510100000400</glbm>
  <qzbfqz>2099-12-31 00:00:00.0</qzbfqz>
  <bdjcs>0</bdjcs>
  <llpz2></llpz2>
  <zzl>1736</zzl>
  <hxnbcd>0</hxnbcd>
  <cllx>K33</cllx>
  <clly>1</clly>
  <rlzl>A</rlzl>
  <zzz></zzz>
  <xgzl>ABMJ</xgzl>
  <yzbm1>610000</yzbm1>
  <yzbm2></yzbm2>
  <jyhgbzbh>115101122795</jyhgbzbh>
  <xh>51010007086048</xh>
  <cyry></cyry>
</veh>
</body></root>";
        }
    }
}