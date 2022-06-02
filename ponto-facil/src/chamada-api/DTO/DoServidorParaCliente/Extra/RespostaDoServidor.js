import DevolvidoMensagensDTO from "../DevolvidoMensagensDTO";
import SessaoEnvioHeaderDTO from "../SessaoEnvioHeaderDTO";

export default class RespostaDoServidor {
    get int_status() {
        return this.status;
    }
    set int_status(value) {
        this.status = value;
    }
    
    get SessaoEnvioHeaderDTO_sessao() {
        return this.sessao;
    }
    set SessaoEnvioHeaderDTO_sessao(value) {
        this.sessao = value;
    }
    
    get UsuarioLogadoDTO_usuario() {
        return this.usuario;
    }
    set UsuarioLogadoDTO_usuario(value) {
        this.usuario = value;
    }

    get DevolvidoMensagensDTO_corpo() {
        return this.corpo;
    }
    set DevolvidoMensagensDTO_corpo(value) {
        this.corpo = value;
    }

    static parse(AxiosResponse_resposta) {
      let RespostaDoServidor_retorno = new RespostaDoServidor();
      RespostaDoServidor_retorno.int_status = AxiosResponse_resposta.status;
      if (RespostaDoServidor_retorno.int_status >= 200 && RespostaDoServidor_retorno.int_status <= 299) {
        let any_sessao = JSON.parse(AxiosResponse_resposta.headers["sessao"]);
        RespostaDoServidor_retorno.SessaoDTO_sessao = any_sessao;
        let any_usuario = JSON.parse(AxiosResponse_resposta.headers["usuario"]);
        RespostaDoServidor_retorno.UsuarioLogadoDTO_usuario = any_usuario;
      }
      RespostaDoServidor_retorno.DevolvidoMensagensDTO_corpo = AxiosResponse_resposta.data;
      return RespostaDoServidor_retorno;
    }
}
