export default class SessaoDTO {
    get int_id() {
        return this.Id;
    }
    set int_id(value) {
        this.Id = value;
    }

    get string_hex_verificacao() {
        return this.Hex_verificacao;
    }
    set string_hex_verificacao(value) {
        this.Hex_verificacao = value;
    }
    
    get date_dataHoraUltimaAutenticacao() {
        return this.Datahora_ultima_autenticacao;
    }
    set date_dataHoraUltimaAutenticacao(value) {
        this.Datahora_ultima_autenticacao = value;
    }
}
