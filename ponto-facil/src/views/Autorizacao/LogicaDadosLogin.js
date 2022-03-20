import ApiUtil from "../../chamada-api/ApiUtil";

class LogicaDadosLogin {
  constructor() {
    this._campoUsuario = "";
    this._campoSenha = "";
    this._listaUsuariosCadastrados = [];
  }

  static doAntigo(logicaDadosLoginAntigo) {
    var novo = new LogicaDadosLogin();
    novo._campoUsuario = logicaDadosLoginAntigo.campoUsuario;
    novo._campoSenha = logicaDadosLoginAntigo.campoSenha;
    novo._listaUsuariosCadastrados =
      logicaDadosLoginAntigo.listaUsuariosCadastrados;
    return novo;
  }

  get campoUsuario() {
    return this._campoUsuario;
  }
  set campoUsuario(value) {
    this._campoUsuario = value;
  }

  get campoSenha() {
    return this._campoSenha;
  }
  set campoSenha(value) {
    this._campoSenha = value;
  }

  get listaUsuariosCadastrados() {
    return this._listaUsuariosCadastrados;
  }

  async carregarDadosApi() {
    const urlListaUsuarios = ApiUtil.urlDominio + "/Usuario/ListarTodos";
    let usuarios = await ApiUtil.getAsync(urlListaUsuarios);
    this._listaUsuariosCadastrados = usuarios.map((x) => x.nmUsuario);
  }
}

export default LogicaDadosLogin;
