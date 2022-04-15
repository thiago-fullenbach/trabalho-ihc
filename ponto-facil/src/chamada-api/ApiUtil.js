import LoginXSenhaDTO from "./DTO/DoClienteParaServidor/LoginXSenhaDTO";
import ManterUsuarioComContaDTO from "./DTO/DoClienteParaServidor/ManterUsuarioComContaDTO";
import RespostaDoServidor from "./DTO/DoServidorParaCliente/Extra/RespostaDoServidor";
import SessaoAtualizadaDTO from "./DTO/DoServidorParaCliente/SessaoAtualizadaDTO";
import axios from 'axios';

class ApiUtil {
  static get string_urlApiV1() {
    const string_urlDominioApi =
      process.env.URL_DOMINIO_API || "https://localhost:7120/api/v1";
    return string_urlDominioApi;
  }

  static async RespostaDoServidor_submitEntrarAsync(
    SetSessaoAtualizadaDTOHandler_setSessao,
    string_login,
    string_senha
  ) {
    // let LoginXSenhaDTO_login = new LoginXSenhaDTO();
    // LoginXSenhaDTO_login.string_login = string_login;
    // LoginXSenhaDTO_login.string_senha = string_senha;
    let AxiosResponse_resposta = await axios.post(
      `${ApiUtil.string_urlApiV1}/Autorizacao/login`,
      {
        login: string_login, senha: string_senha
      }
    );
    let RespostaDoServidor_retorno =
      RespostaDoServidor.RespostaDoServidor_fromAxios(AxiosResponse_resposta);
    SetSessaoAtualizadaDTOHandler_setSessao(
      RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao
    );
    return RespostaDoServidor_retorno;
  }

  static async RespostaDoServidor_submitCadastrarAsync(
    SetSessaoAtualizadaDTOHandler_setSessao,
    string_nome,
    string_cpf,
    date_dataNascimento,
    string_login,
    string_senha
  ) {
    // let ManterUsuarioComContaDTO_dados = new ManterUsuarioComContaDTO();
    // ManterUsuarioComContaDTO_dados.nome = string_nome;
    // ManterUsuarioComContaDTO_dados.cpf = string_cpf;
    // ManterUsuarioComContaDTO_dados.dataNascimento = date_dataNascimento;
    // ManterUsuarioComContaDTO_dados.login = string_login;
    // ManterUsuarioComContaDTO_dados.senha = string_senha;
    let AxiosResponse_resposta = await axios.post(
      `${ApiUtil.string_urlApiV1}/Autorizacao/registrar`,
      {
        nome: string_nome,
        cpf: string_cpf,
        dataNascimento: date_dataNascimento,
        login: string_login,
        senha: string_senha
      }
    );
    let RespostaDoServidor_retorno =
      RespostaDoServidor.RespostaDoServidor_fromAxios(AxiosResponse_resposta);
    SetSessaoAtualizadaDTOHandler_setSessao(
      RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao
    );
    return RespostaDoServidor_retorno;
  }

  static async RespostaDoServidor_getAsync(
    SessaoAtualizadaDTO_sessao,
    SetSessaoAtualizadaDTOHandler_setSessao,
    string_url
  ) {
    let AxiosHeaders_headerComSessao = {
      identidadeSessao: JSON.stringify({
        id: SessaoAtualizadaDTO_sessao.int_id,
        hexVerificacao: SessaoAtualizadaDTO_sessao.string_hexVerificacao,
      }),
    };
    let AxiosResponse_resposta = await axios.get(string_url, {
      headers: AxiosHeaders_headerComSessao,
    });
    let RespostaDoServidor_retorno =
      RespostaDoServidor.RespostaDoServidor_fromAxios(AxiosResponse_resposta);
    SetSessaoAtualizadaDTOHandler_setSessao(
      RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao
    );
    return RespostaDoServidor_retorno;
  }

  static async RespostaDoServidor_postAsync(
    SessaoAtualizadaDTO_sessao,
    SetSessaoAtualizadaDTOHandler_setSessao,
    string_url,
    any_objeto
  ) {
    let AxiosHeaders_headerComSessao = {
      identidadeSessao: JSON.stringify({
        id: SessaoAtualizadaDTO_sessao.int_id,
        hexVerificacao: SessaoAtualizadaDTO_sessao.string_hexVerificacao,
      }),
    };
    let AxiosResponse_resposta = await axios.post(string_url, any_objeto, {
      headers: AxiosHeaders_headerComSessao,
    });
    let RespostaDoServidor_retorno =
      RespostaDoServidor.RespostaDoServidor_fromAxios(AxiosResponse_resposta);
    SetSessaoAtualizadaDTOHandler_setSessao(
      RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao
    );
    return RespostaDoServidor_retorno;
  }

  // Exemplo chamada com RespostaDoServidor_getAsync: trazer todos os usuários
  async listarUsuarios() {
    let SessaoAtualizadaDTO_mockSessao = new SessaoAtualizadaDTO();
    let SetSessaoAtualizadaDTOHandler_setMockSessao = (value) =>
      (SessaoAtualizadaDTO_mockSessao = value);
    let string_urlUsuarioListar = `${ApiUtil.string_urlApiV1}/Usuario/listarTodos`;
    let RespostaDoServidor_usuariosResposta =
      await ApiUtil.RespostaDoServidor_getAsync(
        SessaoAtualizadaDTO_mockSessao,
        SetSessaoAtualizadaDTOHandler_setMockSessao,
        string_urlUsuarioListar
      );
    let any_Array_usuarios = [];
    let SetAny_ArrayHandler_setUsuarios = (value) => {
      any_Array_usuarios = value;
    };
    SetAny_ArrayHandler_setUsuarios(
      RespostaDoServidor_usuariosResposta.DevolvidoMensagemDTO_corpo
        .any_devolvido
    );
  }
}

export default ApiUtil;
