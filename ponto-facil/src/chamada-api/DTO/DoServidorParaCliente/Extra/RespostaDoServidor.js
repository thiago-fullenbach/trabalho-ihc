import IdentidadeSessaoDTO from "../../DoClienteParaServidor/IdentidadeSessaoDTO";
import DevolvidoMensagemDTO from "../DevolvidoMensagemDTO";

export default class RespostaDoServidor {
  get status() {
    return this.status;
  }
  set status(value) {
    this.status = value;
  }
  get identidadeSessaoDTO() {
    return this._identidadeSessaoDTO;
  }
  set identidadeSessaoDTO(value) {
    this._identidadeSessaoDTO = value;
  }
  get devolvidoMensagemDTO() {
    return this._devolvidoMensagemDTO;
  }
  set devolvidoMensagemDTO(value) {
    this._devolvidoMensagemDTO = value;
  }

  static daRequisicaoAxios(requisicaoAxios) {
    let novaResposta = new RespostaDoServidor();
    novaResposta.status = requisicaoAxios.status;
    novaResposta.identidadeSessaoDTO = new IdentidadeSessaoDTO();
    novaResposta.identidadeSessaoDTO.id =
      requisicaoAxios.headers["identidadeSessao_Id"];
    novaResposta.identidadeSessaoDTO.hexVerificacao =
      requisicaoAxios.headers["identidadeSessao_HexVerificacao"];
    novaResposta.devolvidoMensagemDTO = new DevolvidoMensagemDTO();
    novaResposta.devolvidoMensagemDTO.devolvido =
      requisicaoAxios.data.devolvido;
    novaResposta.devolvidoMensagemDTO.mensagem = requisicaoAxios.data.mensagem;
  }
}
