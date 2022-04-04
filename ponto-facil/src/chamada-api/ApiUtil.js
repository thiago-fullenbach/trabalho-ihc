import IdentidadeSessaoDTO from "./DTO/DoClienteParaServidor/IdentidadeSessaoDTO";
import LoginXSenhaDTO from "./DTO/DoClienteParaServidor/LoginXSenhaDTO";
import ManterUsuarioComContaDTO from "./DTO/DoClienteParaServidor/ManterUsuarioComContaDTO";
import RespostaDoServidor from "./DTO/DoServidorParaCliente/Extra/RespostaDoServidor";

class ApiUtil {
  static get urlDominio() {
    const urlDominioApi =
      process.env.URL_DOMINIO_API || "https://localhost:7120/api/v1/";
    return urlDominioApi;
  }

  static async obterIdentidadeSessaoDTO_Entrar(login, senha) {
    let loginXSenhaDTO = new LoginXSenhaDTO();
    loginXSenhaDTO.login = login;
    loginXSenhaDTO.senha = senha;
    let respostaAxios = await axios.post(
      `${ApiUtil.urlDominio}Autorizacao/login`,
      loginXSenhaDTO
    );
    let respostaTraduzida = RespostaDoServidor.daRequisicaoAxios(respostaAxios);
    return respostaTraduzida;
  }

  static async obterIdentidadeSessaoDTO_Registrar(
    nome,
    cpf,
    dataNascimento,
    login,
    senha
  ) {
    let manterUsuarioComContaDTO = new ManterUsuarioComContaDTO();
    manterUsuarioComContaDTO.nome = nome;
    manterUsuarioComContaDTO.cpf = cpf;
    manterUsuarioComContaDTO.dataNascimento = dataNascimento;
    manterUsuarioComContaDTO.login = login;
    manterUsuarioComContaDTO.senha = senha;
    let respostaAxios = await axios.post(
      `${ApiUtil.urlDominio}Autorizacao/registrar`,
      manterUsuarioComContaDTO
    );
    let respostaTraduzida = RespostaDoServidor.daRequisicaoAxios(respostaAxios);
    return respostaTraduzida;
  }

  static async getAsync(identidadeSessaoDTO, setIdentidadeSessaoDTO, url) {
    let cabecalhoCustomizado = {
      identidadeSessao_Id: identidadeSessaoDTO.id,
      identidadeSessao_HexVerificacao: identidadeSessaoDTO.hexVerificacao,
    };
    let respostaAxios = await axios.get(url, {
      headers: cabecalhoCustomizado,
    });
    let respostaTraduzida = RespostaDoServidor.daRequisicaoAxios(respostaAxios);
    setIdentidadeSessaoDTO(respostaTraduzida.identidadeSessaoDTO);
    return respostaTraduzida;
  }

  static async postAsync(
    identidadeSessaoDTO,
    setIdentidadeSessaoDTO,
    url,
    objeto
  ) {
    let cabecalhoCustomizado = {
      identidadeSessao_Id: identidadeSessaoDTO.id,
      identidadeSessao_HexVerificacao: identidadeSessaoDTO.hexVerificacao,
    };
    let respostaAxios = await axios.post(url, objeto, {
      headers: cabecalhoCustomizado,
    });
    let respostaTraduzida = RespostaDoServidor.daRequisicaoAxios(respostaAxios);
    setIdentidadeSessaoDTO(respostaTraduzida.identidadeSessaoDTO);
    return respostaTraduzida;
  }

  // Exemplo chamada com getAsync: trazer todos os usuários
  async listarUsuarios() {
    let mock_identidadeSessaoDTO = new IdentidadeSessaoDTO();
    mock_identidadeSessaoDTO.id = 100;
    mock_identidadeSessaoDTO.hexVerificacao = "ABCDEFABCDEFABCDEFABCDEF";
    let mock_setIdentidadeSessaoDTO = (value) => {
      mock_identidadeSessaoDTO = value;
    };
    let urlUsuarioListar = `${ApiUtil.urlDominio}Usuario/listarTodos`;
    let respostaTraduzidaUsuarios = await ApiUtil.getAsync(
      mock_identidadeSessaoDTO,
      mock_setIdentidadeSessaoDTO,
      urlUsuarioListar
    );
    let mock_resultadosUsuarios = [];
    let mock_setResultadosUsuarios = (value) => {
      mock_resultadosUsuarios = value;
    };
    mock_setResultadosUsuarios(
      respostaTraduzidaUsuarios.devolvidoMensagemDTO.devolvido
    );
  }
}

export default ApiUtil;
