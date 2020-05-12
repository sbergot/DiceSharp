# DiceScript

A small dice scripting language.

## Basic dice commands

Roll 1 D6: `roll D6` or `roll 1D6`  
Roll 2 D10: `roll 2D10`  
Roll 2 D6 and add 3: `roll 2D6+3`  
Roll 2 D6 and substract 2: `roll 2D6-2`  
Roll 3 explosives D8: `roll 3D8(exp)`

An exploding dices is rerolled when it lands one its maximum value.

## Filtering, ranking and aggregating

Roll 3 D6 and keep the ones above 4: `roll 3D6(>4)`  
Roll 3 D6 and count the ones above 4: `roll 3D6(>4,count)`

`>4` is a filter. Other filters are `<` and `=`.

`count` is an aggregation. The other aggregations are `sum`, `max` and `min`.
The default aggregation is `sum`.

Roll 4 D6 and keep the highest one: `roll 4D6(top1)`. Use `bot` to keep the
lowest dices.

## Variables

Roll 1 D8 and 2 D6 and add them together:

```
int $x <- roll D8;
roll 2D6+$x;
```

All variables must start with `$`. The rest of the name is composed of letters,
numbers and underscore `_`. Statements must be separated by a `;`. The `int`
keywords indicate that the variable stores an integer.

They can be used to determine the number of faces of a dice roll: `roll 3D$x`

Or they can be used to specify the number of dices to roll. In this case a `$`
must be used to separate the variable from the rest of the dice declaration:
`roll $x$D6`.

Variables can also be used in options such as filters and rankings: `roll 5D6(>$x)`.

## Match statements

You can write a match statement to introduce an interpretation of a given
results. Let's say that you are rolling 5D6. If you get at least 2 dice above 2
then it is considered as a success. You want to include this interpretation in
the function.

```
int $result <- roll 5D6(>2,count);
match $result ((<3; "failure"), (default; "success"))
```

A match statement takes a value and checks it against a series of filters. The
quoted text associated with first filter that accepts the value is returned. The
`default` keyword is a special filter only useable in a match statement. It
accepts any value.

## Calc expression

You may sometimes want to perform small calculations in a script. You may use a
calc expression for this:

```
int $a <- calc 1 + 2;
calc $a * 2
```

Only a single operation may be performed on a given line. The available
operations are `+`, `-` and `*`. The result may be assigned to an `int`. The
calc expression may be named.

## Dice variables and aggregation expressions

Sometimes you want to implement more complexe rules on a dice roll. For exemple
you want to count dice above 5 as successes and dice equal to 1 as failures. The
result of your roll is successes minus failures. You need to count two different
sets of dice. You can achieve this with dicescript thanks to dice variables and
aggregation expressions:

```
dice $a<- roll 8D6 (exp);
int $successes <- aggregate $a (>4, count);
int $failures <- aggregate $a (<3, count);
calc $successes - $failures
```

The `dice` keyword indicates that the set of dice is stored into `$a` instead of
a single integer. The two following statements are aggregations. They use the
same set of options than the dice expression.

The aggregation expression may also be named.

## Named expressions

A roll can be named using a quoted string. This allows you to add information
about the roll.

```
roll "damage roll" 2D10
```

`calc` and `aggregate` expressions can also be named.

```
int $successes <- aggregate "successes" $a (>4, count);
calc "critical factor" $a * 2
```

## Macros

It is possible to define a macro with the following syntax:

```
macro my_roll($x) {
    roll 2D6+$x;
}
```

A macro name follows the same rules as the variables (letters, digits,
underscores). A macro can also be defined by invoking another one using a
specific `apply` syntax:

```
macro multipledice($faces, $bonus) {
    roll 2D$faces+$bonus;
}

macro specialized($bonus) <- apply multipledice(4, $bonus)
```
