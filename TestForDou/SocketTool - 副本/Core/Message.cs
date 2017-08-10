using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SocketTool.Core
{
    public abstract class Message
    {
        private Guid messageID;
        private DateTime timeStamp;

        public Guid MessageID { get { return messageID; } set { messageID = value; } }
        public DateTime TimeStamp { get { return timeStamp; } set { timeStamp = value; } }

        public Message()
        {
            messageID = Guid.NewGuid();
            timeStamp = DateTime.UtcNow;
        }


        public byte[] ToByte()
        {
            List<byte> tempByte = new List<byte>();
            List<byte> finalByte = new List<byte>();
            Int32 messageLength;

            tempByte.AddRange(toByte());
            messageLength = tempByte.Count;
            finalByte.AddRange(BitConverter.GetBytes(messageLength));
            finalByte.AddRange(tempByte);
            tempByte = null;
            return finalByte.ToArray();

        }

        private byte[] toByte()
        {
            object Msg = this;
            List<byte> data = new List<byte>();
            try
            {
                Type type = Msg.GetType();
                FieldInfo[] fields = type.GetFields();
                PropertyInfo[] properties = type.GetProperties();

                foreach (var field in fields)
                {
                    string name = field.Name;
                    object temp = field.GetValue(Msg);
                    TypeCode typeCode = GetTypeCode(field, temp);// Type.GetTypeCode(temp.GetType());

                    data.AddRange(AddData(typeCode, temp, field.FieldType.FullName));

                }
                foreach (var property in properties)
                {
                    string name = property.Name;
                    object temp = property.GetValue(Msg, null);
                    TypeCode typeCode = GetPropertyTypeCode(property, temp);//Type.GetTypeCode(temp.GetType());

                    data.AddRange(AddData(typeCode, temp, property.PropertyType.FullName));

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //handleError(ex);
            }

            return data.ToArray();
        }

        private byte[] AddData(TypeCode typeCode, object temp, string FullName)
        {
            List<byte> data = new List<byte>();

            switch (typeCode)
            {
                case TypeCode.Int16:
                    Int16 valueint16 = (Int16)temp;
                    data.AddRange(BitConverter.GetBytes(valueint16));
                    break;
                case TypeCode.Int32:
                    int valueint = (int)temp;
                    data.AddRange(BitConverter.GetBytes(valueint));
                    break;
                case TypeCode.Int64:
                    Int64 valueint64 = (Int64)temp;
                    data.AddRange(BitConverter.GetBytes(valueint64));
                    break;
                case TypeCode.Single:
                    Single valueintsingle = (Single)temp;
                    data.AddRange(BitConverter.GetBytes(valueintsingle));
                    break;
                case TypeCode.Boolean:
                    bool valuebool = (bool)temp;
                    data.AddRange(BitConverter.GetBytes(valuebool));
                    break;
                case TypeCode.DateTime:
                    DateTime valuedatetime = (DateTime)temp;
                    data.AddRange(BitConverter.GetBytes(valuedatetime.ToBinary()));
                    break;
                case TypeCode.String:
                    string valuestring = temp as string;
                    if (valuestring != null)
                    {
                        data.AddRange(BitConverter.GetBytes(valuestring.Length));
                        data.AddRange(UnicodeEncoding.UTF8.GetBytes(valuestring));
                    }
                    else
                    {
                        data.AddRange(BitConverter.GetBytes(0));
                    }
                    break;
                case TypeCode.Object:
                    switch (FullName)
                    {
                        case "System.Guid":
                            Guid valueguid = (Guid)temp;
                            data.AddRange(valueguid.ToByteArray());
                            break;
                        case "System.Byte[]":

                            if (temp != null)
                            {
                                byte[] valuebytearray = (byte[])temp;
                                data.AddRange(BitConverter.GetBytes(valuebytearray.Length));
                                data.AddRange(valuebytearray);
                            }
                            else
                            {
                                data.AddRange(BitConverter.GetBytes(0));
                            }
                            break;
                        case "System.Int16[]":
                            if (temp != null)
                            {
                                Int16[] ttemp = (Int16[])temp;
                                if (ttemp.Length > 0)
                                {
                                    List<byte> result = new List<byte>();
                                    int counter = 0;
                                    foreach (Int16 item in (Int16[])temp)
                                    {
                                        result.AddRange(BitConverter.GetBytes(item));
                                        counter++;
                                    }

                                    byte[] valueint16array = result.ToArray();

                                    data.AddRange(BitConverter.GetBytes(valueint16array.Length));
                                    data.AddRange(BitConverter.GetBytes(counter));
                                    data.AddRange(valueint16array);
                                }
                                else
                                {
                                    data.AddRange(BitConverter.GetBytes(0));
                                }
                            }
                            else
                            {
                                data.AddRange(BitConverter.GetBytes(0));
                            }
                            break;
                    }
                    break;
                case TypeCode.Byte:
                    break;
                case TypeCode.Char:
                    break;
                case TypeCode.Double:
                    double valuedouble = (double)temp;
                    data.AddRange(BitConverter.GetBytes(valuedouble));
                    break;
                case TypeCode.UInt16:
                    UInt16 valueuint16 = (UInt16)temp;
                    data.AddRange(BitConverter.GetBytes(valueuint16));
                    break;
                case TypeCode.UInt32:
                    UInt32 valueuint32 = (UInt32)temp;
                    data.AddRange(BitConverter.GetBytes(valueuint32));
                    break;
                case TypeCode.UInt64:
                    UInt64 valueuint64 = (UInt64)temp;
                    data.AddRange(BitConverter.GetBytes(valueuint64));
                    break;

            }
            return data.ToArray();
        }

        public object ToMessage(byte[] data)
        {
            try
            {
                object output = this;// Activator.CreateInstance(this);
                Type type = output.GetType();
                FieldInfo[] fields = type.GetFields();
                PropertyInfo[] properties = type.GetProperties();

                int index = 0;
                int actualMessageLength = 0;
                actualMessageLength = (Int32)extract(data, ref index, actualMessageLength);

                if (actualMessageLength == data.Length - index)
                {


                    foreach (var field in fields)
                    {

                        string name = field.Name; // Get string name
                        object temp = field.GetValue(output); // Get value
                        TypeCode typeCode = GetTypeCode(field, temp);// Type.GetTypeCode(temp.GetType());



                        switch (typeCode)
                        {
                            case TypeCode.Int16:
                                Int16 valueint16 = 0;
                                valueint16 = (Int16)extract(data, ref index, valueint16);
                                field.SetValue(output, valueint16);
                                break;
                            case TypeCode.Int32:
                                int valueint32 = 0;
                                valueint32 = (Int32)extract(data, ref index, valueint32);
                                field.SetValue(output, valueint32);
                                break;
                            case TypeCode.Int64:
                                Int64 valueint64 = 0;
                                valueint64 = (Int64)extract(data, ref index, valueint64);
                                field.SetValue(output, valueint64);
                                break;
                            case TypeCode.Single:
                                Single valuesingle = 0;
                                valuesingle = (Single)extract(data, ref index, valuesingle);
                                field.SetValue(output, valuesingle);
                                break;
                            case TypeCode.Boolean:
                                bool valuebool = false;
                                valuebool = (bool)extract(data, ref index, valuebool);
                                field.SetValue(output, valuebool);
                                break;
                            case TypeCode.DateTime:
                                DateTime valuedatetime = new DateTime();
                                valuedatetime = (DateTime)extract(data, ref index, valuedatetime);
                                field.SetValue(output, valuedatetime);
                                break;
                            case TypeCode.String:
                                int stringlength = 0;
                                stringlength = (Int32)extract(data, ref index, stringlength);
                                string valuestring = string.Empty;
                                if (stringlength > 0)
                                    valuestring = (string)extract(data, ref index, valuestring, length: stringlength);
                                field.SetValue(output, valuestring);
                                break;

                            case TypeCode.Object:
                                switch (field.FieldType.FullName)
                                {
                                    case "System.Guid":
                                        Guid valueguid = new Guid();
                                        valueguid = (Guid)extract(data, ref index, valueguid, start: index);
                                        field.SetValue(output, valueguid);
                                        break;
                                    case "System.Byte[]":
                                        int bytelength = 0;
                                        bytelength = (Int32)extract(data, ref index, bytelength);

                                        byte[] valuebyte = new byte[bytelength];
                                        if (bytelength > 0)
                                            valuebyte = (byte[])extract(data, ref index, valuebyte, length: bytelength);
                                        field.SetValue(output, valuebyte);
                                        break;
                                    case "System.Int16[]":
                                        int int16length = 0;
                                        int16length = (Int32)extract(data, ref index, int16length);
                                        if (int16length > 0)
                                        {
                                            int arraycount = 0;
                                            arraycount = (Int32)extract(data, ref index, arraycount);

                                            Int16[] valueint16array = byteToArray((byte[])extract(data, ref index, new byte[] { }, length: int16length), arraycount);
                                            field.SetValue(output, valueint16array);
                                        }
                                        else
                                        {
                                            field.SetValue(output, new Int16[] { });
                                        }
                                        break;
                                }
                                break;
                            case TypeCode.Byte:
                                break;
                            case TypeCode.Char:
                                break;
                            case TypeCode.Double:
                                double valuedouble = 0;
                                valuedouble = (double)extract(data, ref index, valuedouble);
                                field.SetValue(output, valuedouble);
                                break;
                            case TypeCode.UInt16:
                                UInt16 valueuint16 = 0;
                                valueuint16 = (UInt16)extract(data, ref index, valueuint16);
                                field.SetValue(output, valueuint16);
                                break;
                            case TypeCode.UInt32:
                                UInt32 valueuint32 = 0;
                                valueuint32 = (UInt32)extract(data, ref index, valueuint32);
                                field.SetValue(output, valueuint32);
                                break;
                            case TypeCode.UInt64:
                                UInt64 valueuint64 = 0;
                                valueuint64 = (UInt64)extract(data, ref index, valueuint64);
                                field.SetValue(output, valueuint64);
                                break;
                        }




                    }
                    foreach (var property in properties)
                    {

                        string pname = property.Name; // Get string name
                        object ptemp = property.GetValue(output, null); // Get value
                        TypeCode ptypeCode = GetPropertyTypeCode(property, ptemp);// Type.GetTypeCode(temp.GetType());



                        switch (ptypeCode)
                        {
                            case TypeCode.Int16:
                                Int16 valueint16 = 0;
                                valueint16 = (Int16)extract(data, ref index, valueint16);
                                SetProperty(property, output, valueint16);
                                break;
                            case TypeCode.Int32:
                                int valueint32 = 0;
                                valueint32 = (Int32)extract(data, ref index, valueint32);
                                SetProperty(property, output, valueint32);
                                break;
                            case TypeCode.Int64:
                                Int64 valueint64 = 0;
                                valueint64 = (Int64)extract(data, ref index, valueint64);
                                SetProperty(property, output, valueint64);
                                break;
                            case TypeCode.Single:
                                Single valuesingle = 0;
                                valuesingle = (Single)extract(data, ref index, valuesingle);
                                SetProperty(property, output, valuesingle);
                                break;
                            case TypeCode.Boolean:
                                bool valuebool = false;
                                valuebool = (bool)extract(data, ref index, valuebool);
                                SetProperty(property, output, valuebool);
                                break;
                            case TypeCode.DateTime:
                                DateTime valuedatetime = new DateTime();
                                valuedatetime = (DateTime)extract(data, ref index, valuedatetime);
                                SetProperty(property, output, valuedatetime);
                                break;
                            case TypeCode.String:
                                int stringlength = 0;
                                stringlength = (Int32)extract(data, ref index, stringlength);
                                string valuestring = string.Empty;
                                if (stringlength > 0)
                                    valuestring = (string)extract(data, ref index, valuestring, length: stringlength);
                                SetProperty(property, output, valuestring);
                                break;

                            case TypeCode.Object:
                                switch (property.PropertyType.FullName)
                                {
                                    case "System.Guid":
                                        Guid valueguid = new Guid();
                                        valueguid = (Guid)extract(data, ref index, valueguid, start: index);
                                        SetProperty(property, output, valueguid);
                                        break;
                                    case "System.Byte[]":
                                        int bytelength = 0;
                                        bytelength = (Int32)extract(data, ref index, bytelength);
                                        byte[] valuebyte = new byte[bytelength];
                                        if (bytelength > 0)
                                            valuebyte = (byte[])extract(data, ref index, valuebyte, length: bytelength);
                                        SetProperty(property, output, valuebyte);
                                        break;
                                    case "System.Int16[]":
                                        int int16length = 0;
                                        int16length = (Int32)extract(data, ref index, int16length);
                                        if (int16length > 0)
                                        {
                                            int arraycount = 0;
                                            arraycount = (Int32)extract(data, ref index, arraycount);

                                            Int16[] valueint16array = byteToArray((byte[])extract(data, ref index, new byte[] { }, length: int16length), arraycount);
                                            SetProperty(property, output, valueint16array);
                                        }
                                        else
                                        {
                                            SetProperty(property, output, new Int16[] { });
                                        }
                                        break;
                                }
                                break;
                        }



                    }


                    return output;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("ToMessage Error: {0}", ex.Message);
                return null;
            }

        }

        private void SetProperty(PropertyInfo property, object output, object value)
        {
            try
            {
                property.SetValue(output, value, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private Int16[] byteToArray(byte[] b, Int32 itemCount)
        {
            int i = 0;
            int index = 0;

            List<Int16> result = new List<Int16>();

            for (i = 0; i < itemCount; i++)
            {
                Int16 item = 0;
                item = (Int16)extract(b, ref index, item);
                result.Add(item);
            }
            return result.ToArray();
        }

        private Object extract(byte[] data, ref int index, Object var, int start = 0, int length = 0)
        {
            TypeCode typeCode = Type.GetTypeCode(var.GetType());
            Object v;

            switch (typeCode)
            {
                case TypeCode.Int16:
                    v = BitConverter.ToInt16(data, index);
                    index += sizeof(Int16);
                    return v;
                case TypeCode.Int32:
                    v = BitConverter.ToInt32(data, index);
                    index += sizeof(Int32);
                    return v;
                case TypeCode.Int64:
                    v = BitConverter.ToInt64(data, index);
                    index += sizeof(Int64);
                    return v;
                case TypeCode.Single:
                    v = BitConverter.ToSingle(data, index);
                    index += sizeof(Single);
                    return v;
                case TypeCode.Boolean:
                    v = BitConverter.ToBoolean(data, index);
                    index += sizeof(Boolean);
                    return v;
                case TypeCode.DateTime:
                    v = BitConverter.ToInt64(data, index);
                    index += sizeof(Int64);
                    return DateTime.FromBinary((Int64)v);
                    /**
                case TypeCode.String:
                    v = UnicodeEncoding.UTF8.GetString(data.Skip(index).Take(length).ToArray());
                    index += length;
                    return v;*/
                case TypeCode.Object:
                    switch (var.GetType().ToString())
                    {
                            /**
                        case "System.Guid":
                            int GuidLenght = 16;
                            v = new Guid(data.Skip(start).Take(GuidLenght).ToArray());
                            index += GuidLenght;
                            return v;
                        case "System.Byte[]":
                            v = data.Skip(index).Take(length).ToArray();
                            index += length;
                            return v;
                             * */

                        default:
                            throw new Exception("An Object of type \"" + var.GetType().ToString() + "\" needs to be defined. Please extend extract method in MediaServer.");
                    }
                case TypeCode.Double:
                    v = BitConverter.ToDouble(data, index);
                    index += sizeof(double);
                    return v;
                case TypeCode.UInt16:
                    v = BitConverter.ToUInt16(data, index);
                    index += sizeof(UInt16);
                    return v;
                case TypeCode.UInt32:
                    v = BitConverter.ToUInt32(data, index);
                    index += sizeof(UInt32);
                    return v;
                case TypeCode.UInt64:
                    v = BitConverter.ToUInt64(data, index);
                    index += sizeof(UInt64);
                    return v;
            }
            throw new Exception("A TypeCode \"" + typeCode.ToString() + "\" needs to be defined. Please extend extract method in MediaServer.");
        }

        private TypeCode GetTypeCode(FieldInfo field, object temp)
        {
            if (temp != null)
            {
                return Type.GetTypeCode(temp.GetType());
            }
            TypeCode tpcode = TypeCode.Object;
            switch (field.FieldType.FullName)
            {
                case "System.String":
                    tpcode = TypeCode.String;
                    break;
                default:
                    tpcode = TypeCode.Object;
                    break;
            }
            return tpcode;
        }

        private TypeCode GetPropertyTypeCode(PropertyInfo property, object temp)
        {
            if (temp != null)
            {
                return Type.GetTypeCode(temp.GetType());
            }
            TypeCode tpcode = TypeCode.Object;
            switch (property.PropertyType.FullName)
            {
                case "System.String":
                    tpcode = TypeCode.String;
                    break;
                default:
                    tpcode = TypeCode.Object;
                    break;
            }
            return tpcode;
        }

    }
}
