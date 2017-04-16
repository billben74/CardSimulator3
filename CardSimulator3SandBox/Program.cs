using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CardSimulator3ClassLibrary
{
    class Program
    {
        static void Main(string[] args)
        {

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
    }
}
