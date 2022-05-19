import AplicacaoStateAmbienteDesenvolvimento from "./AplicacaoStateAmbienteDesenvolvimento";
import AplicacaoStateAmbienteProducao from "./AplicacaoStateAmbienteProducao";

export default class AplicacaoStateful {
    static get estado() {
        if (location.hostname === 'ihc-n-ponto-facil.herokuapp.com') {
            return new AplicacaoStateAmbienteProducao();
        } else { return new AplicacaoStateAmbienteDesenvolvimento(); }
    }
}