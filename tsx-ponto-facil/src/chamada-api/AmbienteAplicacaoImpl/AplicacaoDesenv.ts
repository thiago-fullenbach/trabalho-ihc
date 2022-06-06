import IAmbienteAplicacao from "../IAmbienteAplicacao";

export default class AplicacaoDesenv implements IAmbienteAplicacao {
    getUrlDominioApi(): string {
        return "http://localhost:5086";
    }
}