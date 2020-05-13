interface Dice {
  faces: number;
  result: number;
  valid: boolean;
}

interface RollDescription {
  faces: number;
  number: number;
  bonus: number;
  exploding: boolean;
}

interface ValueResult {
  result: number;
  name: string;
}

interface RollResult extends ValueResult {
  description: RollDescription;
  dices: Dice[];
}

interface PrintResult {
  value: string;
}

interface DiceResult {
  dices: Dice[];
  name: string;
  description: RollDescription;
}

type Result = RollResult | PrintResult | ValueResult | DiceResult;

type ResultType = "Roll" | "Print" | "Value" | "Dice";

interface ResultGroup {
  user: User;
  created: string;
  results: TaggedResult[];
}

interface ResultModelG<TRes extends Result, TResType extends ResultType> {
  resultType: TResType;
  result: TRes;
}

type TaggedResult =
  | ResultModelG<RollResult, "Roll">
  | ResultModelG<PrintResult, "Print">
  | ResultModelG<ValueResult, "Value">
  | ResultModelG<DiceResult, "Dice">;

interface FunctionSpec {
  name: string;
  arguments: string[];
}
