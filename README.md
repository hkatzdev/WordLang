# WordLang
 A weird programming language based on words.

## Background

WordLang is an ...

## The Challenge

To create an understandable sentence that compiles to a valid output.

## Grammar

| Character     | Meaning                                                      |
| ------------- | ------------------------------------------------------------ |
| Any character | Increment or decrement the data value by it's corresponding ASCII value. |
| ,             | Toggle from increment to decrement mode, or wise versa.      |
| .             | Print the corresponding data value's ASCII character to screen. And reset the value to 0. |
| '[variable]'  | Add the value of *variable* to the data value.               |
| \>[variable]  | Set the value of *variable* to the current data value.       |
| <             | Read a one character input and add it to the data value.     |
| [name]!       | Jump back to the named point.                                |
| - [name]      | Create a named point that is possible to jump back to.       |
| "[comment]"   | Comment.                                                     |
| [space/tab]   | Ignored.                                                     |
| \\[escaped]   | Any special character is escaped using `\`.                  |

## Other

| Subject         | Description |
| --------------- | ----------- |
| File extensions | .w, .wl     |

## Example

### 1. Hello world

```
Helloo, world. :3
```

1. H = 72 → Increment the data value to 72
2. e = 101 → Increment the data value to 173
3. ...
4. , → Switch to decrement mode.
5. Space is ignored
6. W = 87 → Decrease the data value with 87
7. ...
8. . → Print the character of the data value to screen

Result is: `;`

### 2. Input

```
I.=.<>I
I.=.'I'.
```

This example defeats the purpose of WordLang, the point is that every sentence should be readable. But is for the sake of understanding.

1. Print the text "I="
2. Read the input value
3. Store it in the variable I
4. Print the text "I="
5. Print the value of I