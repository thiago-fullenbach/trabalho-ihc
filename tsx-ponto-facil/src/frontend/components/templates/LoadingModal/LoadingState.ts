import React from "react"

export default class LoadingState {
    loading: boolean
    updateLoading: (loading: boolean) => void

    constructor(
        loading: boolean,
        updateLoading: (loading: boolean) => void
    ) {
        this.loading = loading
        this.updateLoading = updateLoading
    }
}