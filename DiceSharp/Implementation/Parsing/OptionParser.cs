using System;
using System.Collections.Generic;
using System.Linq;

using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal static class OptionParser
    {
        public static Parser<char, OptionGroup> OptionGroup
        {
            get
            {
                return Option
                    .Separated(Char(',').Then(SkipWhitespaces))
                    .Select(opts => new OptionGroup { Options = opts.ToList() })
                    .Between(Char('('), Char(')'));
            }
        }

        private static Parser<char, Option> Option
        {
            get
            {
                var explodingOption = String("exp").ThenReturn(new ExplodingOption() as Option);
                return OneOf(
                    Try(Filter).Cast<Option>(),
                    Try(Ranking).Cast<Option>(),
                    Try(explodingOption),
                    Try(Aggregate).Cast<Option>());
            }
        }

        public static Parser<char, FilterOption> Filter
        {
            get
            {
                var filterTypes = new Dictionary<string, FilterType>
                {
                    { "=", FilterType.Equal },
                    { ">", FilterType.Larger },
                    { "<", FilterType.Smaller },
                };
                var type = OneOf(filterTypes.Keys.Select(String)).Select(s => filterTypes[s]);
                return Map(
                    (t, n) => new FilterOption { Type = t, Scalar = n },
                    type,
                    BaseParser.Scalar);
            }
        }

        public static Parser<char, RankingOption> Ranking
        {
            get
            {
                var rankingTypes = new Dictionary<string, RankingType>
                {
                    { "bot", RankingType.Bottom },
                    { "top", RankingType.Top },
                };
                var type = OneOf(rankingTypes.Keys.Select(s => Try(String(s))))
                    .Select(s => rankingTypes[s]);
                return Map(
                    (t, n) => new RankingOption { Type = t, Scalar = n },
                    type,
                    BaseParser.Scalar);
            }
        }

        private static Parser<char, AggregateOption> Aggregate
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
                var type = OneOf(aggreationTypes.Keys.Select(s => Try(String(s))))
                    .Select(s => aggreationTypes[s]);
                return type.Select(t => new AggregateOption { Type = t });
            }
        }
    }
}