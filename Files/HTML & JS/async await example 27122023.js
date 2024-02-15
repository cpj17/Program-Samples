async function fetchData() {
  const resp = await fetch('https://jsonplaceholder.typicode.com/todos/1')
  const res = await resp.json()
  return res
}

async function fetchData2(params = "def") {
  console.log(params)
  const resp = await fetch('https://jsonplaceholder.typicode.com/posts/1')
  const res = await resp.json()
  console.log(res)
}

pageload();

async function initMethod() {
  const res = await fetchData()
  fetchData2(res)
}

function pageload() {
  initMethod()
  func3()
}

function func3() {
  console.log("func3")
}