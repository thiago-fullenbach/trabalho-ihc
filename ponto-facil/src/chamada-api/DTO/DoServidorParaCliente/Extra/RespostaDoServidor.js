import DevolvidoMensagemDTO from "../DevolvidoMensagemDTO";
import RecursoPermitidoDTO from "../RecursoPermitidoDTO";
import SessaoAtualizadaDTO from "../SessaoAtualizadaDTO";

export default class RespostaDoServidor {
  get int_status() {
    return this._status;
  }
  set int_status(value) {
    this._status = value;
  }
  get SessaoAtualizadaDTO_sessao() {
    return this._sessao;
  }
  set SessaoAtualizadaDTO_sessao(value) {
    this._sessao = value;
  }
  get DevolvidoMensagemDTO_corpo() {
    return this._corpo;
  }
  set DevolvidoMensagemDTO_corpo(value) {
    this._corpo = value;
  }

  static RespostaDoServidor_fromAxios(AxiosResponse_resposta) {
    let RespostaDoServidor_retorno = new RespostaDoServidor();
    RespostaDoServidor_retorno.int_status = AxiosResponse_resposta.status;
    RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao =
      new SessaoAtualizadaDTO();
    let any_dadosSessao = JSON.parse(
      AxiosResponse_resposta.headers["sessao"]
    );
    RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao.int_id =
      any_dadosSessao.Id;
    RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao.string_hexVerificacao =
      any_dadosSessao.HexVerificacao;
    RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao.date_dataHoraUltimaAtualizacao =
      any_dadosSessao.DataHoraUltimaAtualizacao;
    RespostaDoServidor_retorno.SessaoAtualizadaDTO_sessao.RecursoPermitidoDTO_Array_listaRecursosPermitidos =
      any_dadosSessao.ListaRecursosPermitidos.map((x) => {
        let RecursoPermitidoDTO_recurso = new RecursoPermitidoDTO();
        RecursoPermitidoDTO_recurso.int_id = x.Id;
        RecursoPermitidoDTO_recurso.string_nome = x.Nome;
        return RecursoPermitidoDTO_recurso;
      });
    RespostaDoServidor_retorno.DevolvidoMensagemDTO_corpo =
      new DevolvidoMensagemDTO();
    RespostaDoServidor_retorno.DevolvidoMensagemDTO_corpo.any_devolvido =
      AxiosResponse_resposta.data.devolvido;
    RespostaDoServidor_retorno.DevolvidoMensagemDTO_corpo.string_mensagem =
      AxiosResponse_resposta.data.mensagem;
    return RespostaDoServidor_retorno;
  }
}
