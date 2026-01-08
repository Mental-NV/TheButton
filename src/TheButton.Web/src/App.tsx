import './App.css'
import { useButtonCounter } from './hooks/useButtonCounter'

function App() {
  const { count, isLoading, error, handleClick } = useButtonCounter()

  return (
    <div className="card">
      <button onClick={handleClick} disabled={isLoading}>
        {isLoading ? 'Loading...' : `count is ${count}`}
      </button>
      {error && <p className="error" role="alert">{error}</p>}
    </div>
  )
}

export default App

