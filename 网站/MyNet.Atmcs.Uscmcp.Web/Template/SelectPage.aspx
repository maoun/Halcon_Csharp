<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectPage.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.SelectPage" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link href="../Styles/chooser.css" rel="stylesheet" />
    <script type="text/javascript">
        var lookup = {}
        var showDetails = function () {
            var selNode = ImageView.getSelectedNodes();
            var detailEl = ImageDetailPanel.body;
            if (selNode && selNode.length > 0) {
                selNode = selNode[0];
                var data = this.lookup[selNode.id];
                detailEl.hide();
                SelectPage.FeedBackUrl(data.pageid, data.pageurl);
                this.DetailsTemplate.overwrite(detailEl, data);
                detailEl.slideIn('l', { stopFx: true, duration: .2 });
            } else {
                detailEl.update('');
            }
        }
        var formatData = function (data) {
            lookup[data.pageid] = data;
            return data;
        }
    </script>
</head>
<ext:Store ID="StorePage" runat="server">
    <Reader>
        <ext:JsonReader>
            <Fields>
                <ext:RecordField Name="pageid" />
                <ext:RecordField Name="pagename" />
                <ext:RecordField Name="pageurl" />
                <ext:RecordField Name="pageimageurl" />
            </Fields>
        </ext:JsonReader>
    </Reader>
</ext:Store>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="SelectPage" />
        <ext:XTemplate ID="DetailsTemplate" runat="server">
            <Html>
                <div class="details">
			        <tpl for=".">
                       <iframe runat="server"  src="{pageurl}"   frameborder="no" border="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="yes" width="100%" height="430px"></iframe>
			        </tpl>
		        </div>
            </Html>
        </ext:XTemplate>
        <ext:Panel runat="server" Header="false" Border="false" Padding="15" Layout="Fit" Height="600">
            <Items>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:Panel runat="server" AutoScroll="true" Cls="img-chooser-view">
                            <Items>
                                <ext:DataView ID="ImageView" runat="server" SingleSelect="true" OverClass="x-view-over" StoreID="StorePage" ItemSelector="div.thumb-wrap" EmptyText="<div style='padding:10px;'>没有可使用页面</div>">
                                    <Template runat="server">
                                        <Html>
                                            <tpl for=".">
										<div class="thumb-wrap" id="{pageid}">
										<div class="thumb"><img src="{pageimageurl}" title="{pagename}" /></div>
										<span>{pagename}</span></div>
									</tpl>
                                        </Html>
                                    </Template>
                                    <Listeners>
                                        <BeforeSelect Handler="return this.store.getRange().length > 0;" />
                                        <SelectionChange Handler="showDetails();" />
                                    </Listeners>
                                </ext:DataView>
                            </Items>
                        </ext:Panel>
                    </Center>
                    <South Collapsible="True" Split="True" MinHeight="400" MaxHeight="600">
                        <ext:Panel runat="server" ID="ImageDetailPanel" Title="选择页面" Height="430px" />
                    </South>
                </ext:BorderLayout>
            </Items>
        </ext:Panel>
    </form>
</body>
</html>
