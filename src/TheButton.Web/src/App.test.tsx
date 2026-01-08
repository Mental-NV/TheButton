import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import App from './App'

// Mock the hook
vi.mock('./hooks/useButtonCounter', () => ({
    useButtonCounter: vi.fn(),
}))

import { useButtonCounter } from './hooks/useButtonCounter'

const mockUseButtonCounter = vi.mocked(useButtonCounter)

describe('App', () => {
    beforeEach(() => {
        vi.resetAllMocks()
    })

    it('renders button with initial count', () => {
        mockUseButtonCounter.mockReturnValue({
            count: 0,
            isLoading: false,
            error: null,
            handleClick: vi.fn(),
        })

        render(<App />)

        expect(screen.getByRole('button')).toHaveTextContent('count is 0')
    })

    it('displays current count from hook', () => {
        mockUseButtonCounter.mockReturnValue({
            count: 42,
            isLoading: false,
            error: null,
            handleClick: vi.fn(),
        })

        render(<App />)

        expect(screen.getByRole('button')).toHaveTextContent('count is 42')
    })

    it('shows loading state when isLoading is true', () => {
        mockUseButtonCounter.mockReturnValue({
            count: 5,
            isLoading: true,
            error: null,
            handleClick: vi.fn(),
        })

        render(<App />)

        expect(screen.getByRole('button')).toHaveTextContent('Loading...')
        expect(screen.getByRole('button')).toBeDisabled()
    })

    it('displays error message when error exists', () => {
        mockUseButtonCounter.mockReturnValue({
            count: 0,
            isLoading: false,
            error: 'Something went wrong',
            handleClick: vi.fn(),
        })

        render(<App />)

        expect(screen.getByRole('alert')).toHaveTextContent('Something went wrong')
    })

    it('calls handleClick when button is clicked', async () => {
        const mockHandleClick = vi.fn()
        mockUseButtonCounter.mockReturnValue({
            count: 0,
            isLoading: false,
            error: null,
            handleClick: mockHandleClick,
        })

        render(<App />)

        fireEvent.click(screen.getByRole('button'))

        await waitFor(() => {
            expect(mockHandleClick).toHaveBeenCalledTimes(1)
        })
    })

    it('does not call handleClick when button is disabled', async () => {
        const mockHandleClick = vi.fn()
        mockUseButtonCounter.mockReturnValue({
            count: 0,
            isLoading: true,
            error: null,
            handleClick: mockHandleClick,
        })

        render(<App />)

        fireEvent.click(screen.getByRole('button'))

        expect(mockHandleClick).not.toHaveBeenCalled()
    })
})
