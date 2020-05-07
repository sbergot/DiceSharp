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

interface RollResult {
  description: RollDescription;
  dices: Dice[];
  result: number;
  name: string;
}

interface PrintResult {
  value: string;
}

type Result = RollResult | PrintResult;

type ResultType = "Roll" | "Print";

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
  | ResultModelG<PrintResult, "Print">;

interface FunctionSpec {
  name: string;
  arguments: string[];
}
