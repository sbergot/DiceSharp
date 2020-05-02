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

interface ResultModelG<TRes extends Result, TResType extends ResultType> {
  user: User;
  created: string;
  resultType: TResType;
  result: TRes;
}

type ResultModel =
  | ResultModelG<RollResult, "Roll">
  | ResultModelG<PrintResult, "Print">;

interface FunctionSpec {
  name: string;
  arguments: string[];
}
