using System.Numerics;
namespace AdventOfCode2022
{
    public class MathMonkey
    {
        protected string name;
        public static Dictionary<string, MathMonkey> monkeyLookup = new();
        public string leftMonkey = "";
        public string rightMonkey = "";
        public char operation;
        public Decimal value;
        
        public MathMonkey(string name, long value)
        {
            monkeyLookup[name] = this;
            this.name = name;
            this.value = value;
        }
        public MathMonkey(string name, string leftMonkey, string rightMonkey, char operation)
        {
            monkeyLookup[name] = this;
            this.name = name;
            this.leftMonkey = leftMonkey;
            this.rightMonkey = rightMonkey;
            this.operation = operation;
        }

        public Decimal GetValue()
        {
            if (leftMonkey != "" && rightMonkey != "") {
                switch(operation) {
                    case '+':
                        return checked(monkeyLookup[leftMonkey].GetValue() + monkeyLookup[rightMonkey].GetValue());
                    case '-':
                        return checked(monkeyLookup[leftMonkey].GetValue() - monkeyLookup[rightMonkey].GetValue());
                    case '/':
                        return checked(monkeyLookup[leftMonkey].GetValue() / monkeyLookup[rightMonkey].GetValue());
                    case '*':
                        return checked(monkeyLookup[leftMonkey].GetValue() * monkeyLookup[rightMonkey].GetValue());
                }
            }

            return value;
        }

        public bool HasDescendent(string name)
        {
            if (leftMonkey == name || rightMonkey == name) {
                return true;
            }
            if (leftMonkey == "" && rightMonkey == "") {
                return false;
            }
            return monkeyLookup[leftMonkey].HasDescendent(name) || monkeyLookup[rightMonkey].HasDescendent(name);
        }

        public bool HasLeftDescendent(string name) {
            if (leftMonkey == name) {
                return true;
            }
            if (leftMonkey == "") {
                return false;
            }
            return monkeyLookup[leftMonkey].HasDescendent(name);
        }

        public bool HasRightDescendent(string name) {
            if (rightMonkey == name) {
                return true;
            }
            if (rightMonkey == "") {
                return false;
            }
            return monkeyLookup[rightMonkey].HasDescendent(name);
        }

        public Decimal GetLeftValue()
        {
            return monkeyLookup[leftMonkey].GetValue();
        }

        public Decimal GetRightValue()
        {
            return monkeyLookup[rightMonkey].GetValue();
        }

        public void PrintSelf()
        {
            if (leftMonkey != "" && rightMonkey != "") {
                Console.WriteLine(string.Format("{0}: {1} {2} {3}", name, leftMonkey, operation, rightMonkey));
            } else {
                Console.WriteLine(string.Format("{0}: {1}", name, value));
            }
        }
    }
}