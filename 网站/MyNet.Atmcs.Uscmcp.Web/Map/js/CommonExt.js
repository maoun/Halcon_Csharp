var selectWin = null;
function showSelectWin(title, width, height) {
    if (!selectWin) {
        selectWin = new Ext.Window({
            layout: 'fit',
            title: title,
            maximizable: false,
            width: 400,
            draggable: true,
            resizable: false,
            modal: true,
            border: false,
            html: '<div style="padding:20px 20px 20px 20px;">' + cityList + '</div>',
            autoDestroy: true,
            autoHeight: true,
            closeAction: 'hide',
            plain: false
        });
    }
    selectWin.setTitle(title);
    selectWin.doLayout();
    selectWin.show();
    selectWin.center();
    Ext.Window.superclass.onDestroy.call(this);
};