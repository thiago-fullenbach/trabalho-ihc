import { toDisplayedHourLocal, toDisplayedValue, toDisplayedValueLocal } from "../../../utils/formatting"
import { getTimeDia, horaComparavel, horaFoiInformada, interseccaoHorasEntre } from "../../../utils/timeAndHour"
import PresenceFilter from "./presenceFilter"

export default class Periodo {

    inicio: Date = new Date()
    fim: Date = new Date()
    getDiasAssociadosLocal = (): Date[] => {
        let dias: Date[] = []
        let diaPercorrer = new Date(toDisplayedValue(this.inicio))
        let ultimoDia = new Date(toDisplayedValue(this.fim))
        while (toDisplayedValueLocal(diaPercorrer) != toDisplayedValueLocal(ultimoDia)) {
            dias.push(diaPercorrer)
            diaPercorrer = new Date(diaPercorrer.getTime() + getTimeDia())
        }
        dias.push(diaPercorrer)
        return dias
    }
    horasTrabalhadasByFilter = (filtros: PresenceFilter): number => {
        const offset = this.inicio.getTimezoneOffset() * 60 * 1000
        let diasTrabalhados = this.getDiasAssociadosLocal()
            .filter(x => new Date(toDisplayedValue(x)).getTime() >= (filtros.data_inicio ?? new Date()).getTime()
                && new Date(toDisplayedValue(x)).getTime() <= (filtros.data_fim ?? new Date()).getTime())
        if (horaFoiInformada(filtros.hora_inicio) && horaFoiInformada(filtros.hora_fim) && horaComparavel(filtros.hora_inicio) > horaComparavel(filtros.hora_fim)) {
            let inicioPeriodo1 = `00:00`
            let fimPeriodo1 = filtros.hora_fim
            let inicioPeriodo2 = filtros.hora_inicio
            let fimPeriodo2 = `23:59`
            if (diasTrabalhados.length <= 1) {
                let horasPeriodo1 = interseccaoHorasEntre(inicioPeriodo1, fimPeriodo1, this.inicio, this.fim)
                let horasPeriodo2 = interseccaoHorasEntre(inicioPeriodo2, fimPeriodo2, this.inicio, this.fim)
                return horasPeriodo1 + horasPeriodo2
            } else {
                let horasPeriodo1PrimeiroDia = interseccaoHorasEntre(inicioPeriodo1, fimPeriodo1, this.inicio, new Date(new Date(toDisplayedValue(this.inicio)).getTime() + getTimeDia() + offset))
                let horasPeriodo2PrimeiroDia = interseccaoHorasEntre(inicioPeriodo2, fimPeriodo2, this.inicio, new Date(new Date(toDisplayedValue(this.inicio)).getTime() + getTimeDia() + offset))
                let total = horasPeriodo1PrimeiroDia + horasPeriodo2PrimeiroDia
                let horasPeriodo1UltimoDia = interseccaoHorasEntre(inicioPeriodo1, fimPeriodo1, new Date(new Date(toDisplayedValue(this.fim)).getTime() + offset), this.fim)
                let horasPeriodo2UltimoDia = interseccaoHorasEntre(inicioPeriodo2, fimPeriodo2, new Date(new Date(toDisplayedValue(this.fim)).getTime() + offset), this.fim)
                total += horasPeriodo1UltimoDia + horasPeriodo2UltimoDia
                let horasDiaIntermediarioQualquer = horaComparavel(fimPeriodo1) - horaComparavel(`00:00`) - (horaComparavel(inicioPeriodo1) - horaComparavel(`00:00`))
                horasDiaIntermediarioQualquer += horaComparavel(fimPeriodo2) - horaComparavel(`00:00`) - (horaComparavel(inicioPeriodo2) - horaComparavel(`00:00`))
                total += horasDiaIntermediarioQualquer * (diasTrabalhados.length - 2)
                return total
            }
        } else {
            let inicioPeriodo = horaFoiInformada(filtros.hora_inicio) ? filtros.hora_inicio : `00:00`
            let fimPeriodo = horaFoiInformada(filtros.hora_fim) ? filtros.hora_fim : `23:59`
            if (diasTrabalhados.length <= 1) {
                return interseccaoHorasEntre(inicioPeriodo, fimPeriodo, this.inicio, this.fim)
            } else {
                let horasPrimeiroDia = interseccaoHorasEntre(inicioPeriodo, fimPeriodo, this.inicio, new Date(new Date(toDisplayedValue(this.inicio)).getTime() + getTimeDia() + offset))
                let total = horasPrimeiroDia
                let horasUltimoDia = interseccaoHorasEntre(inicioPeriodo, fimPeriodo, new Date(new Date(toDisplayedValue(this.fim)).getTime() + offset), this.fim)
                total += horasUltimoDia
                let horasDiaIntermediarioQualquer = horaComparavel(fimPeriodo) - horaComparavel(`00:00`) - (horaComparavel(inicioPeriodo) - horaComparavel(`00:00`))
                total += horasDiaIntermediarioQualquer * (diasTrabalhados.length - 2)
                return total
            }
        }
    }
}