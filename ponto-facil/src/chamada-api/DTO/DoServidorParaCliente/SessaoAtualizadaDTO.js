export default class SessaoAtualizadaDTO {
  get int_id() {
    return this._id;
  }
  set int_id(value) {
    this._id = value;
  }
  get string_hexVerificacao() {
    return this._hexVerificacao;
  }
  set string_hexVerificacao(value) {
    this._hexVerificacao = value;
  }
  get date_dataHoraUltimaAtualizacao() {
    return this._dataHoraUltimaAtualizacao;
  }
  set date_dataHoraUltimaAtualizacao(value) {
    this._dataHoraUltimaAtualizacao = value;
  }
  get RecursoPermitidoDTO_Array_listaRecursosPermitidos() {
    return this._listaRecursosPermitidos;
  }
  set RecursoPermitidoDTO_Array_listaRecursosPermitidos(value) {
    this._listaRecursosPermitidos = value;
  }
}
