import { useState, useCallback } from 'react'

export interface UseButtonCounterResult {
    count: number
    isLoading: boolean
    error: string | null
    handleClick: () => Promise<void>
}

export function useButtonCounter(): UseButtonCounterResult {
    const [count, setCount] = useState(0)
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const handleClick = useCallback(async () => {
        setIsLoading(true)
        setError(null)

        try {
            const apiUrl = import.meta.env.VITE_API_URL
            const response = await fetch(`${apiUrl}/api/button/click`, {
                method: 'POST',
            })

            if (response.ok) {
                const data = await response.json()
                setCount(data.value)
            } else {
                setError('Failed to increment counter')
                console.error('Failed to increment counter')
            }
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Unknown error'
            setError(errorMessage)
            console.error('Error clicking button:', err)
        } finally {
            setIsLoading(false)
        }
    }, [])

    return { count, isLoading, error, handleClick }
}
