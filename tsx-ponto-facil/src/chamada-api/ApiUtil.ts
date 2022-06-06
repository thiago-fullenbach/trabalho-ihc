import LoginXSenhaDTO from "./DTO/DoClienteParaServidor/LoginXSenhaDTO";
import axios, { AxiosError } from 'axios';
import AmbienteAplicacaoStateful from "./Aplicacao";
import ReturnedXMessagesFailure from "./DTO/DoServidorParaCliente/ReturnedXMessagesImpl/ReturnedXMessagesFailure";
import React from "react";
import LoggedUserDTO from "./DTO/DoServidorParaCliente/LoggedUserDTO";
import ServerResponse from "./DTO/DoServidorParaCliente/Extra/ServerResponse";
import AuthenticatedHeader from "./DTO/DoClienteParaServidor/AuthenticatedHeader";
import Aplicacao from "./Aplicacao";
import IAmbienteAplicacao from "./IAmbienteAplicacao";
import { setSimpleState } from "../utils/stateUtil";

class ApiUtil {
    static urlApiV1(ambiente: IAmbienteAplicacao = Aplicacao.criaAmbienteDetectado()): string {
        const urlDominioApi = `${ambiente.getUrlDominioApi()}/api/v1`;
        return urlDominioApi;
    }

    static refreshObtainedHeaders<TUnused>(
        updateSessao: (stringifiedSession: string | null) => void,
        updateUsuarioLogado: (loggedUser: LoggedUserDTO | null) => void,
        parsedResponse: ServerResponse<TUnused>
    ) {
        updateSessao(parsedResponse.stringifiedSession);
        updateUsuarioLogado(parsedResponse.loggedUser || null);
    }

    static async loginAsync(
        updateSessao: (stringifiedSession: string | null) => void,
        updateUsuarioLogado: (loggedUser: LoggedUserDTO | null) => void,
        login: string,
        senha: string
    ): Promise<ServerResponse<null>> {
        try {
            let loginForm = new LoginXSenhaDTO()
            loginForm.login = login
            loginForm.senha = senha
            let response = await axios.post(`${ApiUtil.urlApiV1()}/Autorizacao/login`, loginForm)
            console.log(response)
            let parsedResponse = new ServerResponse<null>().hasSuccess(response)
            this.refreshObtainedHeaders(updateSessao, updateUsuarioLogado, parsedResponse)
            return parsedResponse
        } catch (error: any) {
            console.log(error)
            let parsedResponse = new ServerResponse<null>().hasFailure(error.response)
            return parsedResponse
        }
    }

    static async getAsync<RType>(
        sessao: string | null,
        updateSessao: (stringifiedSession: string | null) => void,
        updateUsuarioLogado: (loggedUser: LoggedUserDTO | null) => void,
        url: string
    ): Promise<ServerResponse<RType | null>> {
        try {
            let headerComSessao = new AuthenticatedHeader()
            headerComSessao.sessao = sessao + ``
            let response = await axios.get(url, { headers: headerComSessao } )
            let parsedResponse = new ServerResponse<RType>().hasFailure(response)
            this.refreshObtainedHeaders(updateSessao, updateUsuarioLogado, parsedResponse)
            return parsedResponse
        } catch (error: any) {
            let parsedResponse = new ServerResponse<null>().hasFailure(error.response)
            return parsedResponse
        }
    }

    static async postAsync<RType>(
        sessao: string | null,
        updateSessao: (stringifiedSession: string | null) => void,
        updateUsuarioLogado: (loggedUser: LoggedUserDTO | null) => void,
        url: string,
        objeto: any | null
    ): Promise<ServerResponse<RType | null>> {
        try {
            let headerComSessao = new AuthenticatedHeader()
            headerComSessao.sessao = sessao + ``
            let response = await axios.post(url, objeto, { headers: headerComSessao } )
            let parsedResponse = new ServerResponse<RType>().hasFailure(response)
            this.refreshObtainedHeaders(updateSessao, updateUsuarioLogado, parsedResponse)
            return parsedResponse
        } catch (error: any) {
            let parsedResponse = new ServerResponse<null>().hasFailure(error.response)
            return parsedResponse
        }
    }
}

export default ApiUtil
