// See https://aka.ms/new-console-template for more information

using Microsoft.VisualStudio.TestTools.UnitTesting;

Console.WriteLine("Hello, World!");



var input = File.ReadAllText("input.txt");
//var input = File.ReadAllText("sample.txt");


HandType WhatHand(string input)
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

Assert.AreEqual(HandType.FiveOfAKind, WhatHand("AAAAA"));
Assert.AreEqual(HandType.FourOfAKind, WhatHand("AA8AA"));
Assert.AreEqual(HandType.FullHouse, WhatHand("23332"));
Assert.AreEqual(HandType.ThreeOfAKind, WhatHand("TTT98"));
Assert.AreEqual(HandType.TwoPair, WhatHand("23432"));
Assert.AreEqual(HandType.OnePair, WhatHand("A23A4"));
Assert.AreEqual(HandType.HighCard, WhatHand("23456"));


enum HandType
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
