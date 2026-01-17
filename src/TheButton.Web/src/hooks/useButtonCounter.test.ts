import { describe, it, expect, vi, beforeEach } from 'vitest'
import { renderHook, waitFor, act } from '@testing-library/react'
import { useButtonCounter } from './useButtonCounter'

describe('useButtonCounter', () => {
    beforeEach(() => {
        vi.resetAllMocks()
    })

    it('returns initial state with count 0 (GET mocked)', async () => {
        // Ensure initial GET resolves to 0
        vi.stubGlobal('fetch', vi.fn((url, init) => {
            // GET
            if (!init || init.method === undefined) {
                return Promise.resolve({ ok: true, json: () => Promise.resolve({ value: 0 }) })
            }
            // default POST fallback
            return Promise.resolve({ ok: true, json: () => Promise.resolve({ value: 0 }) })
        }))

        const { result } = renderHook(() => useButtonCounter())

        // initial synchronous expectations
        expect(result.current.count).toBe(0)
        expect(result.current.error).toBeNull()
        expect(typeof result.current.handleClick).toBe('function')

        // initialization should set loading true, then false after resolving
        await waitFor(() => {
            expect(result.current.isLoading).toBe(true)
        })

        await waitFor(() => {
            expect(result.current.isLoading).toBe(false)
        })
    })

    it('initializes count from GET /api/v3/counter (success)', async () => {
        vi.stubGlobal('fetch', vi.fn((url, init) => {
            if (!init || init.method === undefined) {
                return Promise.resolve({ ok: true, json: () => Promise.resolve({ value: 5 }) })
            }
            return Promise.resolve({ ok: true, json: () => Promise.resolve({ value: 5 }) })
        }))

        const { result } = renderHook(() => useButtonCounter())

        await waitFor(() => {
            expect(result.current.count).toBe(5)
            expect(result.current.isLoading).toBe(false)
            expect(result.current.error).toBeNull()
        })
    })

    it('initializes count from GET /api/v3/counter (failure)', async () => {
        vi.stubGlobal('fetch', vi.fn(() => Promise.resolve({ ok: false, status: 500 })))

        const { result } = renderHook(() => useButtonCounter())

        await waitFor(() => {
            expect(result.current.count).toBe(0)
            expect(result.current.isLoading).toBe(false)
            expect(result.current.error).toBe('Failed to load counter')
        })
    })

    it('sets loading during initial GET', async () => {
        let resolvePromise: (value: unknown) => void
        const pendingPromise = new Promise((resolve) => {
            resolvePromise = resolve
        })

        // initial GET is pending
        const fetchMock = vi.fn().mockReturnValueOnce(pendingPromise)
        vi.stubGlobal('fetch', fetchMock)

        const { result } = renderHook(() => useButtonCounter())

        await waitFor(() => {
            expect(result.current.isLoading).toBe(true)
        })

        // resolve initial GET
        await act(async () => {
            resolvePromise!({ ok: true, json: () => Promise.resolve({ value: 3 }) })
        })

        await waitFor(() => {
            expect(result.current.isLoading).toBe(false)
            expect(result.current.count).toBe(3)
        })
    })

    it('updates count on successful API response', async () => {
        const mockResponse = { value: 42 }
        vi.stubGlobal('fetch', vi.fn((url, init) => {
            // GET -> initial value
            if (!init || init.method === undefined) {
                return Promise.resolve({ ok: true, json: () => Promise.resolve({ value: 0 }) })
            }

            // POST
            if (init?.headers?.['Idempotency-Key'] !== undefined) {
                return Promise.resolve({ ok: true, json: () => Promise.resolve(mockResponse) })
            }

            return Promise.resolve({ ok: false })
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

        // First call: GET resolves immediately; second call (POST) is pending
        const fetchMock = vi.fn()
            .mockResolvedValueOnce({ ok: true, json: () => Promise.resolve({ value: 0 }) })
            .mockReturnValueOnce(pendingPromise)

        vi.stubGlobal('fetch', fetchMock)

        const { result } = renderHook(() => useButtonCounter())

        act(() => {
            result.current.handleClick()
        })

        await waitFor(() => {
            expect(result.current.isLoading).toBe(true)
        })

        // Cleanup - resolve the POST promise
        await act(async () => {
            resolvePromise!({ ok: true, json: () => Promise.resolve({ value: 1 }) })
        })
    })

    it('sets error on API failure response', async () => {
        // GET succeeds, POST fails with non-ok
        const fetchMock = vi.fn()
            .mockResolvedValueOnce({ ok: true, json: () => Promise.resolve({ value: 0 }) })
            .mockResolvedValueOnce({ ok: false, status: 500 })

        vi.stubGlobal('fetch', fetchMock)

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
        // GET succeeds, POST rejects
        const fetchMock = vi.fn()
            .mockResolvedValueOnce({ ok: true, json: () => Promise.resolve({ value: 0 }) })
            .mockRejectedValueOnce(new Error('Network error'))

        vi.stubGlobal('fetch', fetchMock)

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
