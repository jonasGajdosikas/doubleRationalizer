using System;

namespace doubleRationalizer
{
    internal class Program
    {
        const double epsilon = 0.0000000001;
        const int maxDen = int.MaxValue / 2;
        static void Main()
        {
            frac f1 = new(1, 2);
            frac f2 = new(2, 4);
            //int n1  = 0, n2 = 0, d1 = 0, d2 = 0;
            double phi = (Math.Sqrt(5) + 1) / 2f;
            Rationalize(phi, ref f1, ref f2);
            Console.ReadKey();
        }
        static void Rationalize(double number, ref frac lowerBound, ref frac upperBound)
        {
            
            lowerBound = new ((int)Math.Floor(number), 1);
            upperBound = new ((int)Math.Ceiling(number), 1);
            int count, bestcount;
            frac keptFrac, bestFrac;
            double err1 = number - lowerBound.val;
            double err2 = upperBound.val - number;
            keptFrac = (err1 < err2) ? lowerBound : upperBound;
            bestFrac = keptFrac;
            count = 1;
            bestcount = count;
            double err = Math.Min(err1, err2);
            while ( err > epsilon && lowerBound.den < maxDen && upperBound.den < maxDen)
            {
                frac newFrac = frac.FracBetween(lowerBound, upperBound);
                if (number > newFrac.val)
                {
                    lowerBound = newFrac;
                    if (keptFrac == upperBound)
                    {
                        count++;
                        if (count > bestcount)
                        {
                            bestcount = count;
                            bestFrac = keptFrac;
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0} was the better fraction for {1} iterations", keptFrac, count);
                        count = 1;
                        keptFrac = upperBound;
                    }
                } else
                {
                    upperBound = newFrac;
                    if (keptFrac == lowerBound)
                    {
                        count++;
                        if (count > bestcount)
                        {
                            bestcount = count;
                            bestFrac = keptFrac;
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0} was the better fraction for {1} iterations", keptFrac, count);
                        count = 1;
                        keptFrac = lowerBound;
                    }
                }
                err = Math.Min(number - lowerBound.val, upperBound.val - number);
            }/**/
            Console.WriteLine("{0} was the best approximation overall", bestFrac);
        }
    }
#pragma warning disable IDE1006 // Naming Styles
    class frac
#pragma warning restore IDE1006 // Naming Styles
    {
        public int num;
        public int den;
#pragma warning disable IDE1006 // Naming Styles
        public double val => num / (double)den;
#pragma warning restore IDE1006 // Naming Styles
        public frac(int num, int den)
        {
            if (den == 0) throw new ArgumentException("denominator can't be zero");
            if (den < 0)
            {
                this.num = -num;
                this.den = -den;
            }
            else
            {
                this.num = num;
                this.den = den;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        public frac simplified
#pragma warning restore IDE1006 // Naming Styles
        {
            get
            {
                int a = Math.Max(Math.Abs(num), (int)den);
                int b = Math.Min(Math.Abs(num), (int)den);
                while (b > 0)
                {
                    int t = a % b;
                    a = b;
                    b = t;
                }
                return new frac(num / a, den / a);
            } 
        }
        public static frac FracBetween(frac frac1, frac frac2)
        {
            return new frac(frac1.num + frac2.num, frac1.den + frac2.den);
        }
        #region [operators]
        public override bool Equals(object obj) => this.Equals(obj as frac);
        public bool Equals(frac p)
        {
            if (p is null) return false;
            if (Object.ReferenceEquals(p, this)) return true;
            if (this.GetType() != p.GetType()) return false;
            return (this.simplified.num == p.simplified.num && this.simplified.den == p.simplified.den);
        }
        public override int GetHashCode() => val.GetHashCode();
        public static bool operator ==(frac left, frac right)
        {
            if (left is null)
            {
                if (right is null) return true;
                return false;
            }
            return left.Equals(right);
        }
        public static bool operator !=(frac left, frac right) => !(left == right);
        public override string ToString()
        {
            return num.ToString() + "/" + den.ToString();
        }
        #endregion
    }
}
