using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SmartObjectParser
{
    class SmartObjectParser
    {
        public object getFieldValue(object obj, String fieldName)
        {
            System.Reflection.FieldInfo reflect = obj.GetType().GetField(fieldName);
            System.Reflection.FieldInfo[] reflectFields = obj.GetType().GetFields();
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
            int firstTagLocation = line.IndexOf(">");
            String fieldName = line.Substring(1, firstTagLocation - 1);
            setFieldValue(obj, fieldName, line.Substring(firstTagLocation + 2));
        }

        public void smartParseBlock<GenType>(String blockText, List<GenType> resultList) where GenType : new()
        {
            int fieldCount = getFieldsList(new GenType()).Count;
            
            GenType internalTempObject = new GenType();
            String[] newLineSplitArray = blockText.Split('\n');
            for (int i = 0; i < newLineSplitArray.Length; i++)
            {
                if ((i % fieldCount == 0 && i != 0))
                {
                    GenType B = internalTempObject; // rough clone
                    resultList.Add(B);
                    internalTempObject = new GenType();
                }

                if (newLineSplitArray[i].Length > 2)
                {
                    smartParseSingleLine(newLineSplitArray[i], internalTempObject);
                }

            }
        }

        public void smartParseFile<GenType>(String filePath, List<GenType> resultList) where GenType : new()
        {
                StreamReader reader = new StreamReader(filePath);
                String blockText = reader.ReadToEnd();
                reader.Close();
                smartParseBlock<GenType>(blockText, resultList);
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

        public void storeObjectListToFile<GenType>(String filePath, List<GenType> i , Boolean append)
        {
            StreamWriter writer = new StreamWriter(filePath, append);
            writer.WriteLine(storeObjectsList<GenType>(i));
            writer.Close();
        }
        public void storeSingleObjectToFile<GenType>(String filePath,GenType i, Boolean append)
        {
            StreamWriter writer = new StreamWriter(filePath, append);
            writer.WriteLine(storeSingleObject<GenType>(i));
            writer.Close();
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
