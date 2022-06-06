import AplicacaoDesenv from "./AmbienteAplicacaoImpl/AplicacaoDesenv";
import AplicacaoProd from "./AmbienteAplicacaoImpl/AplicacaoProd";
import IAmbienteAplicacao from "./IAmbienteAplicacao";

export default class Aplicacao {
    static criaAmbienteDetectado(): IAmbienteAplicacao {
        if (window.location.hostname !== 'localhost') {
            return new AplicacaoProd();
        } else {
            return new AplicacaoDesenv();
        }
    }
}