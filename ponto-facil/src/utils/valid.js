function isFieldValid(input) {
    return (input + '').trim() !== "";
}

function isNullOrEmpty(value) {
    if(value == null) return true
    return false
}

module.exports = {
    isFieldValid,
    isNullOrEmpty
}

function getCpfVerifyierDigits(firstNineCpfDigits) {
    let sum = 0
    let mult = 10
    for (let c of firstNineCpfDigits) {
        sum += ((+c) * mult)
        mult--
    }
    let firstDigit = (sum * 10) % 11
    firstDigit = firstDigit > 9 ? 0 : firstDigit
    sum = 0
    mult = 11
    for (let c of firstNineCpfDigits) {
        sum += ((+c) * mult)
        mult--
    }
    let secondDigit = (sum * 10) % 11
    secondDigit = secondDigit > 9 ? 0 : secondDigit
    return `${firstDigit}${secondDigit}`
}

function allCpfDigitsAreSame(cpf) {
    return cpf.every(x => x === cpf[0])
}

function cpfIsValid(cpf) {
    let cpfVerifyierDigits = getCpfVerifyierDigits(cpf)
    return !allCpfDigitsAreSame(cpf) && (+cpfVerifyierDigits[0]) === (+cpf[10]) && (+cpfVerifyierDigits[1]) === (+cpf[11])
}