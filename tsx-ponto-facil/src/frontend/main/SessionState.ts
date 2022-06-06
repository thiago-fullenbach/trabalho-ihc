import React from "react"
import LoggedUserDTO from "../../chamada-api/DTO/DoServidorParaCliente/LoggedUserDTO"
import { resourceDescription } from "../modelo/resourceDescription"

export default class SessionState {
    
    stringifiedSession: string | null
    updateStringifiedSession: (stringifiedSession: string | null) => void
    loggedUser: LoggedUserDTO | null
    updateLoggedUser: (loggedUser: LoggedUserDTO | null) => void

    constructor(
        stringifiedSession: string | null,
        updateStringifiedSession: (stringifiedSession: string | null) => void,
        loggedUser: LoggedUserDTO | null,
        updateLoggedUser: (loggedUser: LoggedUserDTO | null) => void
    ) {
        this.stringifiedSession = stringifiedSession
        this.updateStringifiedSession = updateStringifiedSession
        this.loggedUser = loggedUser
        this.updateLoggedUser = updateLoggedUser
    }
    loggedUserHasEnabledResource = (resourceCode: number): boolean => this.loggedUser?.AccessList?.find(x => x.Recurso_cod_en === resourceCode)?.Eh_habilitado || false
    loggedUserHasEnabledResourceByDescription = (resourceDesc: string): boolean => this.loggedUserHasEnabledResource(resourceDescription.find(x => x.recurso_desc.toLowerCase() === resourceDesc.toLowerCase())?.recurso_cod_en || 0)
}