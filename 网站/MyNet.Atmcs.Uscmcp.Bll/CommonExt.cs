using Ext.Net;
using System;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class CommonExt
    {
        public static Ext.Net.Button AddButton(Toolbar toolbar, string fieldName, string title, string icon, string handler)
        {
            Ext.Net.Button but = new Ext.Net.Button();
            but.ID = fieldName;
            but.Text = title;
            but.Icon = (Icon)Enum.Parse(typeof(Icon), icon, true);
            but.Listeners.Click.Handler = handler;
            toolbar.Add(but);
            return but;
        }

        public static Ext.Net.Button AddButton(string fieldName, string title, string icon, string handler)
        {
            Ext.Net.Button but = new Ext.Net.Button();
            but.ID = fieldName;
            but.Text = title;
            but.Icon = (Icon)Enum.Parse(typeof(Icon), icon, true);
            but.Listeners.Click.Handler = handler;
            return but;
        }

        public static Ext.Net.Button AddButton(string fieldName, string title, string icon, string handler, string style)
        {
            Ext.Net.Button but = new Ext.Net.Button();
            but.ID = fieldName;
            but.Text = title;
            but.Icon = (Icon)Enum.Parse(typeof(Icon), icon, true);
            but.Listeners.Click.Handler = handler;
            but.StyleSpec = style;
            return but;
        }

        public static Ext.Net.Panel AddPanel(string title)
        {
            Ext.Net.Panel tab = new Ext.Net.Panel();
            tab.Title = title;
            tab.Padding = 5;
            return tab;
        }

        public static TextArea AddTextArea(string fieldName, string title)
        {
            TextArea ta = new TextArea();
            ta.ID = fieldName;
            ta.FieldLabel = title;
            ta.AnchorHorizontal = "98%";
            ta.DefaultAnchor = "100%";
            ta.AllowBlank = true;
            return ta;
        }

        public static TextField AddTextFieldPassword(string fieldName, string title, bool allowBlank)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.Width = Unit.Pixel(300);
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "100%";
            tx.InputType = InputType.Password;
            tx.AllowBlank = allowBlank;
            return tx;
        }

        public static TextField AddTextFieldPassword_Confirm(string fieldName, string title, bool allowBlank, string verfiy_field)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.Width = Unit.Pixel(300);
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "90%";
            tx.InputType = InputType.Password;
            tx.AllowBlank = allowBlank;
            tx.Vtype = "password";
            tx.MsgTarget = MessageTarget.Side;
            ConfigItem configItem = new ConfigItem("initialPassField", "#{" + verfiy_field + "}", ParameterMode.Value);
            tx.CustomConfig.Add(configItem);
            return tx;
        }

        public static Ext.Net.Label AddLable(string fieldName, string title, string value)
        {
            Ext.Net.Label lb = new Ext.Net.Label();
            lb.ID = fieldName;
            lb.FieldLabel = title;
            lb.Width = Unit.Pixel(300);
            lb.AnchorHorizontal = "98%";
            lb.DefaultAnchor = "100%";
            lb.Text = value;
            return lb;
        }

        public static Ext.Net.Label AddLable(string fieldName, string title, string value, string style)
        {
            Ext.Net.Label lb = new Ext.Net.Label();
            lb.ID = fieldName;
            lb.FieldLabel = title;
            lb.Width = Unit.Pixel(300);
            lb.AnchorHorizontal = "98%";
            lb.DefaultAnchor = "100%";
            lb.Text = value;
            lb.StyleSpec = style;
            return lb;
        }

        public static Ext.Net.Label AddLable(string fieldName, string title, string value, int width)
        {
            Ext.Net.Label lb = new Ext.Net.Label();
            lb.ID = fieldName;
            lb.FieldLabel = title;
            lb.Width = Unit.Pixel(300);
            lb.AnchorHorizontal = "98%";
            lb.DefaultAnchor = "100%";
            lb.Text = value;
            lb.Width = width;
            return lb;
        }

        public static TextField AddTextField(string fieldName, string title)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.Width = Unit.Pixel(300);
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "100%";
            tx.AllowBlank = true;
            return tx;
        }

        public static TextField AddTextField(string fieldName, string title, string value)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "100%";
            tx.Text = value;
            return tx;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="title"></param>
        /// <param name="value"></param>
        /// <param name="b">控件是否启用</param>
        /// <returns></returns>
        public static TextField AddTextField(string fieldName, string title, string value, bool b)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "100%";
            tx.Text = value;
            tx.Disabled = b;
            return tx;
        }

        /// <summary>
        /// 重载的方法（区间车辆查询中用到）
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="title"></param>
        /// <param name="value"></param>
        /// <param name="meiyong"></param>
        /// <returns></returns>
        public static TextField AddTextField(string fieldName, string title, string value, string meiyong)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "100%";
            tx.Text = value;
            tx.Disabled = true;
            return tx;
        }

        public static TextField AddTextField(string fieldName, string title, string value, string meiyong, int width)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            //tx.AnchorHorizontal = "99%";
            //tx.DefaultAnchor = "100%";
            tx.Width = width;
            tx.Text = value;
            tx.Disabled = true;
            return tx;
        }

        public static TextField AddTextField(string fieldName, string title, bool allowBlank, string emptyText)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "100%";
            tx.AllowBlank = allowBlank;
            tx.EmptyText = emptyText;
            return tx;
        }

        public static TextField AddTextField(string fieldName, string title, bool allowBlank, string emptyText, int width, int labelwidth)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.AllowBlank = allowBlank;
            tx.EmptyText = emptyText;
            tx.Width = width;
            tx.LabelWidth = labelwidth;
            return tx;
        }

        public static TextField AddTextFieldWidth(string fieldName, string title, bool allowBlank, string emptyText)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "100%";
            tx.Width = Unit.Pixel(300);
            tx.AllowBlank = allowBlank;
            tx.EmptyText = emptyText;
            return tx;
        }

        public static TextField AddTextField(string fieldName, string title, bool allowBlank, string emptyText, string note)
        {
            TextField tx = new TextField();
            tx.ID = fieldName;
            tx.FieldLabel = title;
            tx.AnchorHorizontal = "98%";
            tx.DefaultAnchor = "100%";
            tx.AllowBlank = allowBlank;
            tx.EmptyText = emptyText;
            ToolTip tt = new ToolTip();
            tt.Title = note;
            tx.ToolTips.Add(tt);
            return tx;
        }

        public static Ext.Net.ComboBox AddComboBox(string fieldName, string title, string storeID, string emptyText)
        {
            ComboBox cmb = new ComboBox();
            cmb.ID = fieldName;
            cmb.FieldLabel = title;
            cmb.StoreID = storeID;
            cmb.DisplayField = "col1";
            cmb.ValueField = "col0";
            cmb.Editable = false;
            cmb.Mode = DataLoadMode.Local;
            cmb.TriggerAction = TriggerAction.All;
            cmb.EmptyText = emptyText;
            cmb.AnchorHorizontal = "98%";
            cmb.DefaultAnchor = "100%";
            return cmb;
        }

        public static Ext.Net.ComboBox AddComboBox(string fieldName, string title, string storeID, string emptyText, int width)
        {
            ComboBox cmb = new ComboBox();
            cmb.ID = fieldName;
            cmb.FieldLabel = title;
            cmb.StoreID = storeID;
            cmb.DisplayField = "col1";
            cmb.ValueField = "col0";
            cmb.Editable = false;
            cmb.Mode = DataLoadMode.Local;
            cmb.TriggerAction = TriggerAction.All;
            cmb.EmptyText = emptyText;
            cmb.Width = width;
            cmb.AnchorHorizontal = "98.5%";
            cmb.DefaultAnchor = "100%";
            FieldTrigger fieldTrigger = new FieldTrigger();
            fieldTrigger.Icon = TriggerIcon.Clear;
            fieldTrigger.HideTrigger = true;
            fieldTrigger.Qtip = "清除选中";
            cmb.Triggers.Add(fieldTrigger);
            cmb.Listeners.Select.Handler = "getZfgg();this.triggers[0].show();";
            cmb.Listeners.BeforeQuery.Handler = "this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();";
            cmb.Listeners.TriggerClick.Handler = "if (index == 0) { this.clearValue(); this.triggers[0].hide(); };";
            return cmb;
        }

        private static void cmb_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public static Ext.Net.ComboBox AddComboBox(string fieldName, string title, string storeID, string emptyText, bool allowBlank)
        {
            ComboBox cmb = new ComboBox();
            cmb.ID = fieldName;
            cmb.FieldLabel = title;
            cmb.StoreID = storeID;
            cmb.DisplayField = "col1";
            cmb.ValueField = "col0";
            cmb.Editable = false;
            cmb.Mode = DataLoadMode.Local;
            cmb.TriggerAction = TriggerAction.All;
            cmb.EmptyText = emptyText;
            cmb.AnchorHorizontal = "98%";
            cmb.DefaultAnchor = "100%";
            cmb.AllowBlank = allowBlank;
            return cmb;
        }

        public static Ext.Net.ComboBox AddComboBox(string fieldName, string title, string storeID, string emptyText, bool allowBlank, string displayField, string valueField)
        {
            ComboBox cmb = new ComboBox();
            cmb.ID = fieldName;
            cmb.FieldLabel = title;
            cmb.StoreID = storeID;
            cmb.DisplayField = displayField;
            cmb.ValueField = valueField;
            cmb.Editable = false;
            cmb.Mode = DataLoadMode.Local;
            cmb.TriggerAction = TriggerAction.All;
            cmb.EmptyText = emptyText;
            cmb.AnchorHorizontal = "98%";
            cmb.DefaultAnchor = "100%";
            cmb.AllowBlank = allowBlank;
            return cmb;
        }

        public static Ext.Net.ComboBox AddComboBox(string fieldName, string title, string storeID, string emptyText, bool allowBlank, int width, int labelwidth, string selectValue)
        {
            ComboBox cmb = new ComboBox();
            cmb.ID = fieldName;
            cmb.FieldLabel = title;
            cmb.StoreID = storeID;
            cmb.DisplayField = "col1";
            cmb.ValueField = "col0";
            cmb.Editable = false;
            cmb.Mode = DataLoadMode.Local;
            cmb.TriggerAction = TriggerAction.All;
            cmb.EmptyText = emptyText;
            cmb.AnchorHorizontal = "98%";
            cmb.DefaultAnchor = "100%";
            cmb.AllowBlank = allowBlank;
            cmb.Width = width;
            cmb.LabelWidth = labelwidth;
            if (!selectValue.Equals("0"))
            {
                cmb.SetValue(selectValue);
            }
            return cmb;
        }

        public static NumberField AddNumberField(string fieldName, string title)
        {
            NumberField nf = new NumberField();
            nf.ID = fieldName;
            nf.FieldLabel = title;
            nf.Width = Unit.Pixel(300);
            nf.MinValue = 0;
            nf.MaxValue = 100;
            nf.AllowDecimals = true;
            nf.DecimalPrecision = 0;
            return nf;
        }

        public static NumberField AddNumberField(string fieldName, string title, int MinValue, int MaxValue, bool allowBlank, string emptyText)
        {
            NumberField nf = new NumberField();
            nf.ID = fieldName;
            nf.FieldLabel = title;
            nf.Width = Unit.Pixel(300);
            nf.MinValue = MinValue;
            nf.MaxValue = MaxValue;
            nf.AllowDecimals = true;
            nf.DecimalPrecision = 0;
            nf.AllowBlank = allowBlank;
            nf.EmptyText = emptyText;
            return nf;
        }

        public static SpinnerField AddSpinnerField(string fieldName, string title)
        {
            SpinnerField sf = new SpinnerField();
            sf.ID = fieldName;
            sf.FieldLabel = title;
            sf.Width = Unit.Pixel(300);
            sf.MinValue = 0;
            sf.MaxValue = 100;
            sf.AllowDecimals = true;
            sf.DecimalPrecision = 0;
            sf.IncrementValue = 1;
            return sf;
        }

        public static SpinnerField AddSpinnerField(string fieldName, string title, int MinValue, int MaxValue)
        {
            SpinnerField sf = new SpinnerField();
            sf.ID = fieldName;
            sf.FieldLabel = title;
            sf.Width = Unit.Pixel(300);
            sf.MinValue = MinValue;
            sf.MaxValue = MaxValue;
            sf.AllowDecimals = true;
            sf.DecimalPrecision = 0;
            sf.IncrementValue = 1;
            return sf;
        }

        public static Checkbox AddCheckbox(string fieldName, string title)
        {
            Checkbox ck = new Checkbox();
            ck.ID = fieldName;
            ck.FieldLabel = title;
            return ck;
        }

        public static string IsNullComboBox(ComboBox comboBox, string defaltvalue)
        {
            if (comboBox.SelectedIndex != -1)
            {
                return comboBox.Value.ToString();
            }
            else
            {
                return defaltvalue;
            }
        }
    }
}