# DiceSharp

a small dice runner library

## Basic dice commands

Roll 1 D6: `roll D6` or `roll 1D6`  
Roll 2 D10: `roll 2D10`  
Roll 2 D6 and add 3: `roll 2D6+3`  
Roll 2 D6 and substract 2: `roll 2D6-2`  
Roll 3 explosives D8: `roll 3D8(exp)`

An exploding dices is rerolled when it lands one its maximum value.

## Naming rolls

A roll can be named using a quoted string:

```
roll "my roll" 2D10
```

## Filtering and aggregating

Roll 3 D6 and keep the ones above 4: `roll 3D6(>4)`  
Roll 3 D6 and count the ones above 4: `roll 3D6(>4,count)`

`>{number}` is a filter. Other filters are `<` and `=`.

`count` is an aggregation. The other aggregations are `sum`, `max` and `min`. The default aggregation is `sum`.

## Variables

Roll 1 D8 and 2 D6 and add them together:

```
var $x <- roll D8;
roll 2D6+$x;
```

All variables must start with `$`. The rest of the name is composed of letters, numbers and underscore `_`.

They can be used to determine the number of faces of a dice roll: `roll 3D$x`

Or they can be used to specify the number of dices to roll. In this case a `$` must be used to separate the variable from the rest of the dice declaration: `roll $x$D6`.

Statements must be separated by a `;`.

## Functions

It is possible to define function with the following syntax:

```
function my_roll($x) {
    roll 2D6+$x;
}
```

A function can also be defined by invoquing another one using a specific syntax:

```
function multipledice($faces, $bonus) {
    roll 2D$faces+$bonus;
}

function specialized($bonus) <- apply multipledice(4, $bonus)
```
