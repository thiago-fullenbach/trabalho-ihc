import DevolvidoMensagemDTO from "../DevolvidoMensagemDTO";
import SessaoDTO from "../SessaoDTO";

export default class RespostaDoServidor {
    get int_status() {
        return this.status;
    }
    set int_status(value) {
        this.status = value;
    }
    
    get SessaoDTO_sessao() {
        return this.sessao;
    }
    set SessaoDTO_sessao(value) {
        this.sessao = value;
    }
    
    get UsuarioLogadoDTO_usuario() {
        return this.usuario;
    }
    set UsuarioLogadoDTO_usuario(value) {
        this.usuario = value;
    }

    get DevolvidoMensagemDTO_corpo() {
        return this.corpo;
    }
    set DevolvidoMensagemDTO_corpo(value) {
        this.corpo = value;
    }

    static parse(AxiosResponse_resposta) {
      let RespostaDoServidor_retorno = new RespostaDoServidor();
      RespostaDoServidor_retorno.int_status = AxiosResponse_resposta.status;
      let any_sessao = JSON.parse(AxiosResponse_resposta.headers["sessao"]);
      RespostaDoServidor_retorno.SessaoDTO_sessao = any_sessao;
      let any_usuario = JSON.parse(AxiosResponse_resposta.headers["usuario"]);
      RespostaDoServidor_retorno.UsuarioLogadoDTO_usuario = any_usuario;
      RespostaDoServidor_retorno.DevolvidoMensagemDTO_corpo = AxiosResponse_resposta.data;
      return RespostaDoServidor_retorno;
    }
}
