using System;
using System.Collections.Generic;
using System.Text;

namespace reflectionTest
{
    class myTestClass
    {
        public myTestClass()
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    public string property1 = "NumberOne";
    public int property2 = 2;
    public double property3 = 3.5;
    public float property4 = 4.60f;
    public Boolean property5 = true;
    public String property6 = "1234";

    }
}
