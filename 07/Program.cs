// See https://aka.ms/new-console-template for more information

using Microsoft.VisualStudio.TestTools.UnitTesting;

Console.WriteLine("Hello, World!");



var input = File.ReadAllText("input.txt");
//var input = File.ReadAllText("sample.txt");



Assert.AreEqual(HandType.FiveOfAKind, HandComparer.WhatHand("AAAAA"));
Assert.AreEqual(HandType.FourOfAKind, HandComparer.WhatHand("AA8AA"));
Assert.AreEqual(HandType.FullHouse, HandComparer.WhatHand("23332"));
Assert.AreEqual(HandType.ThreeOfAKind, HandComparer.WhatHand("TTT98"));
Assert.AreEqual(HandType.TwoPair, HandComparer.WhatHand("23432"));
Assert.AreEqual(HandType.OnePair, HandComparer.WhatHand("A23A4"));
Assert.AreEqual(HandType.HighCard, HandComparer.WhatHand("23456"));

var data = new List<(string hand, int bid)>();

foreach (var line in input.Split("\r\n"))
{
    var tmp = line.Split(' ');
    data.Add((tmp[0], int.Parse(tmp[1])));
}

int total = 0;
var ordered = data.OrderBy(x => x.hand, new HandComparer()).ToList();
for (int rank = 1; rank <= ordered.Count; rank++)
{
    total += rank * ordered[rank - 1].bid;
}

Console.WriteLine($"Total: {total}");

int total2 = 0;
var ordered2 = data.OrderBy(x => x.hand, new HandComparer2()).ToList();
for (int rank = 1; rank <= ordered.Count; rank++)
{
    total2 += rank * ordered2[rank - 1].bid;
}

Console.WriteLine($"Total2: {total2}");

public enum HandType
{
    FiveOfAKind = 7,
    FourOfAKind = 6,
    FullHouse = 5,
    ThreeOfAKind = 4,
    TwoPair = 3,
    OnePair = 2,
    HighCard = 1,
    None = 0
};

public class HandComparer : IComparer<string>
{
    public static HandType WhatHand(string input)
    {
        var result = input.ToList().GroupBy(x => x)
                            .Select(group => new
                            {
                                Metric = group.Key,
                                Count = group.Count()
                            })
                            .OrderByDescending(x => x.Count)
                            .ToArray();

        if (result[0].Count == 5)
        {
            return HandType.FiveOfAKind;
        }
        else if (result[0].Count == 4)
        {
            return HandType.FourOfAKind;
        }
        else if (result[0].Count == 3 && result[1].Count == 2)
        {
            return HandType.FullHouse;
        }
        else if (result[0].Count == 3)
        {
            return HandType.ThreeOfAKind;
        }
        else if (result[0].Count == 2 && result[1].Count == 2)
        {
            return HandType.TwoPair;
        }
        else if (result[0].Count == 2)
        {
            return HandType.OnePair;
        }
        else if (result.Length == 5)
        {
            return HandType.HighCard;
        }
        return HandType.None;
    }

    int Strength(char c)
    {
        switch (c)
        {
            case 'A':
                return 14;
            case 'K':
                return 13;
            case 'Q':
                return 12;
            case 'J':
                return 11;
            case 'T':
                return 10;
            default:
                return int.Parse(c.ToString());
        }
    }


    public int Compare(string? x, string? y)
    {
        var t1 = (int)WhatHand(x);
        var t2 = (int)WhatHand(y);

        if (t1 == t2)
        {
            for (var i = 0; i < 5; i++)
            {
                t1 = Strength(x[i]);
                t2 = Strength(y[i]);
                if (t1 != t2)
                {
                    return t1.CompareTo(t2);
                }
            }
            return 0;
        }

        return t1.CompareTo(t2);
    }
}

public class HandComparer2 : IComparer<string>
{
    public static string cardOrder = "AKQT98765432J";
    public static HandType BestHand(string input)
    {
        var result = new List<(string hand, HandType handType)>();

        foreach (var card in cardOrder.ToArray())
        {
            var hand = input.Replace('J', card);
            result.Add((hand, HandComparer.WhatHand(hand)));
        }
        return result.MaxBy(x => x.handType).handType;
    }

    int Strength2(char c)
    {
        switch (c)
        {
            case 'A':
                return 14;
            case 'K':
                return 13;
            case 'Q':
                return 12;
            case 'J':
                return 1;
            case 'T':
                return 10;
            default:
                return int.Parse(c.ToString());
        }
    }

    public int Compare(string? x, string? y)
    {
        var t1 = (int)BestHand(x);
        var t2 = (int)BestHand(y);

        if (t1 == t2)
        {
            for (var i = 0; i < 5; i++)
            {
                t1 = Strength2(x[i]);
                t2 = Strength2(y[i]);
                if (t1 != t2)
                {
                    return t1.CompareTo(t2);
                }
            }
            return 0;
        }

        return t1.CompareTo(t2);
    }
}
