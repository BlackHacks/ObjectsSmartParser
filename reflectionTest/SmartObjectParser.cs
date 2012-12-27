using System;
using System.Collections.Generic;
using System.Text;

namespace reflectionTest
{
    class SmartObjectParser
    {
        public object getFieldValue(object i, String fieldName)
        {
            System.Reflection.FieldInfo reflect = i.GetType().GetField(fieldName);
            System.Reflection.FieldInfo[] reflectFields = i.GetType().GetFields();
            String t = reflectFields[0].Name;
            return reflect.GetValue(i);
        }

        public void setFieldValue(object i, String fieldName, String value)
        {
            value = value.Substring(0, value.Length - 1);
            System.Reflection.FieldInfo reflect = i.GetType().GetField(fieldName);
            String type = reflect.FieldType.ToString();
            //System.String  System.Int32  System.Double System.Single System.Boolean
            if (type == "System.String")
            {
                reflect.SetValue(i, value);
            }
            if (type == "System.Int32")
            {
                reflect.SetValue(i, Int32.Parse(value));

            }
            if (type == "System.Double")
            {
                reflect.SetValue(i, double.Parse(value));

            }
            if (type == "System.Single")
            {
                reflect.SetValue(i, float.Parse(value));

            }
            if (type == "System.Boolean")
            {
                reflect.SetValue(i, Boolean.Parse(value));
            }
        }

        public void smartParseSingleLine(String line, object i)
        {
            int loc = line.IndexOf(">");
            String fieldName = line.Substring(1, loc - 1);
            setFieldValue(i, fieldName, line.Substring(loc + 2));
        }

        public void smartParseListBlock<T>(String text, List<T> list) where T : new()
        {
            int fieldNum = getFieldsList(new T()).Count;
            T f = new T();
            String[] splitter = text.Split('\n');
            for (int i = 0; i < splitter.Length; i++)
            {
                if ((i % fieldNum == 0 && i != 0))
                {
                    T B = f;
                    list.Add(B);
                    f = new T();
                }

                if (splitter[i] != "")
                {
                    smartParseSingleLine(splitter[i], f);
                }

            }
        }

        public List<String> getFieldsList(object i)
        {
            List<String> result = new List<String>();
            System.Reflection.FieldInfo[] reflectFields = i.GetType().GetFields();
            foreach (System.Reflection.FieldInfo r in reflectFields)
            {
                result.Add(r.Name.ToString());
            }

            return result;
        }

        public String saveSingleObject<T>(T i)
        {
            List<String> fields = getFieldsList(i);
            String result = "";
            foreach (String f in fields)
            {
                result += "<" + f + "> " + getFieldValue(i, f).ToString().Replace(Environment.NewLine, "\n") + Environment.NewLine;
            }
            return result;
        }

        public String saveObjectsList<T>(List<T> i)
        {
            String result = "";
            foreach (T e in i)
            {
                result += saveSingleObject<T>(e);
            }

            return result;
        }

    }
}
