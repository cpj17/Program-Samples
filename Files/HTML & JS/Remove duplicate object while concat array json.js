let arr1 = [
  {
    id: 1,
    name: "name 1"
  },
  {
    id: 2,
    name: "name 2"
  },
  {
    id: 3,
    name: "name 3"
  }
]

let arr2 = [
  {
    id: 1,
    name: "name 1"
  },
  {
    id: 2,
    name: "name 2"
  },
  {
    id: 4,
    name: "name 4"
  },
  {
    id: 5,
    name: "name 5"
  },
  {
    id: 6,
    name: "name 6"
  }
]

const newArr = [...arr1, ...arr2]

console.log(newArr)

// Remove duplicates based on the 'id' property
// const uniqueArr = Array.from(new Set(newArr.map(obj => obj.id))).map(
//   id => newArr.find(obj => obj.id === id)
// );

const uniqueArr = [...new Map(newArr.map(obj => [obj.id, obj])).values()];

console.log(uniqueArr);