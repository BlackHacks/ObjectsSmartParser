using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace reflectionTest
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


      
        private void button1_Click(object sender, EventArgs e)
        {
            SmartObjectParser O = new SmartObjectParser();
            myTestClass b = new myTestClass();
            b.property1 = "Test";
            b.property2 = 3;
            b.property3 = 5.2;
            myTestClass c = new myTestClass();
            c.property1 = "Test2";
            c.property2 = 4;
            c.property3 = 4.56;
            myTestClass d = new myTestClass();
            d.property1 = "Test3";
            d.property2 = 5;
            d.property3 = 3.1541;
            d.property4 = 4.15f;
            d.property5 = false;

            List<myTestClass> list = new List<myTestClass>();
            list.Add(b);
            list.Add(c);
            list.Add(d);
            String s = O.saveObjectsList<myTestClass>(list);
            List<myTestClass> result = new List<myTestClass>();
            O.smartParseListBlock<myTestClass>(s, result);

            MessageBox.Show(s);
            
        }
    }
}
