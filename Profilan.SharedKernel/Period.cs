using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profilan.SharedKernel
{
    public class Period : ValueObject<Period>
    {
        public int Amount { get; set; }
        public Unit Unit { get; set; }

        public Period(int amount, Unit unit)
        {
            Amount = amount;
            Unit = unit;
        }

        public Period() { }

        protected override bool EqualsCore(Period other)
        {
            return (Amount == other.Amount && Unit == other.Unit);
        }
    }
}
