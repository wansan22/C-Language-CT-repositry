using System;

public class Program
{
    public class Complex
    {
        public double real;
        public double mnimaya;

        public Complex(double real, double mnimaya)
        {
            this.real = real;
            this.mnimaya = mnimaya;
        }

        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.real + b.real, a.mnimaya + b.mnimaya);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.real - b.real, a.mnimaya - b.mnimaya);
        }


        public static Complex operator *(Complex a, Complex b)
        {
            double resultReal = (a.real * b.real) - (a.mnimaya * b.mnimaya);
            double resultMnimaya = (a.real * b.mnimaya) + (a.mnimaya * b.real);
            return new Complex(resultReal, resultMnimaya);
        }

       
        public static Complex operator /(Complex a, Complex b)
        {
            double denominator = (b.real * b.real) + (b.mnimaya * b.mnimaya);

            if (denominator == 0)
            {
                throw new DivideByZeroException("Деление на комплексный ноль невозможно.");
            }

            double resultReal = (a.real * b.real + a.mnimaya * b.mnimaya) / denominator;
            double resultMnimaya = (a.mnimaya * b.real - a.real * b.mnimaya) / denominator;

            return new Complex(resultReal, resultMnimaya);
        }

        public static bool operator ==(Complex a, Complex b)
        {
      
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;

            return a.real == b.real && a.mnimaya == b.mnimaya;
        }

        public static bool operator !=(Complex a, Complex b)
        {
            return !(a == b);
        }

     
        public override bool Equals(object obj)

        {
            
            return obj is Complex other && this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(real, mnimaya);
        }

       
        public override string ToString()
        {
           
            if (mnimaya < 0)
            {
                return $"{real} - {Math.Abs(mnimaya)}i";
            }

            return $"{real} + {mnimaya}i";
        }
    }

    private static void Main()
    {
        Complex z1 = new Complex(5, 7);
        Complex z2 = new Complex(2, -3);

    }
}