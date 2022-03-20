class MdUsuario {
  get idUser() {
    return this._idUser;
  }
  set idUser(value) {
    this._idUser = value;
  }

  get idFunc() {
    return this._idFunc;
  }
  set idFunc(value) {
    this._idFunc = value;
  }

  get idTipo() {
    return this._idTipo;
  }
  set idTipo(value) {
    this._idTipo = value;
  }

  get login() {
    return this._login;
  }
  set login(value) {
    this._login = value;
  }

  get senha() {
    return this._senha;
  }
  set senha(value) {
    this._senha = value;
  }

  get bancoHoras() {
    return this._bancoHoras;
  }
  set bancoHoras(value) {
    this._bancoHoras = value;
  }
}

export default MdUsuario;
