using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CardSimulator3ClassLibrary
{
    class Program
    {
        //class hello {} ;
        //private int <int>hello;

        static void Main(string[] args)
        {

            IEnumerable<char> query = "Not what you might expect";
            string vowels = "aeiou";

            for (int i = 0; i < vowels.Length; i++)
            {
                char vowel = vowels[i];
                query = query.Where(c => c != vowel);
            }
            foreach (char c in query) Console.Write(c);



                foreach (Suit suit in (Enum.GetValues(typeof(Suit)) as IEnumerable<Suit>).Reverse())
                {
                    foreach (FaceValue fv in (Enum.GetValues(typeof(FaceValue)) as IEnumerable<FaceValue>).Reverse())
                    {
                        Console.WriteLine(suit);
                        Console.WriteLine(fv);
                    }
                }
            char[] apple = { 'a', 'p', 'p', 'l', 'e' };

            char[] reversed = apple.Reverse().ToArray();

            foreach (char chr in reversed)
            {
                Console.Write(chr + " ");
            }
            Console.WriteLine();

            Array thing = Enum.GetValues(typeof(Suit));
            Type type = typeof(Suit);

            var thing2 = (thing as IEnumerable<Suit>).Reverse();
            foreach (Suit rev in thing2) {
                Console.WriteLine(rev.ToString());
            }

            

        }

        public static void Consumer()
        {
            foreach (int i in Integers())
            {
                Console.WriteLine(i.ToString());
            }
        }

        public static IEnumerable<int> Integers()
        {
            yield return 1;
            yield return 2;
            yield return 4;
            yield return 8;
            yield return 16;
            yield return 16777216;
        }
    }
    
}
