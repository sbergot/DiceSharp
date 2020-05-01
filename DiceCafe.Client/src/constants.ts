export const allCardTypes: CardType[] = [
  {
    birdType: "Flamingo",
    smallFlock: 2,
    bigFlock: 3,
    amount: 7
  },
  {
    birdType: "Owl",
    smallFlock: 3,
    bigFlock: 4,
    amount: 10
  },
  {
    birdType: "Toucan",
    smallFlock: 3,
    bigFlock: 4,
    amount: 10
  },
  {
    birdType: "Duck",
    smallFlock: 4,
    bigFlock: 6,
    amount: 13
  },
  {
    birdType: "Parrot",
    smallFlock: 4,
    bigFlock: 6,
    amount: 13
  },
  {
    birdType: "Magpie",
    smallFlock: 5,
    bigFlock: 7,
    amount: 17
  },
  {
    birdType: "ReedWarbler",
    smallFlock: 6,
    bigFlock: 9,
    amount: 20
  },
  {
    birdType: "Robin",
    smallFlock: 6,
    bigFlock: 9,
    amount: 20
  }
];

function index<TKey extends string, TVal>(
  values: TVal[],
  keySelector: (key: TVal) => TKey
) {
  const res = {} as Record<TKey, TVal>;
  values.forEach(v => {
    res[keySelector(v)] = v;
  });
  return res;
}

export const cardTypeMap: Record<BirdType, CardType> = index(
  allCardTypes,
  t => t.birdType
);
