let arr1 = [
  { id: 1, name: "name 1" },
  { id: 2, name: "name 2" },
  { id: 3, name: "name 3" }
]

let arr2 = [
  {id: 1, name: "name 1" },
  { id: 2, name: "name 2" },
  { id: 4, name: "name 4" },
  { id: 5, name: "name 5" },
  {id: 6, name: "name 6" }
]

const uniqueArr = [...new Map([...arr1, ...arr2].map(obj => [obj.id, obj])).values()];

console.log(uniqueArr);