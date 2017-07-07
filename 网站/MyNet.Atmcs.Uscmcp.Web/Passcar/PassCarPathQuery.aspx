<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassCarPathQuery.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.PassCarPathQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>车辆轨迹查询</title>
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        function ChangeBackColor(id,hpzl) {

            var obj = document.getElementById(id);
            switch (hpzl) {

                case "01":
                    obj.style.color = "#000000";
                    obj.style.background = "FFFF00";
                    break;
                case "02":
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000080";
                    break;
                case "06":
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000000";
                    break;
                case "23":
                    obj.style.color = "FF0000";
                    obj.style.background = "FFFFFF";
                    break;
                default:
                    obj.style.color = "#FFFFFF";
                    obj.style.background = "000080";
                    break;
            
            }
        }
    </script>
    <style type="text/css">
        .images-view .x-panel-body
        {
            background: white;
            font: 11px Arial, Helvetica, sans-serif;
        }
        .images-view .thumb
        {
            background: #dddddd;
            padding: 3px;
        }
        .images-view .thumb img
        {
            height: 400px;
            width: 600px;
        }
        .images-view .thumb-wrap
        {
            float: left;
            margin: 4px;
            margin-right: 0;
            padding: 5px;
            text-align: center;
        }
        .images-view .thumb-wrap span
        {
            display: block;
            overflow: hidden;
            text-align: center;
        }
        
        .images-view .x-view-over
        {
            border: 1px solid #dddddd;
            background: #efefef url(images/row-over.gif) repeat-x left top;
            padding: 4px;
        }
        
        .images-view .x-view-selected
        {
            background: #eff5fb url(images/selected.gif) no-repeat right bottom;
            border: 1px solid #99bbe8;
            padding: 4px;
        }
        .images-view .x-view-selected .thumb
        {
            background: transparent;
        }
        
        .images-view .loading-indicator
        {
            font-size: 11px;
            background-image: url(images/loading.gif);
            background-repeat: no-repeat;
            background-position: left;
            padding-left: 20px;
            margin: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PassCarPathQuery" />
    <ext:Hidden ID="GridData" runat="server" />
    <ext:Store ID="StoreImage" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="col0" />
                    <ext:RecordField Name="col1" />
                    <ext:RecordField Name="col2" />
                    <ext:RecordField Name="col3" />
                    <ext:RecordField Name="col4" />
                    <ext:RecordField Name="col5" />
                    <ext:RecordField Name="col6" Type="String" />
                    <ext:RecordField Name="col7" />
                    <ext:RecordField Name="col8" />
                    <ext:RecordField Name="col9" />
                    <ext:RecordField Name="col10" />
                    <ext:RecordField Name="col11" />
                    <ext:RecordField Name="col12" />
                    <ext:RecordField Name="col13" />
                    <ext:RecordField Name="col14" />
                    <ext:RecordField Name="col15" />
                    <ext:RecordField Name="col16" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="Viewport2" runat="server" Layout="border">
        <Items>
            <ext:FormPanel ID="Panel1" Region="North" runat="server" Title="查询条件" Collapsible="true"
                Height="40">
                <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Label ID="lblStartTime" runat="server" Html="<font color='white'>&nbsp;&nbsp;开始时间：</font>">
                                    </ext:Label>
                                    <ext:DateField ID="DateStartTime" runat="server" Vtype="daterange">
                                        <Listeners>
                                            <Render Handler="this.endDateField = '#{DateEndTime}'" />
                                        </Listeners>
                                    </ext:DateField>
                                    <ext:TimeField ID="TimeStart" runat="server" Increment="1" Width="61" />
                                    <ext:Label ID="lblEndTime" runat="server" Html="<font color='white'>&nbsp;&nbsp;结束时间：</font>">
                                    </ext:Label>
                                    <ext:DateField ID="DateEndTime" runat="server" Vtype="daterange">
                                        <Listeners>
                                            <Render Handler="this.startDateField = '#{DateStartTime}'" />
                                        </Listeners>
                                    </ext:DateField>
                                    <ext:TimeField ID="TimeEnd" runat="server" Increment="1" Width="61" />
                                     <ext:Label ID="Label1" runat="server" Html="<font color='white'>&nbsp;&nbsp;号牌种类：</font>">
                                    </ext:Label>
                                    <ext:ComboBox ID="CmbPlateType" runat="server" Editable="false" DisplayField="col1"
                                        ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText="请选择..."
                                        SelectOnFocus="true" Width="123">
                                            <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                        <Store>
                                            <ext:Store ID="StorePlateType" runat="server">
                                                <Reader>
                                                    <ext:JsonReader>
                                                        <Fields>
                                                            <ext:RecordField Name="col0" Type="String" />
                                                            <ext:RecordField Name="col1" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:Label ID="Label3" runat="server" Html="<font color='white'>&nbsp;&nbsp;车牌号牌：</font>">
                                    </ext:Label>
                                    <ext:TextField ID="TxtplateId" runat="server" Width="100" AllowBlank="false"  EmptyText="六位号牌号码" />
                                    <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text="查询">
                                        <DirectEvents>
                                               <Click OnEvent="TbutQueryClick"  Timeout="60000" >
                                                    <EventMask  ShowMask="true"/>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                      <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text="重置">
                                        <DirectEvents>
                                            <Click OnEvent="ButResetClick" />
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>

                </TopBar>
            </ext:FormPanel>
            <ext:Panel ID="Panel2" runat="server" Region="Center" DefaultBorder="false" Cls="images-view"
                Frame="true" Collapsible="false" Title="图片列表" AutoScroll="true">
                <Items>
                    <ext:DataView ID="ImageView" runat="server" StoreID="StoreImage" AutoHeight="true"
                        MultiSelect="true" OverClass="x-view-over" ItemSelector="div.thumb-wrap" EmptyText="没有查询到符合条件的记录！">
                        <Template ID="Template1" runat="server">
                            <Html>
                                <tpl for=".">
                                   <div class="thumb-wrap">
                                     <table border="0" cellpadding="0" cellspacing="0" style="width: 840px; height: 316px" >
                                            <tr>
                                               <td colspan="2" style="height: 15px; vertical-align: middle; text-align: center;"><strong><span style="font-size: 16pt; font-family: 微软雅黑"><font color="red" bold="true" >({col0})</font>  {col2}【{col6}】</span></strong></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 220px"></td>
                                                <td rowspan="7" ><div class="thumb"><img src="{col14}" title="{col2}" alt="车辆图片(双击图片进行放大)" onDblClick="OpenPicPage(this.src);" onload ="ChangeBackColor('{col0}','{col4}');"></div></td>
                                            </tr>
                                            <tr><td style="width: 220px; vertical-align: middle; text-align: left;"><strong><div id="{col0}" style="width:100px ;font-size: 14pt; font-family: 微软雅黑; color: white; background-color: blue;" >{col3}</div></strong></td></tr>
                                            <tr><td style="width: 220px; vertical-align: middle; text-align: left;">号牌种类：{col5}</td></tr>
                                            <tr><td style="width: 220px; vertical-align: middle; text-align: left;">过往时间：{col6}</td></tr>
                                            <tr><td style="width: 220px; vertical-align: middle; text-align: left;">行驶方向：{col9}</td></tr>
                                            <tr><td style="width: 220px; vertical-align: middle; text-align: left;">车道：{col10} ，速度：{col11} 千米/时</td></tr>
                                            <tr><td style="width: 220px; vertical-align: middle; text-align: left;">记录类型：{col13}</td></tr>
                                    </table>
                                    <hr color="blue" />
                                         </div>
							     </tpl>
                                <div class="x-clear"></div>
                            </Html>
                        </Template>
                    </ext:DataView>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
