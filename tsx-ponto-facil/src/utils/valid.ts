export function isFieldValid(input: any): boolean {
    return (input + ``).trim() !== ``;
}

export function isNullOrEmpty(value: any): boolean {
    if(value == null) return true
    return false
}

export function getCpfVerifyierDigits(firstNineCpfDigits: string): string {
    let sum = 0
    let mult = 10
    for (let char of firstNineCpfDigits) {
        sum += ((+char) * mult)
        mult--
    }
    let firstDigit = (sum * 10) % 11
    firstDigit = firstDigit > 9 ? 0 : firstDigit
    sum = 0
    mult = 11
    for (let char of firstNineCpfDigits) {
        sum += ((+char) * mult)
        mult--
    }
    let secondDigit = (sum * 10) % 11
    secondDigit = secondDigit > 9 ? 0 : secondDigit
    return `${firstDigit}${secondDigit}`
}

export function allCpfDigitsAreSame(cpf: string): boolean {
    for (let char of cpf) {
        if (char !== cpf[0]) {
            return false;
        }
    }
    return true;
}

export function cpfIsValid(cpf: string): boolean {
    let cpfVerifyierDigits = getCpfVerifyierDigits(cpf)
    return !allCpfDigitsAreSame(cpf) && (+cpfVerifyierDigits[0]) === (+cpf[10]) && (+cpfVerifyierDigits[1]) === (+cpf[11])
}