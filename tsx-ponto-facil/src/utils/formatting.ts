import IReturnedXMessagesAdapter from "../chamada-api/DTO/DoServidorParaCliente/Adapter/IReturnedXMessagesAdapter"
import { message } from "../frontend/modelo/message"

export function formatCpf(cpf: string): string {
    return `${cpf.substring(0, 3)}.${cpf.substring(3, 6)}.${cpf.substring(6, 9)}-${cpf.substring(9, 11)}`
}
export function formatDate_dd_mm_yyyy(date: Date): string {
    return `${(date.getDate() + ``).padStart(2, `0`)}/${(date.getMonth() + 1 + ``).padStart(2, ``)}/${date.getFullYear()}`
}
export function toInputValue(date: Date | null): string {
    if (date === null) {
        return ``
    }
    return `${date.getFullYear()}-${(date.getMonth() + 1 + ``).padStart(2, `0`)}-${(date.getDate() + ``).padStart(2, ``)}T${(date.getHours() + ``).padStart(2, ``)}:${(date.getMinutes() + ``).padStart(2, ``)}:${(date.getSeconds() + ``).padStart(2, ``)}`
}
export function listItemsInPortuguese<T>(items: Array<T>): string {
    if (items.length === 0) {
        return ``
    } else if (items.length === 1) {
        return `${items[0]}`
    } else if (items.length === 2)  {
        return `${items[0]} e ${items[1]}`
    } else {
        return items
            .filter((x, i) => i !== items.length - 1 && i !== items.length - 2)
            .reduceRight((acc, x) => `${x}, ${acc}`, `${items[items.length - 2]} e ${items[items.length - 1]}`)
    }
}
export function mountMessage<T>(body: IReturnedXMessagesAdapter<T>): string {
    let mensagens = body.getMessages()
    return mensagens
        .filter((_vlMsg, idxMsg) => idxMsg !== 0)
        .reduce((prev, current) => prev + '\n' + current, mensagens[0])
}
export function mountMessageServerErrorIfDefault<T>(body: IReturnedXMessagesAdapter<T> | undefined): string {
    return (body !== undefined) ? (mountMessage(body)) : message.serverError
}