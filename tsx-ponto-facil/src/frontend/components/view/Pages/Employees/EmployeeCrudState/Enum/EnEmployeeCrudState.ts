export default class EnEmployeeCrudState {

    enValue: number

    constructor(enValue: number) {
        this.enValue = enValue
    }
    static get Hidden(): EnEmployeeCrudState {
        return new EnEmployeeCrudState(1)
    }
    static get New(): EnEmployeeCrudState {
        return new EnEmployeeCrudState(2)
    }
    static get Edit(): EnEmployeeCrudState {
        return new EnEmployeeCrudState(3)
    }
    equals = (outer: EnEmployeeCrudState) => this.enValue === outer.enValue
}