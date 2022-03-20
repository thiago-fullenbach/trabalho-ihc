class LogicaUsuarioAutorizacao {
  get token() {
    return this._token;
  }
  set token(value) {
    this._token = value;
  }

  get usuario() {
    return this._usuario;
  }
  set usuario(value) {
    this._usuario = value;
  }
}

export default LogicaUsuarioAutorizacao;
