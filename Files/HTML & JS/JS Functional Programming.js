function add(a) {
  return function(b) {
    return a + b
  }
}

//console.log(add(3)(5))

function func1(fn) {
  console.log("func1");
  console.log(fn());
}

function func2() {
  console.log("func2");
  return "rt func2"
}

//func1(func2)

const person = {
  name: "test",
  address: {
    dity: "nkl",
    pin: 123
  }
}

//person.name = "modi"

const updatedPerson = {
  ...person, name: "meloni"
}

console.log(person);
console.log(updatedPerson);