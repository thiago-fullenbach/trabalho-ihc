import LoginXSenhaDTO from "./DTO/DoClienteParaServidor/LoginXSenhaDTO";
import RespostaDoServidor from "./DTO/DoServidorParaCliente/Extra/RespostaDoServidor";
import axios from 'axios';
import CadUsuarioCadastreSeDTO from "./DTO/DoClienteParaServidor/CadUsuarioCadastreSeDTO";
import AmbienteAplicacaoStateful from "./AplicacaoStateful";

class ApiUtil {
    static get string_urlApiV1() {
        const string_urlDominioApi = `${AmbienteAplicacaoStateful.criaEstado().urlDominioApi}/api/v1`;
        return string_urlDominioApi;
    }

    static async RespostaDoServidor_submitEntrarAsync(
        SetSessaoDTOHandler_setSessao,
        SetUsuarioLogadoDTOHandler_setUsuarioLogado,
        string_login,
        string_senha
    ) {
        try {
            let LoginXSenhaDTO_login = new LoginXSenhaDTO()
            LoginXSenhaDTO_login.string_login = string_login
            LoginXSenhaDTO_login.string_senha = string_senha
            let response = await axios.post(`${ApiUtil.string_urlApiV1}/Autorizacao/login`, LoginXSenhaDTO_login)
            let RespostaDoServidor_retorno = RespostaDoServidor.parse(response)
            SetSessaoDTOHandler_setSessao(RespostaDoServidor_retorno.SessaoDTO_sessao)
            SetUsuarioLogadoDTOHandler_setUsuarioLogado(RespostaDoServidor_retorno.UsuarioLogadoDTO_usuario)
            return RespostaDoServidor_retorno
        } catch (error) {
            let RespostaDoServidor_retorno = RespostaDoServidor.parse(error.response)
            return RespostaDoServidor_retorno
        }
    }

    static async RespostaDoServidor_getAsync(
        SessaoDTO_sessao,
        SetSessaoDTOHandler_setSessao,
        SetUsuarioLogadoDTOHandler_setUsuarioLogado,
        string_url
    ) {
        try {
            let AxiosHeaders_headerComSessao = { sessao: JSON.stringify(SessaoDTO_sessao) }
            let response = await axios.get(string_url, { headers: AxiosHeaders_headerComSessao } )
            let RespostaDoServidor_retorno = RespostaDoServidor.parse(response)
            SetSessaoDTOHandler_setSessao(RespostaDoServidor_retorno.SessaoDTO_sessao)
            SetUsuarioLogadoDTOHandler_setUsuarioLogado(RespostaDoServidor_retorno.UsuarioLogadoDTO_usuario)
            return RespostaDoServidor_retorno
        } catch (error) {
            let RespostaDoServidor_retorno = RespostaDoServidor.parse(error.response)
            return RespostaDoServidor_retorno
        }
    }

    static async RespostaDoServidor_postAsync(
        SessaoDTO_sessao,
        SetSessaoDTOHandler_setSessao,
        SetUsuarioLogadoDTOHandler_setUsuarioLogado,
        string_url,
        any_objeto
    ) {
        try {
            let AxiosHeaders_headerComSessao = { sessao: JSON.stringify(SessaoDTO_sessao) }
            let AxiosResponse_resposta = await axios.post(string_url, any_objeto, { headers: AxiosHeaders_headerComSessao })
            let RespostaDoServidor_retorno = RespostaDoServidor.parse(AxiosResponse_resposta)
            SetSessaoDTOHandler_setSessao(RespostaDoServidor_retorno.SessaoDTO_sessao)
            SetUsuarioLogadoDTOHandler_setUsuarioLogado(RespostaDoServidor_retorno.UsuarioLogadoDTO_usuario)
            return RespostaDoServidor_retorno
        } catch (error) {
            let RespostaDoServidor_retorno = RespostaDoServidor.parse(error.response)
            return RespostaDoServidor_retorno
        }
    }
}

export default ApiUtil
