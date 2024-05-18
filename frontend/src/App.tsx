import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

function App() {
  const [count, setCount] = useState(0)
    async function testClick() {
        const response = await fetch('http://localhost:5500/test', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            }
        })
        const data = await response.json()
        console.log(data);
}

  return (
    <>
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + yaya </h1>
        <button onClick={() => testClick()}> Test </button>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          Co is {count}
        </button>
          asfsas
        <p>
            ssf <code>src/App.tsx</code> and save to test HMR
        </p>
          sdf
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
