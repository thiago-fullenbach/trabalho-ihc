export default class SessaoDTO {
    get int_id() {
        return this.Id;
    }
    set int_id(value) {
        this.Id = value;
    }

    get string_hexVerificacao() {
        return this.HexVerificacao;
    }
    set string_hexVerificacao(value) {
        this.HexVerificacao = value;
    }
    
    get date_dataHoraUltimaAutenticacao() {
        return this.Datahora_ultima_autenticacao;
    }
    set date_dataHoraUltimaAutenticacao(value) {
        this.Datahora_ultima_autenticacao = value;
    }
}
