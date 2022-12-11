using System.Net.Sockets;
namespace AdventOfCode2022
{
    class Monkey
    {
        public static List<Monkey> allMonkeys = new();
        protected int index = 0;

        Queue<long> items;

        public int divisor {get; protected set;} = 0;
        protected int trueTarget = 0;
        protected int falseTarget = 0;

        protected char operatorSymbol;
        protected long operand = 0;
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

            items = new(itemString.Split(": ")[1].Split(", ").Select(x => long.Parse(x)));

            string[] operationParts = operationString.Split(" = ")[1].Split(' ');
            operatorSymbol = operationParts[1][0];
            if (operationParts[2] == "old") {
                useOldValue = true;
            } else {
                operand = int.Parse(operationParts[2]);
            }
            
            divisor = int.Parse(testString.Split(' ').Last());
            trueTarget = int.Parse(trueString.Split(' ').Last());
            falseTarget = int.Parse(falseString.Split(' ').Last());
        }

        public long ApplyOperation(long value)
        {
            timesInspected ++;

            if (useOldValue) {
                operand = value;
            }

            if (operatorSymbol == '+') {
                return value + operand;
            }

            return value * operand;
        }

        public bool TestCondition(long value) {
            return value % divisor == 0;
        }

        public void PerformThrow(long item, Func<long, long> reduce)
        {
            item = reduce(ApplyOperation(item));
            if (TestCondition(item)) {
                allMonkeys[trueTarget].AddItem(item);
            } else {
                allMonkeys[falseTarget].AddItem(item);
            }
        }

        public void TakeTurn(Func<long, long> reduce)
        {
            while (items.Count() > 0) {
                PerformThrow(items.Dequeue(), reduce);
            }
        }

        public void AddItem(long item) 
        {
            items.Enqueue(item);
        }

        public void LogDescription()
        {
            Console.WriteLine(string.Format(
                "Monkey {0} carrying {1} items: [{2}]. Inspection count: {3}", 
                index, 
                items.Count(),
                string.Join(',', items.Select(x => x.ToString())),
                timesInspected
            ));
        }

        public static void ClearMonkeyList()
        {
            allMonkeys.Clear();
        }
    }
}