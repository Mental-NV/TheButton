import { useState } from 'react'
import './App.css'

function App() {
  const [count, setCount] = useState(0)

  const handleClick = async () => {
    try {
      const apiUrl = import.meta.env.VITE_API_URL;
      const response = await fetch(`${apiUrl}/api/button/click`, {
        method: 'POST'
      });
      if (response.ok) {
        const data = await response.json();
        setCount(data.value);
      } else {
        console.error('Failed to increment counter');
      }
    } catch (error) {
      console.error('Error clicking button:', error);
    }
  }

  return (
    <div className="card">
      <button onClick={handleClick}>
        count is {count}
      </button>
    </div>
  )
}

export default App
