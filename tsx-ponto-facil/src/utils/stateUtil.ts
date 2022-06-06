import React from 'react'

export function setSimpleState<T>(reactDispatch: React.Dispatch<React.SetStateAction<T>>, currentValue: T): void {
    reactDispatch((_previousValue: T): T => {
        return currentValue
    })
}