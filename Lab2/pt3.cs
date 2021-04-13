using System;

namespace pt3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter multiplicand number: ");
            float multiplicand = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter multiplier number: ");
            float multiplier = float.Parse(Console.ReadLine());
            Console.WriteLine("========================================================");

            IEEE754FloatingPoint Multiplication = new IEEE754FloatingPoint(multiplicand, multiplier);
            Multiplication.Multiplication();
            Console.ReadKey();
        }
    }

    public class IEEE754FloatingPoint
    {
        public float Multiplicand { get; set; }
        public float Multiplier { get; set; }

        public IEEE754FloatingPoint(float x, float y)
        {
            Multiplicand = x;
            Multiplier = y;
        }

        public void Multiplication()
        {
            //Initialization:
            long MultiplicandBits = GetFloatBits(Multiplicand), MultiplierBits = GetFloatBits(Multiplier);
            int sign1 = (int)((MultiplicandBits >> 31) & 1), sign2 = (int)((MultiplierBits >> 31) & 1);
            int expo1 = (int)((MultiplicandBits >> 23) & 255), expo2 = (int)((MultiplierBits >> 23) & 255);
            long mantissa1 = MultiplicandBits & ((int)Math.Pow(2, 23) - 1), mantissa2 = MultiplierBits & ((int)Math.Pow(2, 23) - 1);
            long mantissaMultiplication = ((1 << 23) | mantissa1) * ((1 << 23) | mantissa2);


            //Mantissa multiplication:
            Console.WriteLine("Mantissa multiplication:");
            Console.WriteLine("Multiplicand:   " + FinishStringWithZeros(Convert.ToString(mantissa1, 2), 24) + "\n                x");
            Console.WriteLine("Multiplier:     " + FinishStringWithZeros(Convert.ToString(mantissa2, 2), 24) + "\n                =");
            Console.WriteLine("Mantissa:       " + FinishStringWithZeros(Convert.ToString(mantissaMultiplication, 2), 48));
            mantissaMultiplication >>= 23;
            int expoAddition = ((1 << 24) & mantissaMultiplication) > 0 ? 1 : 0;
            Console.WriteLine();
            Console.WriteLine("========================================================");

            //Normalization check(if it need):
            if (expoAddition > 0)
            {
                Console.WriteLine("Normalization:");
                mantissaMultiplication >>= 1;
                mantissaMultiplication &= ~(1 << 23);
            }
            else
            {
                Console.WriteLine("Normalization is not needed:");
                mantissaMultiplication &= ~(3 << 23);
            }
            Console.WriteLine();
            Console.WriteLine(FinishStringWithZeros(Convert.ToString(mantissaMultiplication, 2), 23));
            Console.WriteLine();
            Console.WriteLine("========================================================");

            //Sign chech by XOR:
            int sign = 1 & (sign1 + sign2); //Logic XOR
            Console.WriteLine("Sign:");
            Console.WriteLine(sign1.ToString() + " XOR " + sign2 + " = " + sign);
            Console.WriteLine();
            Console.WriteLine("========================================================");

            //Exponent check:
            Console.WriteLine("Exponent: ");
            Console.WriteLine("exponent1      " + FinishStringWithZeros(Convert.ToString(expo1, 2), 8) + " ( " + expo1 + " ) \n             +");
            Console.WriteLine("exponent2      " + FinishStringWithZeros(Convert.ToString(expo2, 2), 8) + " ( " + expo2 + " )");
            int expo = expo1 + expo2 - 127 + expoAddition;
            Console.WriteLine("               - 127 + " + expoAddition + "\n             = " + FinishStringWithZeros(Convert.ToString(expo, 2), 8) + " ( " + expo + " ) ");
            int resultMask = (int)mantissaMultiplication;
            resultMask |= expo << 23;
            resultMask |= sign << 31;
            Console.WriteLine("========================================================");

            //Calculate Result:
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(resultMask & 255);
            bytes[1] = (byte)((resultMask >> 8) & 255);
            bytes[2] = (byte)((resultMask >> 16) & 255);
            bytes[3] = (byte)((resultMask >> 24) & 255);
            float result = BitConverter.ToSingle(bytes, 0);
            Console.WriteLine("Result: " + FinishStringWithZeros(Convert.ToString(resultMask, 2), 32) + " ( " + result + " ) ");
            Console.WriteLine("========================================================");
        }

        private int GetFloatBits(float num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            int res = 0;
            res |= bytes[0];
            res |= bytes[1] << 8;
            res |= bytes[2] << 16;
            res |= bytes[3] << 24;
            return res;
        }

        private string FinishStringWithZeros(string val, int bitcount)
        {
            int count = bitcount - val.Length;
            string head = "";
            for (int i = 0; i < count; ++i)
                head += "0";
            return head + val;
        }
    }
}