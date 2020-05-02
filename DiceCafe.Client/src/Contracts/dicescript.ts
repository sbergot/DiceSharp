interface Dice {
  faces: number;
  result: number;
  valid: boolean;
}

interface RollResult {
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
