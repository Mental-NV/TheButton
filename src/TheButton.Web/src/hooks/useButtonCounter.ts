import { useState, useCallback, useEffect } from 'react'

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
            const response = await fetch(`${apiUrl}/api/v3/counter`, {
                method: 'POST',
                headers: {
                    'Idempotency-Key': crypto.randomUUID(),
                },
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

    useEffect(() => {
        let cancelled = false

        const init = async () => {
            setIsLoading(true)
            try {
                const apiUrl = import.meta.env.VITE_API_URL
                const response = await fetch(`${apiUrl}/api/v3/counter`)

                if (cancelled) return

                if (response.ok) {
                    const data = await response.json()
                    setCount(data.value)
                } else {
                    setError('Failed to load counter')
                    console.error('Failed to load counter')
                }
            } catch (err) {
                if (cancelled) return
                const errorMessage = err instanceof Error ? err.message : 'Unknown error'
                setError(errorMessage)
                console.error('Error loading counter:', err)
            } finally {
                if (!cancelled) setIsLoading(false)
            }
        }

        init()

        return () => {
            cancelled = true
        }
    }, [])

    return { count, isLoading, error, handleClick }
}
