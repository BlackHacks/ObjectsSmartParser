using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmartObjectParser
{

    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void testBtn_Click(object sender, EventArgs e)
        {
            logLst.Items.Clear();
            parseResultLst.Items.Clear();
            parsedStringRchTxt.Text = "";

            SmartObjectParser O = new SmartObjectParser();

            Random a = new Random();
            int expectedCount = a.Next(100);
            
            List<myTestClass> testList = new List<myTestClass>();

            for (int i = 0; i < expectedCount; i++)
            {
                myTestClass internalTemp = new myTestClass();
                internalTemp.property1 = "P1 : " + i + a.Next(10000).ToString();
                internalTemp.property2 = a.Next(1000);
                internalTemp.property3 = a.NextDouble()*100;
                internalTemp.property4 = float.Parse(a.NextDouble().ToString());
                internalTemp.property5 = (a.NextDouble() > 0.5) ? true : false;
                internalTemp.property6 = "P6 :  " + a.NextDouble() + " / " + a.Next(100) + " ... ";
                testList.Add(internalTemp);
            }
            
            logLst.Items.Add("Added " + expectedCount +  " random items to list");
           
          
            String resultingParseString = O.storeObjectsList<myTestClass>(testList);
          
            List<myTestClass> resultingParsedList = new List<myTestClass>();
            
            O.smartParseBlock<myTestClass>(resultingParseString, resultingParsedList);

            parsedStringRchTxt.Text = resultingParseString;
            
            logLst.Items.Add("Tried Parsing : " + testList.Count);
            logLst.Items.Add("Parsed : " + resultingParsedList.Count);
            for (int i = 0; i < resultingParsedList.Count; i++)
            {
                parseResultLst.Items.Add("Found one Items . Details : ");
                parseResultLst.Items.Add("Property 1 : " + resultingParsedList[i].property1);
                parseResultLst.Items.Add("Property 2: " + resultingParsedList[i].property2);
                parseResultLst.Items.Add("Property 3 : " + resultingParsedList[i].property3);
                parseResultLst.Items.Add("Property 4 : " + resultingParsedList[i].property4);
                parseResultLst.Items.Add("Property 5 : " + resultingParsedList[i].property5);
                parseResultLst.Items.Add("Property 6 : " + resultingParsedList[i].property6);

            }


            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<String> a = new List<String>();
            a.Add("1");
            a.Add("2");
            a.Add("3");
            a.Add("4");
            a.Add("5");
            a.Add("6");
            SmartObjectParser O = new SmartObjectParser();
            String test = O.storeObjectsList<String>(a);
            List<String> result = new List<String>();
            O.smartParseBlock<String>(test, result);

            MessageBox.Show(test);

        }
    }
}
