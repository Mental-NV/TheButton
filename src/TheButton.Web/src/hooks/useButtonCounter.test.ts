import { describe, it, expect, vi, beforeEach } from 'vitest'
import { renderHook, waitFor, act } from '@testing-library/react'
import { useButtonCounter } from './useButtonCounter'

describe('useButtonCounter', () => {
    beforeEach(() => {
        vi.resetAllMocks()
    })

    it('returns initial state with count 0', () => {
        const { result } = renderHook(() => useButtonCounter())

        expect(result.current.count).toBe(0)
        expect(result.current.isLoading).toBe(false)
        expect(result.current.error).toBeNull()
        expect(typeof result.current.handleClick).toBe('function')
    })

    it('updates count on successful API response', async () => {
        const mockResponse = { value: 42 }
        vi.stubGlobal('fetch', vi.fn().mockResolvedValueOnce({
            ok: true,
            json: () => Promise.resolve(mockResponse),
        }))

        const { result } = renderHook(() => useButtonCounter())

        await act(async () => {
            await result.current.handleClick()
        })

        await waitFor(() => {
            expect(result.current.count).toBe(42)
            expect(result.current.isLoading).toBe(false)
            expect(result.current.error).toBeNull()
        })
    })

    it('sets loading state during API call', async () => {
        let resolvePromise: (value: unknown) => void
        const pendingPromise = new Promise((resolve) => {
            resolvePromise = resolve
        })

        vi.stubGlobal('fetch', vi.fn().mockReturnValueOnce(pendingPromise))

        const { result } = renderHook(() => useButtonCounter())

        act(() => {
            result.current.handleClick()
        })

        await waitFor(() => {
            expect(result.current.isLoading).toBe(true)
        })

        // Cleanup - resolve the promise
        await act(async () => {
            resolvePromise!({ ok: true, json: () => Promise.resolve({ value: 1 }) })
        })
    })

    it('sets error on API failure response', async () => {
        vi.stubGlobal('fetch', vi.fn().mockResolvedValueOnce({
            ok: false,
            status: 500,
        }))

        const { result } = renderHook(() => useButtonCounter())

        await act(async () => {
            await result.current.handleClick()
        })

        await waitFor(() => {
            expect(result.current.error).toBe('Failed to increment counter')
            expect(result.current.isLoading).toBe(false)
        })
    })

    it('sets error on network failure', async () => {
        vi.stubGlobal('fetch', vi.fn().mockRejectedValueOnce(new Error('Network error')))

        const { result } = renderHook(() => useButtonCounter())

        await act(async () => {
            await result.current.handleClick()
        })

        await waitFor(() => {
            expect(result.current.error).toBe('Network error')
            expect(result.current.isLoading).toBe(false)
        })
    })
})
