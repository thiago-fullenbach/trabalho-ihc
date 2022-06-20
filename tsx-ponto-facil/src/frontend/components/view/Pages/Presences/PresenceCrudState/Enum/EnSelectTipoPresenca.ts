export default class EnSelectTipoPresenca {

    enValue: number
    enDesc: string

    constructor(enValue: number, enDesc: string) {
        this.enValue = enValue
        this.enDesc = enDesc
    }
    static get Selecione(): EnSelectTipoPresenca {
        return new EnSelectTipoPresenca(1, `Selecione...`)
    }
    static get InicioTrabalho(): EnSelectTipoPresenca {
        return new EnSelectTipoPresenca(2, `Início Trabalho`)
    }
    static get FimTrabalho(): EnSelectTipoPresenca {
        return new EnSelectTipoPresenca(3, `Fim Trabalho`)
    }
    equals = (outer: EnSelectTipoPresenca) => this.enValue === outer.enValue
}