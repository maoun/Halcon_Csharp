<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoticePicManager.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.NoticePicManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="Css/cabel-v1.css" type="text/css" />
    <link rel="stylesheet" href="Css/showphotostyle.css" type="text/css" />
    <script src="Scripts/common.js" type="text/javascript" charset="utf-8"></script>
    <script src="Scripts/showphoto.js" type="text/javascript" charset="utf-8"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js"></script>
    <!-- 图片引用js-->
    <script type="text/javascript" src="../Scripts/Zoom/jquery.photo.gallery.js"></script>
    <!-- 图片开始js-->
    <script type="text/javascript">
        function ShowImage(image1) {
            if (image1 == "") {
                document.getElementById("zjwj1").src = "../Images/NoImage.png";
            }
            else {
                document.getElementById("zjwj1").src = image1;
            }

        }
    </script>

    <title>公告图片管理</title>

    <style type="text/css">
        .x-form-file-wrap {
            position: relative;
            height: 33px;
        }

        .images-view .x-panel-body {
            background: white;
            font: 11px Arial, Helvetica, sans-serif;
        }

        .images-view .thumb {
            background: #dddddd;
            padding: 3px;
        }

            .images-view .thumb img {
                height: 60px;
                width: 80px;
            }

        .images-view .thumb-wrap {
            float: left;
            margin: 4px;
            margin-right: 0;
            padding: 5px;
            text-align: center;
        }

            .images-view .thumb-wrap span {
                display: block;
                overflow: hidden;
                text-align: center;
            }

        .images-view .x-view-over {
            border: 1px solid #dddddd;
            background: #efefef url(images/row-over.gif) repeat-x left top;
            padding: 4px;
        }

        .images-view .x-view-selected {
            background: #eff5fb url(images/selected.gif) no-repeat right bottom;
            border: 1px solid #99bbe8;
            padding: 4px;
        }

            .images-view .x-view-selected .thumb {
                background: transparent;
            }

        .images-view .loading-indicator {
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
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="NoticePicManager" />

        <ext:Store runat="server" ID="StoreLrr">
            <Reader>
                <ext:JsonReader runat="server">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>

        <ext:Hidden ID="HidType" runat="server"></ext:Hidden>
        <ext:Hidden ID="HidID" runat="server"></ext:Hidden>
        <ext:Hidden ID="img1" runat="server"></ext:Hidden>
        <ext:Hidden ID="CurrentX" runat="server" />
        <ext:Hidden ID="CurrentY" runat="server" />
        <ext:Viewport runat="server" Layout="BorderLayout">
            <Items>
                <ext:Panel runat="server" ID="Panel123" Layout="BorderLayout" Region="Center">
                    <Items>
                        <ext:FormPanel ID="Panel1" Region="North" runat="server">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Layout="Container">
                                    <Items>
                                        <ext:Toolbar ID="Toolbar3" runat="server">
                                            <Items>
                                                <ext:Label ID="Label3" runat="server" Style="color: #000080; margin-left: 10px;" Text='<%# GetLangStr("NoticePicManager1","公告名称：") %>'>
                                                </ext:Label>
                                                <ext:TextField ID="TxtPicName" runat="server" Width="150" />
                                                <ext:Label ID="Label1" runat="server" Style="color: #000080; margin-left: 10px;" Text='<%# GetLangStr("NoticePicManager2","录入人：") %>'>
                                                </ext:Label>

                                                <ext:ComboBox ID="CmbLrr" runat="server" Editable="false" StoreID="StoreLrr"
                                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                                    EmptyText='<%# GetLangStr("NoticePicManager3","选择录入人...") %>' SelectOnFocus="true" Width="150">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("NoticePicManager4","清除选中") %>' AutoDataBind="true" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                </ext:ComboBox>

                                                <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("NoticePicManager5","查询") %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutQueryClick" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("NoticePicManager6","重置") %>'>
                                                    <DirectEvents>
                                                        <Click OnEvent="ButResetClick" />
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:FormPanel>

                        <ext:FormPanel ID="FormPanel2" Region="Center" runat="server" Layout="FitLayout">
                            <Items>
                                <ext:GridPanel ID="GridDeviceManager" runat="server" StripeRows="true" Header="false" ForceFit="true">
                                    <TopBar>
                                        <ext:PagingToolbar runat="server" HideRefresh="true">
                                            <Items>
                                                <ext:Button ID="ButDevAdd" runat="server" Text='<%# GetLangStr("NoticePicManager7","添加信息") %>' Icon="DriveAdd">
                                                    <DirectEvents>
                                                        <Click OnEvent="AddClick" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                            </Items>
                                        </ext:PagingToolbar>
                                    </TopBar>
                                    <Store>
                                        <ext:Store ID="StorePicManager" runat="server" OnRefreshData="MyData_Refresh" PageSize="10">
                                            <AutoLoadParams>
                                                <ext:Parameter Name="start" Value="={0}" />
                                                <ext:Parameter Name="limit" Value="={15}" />
                                            </AutoLoadParams>
                                            <Reader>
                                                <ext:JsonReader runat="server">
                                                    <Fields>
                                                        <ext:RecordField Name="col0" Type="String" />
                                                        <ext:RecordField Name="col1" Type="String" />
                                                        <ext:RecordField Name="col2" Type="String" />
                                                        <ext:RecordField Name="col3" Type="String" />
                                                        <ext:RecordField Name="col4" Type="String" />
                                                        <ext:RecordField Name="col5" Type="string" />
                                                        <ext:RecordField Name="col6" Type="Date" />
                                                        <ext:RecordField Name="col7" Type="String" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <ext:ImageCommandColumn runat="server" Width="35">
                                                <Commands>
                                                    <ext:ImageCommand Icon="NoteEdit" CommandName="Edit">

                                                        <ToolTip Text='<%# GetLangStr("NoticePicManager8","编辑") %>' AutoDataBind="true" />
                                                    </ext:ImageCommand>
                                                    <ext:ImageCommand Icon="NoteDelete" CommandName="Delete">
                                                        <ToolTip Text='<%# GetLangStr("NoticePicManager9","删除") %>' AutoDataBind="true" />
                                                    </ext:ImageCommand>
                                                </Commands>
                                            </ext:ImageCommandColumn>
                                            <ext:Column runat="server" Header='<%# GetLangStr("NoticePicManager10","公告名称") %>' AutoDataBind="true" DataIndex="col1" Sortable="true" Align="left" />
                                            <ext:Column runat="server" Header='<%# GetLangStr("NoticePicManager11","公告简称") %>' AutoDataBind="true" DataIndex="col2" Sortable="true" Align="Center" />
                                            <ext:Column runat="server" Header='<%# GetLangStr("NoticePicManager12","详细描述") %>' AutoDataBind="true" DataIndex="col3" Sortable="true" Align="left" />
                                            <ext:Column runat="server" Header='<%# GetLangStr("NoticePicManager13","违法行为") %>' AutoDataBind="true" DataIndex="col4" Sortable="true" Align="Center" />
                                            <ext:Column runat="server" Header='<%# GetLangStr("NoticePicManager14","录入人") %>' AutoDataBind="true" DataIndex="col5" Sortable="true" Align="Center" />
                                            <ext:DateColumn runat="server" Header='<%# GetLangStr("NoticePicManager15","录入时间") %>' AutoDataBind="true" DataIndex="col6" Format="yyyy-MM-dd HH:mm:ss" Sortable="true" Align="Center" />
                                        </Columns>
                                    </ColumnModel>
                                    <Listeners>
                                        <Command Handler="OnEvl.Exque(command,record.data.col0,record.data.col1,record.data.col2,record.data.col3,record.data.col4,record.data.col7)"></Command>
                                    </Listeners>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                            <Listeners>
                                                <RowSelect Handler="ShowImage(record.data.col7)" Buffer="250" />
                                            </Listeners>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <View>
                                        <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                        </ext:GridView>
                                    </View>
                                </ext:GridPanel>
                            </Items>
                        </ext:FormPanel>

                        <ext:Panel ID="ImageDetailPanel" runat="server" Region="East" Split="true" Frame="true" Title='<%# GetLangStr("NoticePicManager16","详细信息") %>' Width="300" Icon="Images" DefaultAnchor="100%">
                            <Content>
                                <div>
                                    <center>
                                        <div class="thumb">
                                            <img id="zjwj1" style="cursor: pointer" class="photo"
                                                src="../images/NoImage.png" alt='<%# GetLangStr("NoticePicManager17","车辆图片(图片点击滚轮缩放)") %>' onclick="$.openPhotoGalleryImg(this);" width="280" />
                                        </div>
                                    </center>
                                </div>
                            </Content>
                        </ext:Panel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>

        <ext:Window ID="Window1" runat="server" Icon="House" Hidden="true" Height="280px" Width="400px" Title='<%# GetLangStr("NoticePicManager16","详细信息") %>'>
            <Items>
                <ext:FormPanel runat="server" ID="FormPanel3" Height="230px" BodyPadding="10" ButtonAlign="Center" MonitorValid="true">
                    <Items>
                        <ext:Panel runat="server" BodyPadding="5" Height="250" Border="false">
                            <Items>
                                <ext:TextField ID="txtNewPicName" runat="server" FieldLabel='<%# GetLangStr("NoticePicManager10","公告名称") %>' Width="380px" AnchorHorizontal="95%" AllowBlank="false" />
                                <ext:TextField ID="txtNewName" runat="server" FieldLabel='<%# GetLangStr("NoticePicManager11","公告简称") %>' Width="380px" AnchorHorizontal="95%" AllowBlank="false" />
                                <ext:TextField ID="txtNewWfxw" runat="server" FieldLabel='<%# GetLangStr("NoticePicManager13","违法行为") %>' Width="380px" MaxLength="20" AnchorHorizontal="95%" AllowBlank="false" />
                                <ext:TextField ID="txtNewPicDisc" runat="server" FieldLabel='<%# GetLangStr("NoticePicManager17","描述") %>' Width="380px" AnchorHorizontal="95%" AllowBlank="false" />
                                <ext:FileUploadField ID="FileUploadField1" runat="server" AnchorHorizontal="auto" Style="margin-top: 3px; height: 24px; width: 180px;" Icon="ImageAdd" FieldLabel='<%# GetLangStr("NoticePicManager18","文件") %>' ButtonText='<%# GetLangStr("NoticePicManager19","浏览") %>' AllowBlank="false" />
                            </Items>
                        </ext:Panel>
                    </Items>
                    <Buttons>
                        <ext:Button ID="ButUpdate" runat="server" Icon="Add" FormBind="true">
                            <DirectEvents>
                                <Click OnEvent="Add_Fac" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="BtnConcel" runat="server" Icon="TextItalic" Text='<%# GetLangStr("NoticePicManager20","取消") %>'>
                            <Listeners>
                                <Click Handler="#{Window1}.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ClientValidation Handler="#{ButUpdate}.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>