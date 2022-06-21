import Periodo from "../frontend/modelo/presence/periodo"
import SearchedPresence from "../frontend/modelo/presence/searchedPresence"

const horaFoiInformada = (hora: string): boolean => {
    return /[0-9][0-9]:[0-9][0-9]/.test(hora)
}
const horaComparavel = (hora: string): number => {
    return (new Date(`2000-01-01T${hora}`)).getTime()
}
const extraiHorasLocal = (data: Date): number => {
    return (new Date(`2000-01-01T${(data.getHours() + ``).padStart(2, `0`)}:${(data.getMinutes() + ``).padStart(2, `0`)}:${(data.getSeconds() + ``).padStart(2, `0`)}.${(data.getMilliseconds() + ``).padStart(3, `0`)}`)).getTime() 
}
const addTempo = (data: Date, parcela: number): Date => {
    return new Date(data.getTime() + parcela)
}
const subtractTempo = (data: Date, parcela: number): Date => {
    return new Date(data.getTime() - parcela)
}
const addHoras = (data: Date, parcelaHora: string): Date => {
    return horaFoiInformada(parcelaHora) ? addTempo(data, horaComparavel(parcelaHora) - horaComparavel(`00:00`)) : new Date(data)
}
const addHorasCapped = (data: Date, parcelaHora: string): Date => {
    return horaFoiInformada(parcelaHora) ? addTempo(data, horaComparavel(parcelaHora) - horaComparavel(`00:00`) + horaComparavel(`00:01`) - horaComparavel(`00:00`)) : new Date(data)
}
const subtractHoras = (data: Date, parcelaHora: string) => {
    return horaFoiInformada(parcelaHora) ? subtractTempo(data, horaComparavel(parcelaHora) - horaComparavel(`00:00`)) : new Date(data)
}
const getTimeDia = (): number => {
    return (horaComparavel(`12:00`) - horaComparavel(`00:00`)) * 2
}
const obterPeriodosBySearchedPresenceArray = (presencas: SearchedPresence[]): Periodo[] => {
    let usuarioPeriodos: Periodo[] = []
    let presencasMapeadas: { inicio: number, fim: number }[] = []
    for (let index = 0; index < presencas.length; index++) {
        const element = presencas[index]
        if (element.eh_entrada) {
            const jaMapeado = presencasMapeadas.some(y => y.inicio == element.id)
            if (jaMapeado) {
                continue
            }
            let elementFim = presencas
                .filter(y => y.datahora_presenca.getTime() > element.datahora_presenca.getTime() && !y.eh_entrada)
                .sort((a, b) => a.datahora_presenca.getTime() - b.datahora_presenca.getTime())
            let novoPeriodo = new Periodo()
            novoPeriodo.inicio = new Date(element.datahora_presenca)
            if (elementFim.length === 0) {
                novoPeriodo.fim = addHoras(element.datahora_presenca, `12:00`)
                usuarioPeriodos.push(novoPeriodo)
                presencasMapeadas.push({ inicio: element.id, fim: -1 })
                continue
            }
            novoPeriodo.fim = new Date(elementFim[0].datahora_presenca)
            usuarioPeriodos.push(novoPeriodo)
            presencasMapeadas.push({ inicio: element.id, fim: elementFim[0].id })
        } else {
            const jaMapeado = presencasMapeadas.some(y => y.fim == element.id)
            if (jaMapeado) {
                continue
            }
            let elementInicio = presencas
                .filter(y => y.datahora_presenca.getTime() < element.datahora_presenca.getTime() && y.eh_entrada)
                .sort((a, b) => b.datahora_presenca.getTime() - a.datahora_presenca.getTime())
            let novoPeriodo = new Periodo()
            novoPeriodo.fim = new Date(element.datahora_presenca)
            if (elementInicio.length === 0) {
                novoPeriodo.inicio = subtractHoras(element.datahora_presenca, `12:00`)
                usuarioPeriodos.push(novoPeriodo)
                presencasMapeadas.push({ inicio: -1, fim: element.id })
                continue
            }
            novoPeriodo.inicio = new Date(elementInicio[0].datahora_presenca)
            usuarioPeriodos.push(novoPeriodo)
            presencasMapeadas.push({ inicio: elementInicio[0].id, fim: element.id })
        }
    }
    return usuarioPeriodos
}
const interseccaoHorasEntre = (inicioHora1: string, fimHora1: string, inicioHora2: Date, fimHora2: Date): number => {
    let maiorInicio = [horaComparavel(inicioHora1), extraiHorasLocal(inicioHora2)].sort((a, b) => b - a)[0]
    let menorFim = [horaComparavel(fimHora1) + horaComparavel(`00:01`) - horaComparavel(`00:00`), extraiHorasLocal(fimHora2)].sort((a, b) => a - b)[0]
    let horas = menorFim - maiorInicio
    return horas > 0 ? horas : 0
}
export { horaFoiInformada, horaComparavel, extraiHorasLocal, addTempo, subtractTempo, addHoras, addHorasCapped, subtractHoras, getTimeDia, obterPeriodosBySearchedPresenceArray, interseccaoHorasEntre }