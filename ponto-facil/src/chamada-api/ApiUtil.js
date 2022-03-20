class ApiUtil {
  static get urlDominio() {
    const urlDominioApi =
      process.env.URL_DOMINIO_API || "http://localhost:5000/Api/v1";
    return urlDominioApi;
  }

  static async obterLogicaUsuarioAutorizacaoAsync(login, senha) {}

  static async getAsync(url) {}

  static async postAsync(url, objeto) {}
}

export default ApiUtil;
