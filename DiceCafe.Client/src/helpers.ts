import { cardTypeMap } from "./constants";

export function count(values: string[]): Record<string, number> {
  const res = {} as Record<string, number>;
  values.forEach((v) => {
    if (res[v] === undefined) {
      res[v] = 0;
    }
    res[v] += 1;
  });
  return res;
}

export function getFlockableTypes(cards: Card[]): TypeCount[] {
  const countedTypes = count(cards.map((c) => c.type)) as Record<
    BirdType,
    number
  >;
  const res = [] as TypeCount[];
  Object.keys(countedTypes).forEach((t) => {
    const birdType = t as BirdType;
    if (countedTypes[birdType] >= cardTypeMap[birdType].smallFlock) {
      res.push({ type: birdType, amount: countedTypes[birdType] });
    }
  });
  return res;
}

export function getFuzzyHandNumberDescription(i: number): string {
  return i > 8 ? "8+" : i.toString();
}

export function getFuzzyDeckNumberDescription(i: number): string {
  if (i <= 10) {
    return i.toString();
  }
  return (Math.floor(i / 10) * 10).toString() + "+";
}

export function sortCards(cards: Card[]): Card[] {
  return cards.concat().sort(function (a, b) {
    if (a.type < b.type) {
      return -1;
    }
    if (a.type > b.type) {
      return 1;
    }
    return 0;
  });
}

export function getColorBgClass(color: Color): string {
  return `bg-${color.toLowerCase()}-400`;
}
