using System;
using System.Collections.Generic;
using System.Linq;

using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal class OptionParser
    {
        public static Parser<char, OptionGroupExpression> OptionGroup
        {
            get
            {
                return Option
                    .Separated(Char(',').Then(SkipWhitespaces))
                    .Select(opts => new OptionGroupExpression { Options = opts.ToList() })
                    .Between(Char('('), Char(')'));
            }
        }

        private static Parser<char, OptionExpression> Option
        {
            get
            {
                return OneOf(
                    Try(Filter).Cast<OptionExpression>(),
                    Try(Aggregate).Cast<OptionExpression>());
            }
        }

        private static Parser<char, FilterExpression> Filter
        {
            get
            {
                var filterTypes = new Dictionary<string, FilterType>
                {
                    { "bot", FilterType.Bottom },
                    { "=", FilterType.Equal },
                    { ">", FilterType.Larger },
                    { "<", FilterType.Smaller },
                    { "top", FilterType.Top },
                };
                var type = OneOf(filterTypes.Keys.Select(String)).Select(s => filterTypes[s]);
                return Map(
                    (t, n) => new FilterExpression { Type = t, Scalar = n },
                    type,
                    BaseParser.Number);
            }
        }

        private static Parser<char, AggregateExpression> Aggregate
        {
            get
            {
                var aggreationTypes = new Dictionary<string, AggregationType>
                {
                    { "sum", AggregationType.Sum },
                    { "count", AggregationType.Count },
                    { "max", AggregationType.Max },
                    { "min", AggregationType.Min },
                };
                var type = OneOf(aggreationTypes.Keys.Select(s => Try(String(s)))).Select(s => aggreationTypes[s]);
                return type.Select(t => new AggregateExpression { Type = t });
            }
        }
    }
}