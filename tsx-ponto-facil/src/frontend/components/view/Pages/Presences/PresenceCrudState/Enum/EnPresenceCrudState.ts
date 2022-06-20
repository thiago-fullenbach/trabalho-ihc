export default class EnPresenceCrudState {

    enValue: number

    constructor(enValue: number) {
        this.enValue = enValue
    }
    static get Hidden(): EnPresenceCrudState {
        return new EnPresenceCrudState(1)
    }
    static get New(): EnPresenceCrudState {
        return new EnPresenceCrudState(2)
    }
    equals = (outer: EnPresenceCrudState) => this.enValue === outer.enValue
}