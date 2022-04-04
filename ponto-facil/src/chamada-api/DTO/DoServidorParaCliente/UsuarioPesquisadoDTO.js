export default class UsuarioPesquisadoDTO {
  get id() {
    return this._id;
  }
  set id(value) {
    this._id = value;
  }
  get nome() {
    return this._nome;
  }
  set nome(value) {
    this._nome = value;
  }
  get cpf() {
    return this._cpf;
  }
  set cpf(value) {
    this._cpf = value;
  }
  get dataNascimento() {
    return this._dataNascimento;
  }
  set dataNascimento(value) {
    this._dataNascimento = value;
  }
}
