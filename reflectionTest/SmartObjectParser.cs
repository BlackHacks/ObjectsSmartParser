using System;
using System.Collections.Generic;
using System.Text;

namespace SmartObjectParser
{
    class SmartObjectParser
    {
        public object getFieldValue(object obj, String fieldName)
        {
            System.Reflection.FieldInfo reflect = obj.GetType().GetField(fieldName);
            System.Reflection.FieldInfo[] reflectFields = obj.GetType().GetFields();
            String t = reflectFields[0].Name;
            return reflect.GetValue(obj);
        }

        public void setFieldValue(object obj, String fieldName, String value)
        {
            value = value.Substring(0, value.Length - 1);
            System.Reflection.FieldInfo reflect = obj.GetType().GetField(fieldName);
            String type = reflect.FieldType.ToString();
            //System.String  System.Int32  System.Double System.Single System.Boolean
            if (type == "System.String")
            {
                reflect.SetValue(obj, value);
            }
            if (type == "System.Int32")
            {
                reflect.SetValue(obj, Int32.Parse(value));

            }
            if (type == "System.Double")
            {
                reflect.SetValue(obj, double.Parse(value));

            }
            if (type == "System.Single")
            {
                reflect.SetValue(obj, float.Parse(value));

            }
            if (type == "System.Boolean")
            {
                reflect.SetValue(obj, Boolean.Parse(value));
            }
        }

        public void smartParseSingleLine(String line, object obj)
        {
            int loc = line.IndexOf(">");
            String fieldName = line.Substring(1, loc - 1);
            setFieldValue(obj, fieldName, line.Substring(loc + 2));
        }

        public void smartParseListBlock<GenType>(String blockText, List<GenType> resultList) where GenType : new()
        {
            int fieldNum = getFieldsList(new GenType()).Count;
            GenType f = new GenType();
            String[] splitter = blockText.Split('\n');
            for (int i = 0; i < splitter.Length; i++)
            {
                if ((i % fieldNum == 0 && i != 0))
                {
                    GenType B = f;
                    resultList.Add(B);
                    f = new GenType();
                }

                if (splitter[i] != "")
                {
                    smartParseSingleLine(splitter[i], f);
                }

            }
        }

        public List<String> getFieldsList(object obj)
        {
            List<String> result = new List<String>();
            System.Reflection.FieldInfo[] reflectFields = obj.GetType().GetFields();
            foreach (System.Reflection.FieldInfo r in reflectFields)
            {
                result.Add(r.Name.ToString());
            }

            return result;
        }

        public String storeSingleObject<GenType>(GenType i)
        {
            List<String> fields = getFieldsList(i);
            String result = "";
            foreach (String f in fields)
            {
                result += "<" + f + "> " + getFieldValue(i, f).ToString().Replace(Environment.NewLine, "\n") + Environment.NewLine;
            }
            return result;
        }

        public String storeObjectsList<GenType>(List<GenType> i)
        {
            String result = "";
            foreach (GenType e in i)
            {
                result += storeSingleObject<GenType>(e);
            }

            return result;
        }

    }
}
