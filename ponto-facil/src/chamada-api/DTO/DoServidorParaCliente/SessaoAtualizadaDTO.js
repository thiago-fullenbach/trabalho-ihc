export default class SessaoAtualizadaDTO {
  get id() {
    return this._id;
  }
  set id(value) {
    this._id = value;
  }
  get hexVerificacao() {
    return this._hexVerificacao;
  }
  set hexVerificacao(value) {
    this._hexVerificacao = value;
  }
  get dataHoraUltimaAtualizacao() {
    return this._dataHoraUltimaAtualizacao;
  }
  set dataHoraUltimaAtualizacao(value) {
    this._dataHoraUltimaAtualizacao = value;
  }
  get listaRecursosPermitidos() {
    return this._listaRecursosPermitidos;
  }
  set listaRecursosPermitidos(value) {
    this._listaRecursosPermitidos = value;
  }
}
