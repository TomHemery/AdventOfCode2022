using System.Net.Sockets;
namespace AdventOfCode2022
{
    class Monkey
    {
        public static List<Monkey> allMonkeys = new();
        protected int index = 0;

        Queue<int> items;

        protected int testValue = 0;
        protected int trueTarget = 0;
        protected int falseTarget = 0;

        protected char operatorSymbol;
        protected int operand = 0;
        protected bool useOldValue = false;

        public int timesInspected {get; private set;} = 0;

        public Monkey(
            string itemString, 
            string operationString, 
            string testString, 
            string trueString, 
            string falseString
        ) {            
            allMonkeys.Add(this);
            index = allMonkeys.Count() - 1;

            items = new(itemString.Split(": ")[1].Split(", ").Select(x => int.Parse(x)));

            string[] operationParts = operationString.Split(" = ")[1].Split(' ');
            operatorSymbol = operationParts[1][0];
            if (operationParts[2] == "old") {
                useOldValue = true;
            } else {
                operand = int.Parse(operationParts[2]);
            }
            
            testValue = int.Parse(testString.Split(' ').Last());
            trueTarget = int.Parse(trueString.Split(' ').Last());
            falseTarget = int.Parse(falseString.Split(' ').Last());
        }

        public int ApplyOperation(int value)
        {
            timesInspected ++;
            float result;

            if (useOldValue) {
                operand = value;
            }

            if (operatorSymbol == '+') {
                result = value + operand;
            } else {
                result = value * operand;
            }

            return (int)Math.Floor(result / 3);
        }

        public bool TestCondition(int value) {
            return value % testValue == 0;
        }

        public void PerformThrow(int item)
        {
            item = ApplyOperation(item);
            if (TestCondition(item)) {
                allMonkeys[trueTarget].AddItem(item);
            } else {
                allMonkeys[falseTarget].AddItem(item);
            }
        }

        public void TakeTurn()
        {
            while (items.Count() > 0) {
                PerformThrow(items.Dequeue());
            }
        }

        public void AddItem(int item) 
        {
            items.Enqueue(item);
        }

        public void LogDescription()
        {
            Console.WriteLine(string.Format(
                "Monkey {0} carrying {1} items: [{2}]", 
                index, 
                items.Count(),
                string.Join(',', items.Select(x => x.ToString()))
            ));
        }

        public static void ClearMonkeyList()
        {
            allMonkeys.Clear();
        }
    }
}