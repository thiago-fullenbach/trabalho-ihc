import IAmbienteAplicacao from "../IAmbienteAplicacao";

export default class AplicacaoProd implements IAmbienteAplicacao {
    getUrlDominioApi(): string {
        return "https://ihc-n-ponto-facil-api.herokuapp.com";
    }
}