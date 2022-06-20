export default class EnSelectTipoTemVisto {

    enValue: number
    enDesc: string

    constructor(enValue: number, enDesc: string) {
        this.enValue = enValue
        this.enDesc = enDesc
    }
    static get Selecione(): EnSelectTipoTemVisto {
        return new EnSelectTipoTemVisto(1, `Selecione...`)
    }
    static get SemVisto(): EnSelectTipoTemVisto {
        return new EnSelectTipoTemVisto(2, `Sem Visto`)
    }
    static get ComVisto(): EnSelectTipoTemVisto {
        return new EnSelectTipoTemVisto(3, `Com Visto`)
    }
    equals = (outer: EnSelectTipoTemVisto) => this.enValue === outer.enValue
}