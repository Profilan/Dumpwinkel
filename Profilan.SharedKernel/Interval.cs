using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profilan.SharedKernel
{
    public class Interval : ValueObject<Interval>
    {
        public int Amount { get; private set; }
        public Unit Unit { get; private set; }
        public int Seconds {
            get
            {
                int seconds = 0;

                switch (Unit)
                {
                    case Unit.Hours:
                        seconds = 3600 * Amount;
                        break;
                    case Unit.Minutes:
                        seconds = 60 * Amount;
                        break;
                    case Unit.Seconds:
                        seconds = Amount;
                        break;
                }

                return seconds;
            }
            private set
            {

            }
        }
        public int Minutes
        {
            get
            {
                int minutes = 0;

                switch (Unit)
                {
                    case Unit.Hours:
                        minutes = Amount * 60;
                        break;
                    case Unit.Minutes:
                        minutes = Amount;
                        break;
                    case Unit.Seconds:
                        minutes = Amount / 60;
                        break;
                }

                return minutes;
            }
            private set
            {

            }
        }

        public Interval(int amount, Unit unit)
        {
            if (amount < 0)
            {
                throw new InvalidOperationException("Amount cannot be negative");
            }

            Amount = amount;
            Unit = unit;
        }

        protected override bool EqualsCore(Interval other)
        {
            return (Amount == other.Amount && Unit == other.Unit);
        }
    }
}
