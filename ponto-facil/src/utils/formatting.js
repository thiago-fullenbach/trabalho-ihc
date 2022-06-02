export function formatCpf(cpf) {
    return `${cpf.substring(0, 3)}.${cpf.substring(3, 6)}.${cpf.substring(6, 9)}-${cpf.substring(9, 11)}`
}
export function shift2digitsLongNumber(number) {
    let shiftedString = '' + number
    while (shiftedString.length < 2) {
        shiftedString = '0' + shiftedString
    }
    return shiftedString
}
export function formatDate_dd_mm_yyyy(date) {
    return `${shift2digitsLongNumber(date.getDate())}/${shift2digitsLongNumber(date.getMonth() + 1)}/${date.getFullYear()}`
}
export function listItemsInPortuguese(items) {
    if (items.length === 0) {
        return ''
    } else if (items.length === 1) {
        return items[0]
    } else if (items.length === 2)  {
        return `${items[0]} e ${items[1]}`
    } else {
        return items
            .filter((x, i) => i !== items.length - 1 && i !== items.length - 2)
            .reduceRight((acc, x) => `${x}, ${acc}`, `${items[items.length - 2]} e ${items[items.length - 1]}`)
    }
}
export function mountMessage(response) {
    let mensagens = response.corpo.Mensagens || response.corpo.mensagens
    return mensagens
        .filter((_vlMsg, idxMsg) => idxMsg !== 0)
        .reduce((prev, current) => prev + '\n' + current, mensagens[0])
}