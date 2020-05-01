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

interface FunctionSpec {
  name: string;
  arguments: string[];
}
